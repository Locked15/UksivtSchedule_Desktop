using System;

/// <summary>
/// Область с классом пары.
/// </summary>
namespace UksivtScheduler__PC_.Classes
{
    /// <summary>
    /// Класс, представляющий сущность пары.
    /// </summary>
    public class Lesson
    {
        #region Область: Свойства.
        /// <summary>
        /// Свойство, содержащее номер пары.
        /// </summary>
        public Int32 Number { get; set; }

        /// <summary>
        /// Свойство, содержащее название пары.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Свойство, содержащее имя преподавателя.
        /// </summary>
        public String Teacher { get; set; }

        /// <summary>
        /// Свойство, содержащее место проведения пары.
        /// </summary>
        public String Place { get; set; }
        #endregion

        #region Область: Конструкторы.
        /// <summary>
        /// Конструктор класса по умолчанию.
        /// </summary>
        public Lesson()
        {

        }

        /// <summary>
        /// Конструктор класса для заполнения пустой пары.
        /// </summary>
        /// <param name="number">Номер пары.</param>
        public Lesson(Int32 number)
        {
            Number = number;
            Name = null;
            Teacher = null;
            Place = null;
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="number">Номер пары.</param>
        /// <param name="name">Названия пары.</param>
        /// <param name="teacher">Имя преподавателя.</param>
        /// <param name="place">Место проведения.</param>
        public Lesson(Int32 number, String name, String teacher, String place)
        {
            Number = number;
            Name = name;
            Teacher = teacher;
            Place = place;
        }
        #endregion
    }
}
