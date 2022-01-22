using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;
using UksivtScheduler_PC.Classes.General;
using UksivtScheduler_PC.Classes.ScheduleElements;

namespace UksivtScheduler_PC.Other
{
    public partial class Program
    {
        public static void InsertTetheredData()
        {
            String programmingPath = upPath + "Economy\\";
            List<WeekSchedule> weeks = new(1);
            List<DaySchedule> schedules = new(1);
            List<(Int32 LessonId, String LessonName, String TeacherName)> joinedLessons = GetJoinedLessons();
            List<(Int32 DayScheduleId, Int32 LessonId, Int32? SubGroup, Int32? SubWeek)> lessonsTether = new(1);

            foreach (String subProgramming in Directory.GetDirectories(programmingPath))
            {
                foreach (String file in Directory.GetFiles(subProgramming))
                {
                    weeks.Add(JsonConvert.DeserializeObject<WeekSchedule>(File.ReadAllText(file)));
                    schedules.AddRange(JsonConvert.DeserializeObject<WeekSchedule>(File.ReadAllText(file)).Days);
                }
            }

            schedules.ForEach(element => element.Lessons.RemoveAll(lesson => !lesson.CheckHaveValue()));
            schedules.RemoveAll(element => element.Day.Equals("Sunday"));
            weeks.ForEach(element => element.Days.ForEach(subElement => subElement.Lessons.RemoveAll(lesson => !lesson.CheckHaveValue())));
            weeks.ForEach(element => element.Days.RemoveAll(subElement => subElement.Day.Equals("Sunday")));

            for (int i = 0; i < schedules.Count; i++)
            {
                DaySchedule currentSchedule = schedules[i];
                Int32 dayInd = currentSchedule.Day.GetIndexByDay();
                String groupName = GetGroupName(weeks, currentSchedule);
                Int32 indOfValueInDayScheduleTable = InsertDaySchedule(dayInd, groupName);

                for (int j = 0; j < schedules[i].Lessons.Count; j++)
                {
                    Lesson currentLesson = schedules[i].Lessons[j];

                    Int32 lessonId = default;
                    Int32? weekSubData = null;
                    Int32? groupSubData = null;

                    if (currentLesson.Name.Contains("Группа"))
                    {
                        if (currentLesson.Name.Contains(" (1 Группа)"))
                        {
                            groupSubData = 1;
                        }

                        else if (currentLesson.Name.Contains(" (2 Группа)"))
                        {
                            groupSubData = 2;
                        }

                        currentLesson.Name = RemoveStrings(currentLesson.Name, " (1 Группа)", " (2 Группа)");
                    }

                    else if (currentLesson.Name.Contains("Неделя"))
                    {
                        if (currentLesson.Name.Contains(" (1 Неделя)"))
                        {
                            weekSubData = 1;
                        }

                        else if (currentLesson.Name.Contains(" (2 Неделя)"))
                        {
                            weekSubData = 2;
                        }

                        currentLesson.Name = RemoveStrings(currentLesson.Name, " (1 Неделя)", " (2 Неделя)");
                    }

                    try
                    {
                        currentLesson.Name = CheckLessonNameToTypos(currentLesson.Name);
                        currentLesson.Teacher = CheckTeacherNameToTypos(currentLesson.Teacher);

                        lessonId = joinedLessons.Where(element => CheckToSameCriterias(element.LessonName, currentLesson.Name) &&
                        CheckToSameCriterias(element.TeacherName, currentLesson.Teacher)).First().LessonId;
                    }

                    catch (InvalidOperationException)
                    {
                        (currentLesson.Name, currentLesson.Teacher) = (currentLesson.Teacher, currentLesson.Name);

                        currentLesson.Name = CheckLessonNameToTypos(currentLesson.Name);
                        currentLesson.Teacher = CheckTeacherNameToTypos(currentLesson.Teacher);

                        lessonId = joinedLessons.Where(element => CheckToSameCriterias(element.LessonName, currentLesson.Name)).
                        Where(element => CheckToSameCriterias(element.TeacherName, currentLesson.Teacher)).First().LessonId;
                    }

                    lessonsTether.Add(new(indOfValueInDayScheduleTable, lessonId, groupSubData, weekSubData));
                }
            }

            lessonsTether.ForEach(item => InsertLessonsTether(item.DayScheduleId, item.LessonId, item.SubGroup, item.SubWeek));
        }

        private static Int32 InsertDaySchedule(Int32 dayId, String groupName)
        {
            #region Подобласть: Вставка значения.
            SqlCommand command = new($"INSERT INTO Day_Schedule(Day_Id, Group_Name__Temp) VALUES({dayId}, \'{groupName}\');", connection);
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
            #endregion

            #region Подобласть: Получение индекса.
            Int32 indexOfValue = default;
            command = new($"SELECT MAX(Day_Schedule_Id) FROM Day_Schedule;", connection);

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    indexOfValue = reader.GetInt32(0);
                }
            }

            connection.Close();
            #endregion

            return indexOfValue;
        }

        private static void InsertLessonsTether(Int32 dayScheduleId, Int32 lessonId, Int32? lessonSubGroup, Int32? weekSubGroup)
        {
            #region Подобласть: Проверка на дополнительные значения.
            String firstAppend = lessonSubGroup.HasValue ? $", {lessonSubGroup.Value}" : ", null";
            String secondAppend = weekSubGroup.HasValue ? $", {weekSubGroup.Value}" : ", null";
            #endregion

            SqlCommand command = new($"INSERT INTO Lessons_To_Schedules(Day_Schedule_Ids, Lessons_Ids, Lesson_Sub_Group, Lesson_Sub_Week) " +
            $"VALUES({dayScheduleId}, {lessonId}" + firstAppend + secondAppend + ");", connection);
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        private static List<(Int32 DayIndex, String Day)> GetDaysFromDb()
        {
            SqlCommand command = new("SELECT * FROM Days_Of_Week", connection);
            List<(Int32 DayIndex, String Day)> toReturn = new(1);

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Int32 id = reader.GetInt32(0);
                    String day = reader.GetString(1);

                    toReturn.Add(new(id, day));
                }
            }

            connection.Close();
            return toReturn;
        }

        private static List<(Int32 LessonId, String LessonName, String TeacherName)> GetJoinedLessons()
        {
            SqlCommand command = new(@"SELECT Lesson.Lesson_Id,
                                              Lessons_Names.Lesson_Name,
                                              SubQuery.FullName
                                       FROM Lesson
                                            JOIN
                                            (
                                                SELECT Teacher_Id,
                                                        Person_Surname + ' ' + LEFT(Person.Person_Name, 1) + ' ' + LEFT(Person_Middle_Name, 1) AS FullName
                                                FROM Teacher
                                                    JOIN Person
                                                        ON Teacher.Person_Id = Person.Person_Id
                                            ) AS SubQuery
                                                ON Lesson.Lesson_Teacher_Id = SubQuery.Teacher_Id
                                            JOIN Lessons_Names
                                                ON Lesson.Lesson_Name_Id = Lessons_Names.Lesson_Name_Id
                                    ORDER BY Lessons_Names.Lesson_Name;", connection);
            List<(Int32 LessonId, String LessonName, String teacherName)> toReturn = new(1);

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Int32 lessonId = reader.GetInt32(0);
                    String lessonName = reader.GetString(1);
                    String teacherName = reader.GetString(2);

                    toReturn.Add(new(lessonId, lessonName, teacherName));
                }
            }

            connection.Close();
            return toReturn;
        }

        private static List<(Int32 Id, Int32 NameId, Int32 TeacherId, Int32 number, String place)> GetLessons()
        {
            SqlCommand command = new("SELECT * FROM Lesson");
            List<(Int32 Id, Int32 NameId, Int32 TeacherId, Int32 number, String place)> toReturn = new();

            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Int32 id = reader.GetInt32(0);
                    Int32 nameId = reader.GetInt32(1);
                    Int32 teacherId = reader.GetInt32(2);
                    Int32 number = reader.GetInt32(3);
                    String place = reader.GetString(4);

                    toReturn.Add(new(id, nameId, teacherId, number, place));
                }
            }

            connection.Close();
            return toReturn;
        }

        private static void GetIndicesAndInsertNewEntry(Lesson lesson)
        {
            String tmp = RemoveStrings(lesson.Teacher, ".", ",");
            (Int32 Lesson_Name_Id, Int32 Teacher_Id) requiredValues = new();

            #region Подобласть: Получение индекса названия пары.
            SqlCommand command = new($"SELECT Lesson_Name_Id FROM Lessons_Names WHERE Lesson_Name = \'{lesson.Name}\';", connection);
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    throw new Exception();
                }

                else
                {
                    while (reader.Read())
                    {
                        requiredValues.Lesson_Name_Id = reader.GetInt32(0);
                    }
                }
            }

            connection.Close();
            #endregion

            #region Подобласть: Получение индекса преподавателя.
            command = new($"SELECT Teacher_Id FROM Teacher WHERE Teacher.Person_Id = (SELECT Person_Id FROM Person " +
            $"WHERE Person_Surname = \'{tmp[0..tmp.IndexOf(' ')]}\' " +
            $"AND Person_Name LIKE \'{tmp[(tmp.IndexOf(' ') + 1)..(tmp.IndexOf(" ") + 2)]}%\' " +
            $"AND Person_Middle_Name LIKE \'{tmp[(tmp.LastIndexOf(' ') + 2)..]}%\');", connection);
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    throw new Exception();
                }

                else
                {
                    while (reader.Read())
                    {
                        requiredValues.Teacher_Id = reader.GetInt32(0);
                    }
                }
            }

            connection.Close();
            #endregion

            #region Подобласть: Вставка значений.
            command = new($"INSERT INTO Lesson(Lesson_Name_Id, Lesson_Teacher_Id, Lesson_Number, Lesson_Place) " +
            $"VALUES({requiredValues.Lesson_Name_Id}, {requiredValues.Teacher_Id}, {lesson.Number}, \'{lesson.Place}\');", connection);
            connection.Open();

            try
            {
                command.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    Thread.Sleep(1000);

                    System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                    System.Windows.Application.Current.Shutdown();
                }
            }

            connection.Close();
            #endregion
        }

        private static String GetGroupName(List<WeekSchedule> weeks, DaySchedule currentSchedule)
        {
            foreach (WeekSchedule week in weeks)
            {
                var daysSchedules = week.Days;

                foreach (DaySchedule daySchedule in daysSchedules)
                {
                    String first = daySchedule.ToString();
                    String second = currentSchedule.ToString();

                    if (first.Length == second.Length)
                    {
                        if (daySchedule.Equals(currentSchedule))
                        {
                            return week.GroupName;
                        }
                    }
                }
            }

            return String.Empty;
        }
    }
}
