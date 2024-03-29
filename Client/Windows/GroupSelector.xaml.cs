﻿using System;
using System.Windows;
using System.Collections.Generic;
using UksivtScheduler_PC.Controls;
using UksivtScheduler_PC.Listeners;
using UksivtScheduler_PC.Classes.General;
using Bool = System.Boolean;

namespace UksivtScheduler_PC.Windows
{
    /// <summary>
    /// Логика взаимодействия для GroupSelector.xaml.
    /// </summary>
    public partial class GroupSelector : Window, IListItemClickListener
    {
        #region Область: Поля.
        /// <summary>
        /// Внутреннее поле, содержащее префикс нужной группы.
        /// </summary>
        private String prefix;

        /// <summary>
        /// Внутреннее поле, содержащее папку принадлежности нужной группы.
        /// </summary>
        private String subFolder;

        /// <summary>
        /// Внутреннее поле, отвечающее за то, что вызвало закрытие окна.
        /// </summary>
        private Bool returnBack = false;

        /// <summary>
        /// Внутреннее поле, содержащее список с названиями групп.
        /// </summary>
        private List<String> groupNames = new(10);
        #endregion

        #region Область: Свойства.
        /// <summary>
        /// Свойство, содержащее родительское окно данного окна.
        /// </summary>
        public new SubFolderSelection Parent { get; init; }
        #endregion

        #region Область: Конструктор.
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public GroupSelector(String prefix, String subFolder, SubFolderSelection main)
        {
            this.prefix = prefix;
            this.subFolder = subFolder;
            Parent = main;

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
            returnBack = true;

            Parent.Show();
            Close();
        }

        /// <summary>
        /// Реализация интерфейса.
        /// <br/>
        /// Нужна для получения названия группы из нажатой кнопки.
        /// </summary>
        /// <param name="name">Название группы.</param>
        public void ItemIsClicked(String name)
        {
            DaySelector newWindow = new(prefix, subFolder, name, this);

            newWindow.Show();
            Hide();
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

        #region Область: Методы.
        /// <summary>
        /// Метод для инициализации полей.
        /// </summary>
        private void InitializeFields()
        {
            groupNames = Helper.GetGroups(prefix, subFolder);

            InsertDataToList();
        }

        /// <summary>
        /// Метод для добавления элементов в список.
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
