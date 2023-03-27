using System;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace UksivtScheduler_PC.Other
{
    public partial class Program
    {
        public static void InsertWeekSchedulesData()
        {
            for (int i = 0; i < 5; i++)
            {
                List<(Int32 Id, Int32 GroupId, Int32 DayId)> baseJoinedValues = GetBaseDataToWeekSchedule(i);
                baseJoinedValues = baseJoinedValues.OrderBy(element => element.GroupId).ThenBy(element => element.DayId).ToList();
                var finalJoinedValues = baseJoinedValues.GroupBy(element => element.GroupId).ToList();

                foreach (var item in finalJoinedValues)
                {
                    Int32 weekScheduleId = InsertWeekSchedule(item.Key);
                    
                    foreach (var daySchedule in item.ToList())
                    {
                        InsertTetherBetweenWeekAndDay(weekScheduleId, daySchedule.Id);
                    }
                }
            }
        }

        public static List<(Int32 Id, Int32 GroupId, Int32 DayId)> GetBaseDataToWeekSchedule(Int32 branch)
        {
            SqlCommand command = new(@$"SELECT Day_Schedule.Day_Schedule_Id,
                                               Group_Id,
                                               Id_Of_Day
                                        FROM Day_Schedule
                                            JOIN Groups
                                                ON Day_Schedule.Group_Name__Temp = Groups.Group_Name
                                            JOIN Branch_Category
                                                ON Group_Branch_Id = Branch_Id
                                            JOIN Days_Of_Week
                                                ON Days_Of_Week.Id_Of_Day = Day_Id
                                        WHERE Branch_Id = {branch};", connection);
            List<(Int32 Id, Int32 GroupId, Int32 DayId)> toReturn = new();
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Int32 id = reader.GetInt32(0);
                    Int32 groupId = reader.GetInt32(1);
                    Int32 dayId = reader.GetInt32(2);

                    toReturn.Add(new(id, groupId, dayId));
                }
            }

            connection.Close();
            return toReturn;
        }

        public static Int32 InsertWeekSchedule(Int32 groupId)
        {
            #region Подобласть: Вставка значения.
            SqlCommand command = new($"INSERT INTO Week_Schedule(Group_Id) VALUES({groupId});", connection);
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
            #endregion

            #region Подобласть: Получение индекса.
            Int32 index = default;
            command = new("SELECT MAX(Week_Schedule_Id) FROM Week_Schedule;", connection);
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    index = reader.GetInt32(0);
                }
            }

            connection.Close();
            #endregion

            return index;
        }

        public static void InsertTetherBetweenWeekAndDay(Int32 weekScheduleId, Int32 daySchedule)
        {
            SqlCommand command = new("INSERT INTO Day_Schedule_To_Week_Schedules(Week_Schedule_Ids, Day_Schedule_Ids) " +
            $"VALUES({weekScheduleId}, {daySchedule});", connection);
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
