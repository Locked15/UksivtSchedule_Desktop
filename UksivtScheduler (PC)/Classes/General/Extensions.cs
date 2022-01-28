using System;
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
            day = day.GetTranslatedDay();
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
        /// Метод для получения переведенного названия дня.
        /// </summary>
        /// <param name="day">Название дня на другом языке.</param>
        /// <returns>Название дня на русском.</returns>
        public static String GetTranslatedDay(this String day)
        {
            day = day.ToLower();

            switch (day)
            {
                case "monday":
                    return "Понедельник";

                case "tuesday":
                    return "Вторник";

                case "wednesday":
                    return "Среда";

                case "thursday":
                    return "Четверг";

                case "friday":
                    return "Пятница";

                case "saturday":
                    return "Суббота";

                case "sunday":
                    return "Воскресенье";

                default:
                    return day;
            }
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
            String yearEnd;
            groupName = groupName.ToLower();

            #region Подобласть: Вычисление года поступления.
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
            #endregion

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
                check("оиб") || check("бд"))
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
        #endregion
    }
}
