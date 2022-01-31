using System;
using System.Windows;
using System.Threading;
using System.Windows.Media;
using System.Threading.Tasks;
using UksivtScheduler_PC.Controls;
using UksivtScheduler_PC.Classes.General;
using UksivtScheduler_PC.Classes.ScheduleAPI;
using UksivtScheduler_PC.Classes.ScheduleElements;
using Bool = System.Boolean;

/// <summary>
/// Область кода с окном вывода расписания.
/// </summary>
namespace UksivtScheduler_PC.Windows
{
    /// <summary>
    /// Логика взаимодействия для FinalSchedule.xaml.
    /// </summary>
    public partial class FinalSchedule : Window
    {
        #region Область: Поля.
        /// <summary>
        /// Поле, содержащее оригинальное расписание.
        /// </summary>
        private DaySchedule originalSchedule;

        /// <summary>
        /// Поле, содержащее расписание с заменами.
        /// </summary>
        private DaySchedule scheduleWithChanges;

        /// <summary>
        /// Поле, содержащее родительское окно для данного окна.
        /// </summary>
        private DaySelector parent;

        /// <summary>
        /// Поле, содержащее значение, определяющее субъект, вызывающий закрытие окна.
        /// </summary>
        private Bool returnBack = false;

        /// <summary>
        /// Поле, содержащее флаг остановки субпотока.
        /// </summary>
        private Bool subQueryStopped = false;
        #endregion

        #region Область: Конструктор.
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="prefix">Префикс группы.</param>
        /// <param name="subFolder">Принадлежность группы.</param>
        /// <param name="group">Название группы.</param>
        /// <param name="day">День для получения расписания.</param>
        /// <param name="parent">Родительское окно.</param>
        public FinalSchedule(String prefix, String subFolder, String group, String day, DaySelector parent)
        {
            this.parent = parent;

            InitializeComponent();
            InitizlizeFields(prefix, subFolder, group, day);
        }
        #endregion

        #region Область: События.
        /// <summary>
        /// Событие, происходящее при нажатии на "Schedule_GoBack'.
        /// <br/>
        /// Возвращает пользователя на предыдущее окно.
        /// </summary>
        /// <param name="sender">Элемент, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void GoBackClick(Object sender, EventArgs e)
        {
            returnBack = true;

            parent.Show();
            Close();
        }

        /// <summary>
        /// Событие, происходящее при закрытии окна.
        /// <br/>
        /// Нужно для освобождения памяти.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_Closed(Object sender, EventArgs e)
        {
            subQueryStopped = true;

            if (!returnBack)
            {
                Application.Current.Shutdown();
            }
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод для инициализации полей.
        /// </summary>
        /// <param name="prefix">Префикс группы.</param>
        /// <param name="subFolder">Принадлежность группы.</param>
        /// <param name="group">Название группы.</param>
        /// <param name="day">День для получения расписания.</param>
        private void InitizlizeFields(String prefix, String subFolder, String group, String day)
        {
            //Создаем экземпляр диалогового окна для вывода информации:
            MessageWindow message = new MessageWindow();

            //Получаем оригинальное расписание:
            originalSchedule = Helper.GetWeekSchedule(prefix, subFolder, group).Days[day.GetIndexByDay()];

            //Устанавливаем заголовок и тайтл:
            Title += " — " + day;
            Schedule_Header.Content += $" {day}.";

            //Операции занимают много времени, выносим в отдельный поток:
            new Thread(async () =>
            {
                originalSchedule.Lessons.RemoveAll(lesson => !lesson.CheckHaveValue());
                InsertData(originalSchedule);

                await Task.Run(() =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        message = new MessageWindow("Уведомление", "Начато получение данных.\nЭто может занять некоторое время.");
                        message.ShowDialog();
                    });
                });

                try
                {
                    ChangesOfDay changes = new ApiConnector(day.GetIndexByDay(), group).GetChanges();

                    if (!changes.Equals(ChangesOfDay.DefaultChanges) && !subQueryStopped)
                    {
                        scheduleWithChanges = originalSchedule.MergeChanges(changes.NewLessons, changes.AbsoluteChanges);
                        InsertData(scheduleWithChanges);

                        Dispatcher.Invoke(() =>
                        {
                            message.Close();

                            message = new MessageWindow("Уведомление", "Получены данные замен.");
                            message.ShowDialog();
                        });
                    }

                    else if (!subQueryStopped)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            message.Close();

                            message = new MessageWindow("Уведомление", "Данные замен не обнаружены.\nОтображено оригинальное расписание.");
                            message.ShowDialog();
                        });
                    }
                }

                catch (System.Net.Http.HttpRequestException)
                {
                    message.Close();

                    message = new MessageWindow("Ошибка", "ошибка при получении замен.");
                    message.ShowDialog();
                }
            }).Start();
        }

        /// <summary>
        /// Метод для вставки данных о расписании в элементы окна.
        /// </summary>
        /// <param name="schedule">Расписание для вставки в таблицу.</param>
        private void InsertData(DaySchedule schedule)
        {
            //Для очистки списка элементов в другом потоке тоже нужен "Dispatcher".
            Dispatcher.Invoke(() =>
            {
                Schedule_LessonsList.Items.Clear();
            });

            /* Операция выполняется в отдельном потоке, ...
               ... поэтому операции с UI проводятся через "Dispatcher". */
            foreach (Lesson lesson in schedule.Lessons)
            {
                Int32 number = lesson.Number;
                String name = lesson.Name;
                String teacher = lesson.Teacher;
                String place = lesson.Place;
                Bool changed = lesson.LessonChanged;

                //Вызываем "Dispathcer":
                Dispatcher.Invoke(() =>
                {
                    //changed ? Color.Wheat : Color.White;
                    SolidColorBrush color = new(changed ? Color.FromRgb(245, 222, 179) : Color.FromRgb(255, 255, 255));
                    ScheduleElement element = new(number, name, teacher, place);

                    element.Background = color;

                    Schedule_LessonsList.Items.Add(element);
                });
            }
        }
        #endregion
    }
}
