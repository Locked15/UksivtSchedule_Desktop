using System;
using System.Collections.Generic;

/// <summary>
/// Область с классом недельного расписания.
/// </summary>
namespace UksivtScheduler_PC.Classes
{
    /// <summary>
    /// Класс, представляющий сущность Расписания на неделю.
    /// </summary>
    public class WeekSchedule
    {
        #region Область: Свойста.
        /// <summary>
        /// Свойство, содержащее название группы.
        /// </summary>
        public String GroupName { get; set; }

        /// <summary>
        /// Свойство, содержащее расписание группы на неделю.
        /// </summary>
        public List<DaySchedule> Days { get; set; }
        #endregion

        #region Область: Конструкторы.
        /// <summary>
        /// Конструктор класса по умолчанию.
        /// </summary>
        public WeekSchedule()
        {

        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="groupName">Название группы.</param>
        /// <param name="days">Список дней с расписанием.</param>
        public WeekSchedule(String groupName,  List<DaySchedule> days)
        {
            GroupName = groupName;
            Days = days;
        }
        #endregion
    }
}
