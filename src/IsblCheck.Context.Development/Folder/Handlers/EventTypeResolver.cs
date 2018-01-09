using IsblCheck.Core.Context.Development;
using System;
using System.Collections.Generic;

namespace IsblCheck.Context.Development.Folder.Handlers
{
  internal static class EventTypeResolver
  {
    private static readonly Dictionary<string, EventType> EventTypeByFile = new Dictionary<string, EventType>(StringComparer.OrdinalIgnoreCase)
    {
      ["DataSet.Open"] = EventType.OnDataSetOpen,
      ["DataSet.Close"] = EventType.OnDataSetClose,

      ["Card.Open"] = EventType.OnOpenRecord,
      ["Card.Close"] = EventType.OnCloseRecord,

      ["Operation.Execution"] = EventType.OnUpdateRatifiedRecord,

      ["Card.BeforeInsert"] = EventType.BeforeInsert,
      ["Card.AfterInsert"] = EventType.AfterInsert,
      ["Card.ValidUpdate"] = EventType.OnValidUpdate,
      ["Card.BeforeUpdate"] = EventType.BeforeUpdate,
      ["Card.AfterUpdate"] = EventType.AfterUpdate,
      ["Card.ValidDelete"] = EventType.OnValidDelete,
      ["Card.BeforeDelete"] = EventType.BeforeDelete,
      ["Card.AfterDelete"] = EventType.AfterDelete,
      ["Card.BeforeCancel"] = EventType.BeforeCancel,
      ["Card.AfterCancel"] = EventType.AfterCancel,

      ["Form.Show"] = EventType.FormShow,
      ["Form.Hide"] = EventType.FormHide,

      ["ListForm.Show"] = EventType.ListFormShow,
      ["ListForm.Hide"] = EventType.ListFormHide,

      ["Dialog.Create"] = EventType.Create,
      ["Dialog.ValidCloseWithResult"] = EventType.OnValidCloseWithResult,
      ["Dialog.CloseWithResult"] = EventType.CloseWithResult,
      ["Form.DialogShow"] = EventType.DialogShow,
      ["Form.DialogHide"] = EventType.DialogHide,

      ["Table.BeforeInsert"] = EventType.TableBeforeInsert,
      ["Table.AfterInsert"] = EventType.TableAfterInsert,
      ["Table.BeforeDelete"] = EventType.TableBeforeDelete,
      ["Table.AfterDelete"] = EventType.TableAfterDelete,
      ["Table2.BeforeInsert"] = EventType.Table2BeforeInsert,
      ["Table2.AfterInsert"] = EventType.Table2AfterInsert,
      ["Table2.BeforeDelete"] = EventType.Table2BeforeDelete,
      ["Table2.AfterDelete"] = EventType.Table2AfterDelete,
      ["Table3.BeforeInsert"] = EventType.Table3BeforeInsert,
      ["Table3.AfterInsert"] = EventType.Table3AfterInsert,
      ["Table3.BeforeDelete"] = EventType.Table3BeforeDelete,
      ["Table3.AfterDelete"] = EventType.Table3AfterDelete,
      ["Table4.BeforeInsert"] = EventType.Table4BeforeInsert,
      ["Table4.AfterInsert"] = EventType.Table4AfterInsert,
      ["Table4.BeforeDelete"] = EventType.Table4BeforeDelete,
      ["Table4.AfterDelete"] = EventType.Table4AfterDelete,
      ["Table5.BeforeInsert"] = EventType.Table5BeforeInsert,
      ["Table5.AfterInsert"] = EventType.Table5AfterInsert,
      ["Table5.BeforeDelete"] = EventType.Table5BeforeDelete,
      ["Table5.AfterDelete"] = EventType.Table5AfterDelete,
      ["Table6.BeforeInsert"] = EventType.Table6BeforeInsert,
      ["Table6.AfterInsert"] = EventType.Table6AfterInsert,
      ["Table6.BeforeDelete"] = EventType.Table6BeforeDelete,
      ["Table6.AfterDelete"] = EventType.Table6AfterDelete,
      ["Table7.BeforeInsert"] = EventType.Table7BeforeInsert,
      ["Table7.AfterInsert"] = EventType.Table7AfterInsert,
      ["Table7.BeforeDelete"] = EventType.Table7BeforeDelete,
      ["Table7.AfterDelete"] = EventType.Table7AfterDelete,
      ["Table8.BeforeInsert"] = EventType.Table8BeforeInsert,
      ["Table8.AfterInsert"] = EventType.Table8AfterInsert,
      ["Table8.BeforeDelete"] = EventType.Table8BeforeDelete,
      ["Table8.AfterDelete"] = EventType.Table8AfterDelete,
      ["Table9.BeforeInsert"] = EventType.Table9BeforeInsert,
      ["Table9.AfterInsert"] = EventType.Table9AfterInsert,
      ["Table9.BeforeDelete"] = EventType.Table9BeforeDelete,
      ["Table9.AfterDelete"] = EventType.Table9AfterDelete,
      ["Table10.BeforeInsert"] = EventType.Table10BeforeInsert,
      ["Table10.AfterInsert"] = EventType.Table10AfterInsert,
      ["Table10.BeforeDelete"] = EventType.Table10BeforeDelete,
      ["Table10.AfterDelete"] = EventType.Table10AfterDelete,
      ["Table11.BeforeInsert"] = EventType.Table11BeforeInsert,
      ["Table11.AfterInsert"] = EventType.Table11AfterInsert,
      ["Table11.BeforeDelete"] = EventType.Table11BeforeDelete,
      ["Table11.AfterDelete"] = EventType.Table11AfterDelete,
      ["Table12.BeforeInsert"] = EventType.Table12BeforeInsert,
      ["Table12.AfterInsert"] = EventType.Table12AfterInsert,
      ["Table12.BeforeDelete"] = EventType.Table12BeforeDelete,
      ["Table12.AfterDelete"] = EventType.Table12AfterDelete,
      ["Table13.BeforeInsert"] = EventType.Table13BeforeInsert,
      ["Table13.AfterInsert"] = EventType.Table13AfterInsert,
      ["Table13.BeforeDelete"] = EventType.Table13BeforeDelete,
      ["Table13.AfterDelete"] = EventType.Table13AfterDelete,
      ["Table14.BeforeInsert"] = EventType.Table14BeforeInsert,
      ["Table14.AfterInsert"] = EventType.Table14AfterInsert,
      ["Table14.BeforeDelete"] = EventType.Table14BeforeDelete,
      ["Table14.AfterDelete"] = EventType.Table14AfterDelete,
      ["Table15.BeforeInsert"] = EventType.Table15BeforeInsert,
      ["Table15.AfterInsert"] = EventType.Table15AfterInsert,
      ["Table15.BeforeDelete"] = EventType.Table15BeforeDelete,
      ["Table15.AfterDelete"] = EventType.Table15AfterDelete,
      ["Table16.BeforeInsert"] = EventType.Table16BeforeInsert,
      ["Table16.AfterInsert"] = EventType.Table16AfterInsert,
      ["Table16.BeforeDelete"] = EventType.Table16BeforeDelete,
      ["Table16.AfterDelete"] = EventType.Table16AfterDelete,
      ["Table17.BeforeInsert"] = EventType.Table17BeforeInsert,
      ["Table17.AfterInsert"] = EventType.Table17AfterInsert,
      ["Table17.BeforeDelete"] = EventType.Table17BeforeDelete,
      ["Table17.AfterDelete"] = EventType.Table17AfterDelete,
      ["Table18.BeforeInsert"] = EventType.Table18BeforeInsert,
      ["Table18.AfterInsert"] = EventType.Table18AfterInsert,
      ["Table18.BeforeDelete"] = EventType.Table18BeforeDelete,
      ["Table18.AfterDelete"] = EventType.Table18AfterDelete,
      ["Table19.BeforeInsert"] = EventType.Table19BeforeInsert,
      ["Table19.AfterInsert"] = EventType.Table19AfterInsert,
      ["Table19.BeforeDelete"] = EventType.Table19BeforeDelete,
      ["Table19.AfterDelete"] = EventType.Table19AfterDelete,
      ["Table20.BeforeInsert"] = EventType.Table20BeforeInsert,
      ["Table20.AfterInsert"] = EventType.Table20AfterInsert,
      ["Table20.BeforeDelete"] = EventType.Table20BeforeDelete,
      ["Table20.AfterDelete"] = EventType.Table20AfterDelete,
      ["Table21.BeforeInsert"] = EventType.Table21BeforeInsert,
      ["Table21.AfterInsert"] = EventType.Table21AfterInsert,
      ["Table21.BeforeDelete"] = EventType.Table21BeforeDelete,
      ["Table21.AfterDelete"] = EventType.Table21AfterDelete,
      ["Table22.BeforeInsert"] = EventType.Table22BeforeInsert,
      ["Table22.AfterInsert"] = EventType.Table22AfterInsert,
      ["Table22.BeforeDelete"] = EventType.Table22BeforeDelete,
      ["Table22.AfterDelete"] = EventType.Table22AfterDelete,
      ["Table23.BeforeInsert"] = EventType.Table23BeforeInsert,
      ["Table23.AfterInsert"] = EventType.Table23AfterInsert,
      ["Table23.BeforeDelete"] = EventType.Table23BeforeDelete,
      ["Table23.AfterDelete"] = EventType.Table23AfterDelete,
      ["Table24.BeforeInsert"] = EventType.Table24BeforeInsert,
      ["Table24.AfterInsert"] = EventType.Table24AfterInsert,
      ["Table24.BeforeDelete"] = EventType.Table24BeforeDelete,
      ["Table24.AfterDelete"] = EventType.Table24AfterDelete,

      ["Requisite.Change"] = EventType.Change,
      ["Requisite.Select"] = EventType.Select,
      ["Requisite.BeforeSelect"] = EventType.BeforeSelect,
      ["Requisite.AfterSelect"] = EventType.AfterSelect,

      ["Unknown"] = EventType.Unknown
    };

    public static EventType GetExportedEventType(string fileName)
    {
      EventType eventType;
      if (!EventTypeByFile.TryGetValue(fileName, out eventType))
        eventType = EventType.Unknown;
      return eventType;
    }
  }
}
