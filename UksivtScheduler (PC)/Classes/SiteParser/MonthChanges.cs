using System;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// Область с классом, представляющим сущность замен за месяц.
/// </summary>
namespace UksivtScheduler__PC_.Classes.SiteParser
{
    /// <summary>
    /// Класс замен за месяц.
    /// </summary>
    public class MonthChanges
    {
        #region Область: Свойства.
        /// <summary>
        /// Поле, содержащее название месяца.
        /// </summary>
        public String Month { get; set; }

        /// <summary>
        /// Поле, содержащее список замен на данный месяц.
        /// </summary>
        public List<ChangeElement> Changes { get; set; }
        #endregion

        #region Область: Конструкторы класса.
        /// <summary>
        /// Конструктор класса по умолчанию.
        /// </summary>
        public MonthChanges()
        {

        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="month">Название месяца.</param>
        /// <param name="changes">Список замен.</param>
        public MonthChanges(String month, List<ChangeElement> changes)
        {
            Month = month;
            Changes = changes;
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод, выполняющий поиск по заменам текущего месяца и возвращающем день по указанному числу.
        /// <br/>
        /// Если для указанного дня замены отсутствуют, возвращает <i>"null"</i>.
        /// </summary>
        /// <param name="day">Число, по которому нужно искать замены.</param>
        /// <returns>Замены на указанный день.</returns>
        public ChangeElement TryToFindElementByNumberOfDay(Int32 day)
        {
            foreach (ChangeElement element in Changes)
            {
                if (element.DayOfMonth == day && element.CheckHavingChanges())
                {
                    return element;
                }
            }

            return null;
        }

        /// <summary>
        /// Метод, выполняющий поиск по заменам текущего месяца и возвращающем день с указанным названием дня.
        /// <br/>
        /// Если такой день не найден, будет возвращен <i>"null"</i>.
        /// </summary>
        /// <param name="day">Название дня для поиска замен.</param>
        /// <returns>Элемент замен с указанным днем.</returns>
        public ChangeElement TryToFindElementByNameOfDay(String day)
        {
            Changes.Reverse();

            foreach (ChangeElement element in Changes)
            {
                if (element.DayOfWeek.Equals(day) && element.CheckHavingChanges())
                {
                    return element;
                }
            }

            return null;
        }

        /// <summary>
        /// Метод для получения строкового представления объекта.
        /// <br/>
        /// Реализация прямо из Java!
        /// </summary>
        /// <returns>Строковое представление объекта.</returns>
        public override String ToString()
        {
            StringBuilder toReturn = new("\nНовый месяц: \n" +
            "CurrentMonth = " + Month + ";\n" +
            "Changes:" +
            "\n{");

            foreach (ChangeElement change in Changes)
            {
                toReturn.Append(change.ToString("\t"));
            }

            toReturn.Append("}");

            return toReturn.ToString();
        }
        #endregion
    }
}
