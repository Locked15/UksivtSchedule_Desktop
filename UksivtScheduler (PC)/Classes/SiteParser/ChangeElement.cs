using System;
using Bool = System.Boolean;

/// <summary>
/// Область кода с классом, представляющим сущность одной замены.
/// </summary>
namespace UksivtScheduler__PC_.Classes.SiteParser
{
    /// <summary>
    /// Класс, представляющий один элемент с заменами.
    /// </summary>
    public class ChangeElement
    {
        #region Область: Свойства.
        /// <summary>
        /// Свойство, содержащее число месяца, на которое идут замены.
        /// </summary>
        public Int32 DayOfMonth { get; set; }

        /// <summary>
        /// Свойство, содержащее название дня недели, на который идут замены.
        /// </summary>
        public String DayOfWeek { get; set; }

        /// <summary>
        /// Свойство, содержащее ссылку на документ с заменами.
        /// </summary>
        public String LinkToDocument { get; set; }
        #endregion

        #region Область: Конструкторы класса.
        /// <summary>
        /// Конструктор класса по умолчанию.
        /// </summary>
        public ChangeElement()
        {

        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="dayOfMonth">День месяца (число).</param>
        /// <param name="dayOfWeek">День недели.</param>
        /// <param name="linkToDocument">Ссылка на документ с заменами.</param>
        public ChangeElement(Int32 dayOfMonth, String dayOfWeek, String linkToDocument)
        {
            DayOfMonth = dayOfMonth;
            DayOfWeek = dayOfWeek;
            LinkToDocument = linkToDocument;
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод, позволяющий проверить определенный день на наличие замен.
        /// </summary>
        /// <returns>Логическое значение, отвечающее за наличие/отсутствие замен.</returns>
        public Bool CheckHavingChanges()
        {
            return LinkToDocument != null;
        }

        /// <summary>
        /// Собственный метод для получения строкового представления объекта.
        /// </summary>
        /// <param name="append">Строка для дополнительной вставки.</param>
        /// <returns>Строковое представление объекта.</returns>
        public String ToString(String append)
        {
            //Реализация прямиком из Java:
            return "\n" + append + "ChangeElement: \n" +
            append + "DayOfMonth = " + DayOfMonth + ";\n" +
            append + "LinkToDocument = " + LinkToDocument + ";\n" +
            append + "CurrentDay = " + DayOfWeek + ".\n";
        }
        #endregion
    }
}
