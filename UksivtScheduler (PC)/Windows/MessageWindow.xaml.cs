using System;
using System.Windows;

/// <summary>
/// Область кода с пользовательским диалоговым окном.
/// </summary>
namespace UksivtScheduler_PC.Windows
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml.
    /// </summary>
    public partial class MessageWindow : Window
    {
        #region Область: Поля класса.
        /// <summary>
        /// Внутреннее поле, содержащее заголовок окна.
        /// </summary>
        private String capture;

        /// <summary>
        /// Внутреннее поле, содержащее основной текст окна.
        /// </summary>
        private String message;
        #endregion

        #region Область: Конструктор.
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public MessageWindow()
        {

        }

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="capture">Заголовок окна.</param>
        /// <param name="message">Выводимое сообщение.</param>
        public MessageWindow(String capture, String message)
        {
            this.capture = capture;
            this.message = message;

            InitializeComponent();
            InitializeElements();
        }
        #endregion

        #region Область: События.
        /// <summary>
        /// Событие, происходящее при нажатии на "Message_Ok".
        /// <br/>
        /// Закрывает диалоговое окно.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void Message_Ok_Click(Object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Область: Методы.
        /// <summary>
        /// Метод для инициализации состояния элементов.
        /// </summary>
        private void InitializeElements()
        {
            Message_Header.Text = capture;
            Message_MainText.Text = message;
        }
        #endregion
    }
}
