using System;
using System.IO;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;
using UksivtScheduler_PC.Classes.ScheduleElements;

namespace UksivtScheduler_PC.Other
{
    /*
     * Строка подключения для DB:
     * 
     * Server=tcp:uksivtschedule.database.windows.net,1433;Initial Catalog=ScheduleData;Persist Security Info=False;User ID=Scheduler;Password=Uksivt_22;
     * MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Integrated Security=False;
     */
    public partial class Program
    {
        private static String upPath = @"C:\Users\Земфира\source\repos\UksivtScheduler (PC)\UksivtScheduler (PC)\Assets\";

        private static SqlConnection connection = new("Server=tcp:uksivtschedule.database.windows.net, 1433; Initial Catalog=ScheduleDataRu; Persist Security Info=False; User ID=Scheduler; " +
        "Password=Uksivt_22; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=False; Connection Timeout=30; Integrated Security=False;");

        public static void SubMain(String[] args)
        {          
            List<String> files = new(1);
            List<Lesson> fullLessons = new(1);
            List<(Int32 RecordId, String Name)> allLessons = GetLessonsNamesData("SELECT * FROM Lessons_Names ORDER BY Lesson_Name_Id;", connection);
            List<(Int32 TeacherId, String Name, String Surname, String MiddleName)> allTeachers = GetTeachersData("SELECT Teacher_Id," + "Person_Name," + "Person_Surname," + "Person_Middle_Name " + "FROM Person " + "JOIN Teacher " + "ON Person.Person_Id = Teacher.Person_Id " + "ORDER BY Teacher.Teacher_Id;", connection);

            foreach (String path in Directory.GetDirectories(upPath))
            {
                foreach (String dir in Directory.GetDirectories(path))
                {
                    files.AddRange(Directory.GetFiles(Path.Combine(path, dir)));

                    foreach (String file in files)
                    {
                        List<DaySchedule> days = JsonConvert.DeserializeObject<WeekSchedule>(File.ReadAllText(file)).Days;

                        foreach (DaySchedule day in days)
                        {
                            List<Lesson> lessons = day.Lessons;

                            foreach (Lesson lesson in lessons)
                            {
                                if (fullLessons.TrueForAll(element => !CheckToSameCriterias(lesson.Name, element.Name)) ||
                                fullLessons.TrueForAll(element => !CheckToSameCriterias(lesson.Teacher, element.Teacher)) ||
                                fullLessons.TrueForAll(element => !CheckToSameCriterias(lesson.Place, element.Place)))
                                {
                                    fullLessons.Add(lesson);

                                    fullLessons = fullLessons.DistinctBy(element => new { element.Name, element.Place, element.Teacher }).ToList();
                                }
                            }
                        }
                    }
                }
            }

            fullLessons.RemoveAll(obj => obj == null);
            List<(Int32? NameId, Int32? TeacherId, Int32 Number, String Place)> absNew = new(1);

            //Этот цикл для добавления значений в список с новыми парами.
            foreach (Lesson element in fullLessons)
            {
                (Int32? NameId, Int32? TeacherId, Int32 Number, String Place) toAdd = new();

                element.Teacher = CheckTeacherNameToTypos(element.Teacher);

                // Убираем лишние значения из названия предмета:
                element.Name = RemoveStrings(element.Name, " (1 Неделя)", " (2 Неделя)", " (1 Группа)", " (2 Группа)");

                element.Name = CheckLessonNameToTypos(element.Name);

                Int32? NameIndex = allLessons.Where(temp => CheckToSameCriterias(element.Name, temp.Name)).FirstOrDefault().RecordId;
                Int32? TeacherIndex = allTeachers.Where(temp => CheckToSameCriterias(element.Teacher, $"{temp.Surname} {temp.Name[0..1]}. {temp.MiddleName[0..1]}.")).FirstOrDefault().TeacherId;

                if (NameIndex == 0 && TeacherIndex == 0)
                {
                    // Обрабатываем случай перемены Преподавателя и Пары местами:
                    (element.Name, element.Teacher) = (element.Teacher, element.Name);
                }

                NameIndex = allLessons.Where(temp => CheckToSameCriterias(element.Name, temp.Name)).FirstOrDefault().RecordId;
                TeacherIndex = allTeachers.Where(temp => CheckToSameCriterias(element.Teacher, $"{temp.Surname} {temp.Name[0..1]}. {temp.MiddleName[0..1]}.")).FirstOrDefault().TeacherId;

                toAdd = new(NameIndex, TeacherIndex, element.Number, element.Place);
                absNew.Add(toAdd);
            }

            absNew = absNew.OrderBy(lessonInDb => lessonInDb.Number).ThenBy(lessonDb => lessonDb.TeacherId).ThenBy(lessonDb => lessonDb.NameId).ToList();
            var tempValue = absNew.DistinctBy(ele => new { ele.NameId, ele.TeacherId, ele.Place }).ToList();

            //Этот для записи в БД.
            foreach ((Int32? NameId, Int32? TeacherId, Int32 Number, String Place) newValue in absNew)
            {
                SqlCommand command = new($"INSERT INTO Lesson(Lesson_Name_Id, Lesson_Teacher_Id, Lesson_Number, Lesson_Place) " +
                $"VALUES({newValue.NameId}, {newValue.TeacherId}, {newValue.Number}, \'{newValue.Place}\');", connection);
                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static Boolean ContainsInListCaseInsensitive(List<String> list, String value)
        {
            value = value.ToLower();

            if (list.Select(element => element.ToLower()).Contains(value))
            {
                return true;
            }

            return false;
        }

        public static List<String> GetAddedData(String query, SqlConnection connect)
        {
            SqlCommand connectFirstTime = new(query, connect);
            List<String> existData = new(1);

            connect.Open();

            using (SqlDataReader reader = connectFirstTime.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    connect.Close();

                    return existData;
                }

                while (reader.Read())
                {
                    existData.Add(reader.GetString(0));
                }
            }

            connect.Close();
            return existData;
        }

        public static List<(Int32 RecordId, String Name)> GetLessonsNamesData(String query, SqlConnection connect)
        {
            SqlCommand connectFirstTime = new(query, connect);
            List<(Int32 RecordId, String Name)> existData = new(1);

            connect.Open();

            using (SqlDataReader reader = connectFirstTime.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    connect.Close();

                    return existData;
                }

                while (reader.Read())
                {
                    Int32 id = reader.GetInt32(0);
                    String name = reader.GetString(1);

                    existData.Add(new(id, name));
                }
            }

            connect.Close();
            return existData;
        }

        public static List<(Int32 RecordId, Int32 BranchId, String Name)> GetGroupsData(String query, SqlConnection connect)
        {
            SqlCommand connectFirstTime = new(query, connect);
            List<(Int32 RecordId, Int32 BranchId, String Name)> existData = new(1);

            connect.Open();

            using (SqlDataReader reader = connectFirstTime.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    connect.Close();

                    return existData;
                }

                while (reader.Read())
                {
                    Int32 id = reader.GetInt32(0);
                    Int32 branchId = reader.GetInt32(1);
                    String name = reader.GetString(2);

                    existData.Add(new(id, branchId, name));
                }
            }

            connect.Close();
            return existData;
        }

        public static List<(Int32 TeacherId, String Name, String Surname, String MiddleName)> GetTeachersData(String query, SqlConnection connect)
        {
            SqlCommand connectFirstTime = new(query, connect);
            List<(Int32 TeacherId, String Name, String Surname, String MiddleName)> existData = new(1);

            connect.Open();

            using (SqlDataReader reader = connectFirstTime.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    connect.Close();

                    return existData;
                }

                while (reader.Read())
                {
                    Int32 id = reader.GetInt32(0);
                    String name = reader.GetString(1);
                    String surname = reader.GetString(2);
                    String middleName = reader.GetString(3);

                    existData.Add(new(id, name, surname, middleName));
                }
            }

            connect.Close();
            return existData;
        }

        public static Boolean CheckToSameCriterias(String value1, String value2)
        {
            if (value1 == null || value2 == null)
            {
                return true;
            }

            value1 = value1.ToLower();
            value1 = RemoveStrings(value1, ".", ",", " ", "-", "_");

            value2 = value2.ToLower();
            value2 = RemoveStrings(value2, ".", ",", " ", "-", "_");

            return value1.Equals(value2);
        }

        public static String CheckLessonNameToTypos(String name)
        {
            //Обрабатываем частные случаи (Предметы):
            switch (name)
            {
                case "Экономика организации":
                    return "Экономика орг";

                case "Русский язык":
                    return "Русский";

                case "Числ методы":
                    return "Числен методы";

                case "Числе методы":
                    return "Числен методы";

                case "Прикл эл-ка":
                    return "Прикладная эл-ка";

                case "Прикл. эл-ка":
                    goto case "Прикл эл-ка";

                case "Прикл. эл.":
                    goto case "Прикл эл-ка";

                case "МДК 0101":
                    return "МДК 01.01";

                case "МДК 0102":
                    return "МДК 01.02";

                default:
                    return name;
            }
        }

        public static String CheckTeacherNameToTypos(String teacher)
        {
            //Обрабатываем частные случаи (Преподаватели):
            switch (teacher)
            {
                case "Дуйсенов И.А.":
                    // Искандер Дусейнов Анварович.
                    return "Дусейнов И.А.";

                case "Зсасыпкин К.Н.":
                    // Константин Засыпкин Николаевич.
                    return "Засыпкин К.Н.";

                case "Хисамутдинов Р. М.":
                    // Резида Хисамутдинова Махмутовна.
                    return "Хисамутдинова Р. М.";

                case "Юмагулов Г. К.":
                    // Гульназ Юмагулова Камиловна.
                    return "Юмагулова Г. К.";

                default:
                    return teacher;
            }
        }

        public static String RemoveStrings(String value, params String[] stringsToRemove)
        {
            foreach (String str in stringsToRemove)
            {
                value = value.Replace(str, String.Empty);
            }

            return value;
        }
    }
}
