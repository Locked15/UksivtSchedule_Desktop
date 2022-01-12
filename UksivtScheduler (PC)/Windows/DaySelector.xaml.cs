using System;
using System.Windows;
using Bool = System.Boolean;

/// <summary>
/// Область кода с выбором дня.
/// </summary>
namespace UksivtScheduler_PC.Windows
{
    /// <summary>
    /// Логика взаимодействия для DaySelector.xaml.
    /// </summary>
    public partial class DaySelector : Window
    {
        #region Область: Поля.
        /// <summary>
        /// Внутреннее поле, содержащее префикс выбранной группы.
        /// </summary>
        private String prefix;

        /// <summary>
        /// Внутреннее поле, содержащее папку с принадлежностью нужной группы.
        /// </summary>
        private String subFolder;

        /// <summary>
        /// Внутреннее поле, содержащее название группы.
        /// </summary>
        private String groupName;

        /// <summary>
        /// Внутреннее поле, отвечающее за то, что вызвало закрытие окна.
        /// </summary>
        private Bool returnBack = false;
        #endregion

        #region Область: Свойства.
        /// <summary>
        /// Свойство, содержащее родительское окно данного окна.
        /// </summary>
        public new GroupSelector Parent { get; init; }
        #endregion

        #region Область: Конструктор класса.
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="prefix">Префикс группы.</param>
        /// <param name="groupName">Название группы.</param>
        /// <param name="parent">Родительское окно.</param>
        public DaySelector(String prefix, String subFolder, String groupName, GroupSelector parent)
        {
            this.prefix = prefix;
            this.subFolder = subFolder;
            this.groupName = groupName;
            Parent = parent;

            InitializeComponent();
        }
        #endregion

        #region Область: События.
        #region Подобласть: Выбор дня.
        /// <summary>
        /// Событие, происходящее при нажатии на "DaySelector_Monday".
        /// <br/>
        /// Выбирает понедельник.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void MondayClick(Object sender, EventArgs e)
        {
            CreateNewWindow("Понедельник");
        }

        /// <summary>
        /// Событие, происходящее при нажатии на "DaySelector_Tuesday".
        /// <br/>
        /// Выбирает вторник.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void TuesdayClick(Object sender, EventArgs e)
        {
            CreateNewWindow("Вторник");
        }

        /// <summary>
        /// Событие, происходящее при нажатии на "DaySelector_Wednesday".
        /// <br/>
        /// Выбирает среду.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void WednesdayClick(Object sender, EventArgs e)
        {
            CreateNewWindow("Среда");
        }

        /// <summary>
        /// Событие, происходящее при нажатии на "DaySelector_Thursday".
        /// <br/>
        /// Выбирает четверг.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void ThursdayClick(Object sender, EventArgs e)
        {
            CreateNewWindow("Четверг");
        }

        /// <summary>
        /// Событие, происходящее при нажатии на "DaySelector_Friday".
        /// <br/>
        /// Выбирает пятницу.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void FridayClick(Object sender, EventArgs e)
        {
            CreateNewWindow("Пятница");
        }

        /// <summary>
        /// Событие, происходящее при нажатии на "DaySelector_Saturday".
        /// <br/>
        /// Выбирает субботу.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void SaturdayClick(Object sender, EventArgs e)
        {
            CreateNewWindow("Суббота");
        }

        /// <summary>
        /// Событие, происходящее при нажатии на "DaySelector_Sunday".
        /// <br/>
        /// Выбирает воскресенье.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void SundayClick(Object sender, EventArgs e)
        {
            CreateNewWindow("Воскресенье");
        }
        #endregion

        #region Подобласть: Прочие события.
        /// <summary>
        /// Событие, происходящее при нажатии на "DaySelector_GoBack".
        /// <br/>
        /// Возвращает пользователя на предыдущее окно.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void GoBackClick(Object sender, EventArgs e)
        {
            returnBack = true;

            Parent.Show();
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
            if (!returnBack)
            {
                Application.Current.Shutdown();
            }
        }
        #endregion
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод для создания нового окна.
        /// </summary>
        /// <param name="day">День для которого нужно получить расписание.</param>
        private void CreateNewWindow(String day)
        {
            FinalSchedule newWindow = new(prefix, subFolder, groupName, day, this);

            newWindow.Show();
            Hide();
        }
        #endregion
    }
}
