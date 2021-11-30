using System;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;
using UksivtScheduler_PC.Classes;
using UksivtScheduler_PC.Classes.SiteParser;

/// <summary>
/// Область с окном выбора направления.
/// </summary>
namespace UksivtScheduler_PC
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

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

        }
        #endregion
    }
}
