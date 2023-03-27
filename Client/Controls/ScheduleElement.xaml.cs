using System;
using System.Windows.Controls;

/// <summary>
/// Область с элементом отображения расписания.
/// </summary>
namespace UksivtScheduler_PC.Controls
{
    /// <summary>
    /// Логика взаимодействия для ScheduleElement.xaml.
    /// </summary>
    public partial class ScheduleElement : UserControl
    {
        #region Область: Свойства.
        /// <summary>
        /// Свойство, содержащее номер пары.
        /// </summary>
        public Int32 Number { get; init; }

        /// <summary>
        /// Свойство, содержащее название пары.
        /// </summary>
        public String Name { get; init; }

        /// <summary>
        /// Свойство, содержащее имя преподавателя.
        /// </summary>
        public String Teacher { get; init; }

        /// <summary>
        /// Свойство, содержащее место проведения пары.
        /// </summary>
        public String Place { get; init; }
        #endregion

        #region Область: Конструкторы класса.
        /// <summary>
        /// Конструктор класса для пустой пары.
        /// </summary>
        /// <param name="number">Номер пустой пары.</param>
        public ScheduleElement(Int32 number)
        {
            Number = number;
            Name = String.Empty;
            Teacher = String.Empty;
            Place = String.Empty;

            InitializeComponent();
            FillElements();
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="number">Номер пары.</param>
        /// <param name="name">Название пары.</param>
        public ScheduleElement(Int32 number, String name)
        {
            Number = number;
            Name = name;
            Teacher = String.Empty;
            Place = String.Empty;

            InitializeComponent();
            FillElements();
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="number">Номер пары.</param>
        /// <param name="name">Название пары.</param>
        /// <param name="teacher">Имя преподавателя.</param>
        public ScheduleElement(Int32 number, String name, String teacher)
        {
            Number = number;
            Name = name;
            Teacher = teacher;

            InitializeComponent();
            FillElements();
        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="number">Номер пары.</param>
        /// <param name="name">Название пары.</param>
        /// <param name="teacher">Имя преподавателя.</param>
        /// <param name="place">Место проведения пары.</param>
        public ScheduleElement(Int32 number, String name, String teacher, String place)
        {
            Number = number;
            Name = name;
            Teacher = teacher;
            Place = place;

            InitializeComponent();
            FillElements();
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод для заполнения компонентов данными.
        /// </summary>
        private void FillElements()
        {
            ScheduleElement_Number.Text = Number.ToString();
            ScheduleElement_Name.Text = Name;
            ScheduleElement_Teacher.Text = Teacher;
            ScheduleElement_Place.Text = Place;
        }
        #endregion
    }
}
