using System;
using System.Windows;
using System.Collections.Generic;
using UksivtScheduler_PC.Controls;
using UksivtScheduler_PC.Listeners;
using UksivtScheduler_PC.Classes.General;

namespace UksivtScheduler_PC.Windows
{
    /// <summary>
    /// Логика взаимодействия для GroupSelector.xaml.
    /// </summary>
    public partial class GroupSelector : Window, IGroupItemListener
    {
        #region Область: Поля.
        /// <summary>
        /// Внутреннее поле, содержащее префикс нужной группы.
        /// </summary>
        private String prefix;

        /// <summary>
        /// Внутреннее поле, содержащее родительское окно.
        /// </summary>
        private MainWindow parent;

        /// <summary>
        /// Внутреннее поле, содержащее список с названиями групп.
        /// </summary>
        private List<String> groupNames = new(10);
        #endregion

        #region Область: Конструктор.
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public GroupSelector(String prefix, MainWindow main)
        {
            this.prefix = prefix;
            parent = main;

            InitializeComponent();
            InitializeFields();
        }
        #endregion

        #region Область: События.
        /// <summary>
        /// Событие, происходящее при нажатии на "GroupSelector_GoBack".
        /// <br/>
        /// Возвращает пользователя на предыдущее окно.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void GoBackClick(Object sender, EventArgs e)
        {
            parent.Show();
            Close();
        }

        /// <summary>
        /// Реализация интерфейса.
        /// <br/>
        /// Нужна для получения названия группы из нажатой кнопки.
        /// </summary>
        /// <param name="name">Название группы.</param>
        public void GroupIsClicked(String name)
        {
            MessageBox.Show($"Выбрано: {name}.");
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод для инициализации полей.
        /// </summary>
        private void InitializeFields()
        {
            groupNames = Helper.GetGroups(prefix);

            InsertDataToList();
        }

        /// <summary>
        /// Метод для элементов в список.
        /// </summary>
        private void InsertDataToList()
        {
            for (int i = 0; i < groupNames.Count; i++)
            {
                GroupItem item;

                //Если остается ещё, как минимум, 3 элемента:
                if (i + 2 < groupNames.Count)
                {
                    item = new(groupNames[i], groupNames[++i], groupNames[++i], this);
                }

                //Если остается ещё, как минимум, 2 элемента:
                else if (i + 1 < groupNames.Count)
                {
                    item = new(groupNames[i], groupNames[++i], this);
                }

                //Если остается 1 элемент:
                else
                {
                    item = new(groupNames[i], this);
                }

                GroupSelector_GroupList.Items.Add(item);
            }
        }
        #endregion
    }
}
