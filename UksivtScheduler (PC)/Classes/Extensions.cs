using System;

/// <summary>
/// Область с классом расширений.
/// </summary>
namespace UksivtScheduler__PC_.Classes
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
    }
}
