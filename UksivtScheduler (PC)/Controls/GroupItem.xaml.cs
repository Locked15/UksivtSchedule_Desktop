using System;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using UksivtScheduler_PC.Listeners;

/// <summary>
/// Область кода с элементом списка групп.
/// </summary>
namespace UksivtScheduler_PC.Controls
{
    /// <summary>
    /// Логика взаимодействия для GroupItem.xaml.
    /// </summary>
    public partial class GroupItem : UserControl
    {
        #region Область: Поля.
        List<Button> buttons = new List<Button>(3);
        #endregion

        #region Область: Свойства.
        /// <summary>
        /// Свойство, содержащее название первой группы.
        /// </summary>
        public String FirstGroup { get; init; }

        /// <summary>
        /// Свойство, содержащее название второй группы.
        /// </summary>
        public String SecondGroup { get; init; }

        /// <summary>
        /// Свойство, содержащее название третьей группы.
        /// </summary>
        public String ThirdGroup { get; init; }

        /// <summary>
        /// Свойство, содержащее сущность реализующую данный интерфейс.
        /// </summary>
        public IGroupItemListener Listener { get; init; }
        #endregion

        #region Область: Конструкторы.
        /// <summary>
        /// Конструктор класса.
        /// <br/>
        /// Из-за отсутствия данных делает элементы невидимыми.
        /// </summary>
        /// <param name="listener">Реализация интерфейса.</param>
        public GroupItem(IGroupItemListener listener)
        {
            InitializeComponent();
            InitializeFields();

            Listener = listener;

            HideButtons(0, 1, 2);
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="firstGroup">Название первой группы.</param>
        /// <param name="listener">Реализация интерфейса.</param>
        public GroupItem(String firstGroup, IGroupItemListener listener)
        {
            InitializeComponent();
            InitializeFields();

            FirstGroup = firstGroup;
            Listener = listener;

            HideButtons(1, 2);
            AddContentToElements(firstGroup);
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="firstGroup">Название первой группы.</param>
        /// <param name="listener">Реализация интерфейса.</param>
        public GroupItem(String firstGroup, String secondGroup, IGroupItemListener listener)
        {
            InitializeComponent();
            InitializeFields();

            FirstGroup = firstGroup;
            SecondGroup = secondGroup;
            Listener = listener;

            HideButtons(2);
            AddContentToElements(firstGroup, secondGroup);
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="firstGroup">Название первой группы.</param>
        /// <param name="listener">Реализация интерфейса.</param>
        public GroupItem(String firstGroup, String secondGroup, String thirdGroup, IGroupItemListener listener)
        {
            InitializeComponent();
            InitializeFields();

            FirstGroup = firstGroup;
            SecondGroup = secondGroup;
            ThirdGroup = thirdGroup;
            Listener = listener;

            AddContentToElements(firstGroup, secondGroup, thirdGroup);
        }
        #endregion

        #region Область: События.
        /// <summary>
        /// Событие, происходящее при нажатии на одну из кнопок.
        /// <br/>
        /// Вызывает пользовательскую реализацию клика.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        public void OneButtonAreClicked(Object sender, EventArgs e)
        {
            Button? custed = sender as Button;
            TextBlock? text = custed?.Content as TextBlock;

            //На всякий случай делаем проверку:
            if (Listener != null && text != null)
            {
                Listener.GroupIsClicked(text.Text);
            }
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Внутренний метод для инициализации полей.
        /// </summary>
        private void InitializeFields()
        {
            buttons.Add(Selector_FirstGroup);
            buttons.Add(Selector_SecondGroup);
            buttons.Add(Selector_ThirdGroup);
        }

        /// <summary>
        /// Метод для сокрытия определенных кнопок по индексам.
        /// </summary>
        /// <param name="indices">Индексы кнопок для сокрытия (0...2).</param>
        /// <exception cref="ArgumentOutOfRangeException">Какой-либо индекс некорректен.</exception>
        private void HideButtons(params Int32[] indices)
        {
            if (indices.Max() > buttons.Count)
            {
                throw new ArgumentOutOfRangeException($"Введен слишком большой индекс ({indices.Max()})!");
            }

            else if (indices.Min() < 0)
            {
                throw new ArgumentOutOfRangeException($"Обнаружен отрицательный индекс ({indices.Min()})!");
            }

            foreach (Int32 ind in indices)
            {
                buttons[ind].Visibility = System.Windows.Visibility.Hidden;
            }
        }

        /// <summary>
        /// Метод для добавления названий групп в элементы управления.
        /// </summary>
        /// <param name="names">Список названий (не более трех элементов).</param>
        /// <exception cref="ArgumentOutOfRangeException">Слишком много названий.</exception>
        private void AddContentToElements(params String[] names)
        {
            if (names.Length > 3)
            {
                throw new ArgumentOutOfRangeException($"Количество отправленных значений слишком велико ({names.Length})!");
            }

            for (int i = 0; i < names.Length; i++)
            {
                if (i == 0)
                {
                    Selector_FirstGroup_Text.Text = names[i];
                }

                else if (i == 1)
                {
                    Selector_SecondGroup_Text.Text = names[i];
                }

                else
                {
                    Selector_ThirdGroup_Text.Text = names[i];
                }
            }
        }
        #endregion
    }
}
