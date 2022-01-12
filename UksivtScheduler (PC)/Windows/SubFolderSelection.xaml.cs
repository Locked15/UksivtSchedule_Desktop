using System;
using System.Windows;
using System.Collections.Generic;
using UksivtScheduler_PC.Controls;
using UksivtScheduler_PC.Listeners;
using UksivtScheduler_PC.Classes.General;
using Bool = System.Boolean;

namespace UksivtScheduler_PC.Windows
{
    /// <summary>
    /// Логика взаимодействия для SubFolderSelection.xaml.
    /// </summary>
    public partial class SubFolderSelection : Window, IListItemClickListener
    {
        #region Область: Поля.
        /// <summary>
        /// Поле, содержащее строку с выбранным отделением.
        /// </summary>
        private String course;

        /// <summary>
        /// Поле, содержащее логическую переменную, отвечающую за тип закрытия окна.
        /// </summary>
        private Bool goBack;

        /// <summary>
        /// Поле, содержащее список с названиями подпапок-принадлежностей.
        /// </summary>
        private List<String> subFolders = new(1);
        #endregion

        #region Область: Свойства.
        /// <summary>
        /// Свойство, содержащее родительское окно данного окна.
        /// </summary>
        public new MainWindow Parent { get; init; }
        #endregion

        #region Область: Конструкторы класса.
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public SubFolderSelection(String course, MainWindow parent)
        {
            this.course = course;
            Parent = parent;

            InitializeComponent();

            GetData();
        }
        #endregion

        #region Область: События.
        /// <summary>
        /// Событие, происходящее при нажатии на "SubFolderSelection_GoBack".
        /// <br/>
        /// Перемещает пользователя на предыдущее окно.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void GoBackClick(Object sender, EventArgs e)
        {
            goBack = true;
           
            Parent.Show();
            Close();
        }

        /// <summary>
        /// Событие, происходящее при нажатии на элемент списка с принадлежностями.
        /// <br/>
        /// Реализация интерфейса.
        /// </summary>
        /// <param name="name">Название выбранной принадлежности.</param>
        public void ItemIsClicked(String name)
        {
            GroupSelector newWindow = new(course, name, this);

            newWindow.Show();
            Hide();
        }

        /// <summary>
        /// Событие, происходящее при закрытии окна.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void WindowIsClosed(Object sender, EventArgs e)
        {
            if (!goBack)
            {
                Application.Current.Shutdown();
            }
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод для получения списка возможных принадлежностей выбранного отделения.
        /// </summary>
        private void GetData()
        {
            subFolders = Helper.GetSubFolders(course);

            InsertDataToList();
        }

        /// <summary>
        /// Метод для вставки полученных принадлежностей в список.
        /// </summary>
        private void InsertDataToList()
        {
            for (int i = 0; i < subFolders.Count; i++)
            {
                GroupItem item;

                //Если остается ещё, как минимум, 3 элемента:
                if (i + 2 < subFolders.Count)
                {
                    item = new(subFolders[i], subFolders[++i], subFolders[++i], this);
                }

                //Если остается ещё, как минимум, 2 элемента:
                else if (i + 1 < subFolders.Count)
                {
                    item = new(subFolders[i], subFolders[++i], this);
                }

                //Если остается 1 элемент:
                else
                {
                    item = new(subFolders[i], this);
                }

                SubFolderSelector_FoldersList.Items.Add(item);
            }
        }
        #endregion
    }
}
