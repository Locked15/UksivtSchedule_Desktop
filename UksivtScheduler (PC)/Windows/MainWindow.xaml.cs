using System;
using System.Windows;
using UksivtScheduler_PC.Windows;

/// <summary>
/// Область с окном выбора направления.
/// </summary>
namespace UksivtScheduler_PC
{
    /// <summary>
    /// Вот и оно... С днем рождения!
    /// <br/><br/>
    /// <strong><i>Uksivt... is this place where you want to be?
    /// <br/>
    /// I just... don't get it.
    /// <br/>
    /// 4 years at uksivt!</i></strong>
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Область: Конструктор класса.
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Область: Обработка нажатий.
        /// <summary>
        /// Событие, происходящее при нажатии на кнопку "Main_Programming".
        /// <br/>
        /// Перенаправляет на окно выбора группы отделения программистов.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void Main_Programming_Click(Object sender, EventArgs e)
        {
            CreateWindow("Programming");
        }

        /// <summary>
        /// Событие, происходящее при нажатии на кнопку "Main_ComputingTech".
        /// <br/>
        /// Перенаправляет на окно выбора группы отделения вычислительной техники.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void Main_ComputingTech_Click(Object sender, EventArgs e)
        {
            CreateWindow("Technical");
        }

        /// <summary>
        /// Событие, происходящее при нажатии на кнопку "Main_Law".
        /// <br/>
        /// Перенаправляет на окно выбора группы отделения права.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void Main_Law_Click(Object sender, EventArgs e)
        {
            CreateWindow("Law");
        }

        /// <summary>
        /// Событие, происходящее при нажатии на кнопку "Main_Economy".
        /// <br/>
        /// Перенаправляет на окно выбора группы отделения экономики и ЗИО.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void Main_Economy_Click(Object sender, EventArgs e)
        {
            CreateWindow("Economy");
        }

        /// <summary>
        /// Событие, происходящее при нажатии на кнопку "Main_General".
        /// <br/>
        /// Перенаправляет на окно выбора группы общеобразовательного отделения.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void Main_General_Click(Object sender, EventArgs e)
        {
            CreateWindow("General");
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Вынесенный в отдельный метод, функционал создания нового окна.
        /// </summary>
        /// <param name="prefix">Префикс группы.</param>
        private void CreateWindow(String prefix)
        {
            GroupSelector newWindow = new GroupSelector(prefix, this);

            newWindow.Show();
            Hide();
        }
        #endregion
    }
}
