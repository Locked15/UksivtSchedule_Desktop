using System;
using System.IO;
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
        /// Метод для получения списка принадлежностей данного отделения.
        /// </summary>
        /// <param name="course">Выбранное отделение (направление) группы.</param>
        /// <returns>Список с возможными принадлежностями.</returns>
        public static List<String> GetSubFolders(String course)
        {
            return Directory.GetDirectories(PathToAssets + '\\' + course).Select(folder => folder[(folder.LastIndexOf('\\') + 1)..]).ToList();
        }

        /// <summary>
        /// Метод для получения списка групп по указанному направлению.
        /// </summary>
        /// <param name="prefix">Префикс (направление) группы.</param>
        /// <param name="subFolder">Принадлежность (подпапка) группы.</param>
        /// <returns>Список с названиями групп.</returns>
        public static List<String> GetGroups(String prefix, String subFolder)
        {
            List<String> toReturn = Directory.GetFiles(PathToAssets + '\\' + prefix + '\\' + subFolder).ToList();
            
            return toReturn.Select(file => file = Path.GetFileNameWithoutExtension(file)).ToList();
        }

        /// <summary>
        /// Метод для получения расписания группы из ассетов.
        /// </summary>
        /// <param name="prefix">Название подпапки ассетов, где следует искать расписание.</param>
        /// <param name="subFolder">Принадлежность группы.</param>
        /// <param name="groupName">Название группы.</param>
        /// <returns>Оригинальное расписание группы.</returns>
        /// <exception cref="FileNotFoundException">Указанный файл не найден.</exception>
        /// <exception cref="IOException">При прочтении возникла ошибка.</exception>
        public static WeekSchedule GetWeekSchedule(String prefix, String subFolder, String groupName)
        {
            String fullPath = Helper.PathToAssets + '\\' + prefix + '\\' + subFolder + '\\' + groupName + ".json";

            using (StreamReader sr1 = new(fullPath, System.Text.Encoding.Default))
            {
                return JsonConvert.DeserializeObject<WeekSchedule>(sr1.ReadToEnd());
            }
        }
        #endregion
    }
}
