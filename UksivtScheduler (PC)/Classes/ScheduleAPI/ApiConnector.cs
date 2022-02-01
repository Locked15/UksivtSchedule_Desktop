using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace UksivtScheduler_PC.Classes.ScheduleAPI
{
    public class ApiConnector
    {
        #region Область: Поля.
        /// <summary>
        /// Поле, содержащее индекс нужного дня.
        /// </summary>
        public Int32 DayInd;

        /// <summary>
        /// Поле, содержащее название группы.
        /// </summary>
        public String GroupName;
        #endregion

        #region Область: Статические поля.
        /// <summary>
        /// Статическое поле, содержащее базовый URL сайта с API.
        /// </summary>
        private static readonly String baseUrl;

        /// <summary>
        /// Статическое поле, содержащее URL путь к контроллерам по дням.
        /// </summary>
        private static readonly String pathToDay;

        /// <summary>
        /// Статическое поле, содержащее URL путь к контроллеру замен.
        /// </summary>
        private static readonly String changesController;

        /// <summary>
        /// Статическое поле, содержащее параметр индекса дня.
        /// </summary>
        private static readonly String daySelector;

        /// <summary>
        /// Статическое поле, содержащее параметр названия группы.
        /// </summary>
        private static readonly String groupSelector;
        #endregion

        #region Область: Конструкторы класса.
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="day">Индекс нужного дня.</param>
        /// <param name="group">Название нужной группы.</param>
        public ApiConnector(Int32 day, String group)
        {
            DayInd = day;
            GroupName = group;
        }

        /// <summary>
        /// Статический конструктор класса.
        /// </summary>
        static ApiConnector()
        {
            //region Подобласть: Инициализация "baseUrl'.
            baseUrl = "http://uksivtscheduleapi.azurewebsites.net/";
            //endregion

            //region Подобласть: Инициализация "pathToDay".
            pathToDay = "/api/day/";
            //endregion

            //region Подобласть: Инициализация "changesController".
            changesController = "changesday";
            //endregion

            //region Подобласть: Инициализация "daySelector".
            daySelector = "dayIndex=";
            //endregion

            //region Подобласть: Инициализация "groupSelector".
            groupSelector = "groupName=";
            //endregion
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод для получения замен из API.
        /// </summary>
        /// <returns>Объект, содержащий данные о заменах на день.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public ChangesOfDay GetChanges()
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(String.Format("{0}{1}{2}", baseUrl, pathToDay, changesController))
            };

            HttpResponseMessage message = client.GetAsync(String.Format("?{0}{1}&{2}{3}",
            daySelector, DayInd, groupSelector, GroupName)).Result;

            if (message.IsSuccessStatusCode)
            {
                String jsonValue = message.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<ChangesOfDay>(jsonValue);
            }

            return ChangesOfDay.DefaultChanges;
        }
    }
    #endregion
}
