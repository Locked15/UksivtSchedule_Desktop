using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using UksivtScheduler_PC.Classes.ScheduleElements;

/// <summary>
/// Область кода с классом-помощником.
/// </summary>
namespace UksivtScheduler_PC.Classes.General
{
    /// <summary>
    /// Класс-помощник, нужный для различных задач.
    /// </summary>
    public static class Helper
    {
        #region Область: Поля.
        /// <summary>
        /// Поле, содержащее путь к ассетам проекта.
        /// </summary>
        public static readonly String PathToAssets;
        #endregion

        #region Область: Константы.
        /// <summary>
        /// Константа, содержащая шаблон для создания ссылки для скачивания файла с Google Drive.
        /// </summary>
        private const String GoogleDriveDownloadLinkTemplate =
        "https://drive.google.com/uc?export=download&id=";
        #endregion

        #region Область: Конструктор.
        /// <summary>
        /// Статический конструктор класса.
        /// </summary>
        static Helper()
        {
            #region Подобласть: Инициализация поля "PathToAssets".
            String pathToProject = Environment.CurrentDirectory;

            for (int i = 0; i < 3; i++)
            {
                pathToProject = pathToProject.Substring(0, pathToProject.LastIndexOf('\\'));
            }

            PathToAssets = pathToProject + "\\Assets";
            #endregion
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод для получения списка групп по указанному направлению.
        /// </summary>
        /// <param name="prefix">Префикс (направление) группы.</param>
        /// <returns>Список с названиями групп.</returns>
        public static List<String> GetGroups(String prefix)
        {
            List<String> toReturn = Directory.GetFiles(PathToAssets + '\\' + prefix).ToList();
            
            return toReturn.Select(file => file = Path.GetFileNameWithoutExtension(file)).ToList();
        }

        /// <summary>
        /// Метод для получения расписания группы из ассетов.
        /// </summary>
        /// <param name="groupName">Название группы.</param>
        /// <returns>Оригинальное расписание группы.</returns>
        /// <exception cref="FileNotFoundException">Указанный файл не найден.</exception>
        /// <exception cref="IOException">При прочтении возникла ошибка.</exception>
        public static WeekSchedule GetWeekSchedule(String groupName)
        {
            String prefix = groupName.GetPrefixFromName();

            return GetWeekSchedule(prefix, groupName);
        }

        /// <summary>
        /// Метод для получения расписания группы из ассетов.
        /// </summary>
        /// <param name="prefix">Название подпапки ассетов, где следует искать расписание.</param>
        /// <param name="groupName">Название группы.</param>
        /// <returns>Оригинальное расписание группы.</returns>
        /// <exception cref="FileNotFoundException">Указанный файл не найден.</exception>
        /// <exception cref="IOException">При прочтении возникла ошибка.</exception>
        public static WeekSchedule GetWeekSchedule(String prefix, String groupName)
        {
            String fullPath = Helper.PathToAssets + '\\' + prefix + '\\' + groupName + ".json";

            using (StreamReader sr1 = new(fullPath, System.Text.Encoding.Default))
            {
                return JsonConvert.DeserializeObject<WeekSchedule>(sr1.ReadToEnd());
            }
        }

        /// <summary>
        /// Метод для получения рабочей ссылки для скачивания файла с заменами.
        /// <br/>
        /// Оригинальная (без обработки) ссылка скачивает поврежденный файл, так что её надо обработать.
        /// </summary>
        /// <param name="originalLink">Оригинальная ссылка на файл.</param>
        /// <returns>Обработанная и пригодная для скачивания ссылка.</returns>
        public static String GetDownloadableFileLink(String originalLink)
        {
            String id = originalLink.Substring(0, originalLink.LastIndexOf('/'));
            id = id.Substring(id.LastIndexOf('/') + 1);

            return GoogleDriveDownloadLinkTemplate + id;
        }

        /// <summary>
        /// Метод для скачивания файла с заменами по обработанной ссылке.
        /// </summary>
        /// <param name="url">Ссылка на скачивание файла.</param>
        /// <returns>Путь к скачанному файлу.</returns>
        /// <exception cref="ArgumentException">Отправленная ссылка была некорректна.</exception>
        public static String DownloadFileFromURL(String url)
        {
            //Для хранения данных будем использовать специальную директорию.
            String destination = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            //Чтобы предотвратить попытки скачать файл по оригинальной ссылке, делаем проверку:
            if (!url.Contains(GoogleDriveDownloadLinkTemplate))
            {
                throw new ArgumentException("Отправленная ссылка некорректна.");
            }

            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(Environment.UserName, "Password");

                client.DownloadFile(url, destination + "\\Changes.docx");
            }

            return destination + "\\Changes.docx";
        }
        #endregion
    }
}
