using System;
using Bool = System.Boolean;

/// <summary>
/// Область с классом пары.
/// </summary>
namespace UksivtScheduler_PC.Classes.ScheduleElements
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

        /// <summary>
        /// Свойство, содержащее значение, отвечающее за то, была ли заменена пара.
        /// <br/>
        /// Значение по умолчанию: False.
        /// </summary>
        public Bool LessonChanged { get; set; } = false;
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
        public Lesson(Int32 number, String name, String teacher, String place) : this(number)
        {
            Name = name;
            Teacher = teacher;
            Place = place;
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="number">Номер пары.</param>
        /// <param name="name">Название пары.</param>
        /// <param name="teacher">Имя преподавателя.</param>
        /// <param name="place">Место проведения пары.</param>
        /// <param name="lessonChanged">Изменялась ли пара по заменам?</param>
        public Lesson(Int32 number, String name, String teacher, String place, Bool lessonChanged) : this(number, name, teacher, place)
        {
            LessonChanged = lessonChanged;
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод для проверки пары на полноту (наличие самой пары).
        /// </summary>
        /// <returns>Логическое значение, отвечающее за полноту.</returns>
        public Bool CheckHaveValue()
        {
            return Name != null;
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод для сравнения двух объектов.
        /// </summary>
        /// <param name="lesson">Второй объект для сравнения.</param>
        /// <returns>Результат сравнения.</returns>
        public Bool Equals(Lesson lesson)
        {
            return Number == lesson.Number && Name == lesson.Name && 
            Teacher == lesson.Teacher && Place == lesson.Place;
        }
        #endregion
    }
}
