using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using IsblCheck.Common.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IsblCheck.ViewModels.Dialogs
{
  /// <summary>
  /// Модель представления диалога.
  /// </summary>
  public abstract class DialogViewModelBase : ViewModelBase, IDialog, INotifyDataErrorInfo
  {
    #region INotifyDataErrorInfo

    /// <summary>
    /// Событие изменения ошибок.
    /// </summary>
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    /// <summary>
    /// Наличие ошибок.
    /// </summary>
    public bool HasErrors
    {
      get
      {
        return this.Errors.Count > 0;
      }
    }

    /// <summary>
    /// Получить список ошибок.
    /// </summary>
    /// <param name="propertyName">Имя свойства.</param>
    /// <returns>Список ошибок.returns>
    public IEnumerable GetErrors(string propertyName)
    {
      if (this.Errors.ContainsKey(propertyName))
        return this.Errors[propertyName];

      return Enumerable.Empty<string>();
    }

    #endregion

    #region IDispose

    /// <summary>
    /// Уничтожить объект.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion

    #region Поля и свойства

    /// <summary>
    /// Список ошибок.
    /// </summary>
    protected readonly Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();

    /// <summary>
    /// Результат выполнения диалога.
    /// </summary>
    public bool? DialogResult
    {
      get { return this.dialogResult; }
      set
      {
        if (this.dialogResult == value)
          return;
        this.dialogResult = value;
        this.RaisePropertyChanged();
      }
    }
    private bool? dialogResult;

    #endregion

    #region Команды

    /// <summary>
    /// Команда ок.
    /// </summary>
    public ICommand OkCommand { get; private set; }

    /// <summary>
    /// Команда отмены.
    /// </summary>
    public ICommand CancelCommand { get; private set; }

    #endregion

    #region Методы

    /// <summary>
    /// Сгенерировать событие изменения ошибок проверки данных.
    /// </summary>
    /// <param name="propertyName">Имя свойства.</param>
    protected virtual void OnErrorChanged(string propertyName)
    {
      this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Выполнить уничтожение.
    /// </summary>
    /// <param name="disposing">Уничтожить управляемые ресурсы.</param>
    protected virtual void Dispose(bool disposing)
    {
    }

    /// <summary>
    /// Проверить форму.
    /// </summary>
    protected virtual void ValidateForm()
    {
    }

    /// <summary>
    /// Проверить поле.
    /// </summary>
    /// <param name="propertyName">Имя поля.</param>
    protected virtual void ValidateProperty([CallerMemberName] string propertyName = null)
    {
    }

    /// <summary>
    /// Ок.
    /// </summary>
    protected virtual void Ok()
    {
      this.ValidateForm();
      if (this.HasErrors)
        return;

      this.DialogResult = true;
    }

    /// <summary>
    /// Отмена.
    /// </summary>
    private void Cancel()
    {
      this.DialogResult = false;
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected DialogViewModelBase()
    {
      this.OkCommand = new RelayCommand(this.Ok);
      this.CancelCommand = new RelayCommand(this.Cancel);
    }

    /// <summary>
    /// Деструктор.
    /// </summary>
    ~DialogViewModelBase()
    {
      this.Dispose(false);
    }

    #endregion
  }
}
