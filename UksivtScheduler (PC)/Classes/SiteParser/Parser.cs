﻿using System;
using System.Linq;
using System.Collections.Generic;
using AngleSharp;
using AngleSharp.Dom;
using UksivtScheduler_PC.Classes.General;
using Bool = System.Boolean;

/// <summary>
/// Область с парсером сайта.
/// </summary>
namespace UksivtScheduler_PC.Classes.SiteParser
{
    /// <summary>
    /// Класс, представляющий сущность парсера и его логику.
    /// </summary>
    public class Parser
    {
        #region Область: Поля.
        /// <summary>
        /// Внутреннее поле, содержащее всю Web-страницу с заменами.
        /// </summary>
        private Document webPage;
        #endregion

        #region Область: Константы.
        /// <summary>
        /// Внутренняя константа, содержащая CSS-селектор для парса страницы.
        /// </summary>
        private const String selector = "section > div > div > div > div > div > div" +
        " > div > div > div > div > div > div > div > div > div > div > div";

        /// <summary>
        /// Внутренняя константа, содержащая путь к странице с заменами.
        /// </summary>
        private const String webPagePath = "https://www.uksivt.ru/zameny";

        /// <summary>
        /// Внутренняя константа, содержащая неразрывный пробел.
        /// </summary>
        private const String NonBreakSpace = "\u00A0";
        #endregion

        #region Область: Конструктор класса.
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <exception cref="Exception">Общее исключение (отсутствие подключения к сети).</exception>
        public Parser()
        {
            webPage = (Document)BrowsingContext.New(Configuration.Default.WithDefaultLoader()).OpenAsync(webPagePath).Result;
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод для получения списка возможных дней для просмотра замен.
        /// <br/>
        /// <strong>Выполнение занимает много времени, вызов стоит выполнять асинхронно.</strong>
        /// </summary>
        /// <param name="limit">Количество месяцев, которые нужно получить.</param>
        /// <returns>Список с доступными заменами по месяцам.</returns>
        /// <exception cref="GeneralParseException">Общее исключение парса страницы.</exception>
        public List<MonthChanges> ParseAvailableNodes(Int32 limit = 2)
        {
            #region Подобласть: Переменные считывания доступных замен.
            Int32 i = 0;
            Int32 monthCounter = 0;
            Int32 currentYear = DateTime.Now.Year;
            String currentMonth = "Январь";
            List<ChangeElement> changes = new(30);
            List<MonthChanges> monthChanges = new(2);
            #endregion

            #region Подобласть: Переменные парса веб-страницы.
            IElement? generalChange = webPage.QuerySelector(selector);
            List<IElement>? listOfChanges = generalChange?.Children.ToList();
            #endregion

            //Совершаем проверку на возможные ошибки:
            if (generalChange == null || listOfChanges == null)
            {
                throw new GeneralParseException("В процессе обработки страницы произошла ошибка.");
            }

            //Если же ошибок получения данных нет, начинаем парсить страницу:
            foreach (IElement? element in listOfChanges)
            {
                String text = element.Text();
                String nodeName = element.NodeName.ToLower();

                //Обработка отступов с неразрывными пробелами:
                if (nodeName.Equals("p") && !text.Equals(NonBreakSpace))
                {
                    //В первой итерации программа также зайдет сюда, обрабатываем этот случай:
                    if (changes.Any())
                    {
                        monthChanges.Add(new MonthChanges(currentMonth, changes));
                    }

                    changes = new(30);
                    currentMonth = text.Substring(0, text.LastIndexOf(' ')).Replace(NonBreakSpace, "");
                    currentYear = Int32.Parse(text.Substring(text.LastIndexOf(' ')).Replace(NonBreakSpace, ""));

                    i = 1;
                }

                //Если мы встречаем тег "table", то мы дошли до таблицы с заменами на какой-либо месяц.
                else if (nodeName.Equals("table") && monthCounter < limit)
                {
                    /* Первым тегом таблицы всегда идет "<thead>", определяющий заголовок, ...
                       ... а следом за ним — "<tbody>", определяющий тело таблицы. Он нам и нужен. */
                    IElement tableBody = element.Children[1];
                    List<IElement> tableRows = tableBody.Children.ToList();

                    for (int j = 0; j < tableRows.Count; j++)
                    {
                        Int32 dayCounter = 0;
                        Bool firstIteration = true;
                        IElement currentRow = tableRows[j];

                        //В первой строке содержатся ненужные значения, пропускаем:
                        if (j == 0)
                        {
                            continue;
                        }

                        //В иных случаях начинаем итерировать ячейки таблицы:
                        foreach (IElement tableCell in currentRow.Children)
                        {
                            String cellText = tableCell.Text();

                            /* Первая ячейка, видимо для отступа, содержит неразрывный пробел, так что ...
                               ... пропускаем такую итерацию.
                               Кроме того, если 1 число месяца выпадает не на понедельник, то ...
                               ... некоторое количество ячеек также будет пустым.                          */
                            if (cellText.Equals(NonBreakSpace))
                            {
                                if (!firstIteration)
                                {
                                    ++dayCounter;
                                }

                                firstIteration = false;
                                continue;
                            }

                            //В некоторых ячейках (дни без замен) нет содержимого, так что учитываем это.
                            if (tableCell.Children.Length < 1)
                            {
                                changes.Add(new ChangeElement(new DateTime(currentYear, currentMonth.GetMonthNumber(), i), 
                                dayCounter.GetDayByIndex(), null));
                            }

                            //В ином случае замены есть и нам нужно получить дочерний элемент.
                            else
                            {
                                IElement? link = tableCell.FirstElementChild;

                                //На всякий случай обрабатываем возможную ошибку с получением атрибута:
                                changes.Add(new ChangeElement(new DateTime(currentYear, currentMonth.GetMonthNumber(), i), 
                                dayCounter.GetDayByIndex(), link.GetAttribute("href")));
                            }

                            ++i;
                            ++dayCounter;
                        }
                    }

                    ++monthCounter;
                }
            }

            return monthChanges;
        }
        #endregion
    }
}
