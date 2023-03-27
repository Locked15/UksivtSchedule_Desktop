using System;
using System.Linq;
using System.Collections.Generic;
using UksivtScheduler_PC.Classes.ScheduleElements;
using Bool = System.Boolean;

namespace UksivtScheduler_PC.Classes.ScheduleAPI
{
    /// <summary>
    /// Класс, содержащий логику для получения замен из API.
    /// </summary>
    public class ChangesOfDay
    {
        #region Область: Свойства.
        /// <summary>
        /// Свойство, отвечающее за то, что элемент с заменами на указанный день найден.
        /// <br/>
        /// Это нужно, чтобы можно было выводить разные сообщения, в зависимости от того,
        /// не найдены ли замены для текущей даты вообще, или не найдены только для указанной группы.
        /// </summary>
        public Bool ChangesFound { get; set; }

        /// <summary>
        /// Свойство, содержащее значение, отвечающее за то, на весь ли день замены или нет.
        /// </summary>
        public Bool AbsoluteChanges { get; set; }

        /// <summary>
        /// Свойство, отвечающее за дату, на которую предназначены замены.
        /// </summary>
        public DateTime? ChangesDate { get; set; }

        /// <summary>
        /// Свойство, содержащее список с новыми парами.
        /// </summary>
        public List<Lesson> NewLessons { get; set; }
        #endregion

        #region Область: Статические свойства.
        /// <summary>
        /// Статическое свойство, содержащее пустые замены.
        /// </summary>
        public static ChangesOfDay DefaultChanges { get; private set; }
        #endregion

        #region Область: Конструкторы класса.
        /// <summary>
        /// Конструктор класса по умолчанию.
        /// <br/>
        /// Присваивает "дефолтные" значения для свойств.
        /// </summary>
        public ChangesOfDay()
        {
            ChangesFound = false;
            AbsoluteChanges = false;
            NewLessons = Enumerable.Empty<Lesson>().ToList();
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="absoluteChanges">Замены на весь день?</param>
        /// <param name="newLessons">Список с новыми парами.</param>
        public ChangesOfDay(Bool absoluteChanges, List<Lesson> newLessons)
        {
            AbsoluteChanges = absoluteChanges;
            NewLessons = newLessons;
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="changesFound">Найдены ли замены.</param>
        /// <param name="absoluteChanges">Замены на весь день?</param>
        /// <param name="newLessons">Список с новыми парами.</param>
        public ChangesOfDay(Bool changesFound, Bool absoluteChanges, List<Lesson> newLessons)
        {
            ChangesFound = changesFound;
            AbsoluteChanges = absoluteChanges;
            NewLessons = newLessons;
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="changesFound">Найдены ли замены.</param>
        /// <param name="absoluteChanges">Замены на весь день?</param>
        /// <param name="changesDate">Дата, на которую предназначены замены.</param>
        /// <param name="newLessons">Список с новыми парами.</param>
        public ChangesOfDay(Bool changesFound, Bool absoluteChanges, DateTime? changesDate, List<Lesson> newLessons)
        {
            ChangesFound = changesFound;
            AbsoluteChanges = absoluteChanges;
            ChangesDate = changesDate;
            NewLessons = newLessons;
        }

        /// <summary>
        /// Статический конструктор класса.
        /// </summary>
        static ChangesOfDay()
        {
            DefaultChanges = new ChangesOfDay();
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод для сравнения двух объектов.
        /// </summary>
        /// <param name="obj">Второй объект для сравнения.</param>
        /// <returns>Результат сравнения.</returns>
        public Bool Equals(ChangesOfDay obj)
        {
            if (AbsoluteChanges == obj.AbsoluteChanges)
            {
                if (NewLessons.Count == obj.NewLessons.Count)
                {
                    for (int i = 0; i < NewLessons.Count; i++)
                    {
                        if (NewLessons[i] != obj.NewLessons[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
