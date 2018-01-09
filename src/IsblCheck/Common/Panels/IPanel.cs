using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace IsblCheck.Common.Panels
{
  /// <summary>
  /// Интерфейс панели.
  /// </summary>
  public interface IPanel : INotifyPropertyChanged, IDisposable
  {
    /// <summary>
    /// Идентификатор контента.
    /// </summary>
    string ContentId { get; set; }

    /// <summary>
    /// Заголовок.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Строка локализации для заголовка.
    /// </summary>
    string TitleLocalizationKey { get; }

    /// <summary>
    /// Есть строка локализации для заголовка.
    /// </summary>
    bool HasTitleLocalization { get; }

    /// <summary>
    /// Признак выделения.
    /// </summary>
    bool IsSelected { get; set; }

    /// <summary>
    /// Признак активности.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Признак видимости.
    /// </summary>
    bool IsVisible { get; set; }

    /// <summary>
    /// Признак возможности закрытия вкладки.
    /// </summary>
    bool CanClose { get; }

    /// <summary>
    /// Иконка.
    /// </summary>
    ImageSource IconSource { get; }

    /// <summary>
    /// Событие до закрытия презентера.
    /// </summary>
    event EventHandler Closing;

    /// <summary>
    /// Событие закрытия презентера.
    /// </summary>
    event EventHandler Closed;

    /// <summary>
    /// Команда закрытия вкладки.
    /// </summary>
    ICommand CloseCommand { get; }

    /// <summary>
    /// Показать панель.
    /// </summary>
    void Show();
  }
}
