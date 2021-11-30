using System;
using System.Linq;
using System.Collections.Generic;
using UksivtScheduler_PC.Classes.SiteParser;

/// <summary>
/// Область с классом расширений.
/// </summary>
namespace UksivtScheduler_PC.Classes
{
    /// <summary>
    /// Класс расширений.
    /// </summary>
    public static class Extensions
    {
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

    }
}
