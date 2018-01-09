namespace IsblCheck.Core.Context.Development
{
  /// <summary>
  /// Тип события
  /// </summary>
  public enum EventType
  {
    /// <summary>
    /// Набор данных. Открытие.
    /// </summary>
    OnDataSetOpen,

    /// <summary>
    /// Набор данных. Закрытие.
    /// </summary>
    OnDataSetClose,

    /// <summary>
    /// Запись. Открытие.
    /// </summary>
    OnOpenRecord,

    /// <summary>
    /// Запись. Закрытие.
    /// </summary>
    OnCloseRecord,

    /// <summary>
    /// Операция. Выполнение.
    /// </summary>
    OnUpdateRatifiedRecord,

    /// <summary>
    /// Запись. Добавление До.
    /// </summary>
    BeforeInsert,

    /// <summary>
    /// Запись. Добавление После.
    /// </summary>
    AfterInsert,

    /// <summary>
    /// Запись. Сохранение Возможность.
    /// </summary>
    OnValidUpdate,

    /// <summary>
    /// Запись. Сохранение До.
    /// </summary>
    BeforeUpdate,

    /// <summary>
    /// Запись. Сохранение После.
    /// </summary>
    AfterUpdate,

    /// <summary>
    /// Запись. Удаление Возможность.
    /// </summary>
    OnValidDelete,

    /// <summary>
    /// Запись. Удаление До.
    /// </summary>
    BeforeDelete,

    /// <summary>
    /// Запись. Удаление После.
    /// </summary>
    AfterDelete,

    /// <summary>
    /// Запись. Отмена До.
    /// </summary>
    BeforeCancel,

    /// <summary>
    /// Запись. Отмена После.
    /// </summary>
    AfterCancel,

    /// <summary>
    /// Форма-карточка. Показ.
    /// </summary>
    FormShow,

    /// <summary>
    /// Форма-карточка. Скрытие.
    /// </summary>
    FormHide,

    /// <summary>
    /// Форма-список. Показ.
    /// </summary>
    ListFormShow,

    /// <summary>
    /// Форма-список. Скрытие.
    /// </summary>
    ListFormHide,

    /// <summary>
    /// Диалог. Создание.
    /// </summary>
    Create,

    /// <summary>
    /// Диалог. Закрытие Возможность.
    /// </summary>
    OnValidCloseWithResult,

    /// <summary>
    /// Диалог. Закрытие.
    /// </summary>
    CloseWithResult,

    /// <summary>
    /// Форма. Показ.
    /// </summary>
    DialogShow,

    /// <summary>
    /// Форма. Скрытие.
    /// </summary>
    DialogHide,

    /// <summary>
    /// Таблица. Добавление до.
    /// </summary>
    TableBeforeInsert,

    /// <summary>
    /// Таблица. Добавление после.
    /// </summary>
    TableAfterInsert,

    /// <summary>
    /// Таблица. Удаление до.
    /// </summary>
    TableBeforeDelete,

    /// <summary>
    /// Таблица. Удаление после.
    /// </summary>
    TableAfterDelete,

    /// <summary>
    /// Таблица 2. Добавление до.
    /// </summary>
    Table2BeforeInsert,

    /// <summary>
    /// Таблица 2. Добавление после.
    /// </summary>
    Table2AfterInsert,

    /// <summary>
    /// Таблица 2. Удаление до.
    /// </summary>
    Table2BeforeDelete,

    /// <summary>
    /// Таблица 2. Удаление после.
    /// </summary>
    Table2AfterDelete,

    /// <summary>
    /// Таблица 3. Добавление до.
    /// </summary>
    Table3BeforeInsert,

    /// <summary>
    /// Таблица 3. Добавление после.
    /// </summary>
    Table3AfterInsert,

    /// <summary>
    /// Таблица 3. Удаление до.
    /// </summary>
    Table3BeforeDelete,

    /// <summary>
    /// Таблица 3. Удаление после.
    /// </summary>
    Table3AfterDelete,

    /// <summary>
    /// Таблица 4. Добавление до.
    /// </summary>
    Table4BeforeInsert,

    /// <summary>
    /// Таблица 4. Добавление после.
    /// </summary>
    Table4AfterInsert,

    /// <summary>
    /// Таблица 4. Удаление до.
    /// </summary>
    Table4BeforeDelete,

    /// <summary>
    /// Таблица 4. Удаление после.
    /// </summary>
    Table4AfterDelete,

    /// <summary>
    /// Таблица 5. Добавление до.
    /// </summary>
    Table5BeforeInsert,

    /// <summary>
    /// Таблица 5. Добавление после.
    /// </summary>
    Table5AfterInsert,

    /// <summary>
    /// Таблица 5. Удаление до.
    /// </summary>
    Table5BeforeDelete,

    /// <summary>
    /// Таблица 5. Удаление после.
    /// </summary>
    Table5AfterDelete,

    /// <summary>
    /// Таблица 6. Добавление до.
    /// </summary>
    Table6BeforeInsert,

    /// <summary>
    /// Таблица 6. Добавление после.
    /// </summary>
    Table6AfterInsert,

    /// <summary>
    /// Таблица 6. Удаление до.
    /// </summary>
    Table6BeforeDelete,

    /// <summary>
    /// Таблица 6. Удаление после.
    /// </summary>
    Table6AfterDelete,

    /// <summary>
    /// Таблица 7. Добавление до.
    /// </summary>
    Table7BeforeInsert,

    /// <summary>
    /// Таблица 7. Добавление после.
    /// </summary>
    Table7AfterInsert,

    /// <summary>
    /// Таблица 7. Удаление до.
    /// </summary>
    Table7BeforeDelete,

    /// <summary>
    /// Таблица 7. Удаление после.
    /// </summary>
    Table7AfterDelete,

    /// <summary>
    /// Таблица 8. Добавление до.
    /// </summary>
    Table8BeforeInsert,

    /// <summary>
    /// Таблица 8. Добавление после.
    /// </summary>
    Table8AfterInsert,

    /// <summary>
    /// Таблица 8. Удаление до.
    /// </summary>
    Table8BeforeDelete,

    /// <summary>
    /// Таблица 8. Удаление после.
    /// </summary>
    Table8AfterDelete,

    /// <summary>
    /// Таблица 9. Добавление до.
    /// </summary>
    Table9BeforeInsert,

    /// <summary>
    /// Таблица 9. Добавление после.
    /// </summary>
    Table9AfterInsert,

    /// <summary>
    /// Таблица 9. Удаление до.
    /// </summary>
    Table9BeforeDelete,

    /// <summary>
    /// Таблица 9. Удаление после.
    /// </summary>
    Table9AfterDelete,

    /// <summary>
    /// Таблица 10. Добавление до.
    /// </summary>
    Table10BeforeInsert,

    /// <summary>
    /// Таблица 10. Добавление после.
    /// </summary>
    Table10AfterInsert,

    /// <summary>
    /// Таблица 10. Удаление до.
    /// </summary>
    Table10BeforeDelete,

    /// <summary>
    /// Таблица 10. Удаление после.
    /// </summary>
    Table10AfterDelete,

    /// <summary>
    /// Таблица 11. Добавление до.
    /// </summary>
    Table11BeforeInsert,

    /// <summary>
    /// Таблица 11. Добавление после.
    /// </summary>
    Table11AfterInsert,

    /// <summary>
    /// Таблица 11. Удаление до.
    /// </summary>
    Table11BeforeDelete,

    /// <summary>
    /// Таблица 11. Удаление после.
    /// </summary>
    Table11AfterDelete,

    /// <summary>
    /// Таблица 12. Добавление до.
    /// </summary>
    Table12BeforeInsert,

    /// <summary>
    /// Таблица 12. Добавление после.
    /// </summary>
    Table12AfterInsert,

    /// <summary>
    /// Таблица 12. Удаление до.
    /// </summary>
    Table12BeforeDelete,

    /// <summary>
    /// Таблица 12. Удаление после.
    /// </summary>
    Table12AfterDelete,

    /// <summary>
    /// Таблица 13. Добавление до.
    /// </summary>
    Table13BeforeInsert,

    /// <summary>
    /// Таблица 13. Добавление после.
    /// </summary>
    Table13AfterInsert,

    /// <summary>
    /// Таблица 13. Удаление до.
    /// </summary>
    Table13BeforeDelete,

    /// <summary>
    /// Таблица 13. Удаление после.
    /// </summary>
    Table13AfterDelete,

    /// <summary>
    /// Таблица 14. Добавление до.
    /// </summary>
    Table14BeforeInsert,

    /// <summary>
    /// Таблица 14. Добавление после.
    /// </summary>
    Table14AfterInsert,

    /// <summary>
    /// Таблица 14. Удаление до.
    /// </summary>
    Table14BeforeDelete,

    /// <summary>
    /// Таблица 14. Удаление после.
    /// </summary>
    Table14AfterDelete,

    /// <summary>
    /// Таблица 15. Добавление до.
    /// </summary>
    Table15BeforeInsert,

    /// <summary>
    /// Таблица 15. Добавление после.
    /// </summary>
    Table15AfterInsert,

    /// <summary>
    /// Таблица 15. Удаление до.
    /// </summary>
    Table15BeforeDelete,

    /// <summary>
    /// Таблица 15. Удаление после.
    /// </summary>
    Table15AfterDelete,

    /// <summary>
    /// Таблица 16. Добавление до.
    /// </summary>
    Table16BeforeInsert,

    /// <summary>
    /// Таблица 16. Добавление после.
    /// </summary>
    Table16AfterInsert,

    /// <summary>
    /// Таблица 16. Удаление до.
    /// </summary>
    Table16BeforeDelete,

    /// <summary>
    /// Таблица 16. Удаление после.
    /// </summary>
    Table16AfterDelete,

    /// <summary>
    /// Таблица 17. Добавление до.
    /// </summary>
    Table17BeforeInsert,

    /// <summary>
    /// Таблица 17. Добавление после.
    /// </summary>
    Table17AfterInsert,

    /// <summary>
    /// Таблица 17. Удаление до.
    /// </summary>
    Table17BeforeDelete,

    /// <summary>
    /// Таблица 17. Удаление после.
    /// </summary>
    Table17AfterDelete,

    /// <summary>
    /// Таблица 18. Добавление до.
    /// </summary>
    Table18BeforeInsert,

    /// <summary>
    /// Таблица 18. Добавление после.
    /// </summary>
    Table18AfterInsert,

    /// <summary>
    /// Таблица 18. Удаление до.
    /// </summary>
    Table18BeforeDelete,

    /// <summary>
    /// Таблица 18. Удаление после.
    /// </summary>
    Table18AfterDelete,

    /// <summary>
    /// Таблица 19. Добавление до.
    /// </summary>
    Table19BeforeInsert,

    /// <summary>
    /// Таблица 19. Добавление после.
    /// </summary>
    Table19AfterInsert,

    /// <summary>
    /// Таблица 19. Удаление до.
    /// </summary>
    Table19BeforeDelete,

    /// <summary>
    /// Таблица 19. Удаление после.
    /// </summary>
    Table19AfterDelete,

    /// <summary>
    /// Таблица 20. Добавление до.
    /// </summary>
    Table20BeforeInsert,

    /// <summary>
    /// Таблица 20. Добавление после.
    /// </summary>
    Table20AfterInsert,

    /// <summary>
    /// Таблица 20. Удаление до.
    /// </summary>
    Table20BeforeDelete,

    /// <summary>
    /// Таблица 20. Удаление после.
    /// </summary>
    Table20AfterDelete,

    /// <summary>
    /// Таблица 21. Добавление до.
    /// </summary>
    Table21BeforeInsert,

    /// <summary>
    /// Таблица 21. Добавление после.
    /// </summary>
    Table21AfterInsert,

    /// <summary>
    /// Таблица 21. Удаление до.
    /// </summary>
    Table21BeforeDelete,

    /// <summary>
    /// Таблица 21. Удаление после.
    /// </summary>
    Table21AfterDelete,

    /// <summary>
    /// Таблица 22. Добавление до.
    /// </summary>
    Table22BeforeInsert,

    /// <summary>
    /// Таблица 22. Добавление после.
    /// </summary>
    Table22AfterInsert,

    /// <summary>
    /// Таблица 22. Удаление до.
    /// </summary>
    Table22BeforeDelete,

    /// <summary>
    /// Таблица 22. Удаление после.
    /// </summary>
    Table22AfterDelete,

    /// <summary>
    /// Таблица 23. Добавление до.
    /// </summary>
    Table23BeforeInsert,

    /// <summary>
    /// Таблица 23. Добавление после.
    /// </summary>
    Table23AfterInsert,

    /// <summary>
    /// Таблица 23. Удаление до.
    /// </summary>
    Table23BeforeDelete,

    /// <summary>
    /// Таблица 23. Удаление после.
    /// </summary>
    Table23AfterDelete,

    /// <summary>
    /// Таблица 24. Добавление до.
    /// </summary>
    Table24BeforeInsert,

    /// <summary>
    /// Таблица 24. Добавление после.
    /// </summary>
    Table24AfterInsert,

    /// <summary>
    /// Таблица 24. Удаление до.
    /// </summary>
    Table24BeforeDelete,

    /// <summary>
    /// Таблица 24. Удаление после.
    /// </summary>
    Table24AfterDelete,

    /// <summary>
    /// Реквизит. Изменение.
    /// </summary>
    Change,

    /// <summary>
    /// Реквизит. Выбор из справочника.
    /// </summary>
    Select,

    /// <summary>
    /// Реквизит. До выбора из справочника.
    /// </summary>
    BeforeSelect,

    /// <summary>
    /// Реквизит. После выбора из справочника.
    /// </summary>
    AfterSelect,
    
    /// <summary>
    /// Неизвестный.
    /// </summary>
    Unknown
  }
}
