﻿using System;
using System.Linq;
using System.Collections.Generic;
using NPOI.XWPF.UserModel;
using UksivtScheduler_PC.Classes.SiteParser;
using Bool = System.Boolean;

/// <summary>
/// Область с классом расширений.
/// </summary>
namespace UksivtScheduler_PC.Classes.General
{
    /// <summary>
    /// Класс расширений.
    /// </summary>
    public static class Extensions
    {
        #region Область: Делегаты.
        /// <summary>
        /// Делегат, инкапсулирующий метод, нужный для проверки содержания подстроки в строке.
        /// </summary>
        /// <param name="value">Значение, наличие которого нужно проверить.</param>
        /// <returns>Логическое значение, отвечающее за наличие подстроки в строке.</returns>
        public delegate Bool Check(String value);
        #endregion

        #region Область: Методы расширения, связанные с датами.
        /// <summary>
        /// Метод расширения, позволяющий получить день по указанному индексу.
        /// </summary>
        /// <param name="index">Индекс дня (0 : 6).</param>
        /// <returns>День, соответствующий данному индексу.</returns>
        /// <exception cref="IndexOutOfRangeException">Введен некорректный индекс.</exception>
        public static String GetDayByIndex(this Int32 index)
        {
            switch (index)
            {
                case 0:
                    return "Понедельник";

                case 1:
                    return "Вторник";

                case 2:
                    return "Среда";

                case 3:
                    return "Четверг";

                case 4:
                    return "Пятница";

                case 5:
                    return "Суббота";

                case 6:
                    return "Воскресенье";

                default:
                    throw new IndexOutOfRangeException($"Введен некорректный индекс ({index}).");
            }
        }

        /// <summary>
        /// Метод расширения, позволяющий получить индекс по названию дня.
        /// </summary>
        /// <param name="day">Название дня.</param>
        /// <returns>День, соответствующий данному индексу.</returns>
        /// <exception cref="ArgumentException">Введен некорректный день.</exception>
        public static Int32 GetIndexByDay(this String day)
        {
            day = day.ToLower();

            switch (day)
            {
                case "понедельник":
                    return 0;

                case "вторник":
                    return 1;

                case "среда":
                    return 2;

                case "четверг":
                    return 3;

                case "пятница":
                    return 4;

                case "суббота":
                    return 5;

                case "воскресенье":
                    return 6;

                default:
                    throw new ArgumentException($"Введен некорректный день ({day}).");
            }
        }

        /// <summary>
        /// Метод расширения, позволяющий получить номер месяца по его названию.
        /// </summary>
        /// <param name="monthName">Название месяца для получения его номера.</param>
        /// <returns>Номер месяца.</returns>
        public static Int32 GetMonthNumber(this String monthName)
        {
            monthName = monthName.ToLower();

            switch (monthName)
            {
                case "январь":
                    return 1;

                case "февраль":
                    return 2;

                case "март":
                    return 3;

                case "апрель":
                    return 4;

                case "май":
                    return 5;

                case "июнь":
                    return 6;

                case "июль":
                    return 7;

                case "август":
                    return 8;

                case "сентябрь":
                    return 9;

                case "октябрь":
                    return 10;

                case "ноябрь":
                    return 11;

                case "декабрь":
                    return 12;

                default:
                    throw new ArgumentException("Отправленное значение некорректно.");
            }
        }
        
        /// <summary>
        /// Метод расширения, позволяющий получить дату начала недели.
        /// </summary>
        /// <param name="time">Дата, для которой нужно получить дату начала недели.</param>
        /// <returns>Дата начала недели.</returns>
        public static DateTime GetStartOfWeek(this DateTime time)
        {
            while (time.DayOfWeek != DayOfWeek.Monday)
            {
                time = time.AddDays(-1);
            }

            return time;
        }

        /// <summary>
        /// Метод расширения, позволяющий получить дату конца недели.
        /// </summary>
        /// <param name="time">Дата, для которой нужно получить дату конца недели.</param>
        /// <returns>Дата конца недели.</returns>
        public static DateTime GetEndOfWeek(this DateTime time)
        {
            while (time.DayOfWeek != DayOfWeek.Sunday)
            {
                time = time.AddDays(1);
            }

            return time;
        }
        #endregion

        #region Область: Методы расширений, связанные с расписанием.
        /// <summary>
        /// Метод расширения для получения названия подпапки ассетов из названия группы.
        /// </summary>
        /// <param name="groupName">Название группы.</param>
        /// <returns>Название подпапки (префикс).</returns>
        public static String GetPrefixFromName(this String groupName)
        {
            groupName = groupName.ToLower();
            String yearEnd;

            //Если сейчас второй семестр, то первый курс поступал в прошлом году.
            if (DateTime.Now.Month <= 6)
            {
                yearEnd = DateTime.Now.AddYears(-1).Year.ToString()[2..];
            }

            //В ином случае, они поступили в этом году.
            else
            {
                yearEnd = DateTime.Now.Year.ToString()[2..];
            }
 
            //Общеобразовательное.
            if (groupName.Contains(yearEnd) && (!groupName.Contains("укск")))
            {
                return "General";
            }

            //Для красоты выделим прочие блоки в отдельную иерархию "if ... else if ... else".
            else
            {
                //Для краткости записи определяем делегат:
                Check check = (String val) => groupName.Contains(val);

                //Экономика и ЗИО.
                if (check("зио") || check("э") || check("уэ") || check("ул"))
                {
                    return "Economy";
                }

                //Право.
                else if (check("пд") || check("по") || check("пса"))
                {
                    return "Law";
                }

                //Информатика и Программирование.
                else if (check("п") || check("ис") || check("и") || check("веб") || 
                check("оиб"))
                {
                    return "Programming";
                }

                //Вычислительная техника.
                else if (check("кск") || check("са") || check("укск"))
                {
                    return "Technical";
                }

                //Выброс исключения.
                else
                {
                    throw new ArgumentException("Указанная группа не обнаружена в системе.");
                }
            }
        }

        /// <summary>
        /// Метод расширения, дублирующий функционал такого же метода у MonthChange, но применяется к списку.
        /// <br/>
        /// Если такой день не найден, будет возвращен <i>"null"</i>.
        /// </summary>
        /// <param name="day">Название дня для поиска.</param>
        /// <returns>Элемент замен с указанным днем.</returns>
        public static ChangeElement TryToFindElementByNameOfDayWithoutPreviousWeeks(this List<MonthChanges> allChanges, String day)
        {
            /* Так как в целях совместимости была использована структура "DateTime", а не "DateOnly", ...
               ... то в дело сравнения вмешивается ещё и время, что может нарушить работу.
               Для исправления потенциальных проблем берутся сдвинутые на 1 день границы.                 */
            DateTime start = DateTime.Now.GetStartOfWeek().AddDays(-1);
            DateTime end = DateTime.Now.GetEndOfWeek().AddDays(1);

            //Месяцы идут в порядке убывания (Декабрь -> Ноябрь -> ...).
            foreach (MonthChanges monthChanges in allChanges)
            {
                //А вот дни - наоборот, в порядке возрастания, так что их надо инвертировать.
                List<ChangeElement> changes = monthChanges.Changes.OrderByDescending(change => change.Date).ToList(); 

                foreach (ChangeElement change in changes)
                {
                    //Если текущая дата больше конца таргетированной недели, мы ещё не добрались до нужной даты.
                    if (change.Date > end)
                    {
                        continue;
                    }

                    //А если текущая дата МЕНЬШЕ, чем начало таргетированной недели, мы уже её прошли.
                    else if (change.Date < start)
                    {
                        return null;
                    }

                    if (change.DayOfWeek.Equals(day) && change.CheckHavingChanges())
                    {
                        return change;
                    }
                }
            }

            return null;
        }
        #endregion

        #region Область: Методы расширений, связанные с парсом документа.
        /// <summary>
        /// Внутренний метод, нужный для конвертации нумератора в список.
        /// </summary>
        /// <param name="enumerator">Нумератор параграфов.</param>
        /// <returns>Список, содержащий параграфы.</returns>
        public static List<XWPFParagraph> GetParagraphs(this IEnumerator<XWPFParagraph> enumerator)
        {
            List<XWPFParagraph> paragraphs = new(1);

            while (enumerator.MoveNext())
            {
                paragraphs.Add(enumerator.Current);
            }

            return paragraphs;
        }
        #endregion
    }
}
