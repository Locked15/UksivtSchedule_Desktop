using System;

/// <summary>
/// Область кода с интерфейсом, нужным для создания контракта на прослушивание нажатия кнопки.
/// </summary>
namespace UksivtScheduler_PC.Listeners
{
    /// <summary>
    /// Интерфейс, представляющий контракт, на реализацию обработчика нажатия кнопки в списке групп.
    /// </summary>
    public interface IListItemClickListener
    {
        /// <summary>
        /// Событие, происходящее при нажатии на одну из групп в списке.
        /// </summary>
        /// <param name="groupName">Название выбранной группы.</param>
        void ItemIsClicked(String groupName);
    }
}
