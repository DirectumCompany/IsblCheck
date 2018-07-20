/* ------------------------------------------------------------------------------------
 * Файл сгенерирован утилитой GenerateSbrteClasses. 
 * Для актуализации файла нужно обновить исходные данные и библиотеки утилиты
 * с последнего билда IS-Builder 7, запустить утилиту и обновить сгенерированные файлы.
 * ------------------------------------------------------------------------------------ 
 */

// ReSharper disable InconsistentNaming

using IsblCheck.Context.Application.Enums;
using System;

#pragma warning disable 0465

namespace IsblCheck.Context.Application.Interfaces
{
  internal interface Application : IApplication
  {
  }

  internal interface IAccessRights
  {
    bool CanAdministrate { get; }
    bool CanManage { get; }
    bool CanModify { get; }
    bool CanRead { get; }
    IUserList Readers { get; }
    IUserList Writers { get; }
    IUserList Managers { get; }
    void CheckAdministrate();
    void CheckManage();
    void CheckModify();
    void CheckRead();
    bool IsManager(IUser UserOrGroup);
    bool IsWriter(IUser UserOrGroup);
    bool IsReader(IUser UserOrGroup);
    bool UserCanRead(IUser UserOrGroup);
    bool UserCanWrite(IUser UserOrGroup);
    bool UserCanManage(IUser UserOrGroup);
  }

  internal interface IAccountRepository
  {
    void Add(string System, IList Accounts);
    void Delete(string System, IList Accounts);
    void Update(string System, IList Accounts);
    void AddAccount(string System, string Name, string FullName, string AdditionalInformation);
    void UpdateAccount(string System, string Name, string FullName, string AdditionalInformation);
    void DeleteAccount(string System, string Name);
  }

  internal interface IAccountSelectionRestrictions
  {
    bool HideLocal { get; }
    bool HideRemote { get; }
  }

  internal interface IAction
  {
    string Code { get; }
    string Category { get; }
    bool Checked { get; set; }
    string Caption { get; }
    string Hint { get; set; }
    bool Enabled { get; set; }
    IForm Form { get; }
    int ShortCut { get; }
    bool Visible { get; set; }
    string DisabledHint { get; set; }
    string Name { get; }
    string Title { get; set; }
    TActionEnabledMode EnabledMode { get; set; }
    IEnabledMode EnabledWhen { get; }
    bool Execute();
  }

  internal interface IActionList : IList
  {
    IAction FindAction(string Name);
  }

  internal interface IAddValidationDataOperationResult
  {
    string ErrorMessage { get; }
    DateTime NextTryMinDate { get; }
    bool IsSuccess { get; }
  }

  internal interface IAdministrationHistoryDescription : IReferenceHistoryDescription
  {
    string RecordFieldID { get; set; }
    string UserType { get; set; }
  }

  internal interface IAnchors
  {
    bool Left { get; set; }
    bool Top { get; set; }
    bool Right { get; set; }
    bool Bottom { get; set; }
  }

  internal interface IApplication
  {
    IConnection Connection { get; }
    string OurFirmContext { get; set; }
    string DepartmentContext { get; set; }
    ILocalization Localization { get; }
    IFolderFactory FolderFactory { get; }
    IEDocumentFactory EDocumentFactory { get; }
    ITaskFactory TaskFactory { get; }
    IJobFactory JobFactory { get; }
    IServiceFactory ServiceFactory { get; }
    ISearchFactory SearchFactory { get; }
    IComponentTokenFactory ComponentTokenFactory { get; }
    IScriptFactory ScriptFactory { get; }
    IReportFactory ReportFactory { get; }
    IReferencesFactory ReferencesFactory { get; }
    string SessionID { get; }
    IDICSFactory DICSFactory { get; }
    int PID { get; }
    ISystemDialogsFactory SystemDialogsFactory { get; }
    IMessagingFactory MessagingFactory { get; }
    IWizardFactory WizardFactory { get; }
    ILicenseInfo LicenseInfo { get; }
    IDialogsFactory DialogsFactory { get; }
    IServerEventFactory ServerEventFactory { get; }
    IGlobalIDFactory GlobalIDFactory { get; }
    IProcessFactory ProcessFactory { get; }
    IList GetList();
    int Connect(string ConnectParams);
    object GetComponent(TCompType ComponentType, string ComponentName);
    IObjectDescription CreateComponentDescription(TCompType AType);
    IObject ExecuteComponent(IObjectDescription ADescription);
    Application CloneApplication();
    void Finalize();
    ILock CreateLock(int ObjectType, int ObjectID);
    void ModifySharedComponent(string ComponentName);
    IHistoryDescription CreateHistoryDescription(TContentKind Kind, int ObjectID);
    void UpdateAccessRights();
    bool UnRegistered();
    void SetMLT(string AMLT);
  }

  internal interface IArchiveInfo
  {
    DateTime ArchiveDate { get; }
    int ArchiveTag { get; }
    void MoveToArchive(int ArchiveTag);
    void RestoreFromArchive();
  }

  internal interface IAttachment
  {
    bool AddedHere { get; }
    IObjectInfo ObjectInfo { get; }
  }

  internal interface IAttachmentList : IForEach
  {
    IAttachment Values { get; }
    IAttachment Find(IObjectInfo AttachmentInfo);
    void Add(IObjectInfo AttachmentInfo);
    void Delete(IObjectInfo AttachmentInfo);
    bool DeleteAttachments(IList ObjectInfoList, out string ErrorMessage);
  }

  internal interface ICheckListBox : ICustomListBox
  {
    bool AllowGrayed { get; set; }
    bool Checked { get; set; }
    bool ItemEnabled { get; set; }
    bool Header { get; set; }
    TCheckListBoxItemState State { get; set; }
    object SnapShot { get; }
    void UnCheckAll();
    void CheckAll();
  }

  internal interface ICheckPointedList : IList
  {
    void AddCheckPoint();
    void ReleaseCheckPoint();
  }

  internal interface IColor
  {
    int Value { get; set; }
    bool IsDefault { get; }
    void Reset();
  }

  internal interface IColumn
  {
    IRequisite Requisite { get; }
    int Index { get; }
    string Name { get; }
    bool Visible { get; set; }
    bool ReadOnly { get; set; }
    bool Enabled { get; set; }
    string Title { get; set; }
  }

  internal interface IComponent : IObject
  {
    bool EOF { get; }
    bool BOF { get; }
    object Bookmark { get; set; }
    IForm ComponentForm { get; }
    string Filter { get; set; }
    bool Filtered { get; set; }
    object Index { get; set; }
    bool Indexed { get; set; }
    int RecordCount { get; }
    bool RecordOpened { get; }
    int ActualRecordCount { get; }
    void Close();
    void Insert();
    void Append();
    void OpenRecord();
    void CloseRecord();
    void First();
    void Next();
    void Prior();
    void Last();
    bool Locate(object KeyNames, object KeyValues);
    void Delete();
    void Open();
  }

  internal interface IComponentDescription : IObjectDescription
  {
    string ACode { get; set; }
    string AViewCode { get; set; }
    IList AParams { get; }
    string AFormName { get; set; }
    int RecordID { get; set; }
    string AName { get; set; }
    string AHierarchyName { get; set; }
    bool AIsHierarchySet { get; set; }
  }

  internal interface IComponentToken : IEdmsObject
  {
  }

  internal interface IComponentTokenFactory : IEdmsObjectFactory
  {
    IComponentToken CreateNew();
    IComponentToken Copy(IComponentToken SourceComponentToken);
    void Execute(IObjectInfo ObjectInfo);
    void ExecuteInNewProcess(IObjectInfo ObjectInfo);
    void Export(string FileName);
    bool Import(string FileName, out string Error);
  }

  internal interface IComponentTokenInfo : IEdmsObjectInfo
  {
    int TokenComponentID { get; }
    string TokenComponentCode { get; }
    string TokenComponentName { get; }
    TCompType TokenComponentType { get; }
    string ExtraParams { get; }
    bool CanModify { get; }
    IComponentToken ComponentToken { get; }
    string TokenComponentTitle { get; }
  }

  internal interface ICompRecordInfo : IObjectInfo
  {
    string Code { get; }
    string ViewCode { get; }
  }

  internal interface IConnection
  {
    int TranCount { get; }
    string ServerName { get; }
    string DatabaseName { get; }
    string UserName { get; }
    TUserType UserType { get; }
    ISystemInfo SystemInfo { get; }
    bool Active { get; set; }
    bool OSAuthentification { get; }
    string LoginName { get; }
    string HostName { get; }
    string HostAddress { get; }
    string PrimaryLanguage { get; }
    int SPID { get; }
    void StartTransaction();
    void CommitTransaction();
    void RollbackTransaction();
    void Open();
    void Close();
    IPrivilege UserPrivilege(string PrivilegeName);
  }

  internal interface IContents : IForEach
  {
    bool IsResultExceedsMaxRecordCount { get; }
    void Sort(object Properties, object AscendingOrder);
    IContents Combine(object Value);
  }

  internal interface IControl
  {
    int Top { get; set; }
    int Left { get; set; }
    int Width { get; set; }
    int Height { get; set; }
    bool Visible { get; set; }
    bool Enabled { get; set; }
    string Name { get; }
    int LabelSpacing { get; set; }
    TLabelPos LabelPosition { get; set; }
    bool LabelVisible { get; set; }
    IRequisite Requisite { get; }
    IAction Action { get; }
    string Text { get; set; }
    bool ReadOnly { get; set; }
    string Hint { get; set; }
    string DisabledHint { get; set; }
    IControl Parent { get; }
    TControlType ControlType { get; }
    string LabelText { get; }
    int TabOrder { get; }
    bool TabStop { get; }
    string Title { get; set; }
    IForm Form { get; }
    IAnchors Anchors { get; }
    IColor Color { get; }
    IColor TextColor { get; }
    object CheckSpelling(IConnection Connection);
  }

  internal interface IControlJob : ICustomJob
  {
    void Accept();
    void Resume();
  }

  internal interface IControlJobInfo : ICustomJobInfo
  {
    IControlJob ControlJob { get; }
  }

  internal interface IControlList : IList
  {
    IControl FindControl(string Name);
    IControl FindControlByRequisiteCode(string RequisiteCode);
    IControlList FindControlsByRequisite(IRequisite Requisite);
    IControl FindControlByAction(IAction Action);
    IControlList FindControlsByAction(IAction Action);
    IControlList FindControlsByType(TControlType ControlType);
    IControl FindControlByRequisite(IRequisite Requisite);
  }

  internal interface ICrypto
  {
    IDataSigner DataSigner { get; }
    IECertificate ECertificate { get; }
    IECertificates ECertificates { get; }
    IHasher Hasher { get; }
    IEncrypter Encrypter { get; }
  }

  internal interface ICrypto2 : ICrypto
  {
    IECertificate ECertificate2 { get; }
  }

  internal interface ICustomJob : ICustomWork
  {
    TJobKind Kind { get; }
    IWorkflowBlock WorkflowBlock { get; }
    void MarkAsReaded();
    void MarkAsUnreaded();
  }

  internal interface ICustomJobInfo : ICustomWorkInfo
  {
    bool IsRead { get; }
    IUser Performer { get; }
    TJobKind JobKind { get; }
    bool UserHasPerformerRights { get; }
    int TaskID { get; }
    ICustomJob CustomJob { get; }
    bool HasActiveSubTasks { get; }
  }

  internal interface ICustomListBox : IControl
  {
    int Columns { get; set; }
    bool ExtendedSelect { get; set; }
    bool MultiSelect { get; set; }
    int Count { get; }
    int ItemIndex { get; set; }
    bool Selected { get; set; }
    int SelCount { get; }
    string Items { get; set; }
    bool Sorted { get; set; }
    void Add(string Value);
    void Delete(int Index);
    void Clear();
    void ClearSelection();
    void DeleteSelected();
    void SelectAll();
  }

  internal interface ICustomObjectWizardStep : IWizardStep
  {
    string ParamName { get; }
    bool WaitForClose { get; }
    bool AutomaticallyShow { get; }
    string StepLabel { get; set; }
  }

  internal interface ICustomWork : IEdmsObject
  {
    string ActiveText { get; set; }
    IList WorkflowParams { get; }
    IWorkflowAskableParams AskableParams { get; }
    bool CanObjectModifyByDefault { get; }
    IList WorkflowProperties { get; }
    IExternalEvents ExternalEvents { get; set; }
    IActionList Actions { get; }
    IList GetSignatures(bool IsForAllTaskObjects);
    bool GetSigned(bool IsForAllTaskObjects);
    void Sign(object Certificate);
    void VerifySignatures(IUser User);
    bool CheckAndSetAttachmentRights(bool IsForFamilyTask, TDeaAccessRights SetRigths, out string ErrorMessage);
    IList GetTreeRootNodes(bool UseCache = true);
    bool AddTimestampToSignature(ISignature Signature);
    IAttachmentList GetAttachments(bool IsForFamilyTask);
    string GetFullText(bool IsForFamilyTask);
    string GetText(TWorkTextBuildingMode BuildingMode);
    void SaveWorkflowAsImage(string FileName, TImageType ImageType);
    string GetTree();
  }

  internal interface ICustomWorkInfo : IEdmsObjectInfo
  {
    DateTime DeadLine { get; }
    IWorkState State { get; }
    TWorkImportance Importance { get; }
    DateTime Executed { get; }
    DateTime SuspendTime { get; }
    TEncodeType EncodeType { get; }
    string StandardRouteName { get; }
    string StandardRouteCode { get; }
    IUserList TaskParticipants { get; }
    bool Signed { get; }
    ICustomWork Work { get; }
    int LeaderTaskID { get; }
    int MainTaskID { get; }
  }

  internal interface IDataSet : IQuery
  {
    IComponent Component { get; }
    int RequisiteCount { get; }
    IDataSet MainDataSet { get; }
    IRequisiteFactory RequisiteFactory { get; }
    bool Modified { get; }
    TDataSetState State { get; }
    bool RecordOpened { get; }
    IRequisite Requisites { get; }
    bool Inserted { get; }
    object Bookmark { get; set; }
    string TableName { get; }
    IQuery AsQuery { get; }
    string SQLTableName { get; }
    IEventList Events { get; }
    bool PreviousValuesStored { get; set; }
    IDataSetAccessInfo AccessInfo { get; }
    bool Copied { get; }
    int ActualRecordCount { get; }
    void Insert();
    void Delete();
    void OpenRecord();
    void CloseRecord();
    void ApplyUpdates();
    void CancelUpdates();
    IRequisite RequisiteByIndex(int Index);
    IDataSet DetailDataSet(int Index);
    bool DetailExists(int Index);
    void Refresh();
    IRequisite FindRequisite(string Name);
    void EnableControls();
    void DisableControls();
    void Append();
    void AssignRecord(IDataSet SourceDataSet, object DetailIndexes);
  }

  internal interface IDataSetAccessInfo
  {
    bool CanInsert { get; }
    bool CanUpdate { get; }
    bool CanDelete { get; }
    bool CanRatify { get; }
  }

  internal interface IDataSigner
  {
    string Content { get; set; }
    string Signature { get; set; }
    IECertificate CreateCertificate { get; set; }
    IECertificate VerifyCertificate { get; }
    DateTime SignDate { get; }
    string PluginName { get; set; }
    string PluginVersion { get; set; }
    IStringList AdditionalInfo { get; set; }
    TSignerContentType ContentType { get; set; }
    object ContentStream { get; set; }
    string PluginTitle { get; set; }
    IETimestamp Timestamp { get; }
    IList ArchiveTimestampsVerificationInfo { get; }
    bool VerifySignature(out string VerifyMsg);
    bool CreateSignature(out string CreateMsg, out DateTime CreateDateTime);
    bool AddTimestamp();
    IAddValidationDataOperationResult AddValidationData(bool OcspResponsesRequired);
    IETimestamp AddArchiveTimestamp();
  }

  internal interface IDateCriterion : ISimpleCriterion
  {
    string DisplayFormat { get; }
    TDateFormatType FormatType { get; }
    TDateTimeKind DateTimeKind { get; }
    IDateValue AddSingleValue(object Value, TRangeValueType ValueType);
    IDateValue AddRange(object LeftBound, object RightBound);
  }

  internal interface IDateRequisite : IRequisite
  {
    string DisplayFormat { get; set; }
    TDateFormatType DateFormatType { get; set; }
    TDateTimeKind DateTimeKind { get; set; }
  }

  internal interface IDateRequisiteDescription : IRequisiteDescription
  {
    string DisplayFormat { get; set; }
    TDateFormatType FormatType { get; set; }
    TDateTimeKind DateTimeKind { get; set; }
  }

  internal interface IDateValue : IValue
  {
    string Value { get; set; }
    string LeftBound { get; set; }
    string RightBound { get; set; }
    TRangeValueType ValueType { get; set; }
  }

  internal interface IDeaAccessRights : IAccessRights
  {
  }

  internal interface IDeaObjectInfo : IEdmsObjectInfo
  {
    bool UserCanModify { get; }
    bool UserCanManage { get; }
  }

  internal interface IDevelopmentComponentLock
  {
    DateTime LockTime { get; }
    bool Locked { get; }
    string Comment { get; }
    string UserName { get; }
    void Lock(string UserName, string Comment = "");
    void Unlock();
    bool CanEdit(out string CannotEditMessage);
  }

  internal interface IDialog : IComponent
  {
    IActionList Actions { get; }
    IControlList Controls { get; }
    object Result { get; set; }
    object Show();
    void CheckRequiredRequisitesFullness();
  }

  internal interface IDialogFactory : IFactory
  {
    IDialog CreateNew(string CallID = "");
  }

  internal interface IDialogPickRequisiteItems : IPickRequisiteItems
  {
    void Add(string ID, string Value, string DisplayText);
    void Delete(string Value);
    void Clear();
  }

  internal interface IDialogsFactory
  {
    IDialogFactory DialogFactory { get; }
  }

  internal interface IDICSFactory
  {
    IObjectImporter ObjectImporter { get; }
    IMetadataRepository MetadataRepository { get; }
    IAccountRepository AccountRepository { get; }
    Application Application { get; }
  }

  internal interface IDocRequisite : IRequisite
  {
    ISearchDescription GetLookupSearchDescription(string LikeText);
  }

  internal interface IDocumentInfo : ICompRecordInfo
  {
  }

  internal interface IDualListDialog
  {
    IStringList AvailableItems { get; }
    IStringList SelectedItems { get; }
    string Title { get; set; }
    string AvailableItemsTitle { get; set; }
    string SelectedItemsTitle { get; set; }
    string Name { get; }
    bool Execute();
  }

  internal interface IECertificate
  {
    string Thumbprint { get; set; }
    string SerialNumber { get; }
    string SubjectName { get; }
    string IssuerName { get; }
    DateTime ValidFromDate { get; }
    DateTime ValidToDate { get; }
    object Certificate { get; set; }
    string CertificateID { get; }
    string PluginName { get; }
    void Display();
    void Export(string FileName, string Password, int SaveType);
    string DetailInfo(int TypeInfo);
    void Load(string FileName, string Password);
    bool IsValid(double VerificationDate = 0, bool NeedCheckTimeValidity = true);
    void LoadFromStorage(string CertificateID);
    bool IsValidEx(DateTime VerificationDate, bool NeedCheckTimeValidity, out TCertificateInvalidationReason InvalidationReason);
  }

  internal interface IECertificateInfo
  {
    string Name { get; }
    IECertificate ECertificate { get; }
    bool IsDefault { get; }
    TCertificateType CertificateType { get; }
  }

  internal interface IECertificates
  {
    object Certificates { get; }
    string PluginName { get; set; }
    string PluginVersion { get; set; }
    string Select(string Title, string DisplayString);
    void Export(string CertificateID, string FileName, string Password, int SaveType);
    bool Find(string CertificateID);
    int Count();
  }

  internal interface IEditControl : IControl
  {
    bool IsPasswordMode { get; set; }
  }

  internal interface IEditorForm : IForm
  {
    string FileName { get; set; }
    IRichEdit Editor { get; }
    string Templates { get; set; }
  }

  internal interface IEdmsExplorer
  {
    object TreeRootContents { get; set; }
    object ListContents { get; set; }
    TItemShow ShowTree { get; set; }
    bool ShowList { get; set; }
    TItemShow ShowToolbar { get; set; }
    bool ShowMainMenu { get; set; }
    TItemShow ShowStatusbar { get; set; }
    string Caption { get; set; }
    TContentKind ListContentKind { get; set; }
    string Title { get; set; }
    IList Params { get; }
    bool ShowAddressBar { get; set; }
    bool ShowRibbon { get; set; }
    void Show();
    IContents SelectFromTree();
    IContents SelectFromList();
    void SetIsComponentTokensDesign(bool Value);
    void ShowModal();
  }

  internal interface IEdmsObject : IObject
  {
    IAccessRights AccessRights { get; }
    bool TryEdit(TEditMode EditMode, out string ErrorMessage);
    void EnterEditMode();
    void LeaveEditMode();
    string GetGlobalID();
    void AssignGlobalID(string GlobalID);
  }

  internal interface IEdmsObjectDescription : IObjectDescription
  {
    TEdmsApplicationAction Action { get; set; }
    int ID { get; set; }
    string Password { get; set; }
  }

  internal interface IEdmsObjectFactory : IFactory
  {
    IEdmsObject Component { get; }
    TUserObjectStatus UserStatus { get; set; }
    IComponent History { get; }
    IUser CurrentUser { get; }
    IContents GetContents(object ObjectsID);
    void Delete(int ID);
    void DeleteByID(int ID);
  }

  internal interface IEdmsObjectInfo : IObjectInfo
  {
    DateTime Created { get; }
    TContentKind Kind { get; }
    DateTime Modified { get; }
    IEdmsObjectInfo Parent { get; }
    TUserObjectStatus UserStatus { get; }
    IUser Author { get; }
    IContents AllContents(bool UseCache = true);
    IContents ContentsByKind(TContentKind Kind, bool UseCache = true);
    void Refresh();
  }

  internal interface IEDocument : IEdmsObject
  {
    IList Versions { get; }
    ILifeCycleStage LifeCycleStage { get; }
    string LockedForServer { get; }
    IActionList Actions { get; }
    void SaveShadowCopyToFile(Int64 HistoryRecordID, string FileName);
    void DeleteShadowCopies(int VersionNumber = -1);
    IList GetAccessibleLifeCycleStages();
    void SaveLifeCycleAsImage(string FileName, TImageType ImageType);
    void Lock();
    void Unlock();
    void ImportFromTemplate(int VersionNumber, string VersionNote, string EDocumentTemplateCode, bool NeedUnlock = true);
    void ImportFromFile(int VersionNumber, string VersionNote, string FileName, bool NeedUnlock = true, string EditorCode = "", bool InExtendedFormat = true);
    void ImportFromScannedFile(int VersionNumber, string VersionNote, string FileName, bool NeedUnlock = true, string EditorCode = "");
    void ImportFromEDocument(int VersionNumber, string VersionNote, object SourceEDocument, int SourceEDocumentVersionNumber, bool NeedUnlock = true);
    void LockForServer(string ServerCode);
    void UnlockFromServer();
    bool ImportSignsFromExtendedFormat(int VersionNumber, string FileName, bool CheckSigns);
    void ExportFromAnotherServer(int VersionNumber, string FileName, string ServerCode, bool NeedCompress = true, bool InExtendedFormat = true, TExportedSignaturesType SignaturesType = default(TExportedSignaturesType));
    string GetMetadataForStorage();
    void Export(int VersionNumber, string FileName, bool NeedLock = true, bool NeedCompress = true, bool InExtendedFormat = true, TExportedSignaturesType SignaturesType = default(TExportedSignaturesType));
    void Import(int VersionNumber, string FileName, bool NeedUnlock = true, string EditorCode = "", bool FromExtendedFormat = true, bool NeedCheckDocumentSize = true);
    void UnlockExportedWithLock();
    void Open(bool OpenForWrite, int VersionNumber);
    void MoveToStorage(IEDocumentStorage Storage);
    void ExportInExtendedFormat(int VersionNumber, string FileName, bool NeedLock = true, bool NeedCompress = true, TExportedSignaturesType SignaturesType = default(TExportedSignaturesType));
    bool ImportInExtendedFormat(int VersionNumber, string FileName, bool NeedUnlock, string EditorCode = "");
    void SetLifeCycleStageByName(string Value, bool NeedLock = true);
  }

  internal interface IEDocumentAccessRights : IDeaAccessRights
  {
    bool CanSign { get; }
    TEncodeType EncodeType { get; set; }
    string Password { set; }
    bool IsEmptyPassword { get; }
    void CheckSign();
    void ChangePasswordTo(string NewPassword);
    void ReEncode();
  }

  internal interface IEDocumentCriterion : IIntegerCriterion
  {
  }

  internal interface IEDocumentDescription : IEdmsObjectDescription
  {
    string TypeCode { get; set; }
    string KindCode { get; set; }
    string TemplateCode { get; set; }
    string EditorCode { get; set; }
    string ASourceFileName { get; set; }
    IEDocumentVersion AVersion { get; set; }
    string TemplatePassword { get; set; }
  }

  internal interface IEDocumentEditor
  {
    string Name { get; }
    string Extension { get; }
    int ID { get; }
    string Code { get; }
    TEditorCloseObservType ObservType { get; }
    bool ReplaceSpecialChars { get; }
    bool CanCreateSeveralProcesses { get; }
    bool CanViewDocumentOpenedToEdit { get; }
    bool IsOperating { get; }
    bool UsePlugins { get; }
    TDocumentDefaultAction DefaultAction { get; }
  }

  internal interface IEDocumentFactory : IEdmsObjectFactory
  {
    IEDocument Templates { get; }
    string StoredEDocumentPath { get; }
    IEDocument CreateNewFromTemplate(string EDocumentTypeCode, string EDocumentKindCode, string EDocumentTemplateCode);
    void BindTo(IObjectInfo Source, object Dest);
    void Branch(IObjectInfo Source, object Dest);
    IEDocument CreateNewFromFile(string EDocumentTypeCode, string EDocumentKindCode, string EDocumentEditorCode, string ASourceFileName, bool InExtendedFormat = true);
    IEDocument Copy(IEDocumentVersion AEDocumentVersion, string EDocumentTypeCode = "", string EDocumentKindCode = "");
    void Open(int ID, bool OpenForWrite, object VersionNumber);
    IEDocument CreateNewFromScannedFile(string EDocumentTypeCode, string EDocumentKindCode, string EDocumentEditorCode, string ScannedFileName);
    IEDocument CreateNewFromTemplateComponent(IEDocument Template, string EDocumentType, string EDocumentKind);
    IEDocumentStorage GetStorageByName(string StorageName);
    IEDocumentStorage GetStorageByID(int StorageID);
    void OpenByID(int ID, bool OpenForWrite, object VersionNumber);
    IEDocument CreateNewFromFileInExtendedFormat(string EDocumentTypeCode, string EDocumentKindCode, string EDocumentEditorCode, string ASourceFileName);
    void CheckStorageIntegrity(IEDocumentStorage Storage, string ReportFileName);
    void Browse(IEDocumentInfo EDocumentInfo, TAreaShowMode SignaturesShowMode, TAreaShowMode JobsShowMode);
    IEDocument ChangeTypeAndKind(int EDocumentID, string EDocumentNewTypeCode, string EDocumentNewKindCode, bool NeedMoveToDefaultStorage, string LifeCycleStageCode);
    void BindToClipboard(IObjectInfo Source);
    bool ExistsDocumentInStorage(IEDocumentInfo EDocumentInfo, int VersionNumber);
    IEDocument GetDocumentNoLock(int ID);
  }

  internal interface IEDocumentInfo : IDeaObjectInfo
  {
    IEDocumentEditor Editor { get; }
    IEDocVersionState LifeStage { get; }
    IEDocument Document { get; }
    bool HasBoundDocuments { get; }
    bool Signed { get; }
    IUser Exporter { get; }
    TEncodeType EncodeType { get; }
    IEDocumentStorage Storage { get; }
    DateTime TextModified { get; }
    IReferenceInfo CardType { get; }
    IReferenceInfo EDocKind { get; }
    ILifeCycleStage LifeCycleStage { get; }
    TSignatureType SignatureType { get; }
    bool DelegateAccessRightsEnabled { get; }
  }

  internal interface IEDocumentStorage
  {
    string Title { get; }
    string Name { get; }
    TEDocumentStorageType StorageType { get; }
    TEDocumentStorageFunction StorageFunction { get; }
    string ComputerName { get; }
    string LocalPath { get; }
    string SharedSourceName { get; }
    bool IsEditingAllowed { get; }
    bool IsCheckingAccessRights { get; }
  }

  internal interface IEDocumentVersion
  {
    IUser Author { get; }
    IEDocVersionState LifeStage { get; }
    DateTime Modified { get; }
    string Note { get; set; }
    int Number { get; }
    IEDocument Parent { get; }
    IList Signatures { get; }
    Int64 Size { get; }
    bool Signed { get; }
    bool IsHidden { get; set; }
    IEDocumentEditor Editor { get; }
    ILock GlobalLock { get; }
    TSignatureType SignatureType { get; }
    DateTime TextModified { get; }
    IEDocVersionState State { get; }
    string CurrentStateName { get; }
    TEDocumentVersionState CurrentState { get; }
    string LockedForServer { get; }
    void Sign(object Certificate, TSignatureType SignatureType, string Comment = "");
    void VerifySignatures();
    void Clone(string Note, string StateCode = "", bool CreateAsHidden = true);
    void SignByAnotherUser(object Certificate, IUser User, TSignatureType SignatureType, string Comment = "");
    void EnterEditMode();
    void LeaveEditMode();
    bool TryEdit(TEditMode EditMode, out string ErrorMessage);
    void ChangeState(TEDocumentVersionState NewState);
    void CreateClone(string Note, TEDocumentVersionState State, bool CreateAsHidden = true);
    void Export(string FileName, bool NeedLock = true, bool NeedCompress = true, bool InExtendedFormat = true, TExportedSignaturesType SignaturesType = default(TExportedSignaturesType), TEditMode EditMode = default(TEditMode));
    void Import(string FileName, bool NeedUnlock = true, string EditorCode = "", bool FromExtendedFormat = true, TEditMode EditMode = default(TEditMode));
    void Lock();
    void Unlock();
    void ExportInExtendedFormat(string FileName, bool NeedLock = true, bool NeedCompress = true, TExportedSignaturesType SignaturesType = default(TExportedSignaturesType));
    bool ImportInExtendedFormat(string FileName, bool NeedUnlock = true, string EditorCode = "");
    void ImportFromTemplate(string EDocumentTemplateCode, bool NeedUnlock = true);
    void ImportFromEDocument(object SourceEDocument, int SourceEDocumentVersionNumber, bool NeedUnlock = true);
    void ImportFromScannedFile(string FileName, bool NeedUnlock = true, string EditorCode = "");
    void ImportFromFile(string FileName, bool NeedUnlock = true, string EditorCode = "", bool FromExtendedFormat = true, TEditMode EditMode = default(TEditMode), bool NeedCheckDocumentSize = true);
    void LockForServer(string ServerCode);
    void UnlockFromServer();
    bool AddTimestampToSignature(ISignature Signature);
    IAddValidationDataOperationResult AddValidationDataToSignature(ISignature Signature, bool OcspResponsesRequired);
  }

  internal interface IEDocumentVersionListDialog : ISelectDialog
  {
    IEDocument EDocument { get; }
  }

  internal interface IEDocumentVersionSource
  {
    TEDocumentVersionSourceType SourceType { get; }
    object Value { get; }
    IFolderInfo DestinationFolder { get; }
    int DestinationVersionNumber { get; }
  }

  internal interface IEDocumentWizardStep : ICustomObjectWizardStep
  {
    TEDocumentStepShowMode ShowMode { get; }
    TEDocumentStepVersionType VersionType { get; }
    int VersionNumber { get; }
    bool OpenForWrite { get; }
  }

  internal interface IEDocVerSignature : ISignature
  {
    TSignatureType SignatureType { get; }
    IUser ByUser { get; }
    string Comment { get; }
  }

  internal interface IEDocVersionState
  {
    string Name { get; }
    string Code { get; }
    void ChangeTo(string NewStageCode);
  }

  internal interface IEnabledMode
  {
    TActionEnabledMode Condition { get; set; }
  }

  internal interface IEncodeProvider
  {
    string DecryptedKey { set; }
    string PluginName { get; }
    string EncryptedKey { get; }
    bool DecryptContextAssigned { get; }
  }

  internal interface IEncrypter
  {
    int Algorithm { get; set; }
    int KeyLength { get; set; }
    string Password { get; set; }
    string Content { get; set; }
    string CryptContent { get; set; }
    string PluginName { get; set; }
    string PluginVersion { get; set; }
    void Encrypt();
    void Decrypt();
  }

  internal interface IETimestamp
  {
    IECertificate Certificate { get; }
    DateTime Time { get; }
  }

  internal interface IETimestampVerificationInfo
  {
    IETimestamp Timestamp { get; }
    bool IsValid { get; }
    string VerificationMessage { get; }
  }

  internal interface IEvent
  {
    int ID { get; }
    bool Enabled { get; set; }
    IExternalHandler ExternalHandler { get; set; }
  }

  internal interface IEventList : ICheckPointedList
  {
    IEvent Events { get; }
    void EnableAll();
    void DisableAll();
  }

  internal interface IException
  {
    string Name { get; }
    TExceptionCategory Category { get; }
    string Message { get; set; }
    string RawMessage { get; }
    void WriteToLog();
  }

  internal interface IExternalEvents
  {
    void OnExternalAskParams(ICustomWork CustomWork, out object Handled);
    bool OnPerformEvent(string EventName);
  }

  internal interface IExternalHandler
  {
    void OnExecute(object Sender, TISBLContext Context);
  }

  internal interface IFactory
  {
    Application Application { get; }
    IObjectInfo ObjectInfo { get; }
    TContentKind Kind { get; }
    IObject GetObjectByID(int ID);
  }

  internal interface IField
  {
    TFieldDataType DataType { get; }
    string Name { get; }
    object Value { get; set; }
    IQuery Query { get; }
    string AsString { get; set; }
    double AsNumeric { get; set; }
    int AsInteger { get; set; }
    DateTime AsDate { get; set; }
    bool IsNull { get; }
    string SQLFieldName { get; }
    void SaveToFile(string FileName);
    void LoadFromFile(string FileName);
  }

  internal interface IFileDialog : IFolderDialog
  {
    int FilterIndex { get; set; }
    string Filter { get; set; }
    bool MultiSelect { get; set; }
    bool OverwritePrompt { get; set; }
  }

  internal interface IFolder : IEdmsObject
  {
    IActionList Actions { get; }
  }

  internal interface IFolderDescription : IEdmsObjectDescription
  {
    TContentKind ContentKind { get; set; }
    IFolder SourceFolder { get; set; }
  }

  internal interface IFolderDialog
  {
    string InitialDir { get; set; }
    string Title { get; set; }
    string Result { get; set; }
    bool Execute();
  }

  internal interface IFolderFactory : IEdmsObjectFactory
  {
    IContents RootFolders { get; }
    IFolder CreateNew(TContentKind ContentKind);
    int CutFromFolder(IFolderInfo FolderInfo, object Contents);
    int PasteToFolder(IFolderInfo FolderInfo, object Contents);
    IFolder Copy(IFolder SourceFolder);
    void ClearCache();
    int PasteToFolderFromClipboard(IFolderInfo FolderInfo);
    void SetupUserManagedFolders(int UserID);
  }

  internal interface IFolderInfo : IDeaObjectInfo
  {
    IFolder Folder { get; }
    TContentKind ContentKind { get; }
    TFolderType FolderType { get; }
    bool HasChildren { get; }
    string URL { get; }
    bool ShowUnread { get; }
    bool ShowCoverOnly { get; }
    bool IsManagedFolder { get; }
    int ManagedFolderID { get; }
    int SortOrder { get; }
  }

  internal interface IForEach
  {
    bool EOF { get; }
    object Value { get; }
    int Count { get; }
    void Next();
    void Reset();
  }

  internal interface IForm
  {
    IFormTitle Caption { get; }
    int Top { get; set; }
    int Left { get; set; }
    IActionList Actions { get; }
    int Width { get; set; }
    int Height { get; set; }
    IControlList Controls { get; }
    bool Visible { get; set; }
    object Result { get; set; }
    IView View { get; }
    string Name { get; }
    IControl ActiveControl { get; }
    TCloseOnEsc CloseOnEsc { get; set; }
    IFormTitle Title { get; }
    bool Sizeable { get; }
    IInplaceHint InplaceHint { get; }
    void Show();
    void Hide();
    void ShowModal();
    void ShowNoModal();
  }

  internal interface IFormTitle
  {
    string BasePart { get; set; }
    string CurrentPeriodPart { get; }
    string ViewModePart { get; }
    string FullCaption { get; }
    string AccountSectionPart { get; set; }
    string FullTitle { get; }
  }

  internal interface IFormWizardStep : IWizardStep
  {
    IList Elements { get; }
  }

  internal interface IGlobalIDFactory
  {
    IObject GetObjectByGlobalID(string GlobalID);
    IObjectInfo ObjectInfoByGlobalID(string GlobalID);
    IList FindLocalIDs(string GlobalID);
    IList FindGlobalIDs(int LocalID, TCompType ComponentType, string ComponentName);
    string CreateGlobalID(int LocalID, TCompType ComponentType, string ComponentName, bool IsDefault, string AdditionalInfo);
    void SaveGlobalID(string GlobalID, int LocalID, TCompType ComponentType, string ComponentName, bool IsDefault, string AdditionalInfo);
  }

  internal interface IGlobalIDInfo
  {
    string GlobalID { get; }
    bool IsDefault { get; }
    string AdditionalInfo { get; }
  }

  internal interface IGrid : IControl
  {
    IColumn ColumnByIndex { get; }
    int ColumnCount { get; }
    TGridRowHeight RowHeight { get; set; }
    IColumn FindColumn(string Name);
    IColumn FindColumnByRequisiteCode(string RequisiteCode);
    IColumn FindColumnByRequisiteName(string RequisiteName);
  }

  internal interface IHasher
  {
    int Algorithm { get; set; }
    string Content { get; set; }
    string Hash { get; }
    string PluginName { get; set; }
    string PluginVersion { get; set; }
    object ContentStream { get; set; }
    TSignerContentType ContentType { get; set; }
    void HashData();
    void HashDataByType(THashType HashType);
  }

  internal interface IHistoryDescription : IObjectDescription
  {
    TContentKind AKind { get; set; }
    int ObjectID { get; set; }
  }

  internal interface IHyperLinkControl : IControl
  {
    string Hyperlink { get; set; }
  }

  internal interface IImageButton : IControl
  {
    string ImageName { get; set; }
    bool ShowCaption { get; set; }
  }

  internal interface IImageControl : IControl
  {
    bool Center { get; set; }
    bool Stretch { get; set; }
    bool Proportional { get; set; }
    bool Bordered { get; set; }
  }

  internal interface IInnerPanel : IControl
  {
    int Size { get; set; }
    int MinSize { get; set; }
    double SizeInPercent { get; set; }
    bool IsSplitterFixed { get; set; }
    bool AutoSizable { get; set; }
  }

  internal interface IInplaceHint : IControl
  {
    TInplaceHintKind Kind { get; set; }
  }

  internal interface IIntegerCriterion : ISimpleCriterion
  {
    IIntegerValue AddSingleValue(int Value, TRangeValueType ValueType);
    IIntegerValue AddRange(int LeftRange, int RightRange);
  }

  internal interface IIntegerList : IForEach
  {
    bool Sorted { get; set; }
    int Values { get; set; }
    string CommaText { get; }
    int Add(int Value);
    int IndexOf(int Value);
    void Delete(int Index);
    void Clear();
    void Sort();
    bool Contains(IIntegerList List);
    void Join(IIntegerList Source);
  }

  internal interface IIntegerRequisite : IRequisite
  {
  }

  internal interface IIntegerValue : IValue
  {
    TRangeValueType ValueType { get; set; }
    int Value { get; set; }
    int LeftBound { get; set; }
    int RightBound { get; set; }
  }

  internal interface IISBLEditorForm : IEditorForm
  {
    TISBLContext ISBLContext { get; set; }
    string AdditionalContextInfo { get; set; }
    IList PredefinedVariableOverrides { get; }
  }

  internal interface IJob : ICustomJob
  {
    IWorkflowBlockResult ExecutionResult { get; }
    void Perform();
    void PerformWithResult(string PerformResult);
  }

  internal interface IJobDescription : IWorkDescription
  {
  }

  internal interface IJobFactory : IEdmsObjectFactory
  {
    DateTime SuspendTime { get; set; }
    DateTime SuspendTimeForUser { get; set; }
    void ClearSuspendTime(int ID);
    void ClearSuspendTimeForUser(int ID, IUser User);
  }

  internal interface IJobForm : IForm
  {
    bool PerformWithResult(string PerformResult);
  }

  internal interface IJobInfo : ICustomJobInfo
  {
    IJob Job { get; }
  }

  internal interface ILabelControl : IControl
  {
    bool AutoSize { get; set; }
    bool WordWrap { get; set; }
  }

  internal interface ILargeIntegerCriterion : ISimpleCriterion
  {
    ILargeIntegerValue AddSingleValue(Int64 Value, TRangeValueType ValueType);
    ILargeIntegerValue AddRange(Int64 LeftRange, Int64 RightRange);
  }

  internal interface ILargeIntegerRequisite : IRequisite
  {
    Int64 AsLargeInteger { get; set; }
  }

  internal interface ILargeIntegerValue
  {
    TRangeValueType ValueType { get; set; }
    Int64 Value { get; set; }
    Int64 LeftBound { get; set; }
    Int64 RightBound { get; set; }
  }

  internal interface ILicenseInfo
  {
    int LicensesAvailable { get; }
    int LicensesTotal { get; }
    int ConnectedUsersCount { get; }
    int ConnectedSystemUsersCount { get; }
    int ReservedLicensesInUseCount { get; }
    int ReservedLicensesCount { get; }
    TLicensingType LicensingType { get; }
  }

  internal interface ILifeCycleStage
  {
    string Name { get; }
    string Title { get; }
    TLifeCycleStageFontStyle FontStyle { get; }
    TLifeCycleStageFontColor FontColor { get; }
  }

  internal interface IList : IForEach
  {
    string Names { get; }
    object Values { get; set; }
    void Add(string Name, object Value);
    void Insert(int Index, string Name, object Value);
    object ValueByName(string Name);
    int IndexOf(object Value);
    int IndexOfName(string Name);
    void Delete(int Index);
    void Clear();
    object FindItem(string Name);
    object PopVar(string Name);
    void SetVar(string Name, object Value);
  }

  internal interface IListBox : ICustomListBox
  {
    TStringsSortType SortType { get; set; }
  }

  internal interface ILocalIDInfo
  {
    int LocalID { get; }
    TCompType ComponentType { get; }
    string ComponentName { get; }
  }

  internal interface ILocalization
  {
    string PrimaryLanguage { get; }
    IList SupportedLanguages { get; }
    bool IsMultiLanguageSystem { get; }
    string CurrentLanguage { get; set; }
    int Locale { set; }
    string GetString(string Code, string GroupCode);
  }

  internal interface ILock
  {
    int ObjectType { get; }
    int ObjectID { get; }
    string SystemCode { get; }
    string HostName { get; }
    string UserName { get; }
    DateTime LockTime { get; }
    bool Locked { get; }
    bool LockedByThis { get; }
    bool TryLockObject();
    void UnlockObject();
    bool LockObjectTimeout(int Milliseconds);
  }

  internal interface IMemoryDataSet : IDataSet
  {
    void LoadDescription(string FileName, string Node);
  }

  internal interface IMessagingFactory
  {
    int MyStatusID { get; }
    string MyStatusDisplayName { get; }
    int StatusID { get; }
    string StatusDisplayName { get; }
    void StartConversation(object UserList);
    void SendMessage(object UserList, string Message);
  }

  internal interface IMetadataRepository
  {
    void Add(string ID, string Version, string Controller, string Value);
    void Delete(string ID, string Version);
  }

  internal interface INotice : ICustomJob
  {
  }

  internal interface INoticeInfo : ICustomJobInfo
  {
    INotice Notice { get; }
  }

  internal interface INumericCriterion : ISimpleCriterion
  {
    INumericValue AddSingleValue(double Value, TRangeValueType ValueType);
    INumericValue AddRange(double LeftBound, double RightBound);
  }

  internal interface INumericRequisite : IRequisite
  {
    int Precision { get; }
  }

  internal interface INumericValue : IValue
  {
    TRangeValueType ValueType { get; set; }
    double Value { get; set; }
    double LeftBound { get; set; }
    double RightBound { get; set; }
  }

  internal interface IObject
  {
    string Code { get; }
    TCompType ComponentType { get; }
    IDataSet DataSet { get; }
    string MainViewCode { get; }
    string Name { get; }
    IView View { get; }
    IList Reports { get; }
    IRuleList Rules { get; }
    IList Environment { get; }
    IList Params { get; }
    IConnection Connection { get; }
    int ComponentID { get; }
    Application Application { get; }
    IRequisite Requisites { get; }
    int RequisiteCount { get; }
    IDataSetAccessInfo AccessInfo { get; }
    bool Active { get; }
    IForm ActiveForm { get; }
    string CommandText { get; }
    IEventList Events { get; }
    ILock GlobalLock { get; }
    int ID { get; }
    IObjectInfo Info { get; }
    bool Inserted { get; }
    string SQLTableName { get; }
    string TableName { get; }
    TDataSetState State { get; }
    string ViewCode { get; set; }
    IForm Form { get; }
    IList RequisiteValues { get; }
    IFactory Factory { get; }
    bool PreviousValuesStored { get; set; }
    bool Modified { get; }
    string Title { get; }
    string ViewName { get; set; }
    bool Copied { get; }
    string Hyperlink { get; }
    IList SQLParams { get; }
    IArchiveInfo ArchiveInfo { get; }
    Int64 LargeID { get; }
    IView CreateView(string ViewCode);
    void ReleaseView();
    void Finalize();
    IComponent AsComponent();
    IRequisite FindRequisite(string Name);
    int AddFrom(string TableName);
    int AddWhere(string Condition);
    int AddSelect(string FieldName, TFieldDataType FieldType, int Size = 0);
    int AddOrderBy(string Condition);
    void DelFrom(int ID);
    void DelWhere(int ID);
    void DelSelect(int ID);
    void DelOrderBy(int ID);
    void DisableControls();
    void EnableControls();
    IQuery AsQuery();
    void AssignObject(IObject SourceObject, object DetailIndexes);
    void Cancel();
    void Save();
    IDataSet DetailDataSet(int Index);
    bool DetailExists(int Index);
    void Refresh();
    IRequisite RequisiteByIndex(int Index);
    void CopyToClipboard();
    int AddJoin(string TableName, TJoinType JoinType, string Condition, TAddPosition Position = TAddPosition.apEnd);
    void DelJoin(int ID);
  }

  internal interface IObjectDescription
  {
    TCompType AType { get; set; }
  }

  internal interface IObjectImporter
  {
    void ImportFiles(IList Files, string ReceiverName, string SenderName);
    void ImportErrorFiles(IList Files, string Error);
  }

  internal interface IObjectInfo
  {
    string Name { get; }
    int ID { get; }
    TCompType ComponentType { get; }
    string ComponentCode { get; }
    string ComponentName { get; }
    string SystemCode { get; }
    string ComponentTitle { get; }
    IContents Combine(IObjectInfo Value);
    string GetGlobalID();
    void AssignGlobalID(string GlobalID);
  }

  internal interface IObserver
  {
    IPropertyChangeEvent OnPropertyChange { get; set; }
    void Attach(object Target);
    void Detach(object Target);
  }

  internal interface IPanelGroup : IControl
  {
    int PanelCount { get; }
    IInnerPanel Panels { get; }
  }

  internal interface IPickCriterion : ISimpleCriterion
  {
    IList PickValues { get; }
    IList DisplayPickValues { get; }
    IPickValue Add(string Value);
  }

  internal interface IPickProperty : IProperty
  {
    IList PickValues { get; }
  }

  internal interface IPickRequisite : IRequisite
  {
    IList ValueList { get; }
    IPickRequisiteItems Items { get; }
  }

  internal interface IPickRequisiteDescription : IRequisiteDescription
  {
    IList PickValues { get; }
  }

  internal interface IPickRequisiteItem
  {
    string ID { get; }
    string Value { get; }
    string DisplayText { get; }
  }

  internal interface IPickRequisiteItems : IForEach
  {
    IPickRequisiteItem ItemByValue(string Value);
    string IDByValue(string Value);
    string DisplayTextByValue(string Value);
  }

  internal interface IPickValue : IValue
  {
    string DisplayValue { get; set; }
    string InternalValue { get; set; }
  }

  internal interface IPrivilege
  {
    string Name { get; }
    string FullName { get; }
    bool Enabled { get; }
    void Check();
  }

  internal interface IPrivilegeList : IForEach
  {
    IPrivilege Privilege(string PrivilegeName);
  }

  internal interface IProcess
  {
    int ID { get; }
    string GlobalID { get; }
    DateTime CreateDate { get; }
    DateTime EndDate { get; }
    string ProcessKindName { get; }
    IList Params { get; }
    bool Locked { get; }
    bool LockedByThis { get; }
    void Stop();
    IWorkflowParam CreateParam(TWorkflowDataType DataType);
    void Save();
    void ExecuteSendHandler(IProcessMessage ProcessMessage, IList Params);
    void ExecuteReceiveHandler(IProcessMessage ProcessMessage, IList Params);
    IProcessMessage CreateProcessMessage(string MessageName);
    void LockWithTimeout(int Milliseconds);
    void Lock();
    void Unlock();
  }

  internal interface IProcessFactory : IFactory
  {
    IProcess CreateProcess(string ProcessKindName);
    IProcess GetProcessByGlobalID(string GlobalID);
    IProcess GetProcessByID(int ID);
    IProcessMessage LoadProcessMessageFromFile(string ProcessKindName, string ProcessGlobalID, string MessageGlobalID);
    IProcess GetProcessByMessage(IProcessMessage ProcessMessage);
  }

  internal interface IProcessMessage
  {
    string Sender { get; set; }
    string Receiver { get; set; }
    string Name { get; }
    IList Files { get; }
    string FolderName { get; }
    string ProcessKindName { get; }
    string ProcessGlobalID { get; }
    DateTime ProcessCreateDate { get; }
    DateTime ProcessEndDate { get; }
    IList Params { get; }
    string GlobalID { get; set; }
    void Export();
  }

  internal interface IProgress
  {
    int Max { get; set; }
    int Position { get; set; }
    string Caption { get; set; }
    string Text { get; set; }
    string Title { get; set; }
    void MoveBy(object Step);
    void Next();
    void Show();
    void Hide();
  }

  internal interface IProperty
  {
    string Name { get; }
    TStructuredObjectAttributeType PropertyType { get; }
    bool Required { get; }
    bool IsNull { get; }
    object Value { get; set; }
    string DisplayName { get; }
    object Values { get; set; }
    int Count { get; }
  }

  internal interface IPropertyChangeEvent
  {
    void OnExecute(object Sender, string PropertyName);
  }

  internal interface IQuery
  {
    string CommandText { get; set; }
    bool BOF { get; }
    bool EOF { get; }
    string Filter { get; set; }
    int FieldCount { get; }
    int RecordCount { get; }
    bool Filtered { get; set; }
    bool Active { get; }
    IField Fields { get; }
    object Index { get; set; }
    bool Indexed { get; set; }
    IList SQLParams { get; }
    void Open();
    void Close();
    void Execute();
    int AddSelect(string Field, TFieldDataType FieldType, int Size = 0);
    int AddFrom(string TableName);
    int AddWhere(string Condition);
    void First();
    void Last();
    void Prior();
    void Next();
    void DelSelect(int AddID);
    void DelFrom(int AddID);
    void DelWhere(int AddID);
    bool Locate(object KeyNames, object KeyValues);
    int AddOrderBy(string Condition);
    void DelOrderBy(int AddID);
    IField FieldByIndex(int Index);
    int AddJoin(string TableName, TJoinType JoinType, string Condition, TAddPosition Position = TAddPosition.apEnd);
    void DelJoin(int ID);
  }

  internal interface IReference : IComponent
  {
    IRequisite DisplayRequisite { get; }
    bool FullShowingRestricted { get; }
    IActionList Actions { get; }
    IReference GetLinkedReference(string Name);
    void ShowLinkedReferenceComponentForm(string Name, string ViewCode = "");
    void ShowLinkedReferenceForm(string Name, string ViewCode = "");
    void SetRecordCountRestriction();
    string GetGlobalID();
    void AssignGlobalID(string GlobalID);
    void Copy();
  }

  internal interface IReferenceCriterion : ISimpleCriterion
  {
    string ReferenceCode { get; }
    string ReferenceName { get; }
    IReferenceValue ValueByPhysical(int PhysicalValue);
    IReferenceValue Add(string LogicalValue);
    IReferenceValue AddWithPhysical(string LogicalValue, int PhysicalValue);
  }

  internal interface IReferenceEnabledMode : IEnabledMode
  {
    bool MultiSelect { get; set; }
    bool SelectMode { get; set; }
    bool SelectionExists { get; set; }
  }

  internal interface IReferenceFactory : IFactory
  {
    string Name { get; }
    IStringList LinkedReferenceNames { get; }
    string Title { get; }
    IComponent History { get; }
    IComponent AdministrationHistory { get; }
    IObjectInfo ObjectInfoByCode { get; }
    int ID { get; }
    void DeleteByCode(string Code);
    IReference GetComponent();
    IObject GetObjectByCode(string Code);
    void DeleteByID(int ID);
    IObject CreateNew();
  }

  internal interface IReferenceHistoryDescription : IHistoryDescription
  {
    string ComponentName { get; set; }
    string RecordName { get; set; }
  }

  internal interface IReferenceInfo : ICompRecordInfo
  {
    IReference Reference { get; }
    IUser Author { get; }
    DateTime Created { get; }
    DateTime Modified { get; }
  }

  internal interface IReferenceRecordCardWizardStep : ICustomObjectWizardStep
  {
    bool ShowMainView { get; }
    string ViewCode { get; }
  }

  internal interface IReferenceRequisiteDescription : IRequisiteDescription
  {
    string ReferenceCode { get; set; }
    string ReferenceName { get; set; }
  }

  internal interface IReferencesFactory
  {
    IReferenceFactory ReferenceFactory { get; }
  }

  internal interface IReferenceValue : IValue
  {
    int PhysicalValue { get; set; }
    string LogicalValue { get; set; }
  }

  internal interface IRefRequisite : IRequisite
  {
    string ReferenceCode { get; }
    int ValueID { get; }
    string ReferenceName { get; }
    IStringList LeaderRequisites { get; }
    IStringList DrivenRequisites { get; }
    IStringList LookUpLeaderRequisites { get; }
    bool IsLeading { get; }
    IReference GetLookupReference(string LikeText);
  }

  internal interface IReport : IObject
  {
    TReportDestination Destination { get; set; }
    string FileName { get; set; }
    object Execute();
    void WriteExecutionToHistory();
  }

  internal interface IReportFactory : IFactory
  {
    object ExecuteByName(string Name);
    IObject GetObjectByName(string Name);
  }

  internal interface IRequisite
  {
    TReqDataType DataType { get; }
    string Name { get; }
    object Value { get; set; }
    object PreviousValue { get; }
    IDataSet DataSet { get; }
    IField Field { get; }
    string Code { get; }
    string AsString { get; set; }
    double AsNumeric { get; set; }
    int AsInteger { get; set; }
    DateTime AsDate { get; set; }
    bool Enabled { get; set; }
    bool IsNull { get; }
    bool Stored { get; }
    IField DisplayField { get; }
    TAlignment Alignment { get; set; }
    bool IsHidden { get; set; }
    string FieldName { get; }
    bool Preloaded { get; set; }
    string SQLFieldName { get; }
    IEventList Events { get; }
    bool Required { get; set; }
    bool CanGUIRead { get; set; }
    bool CanGUIWrite { get; set; }
    string DisplayText { get; }
    string Title { get; }
    bool IsAttached { get; }
    bool Modified { get; }
    bool SaveLastValue { get; set; }
  }

  internal interface IRequisiteDescription
  {
    string Name { get; set; }
    string Code { get; set; }
    string FieldName { get; set; }
    int DataSetIndex { get; set; }
    string Title { get; set; }
  }

  internal interface IRequisiteDescriptionList : IForEach
  {
    IRequisiteDescription Items { get; }
    IRequisiteDescription RequisiteDescriptions { get; }
  }

  internal interface IRequisiteFactory
  {
    void CreateStringRequisite(string Title, string Name, string FieldName, TAlignment Alignment);
    void CreateIntegerRequisite(string Title, string Name, string FieldName);
    void CreateNumericRequisite(string Title, string Name, string FieldName, int Precision);
    void CreateDateRequisite(string Title, string Name, string FieldName, string DisplayFormat);
    void CreatePickRequisite(string Title, string Name, string FieldName, string PickList);
    void CreateDateTimeRequisite(string Title, string Name, string FieldName, TDateFormatType DateFormatType, TDateTimeKind DateTimeKind);
    void CreateWideStringRequisite(string Title, string Name, string FieldName, TAlignment Alignment);
    void CreateLargeIntegerRequisite(string Title, string Name, string FieldName);
  }

  internal interface IRichEdit : IControl
  {
    bool Modified { get; }
  }

  internal interface IRouteStep
  {
    int Number { get; }
    IUser Performer { get; set; }
    object FinalDate { get; set; }
    TJobKind JobKind { get; set; }
    string Note { get; set; }
    string StartCondition { get; set; }
  }

  internal interface IRule
  {
    int ID { get; }
    bool Enabled { get; set; }
    bool Successful { get; }
    string Description { get; }
    bool Severe { get; set; }
  }

  internal interface IRuleList : ICheckPointedList
  {
    IRule Rules { get; }
    void EnableAll();
    void DisableAll();
  }

  internal interface ISchemeBlock
  {
    string BlockType { get; }
    TWorkflowBlockType SystemBlockType { get; }
    IList Properties { get; }
    int ID { get; }
  }

  internal interface IScript : IObject
  {
    object Execute();
  }

  internal interface IScriptFactory : IFactory
  {
    object ExecuteByName(string Name);
    IObject GetObjectByName(string Name);
    IScript FindByName(string Name);
  }

  internal interface ISearchCriteria : IForEach
  {
    string AddWhere { get; set; }
    string AddFrom { get; set; }
    bool HasQueriedCriterion { get; }
    ISearchCriterion Items { get; }
    ISearchCriterion Add(string RequisiteName);
    void Delete(int Index);
    void Clear();
    int IndexOf(ISearchCriterion Criterion);
    void AddJoin(string TableName, TJoinType JoinType, string Condition);
  }

  internal interface ISearchCriterion
  {
    string RequisiteCode { get; set; }
    IRequisiteDescription RequisiteDescription { get; }
    TCriterionContentType ContentType { get; }
    string AsString { get; }
    bool QueryValue { get; set; }
    string RequisiteName { get; set; }
  }

  internal interface ISearchDescription
  {
    string OnPasteLinks { get; set; }
    string OnCutLinks { get; set; }
    string Name { get; set; }
    ISearchCriteria SearchCriteria { get; }
    bool ForAllUsers { get; set; }
    IRequisiteDescriptionList RequisiteDescriptionList { get; }
    bool ForObject { get; }
    string BeforeSearch { get; set; }
    Application Application { get; }
    string Description { get; set; }
    IContents SelectedContents { get; }
    string CommandText { get; }
    ISearchResultRestrictions SearchResultRestrictions { get; }
    TContentKind GetContentKind();
    void Show(TSearchShowMode Mode, bool SuppressQuerySearchCriteria);
    IContents Execute();
    void WriteExecutionToHistory();
  }

  internal interface ISearchFactory
  {
    Application Application { get; }
    ISearchDescription CreateNew(TContentKind SearchKind);
    ISearchDescription Load(string SearchName);
    IContents Execute(ISearchDescription SearchDesc);
    IContents ExecuteByName(string SearchName);
    IList LoadAll();
    void Show(ISearchDescription SearchDesc);
    void ShowByName(string SearchName, IEdmsObjectInfo ObjectInfo);
    void UpdateIndexData();
    void DeleteIndexData();
  }

  internal interface ISearchFolderInfo : IFolderInfo
  {
    ISearchDescription SearchDescription { get; }
  }

  internal interface ISearchForObjectDescription : ISearchDescription
  {
    string CaptionFormat { get; set; }
    IObjectInfo ObjectInfo { get; }
    string TitleFormat { get; set; }
    void InitializeSearch(IObjectInfo ObjectInfo);
    bool IsAllowedType(TCompType ComponentType);
  }

  internal interface ISearchResultRestrictions
  {
    int MaxRecordCount { get; set; }
    TMaxRecordCountRestrictionType MaxRecordCountRestrictionType { get; set; }
    bool NeedGenerateExceptionOnResultExceedsMaxRecordCount { get; set; }
    bool IsResultExceedsMaxRecordCount { get; }
  }

  internal interface ISecuredContext
  {
    void TrySetContext(string ConnectionHash);
    void TryResetContext();
  }

  internal interface ISelectDialog
  {
    void Show();
    object Select();
  }

  internal interface IServerEvent
  {
    IList Params { get; }
    string Name { get; }
    int Start();
    int StartAt(DateTime StartDate);
  }

  internal interface IServerEventFactory : IFactory
  {
    IServerEvent GetObjectByName(string Name);
    void StartByName(string Name);
    void StartByNameAt(string Name, DateTime StartDate);
  }

  internal interface IServiceDialog
  {
    object Execute();
    object Show(bool UseUserSettings);
  }

  internal interface IServiceFactory
  {
    Application Application { get; }
    IServiceDialog CustomizeSearchDialog { get; }
    IServiceDialog QuerySearchDialog { get; }
    string CurrentServerCode { get; }
    string WebServerAddress { get; }
    bool UserCanDeleteDeaObjects { get; }
    bool UserCanDeleteTasks { get; }
    IForEach ECertificateList { get; }
    TDeaAccessRights DefaultDeaAccessRightsType { get; set; }
    IServiceDialog GetCreateEDocumentFromTemplateDialog(string TemplateCode, object Container, string KindCode, string TypeCode, bool NeedOpen);
    IServiceDialog GetCreateEDocumentFromFileDialog(string SourceFileName, object Container, string KindCode, string TypeCode, string EditorCode, bool NeedOpen, bool NeedDeleteSourceFile);
    IServiceDialog GetExportSingleEDocumentDialog(object EDocument, int EDocumentVersionNumber, bool NeedLock, string FileName, bool NeedCompress = true, TExportedSignaturesType SignaturesType = default(TExportedSignaturesType));
    IServiceDialog GetExportMultipleEDocumentDialog(IContents SourceContents, TEDocumentLockType LockType, string DestFolderName, TExportedVersionType VersionType, bool InExtendedFormat, bool NeedCompress = true, TExportedSignaturesType SignaturesType = default(TExportedSignaturesType));
    IServiceDialog GetImportEDocumentDialog(object DestEDocument, int DestEDocumentVersionNumber, string SourceFileName, bool NeedUnlock, bool NeedDeleteSourceFile);
    IEdmsExplorer GetExplorer(bool IsMain);
    IServiceDialog GetCopyEDocumentDialog(object SourceEDocument, int SourceEDocumentVersionNumber, object Container, bool NeedOpen);
    IEDocumentVersionListDialog GetEDocumentVersionListDialog(object AEDocument);
    ISelectDialog GetSignatureListDialog(object SignedObject);
    IServiceDialog GetSignEDocumentDialog(object EDocument, int EDocumentVersionNumber, object Certificate, bool IsAnotherUser, IUser User, TSignatureType SignatureType, string Comment = "");
    IUser GetUserByID(int UserID);
    IUser GetUserByName(string UserName);
    IUser GetGroupByID(int GroupID);
    IReferenceInfo GetReferenceInfo(IReference Reference, int ID);
    IUser GetAccountByID(int AccountID);
    IUser GetGroupByName(string GroupName);
    IUser GetUserByCode(string UserCode);
    IServiceDialog GetCreateEDocumentVersionDialog(object EDocument, int VersionNumber, string NewVersionNote, bool NeedOpenNewVersion, string VersionStateCode = "", bool NeedCreateVersionHidden = true);
    IUserList GetRoleMembers(IUser Role, IObject Sender);
    DateTime GetRelativeDate(DateTime StartDate, int Number, TDateOffsetType NumberType, int UserID = 0);
    IUserList GetGroupMembers(IUser AGroup);
    IUser GetGroupByCode(string Code);
    IUser GetRoleByName(string RoleName);
    IUser GetRoleByCode(string RoleCode);
    bool ScanToFile(string FileName, TImageFileFormat FileFormat, int CompressLevel, bool RewriteFile, TImageMode ImageMode);
    IServiceDialog GetCreateEDocumentFromScannerDialog(object Container, string TypeCode, string KindCode, TImageFileFormat ImageFormat, TImageMode ImageMode, string EditorCode, bool NeedOpen, bool NeedQueryScanOptions, bool NeedPreview, bool NeedQueryScanOptionsForEachScanSession = true);
    IForEach GetUserECertificateList(IUser User);
    string QueryPassword(bool NeedConfirmPassword, bool NeedQueryOldPassword, out string OldPassword, string QueryTitle = "", IEdmsObjectInfo ObjectInfo = default(IEdmsObjectInfo));
    IList GetList();
    IStringList GetStringList();
    IEDocumentEditor GetEditorByID(int EditorID);
    IEDocumentEditor GetEditorByCode(string EditorCode);
    IEDocumentEditor GetEditorByExtension(string Extension);
    IServiceDialog GetEmailSingleEDocumentDialog(object EDocument, int EDocumentVersionNumber, bool NeedLock, bool InExtendedFormat, bool NeedCompress = true, TExportedSignaturesType SignaturesType = default(TExportedSignaturesType));
    IServiceDialog GetEmailMultipleEDocumentsDialog(IContents SourceContents, TEDocumentLockType LockType, TExportedVersionType VersionType, bool InExtendedFormat, bool NeedCompress = true, TExportedSignaturesType SignaturesType = default(TExportedSignaturesType));
    IServiceDialog GetSignMultipleEDocumentsDialog(IContents SourceContents, TExportedVersionType VersionType, object Certificate, bool IsAnotherUser, IUser User, TSignatureType SignatureType, string Comment = "");
    IServiceDialog GetCreateEDocumentNewVersionDialog(object EDocument, int VersionNumber, string NewVersionNote, bool NeedOpenNewVersion, TEDocumentVersionState VersionState, bool NeedCreateVersionHidden = true);
    object GetCryptoPluginSettings(string PluginName);
    void SaveCryptoPluginSettings(string PluginName, object PluginSettings);
    IServiceDialog GetSignMultipleObjectsDialog(IContents SourceContents, object Certificate, bool IsAnotherUser, IUser User, string Comment = "", TExportedVersionType EDocumentVersionType = default(TExportedVersionType), TSignatureType SignatureType = default(TSignatureType));
    IUserList GetUserList();
    IPrivilegeList GetUserPrivileges(IUser User);
    IPrivilegeList GetPrivileges(IUser UserOrGroup);
    IDevelopmentComponentLock GetDevelopmentComponentLock(TLockableDevelopmentComponentType ComponentType, string ObjectName);
    void PlayVideo(string VideoName);
    IIntegerList GetIntegerList();
    IObserver CreateObserver();
    int GetCalendarID(DateTime Date, IUser User);
    object GetFileStream(string FileName);
  }

  internal interface ISignature
  {
    object Certificate { get; }
    DateTime Date { get; }
    IUser Author { get; }
    IStringList AdditionalInfo { get; }
    IETimestamp Timestamp { get; }
    int ID { get; }
    void CheckValidity();
    bool IsValid();
  }

  internal interface ISignProvider
  {
    string Data { get; }
    IReference SubstitutedSigners { get; }
    void AddSignature(string PluginName, string Signature, TSignatureType SignatureType, string Comment, IUser User);
  }

  internal interface ISignProvider2 : ISignProvider
  {
    void AddSignatureEx(string PluginName, string PluginVersion, string Signature, TSignatureType SignatureType, string Comment, IUser User);
  }

  internal interface ISignProvider3
  {
    string GetSigningAttributes(IECertificate Certificate, out DateTime CreateDateTime, IStringList AdditionalInfo);
    string BuildSignature(IECertificate Certificate, string SigningAttributes, string SignedAttributes, IStringList AdditionalInfo);
    string SignAttributes(IECertificate Certificate, string SigningAttributes);
    bool SupportsStreamSigning(IECertificate Certificate);
  }

  internal interface ISimpleCriterion : ISearchCriterion
  {
    int ValueCount { get; }
    IValue Values { get; }
    TValuesBuildType ValuesBuildType { get; set; }
    void Clear();
    void Delete(int Index);
    void StoreValues();
    void RestoreValues();
    void SetSimpleValue(string Value);
    void SetCompleteValue(string Value);
    bool BuildTypeSupported(TValuesBuildType BuildType);
  }

  internal interface IStringCriterion : ISimpleCriterion
  {
    IStringValue Add(string Value, TStringValueType ValueType);
  }

  internal interface IStringList : IForEach
  {
    string DelimitedText { get; set; }
    string Delimiter { get; set; }
    string Values { get; set; }
    bool Sorted { get; set; }
    string Text { get; set; }
    TStringsSortType SortType { get; set; }
    int Add(string Value);
    void AddStrings(IStringList Source);
    void Clear();
    void Delete(int Index);
    void Exchange(int Index1, int Index2);
    int IndexOf(string Value);
  }

  internal interface IStringRequisite : IRequisite
  {
    int Length { get; }
    string AsMIME { get; set; }
  }

  internal interface IStringRequisiteDescription : IRequisiteDescription
  {
    int Length { get; set; }
  }

  internal interface IStringValue : IValue
  {
    string Value { get; set; }
    TStringValueType ValueType { get; set; }
  }

  internal interface ISystemDialogsFactory
  {
    IServiceDialog QuerySearchDialog { get; }
    IServiceDialog CustomizeSearchDialog { get; }
    IServiceDialog GetImportEDocumentVersionFromFileDialog(object DestEDocument, int DestEDocumentVersionNumber, string SourceFileName, string EditorCode, bool NeedUnlock, bool NeedDeleteSourceFile);
    IServiceDialog GetImportEDocumentVersionFromScannerDialog(object DestEDocument, int DestEDocumentVersionNumber, TImageFileFormat ImageFormat, TImageMode ImageMode, string EditorCode, bool NeedUnlock, bool NeedQueryScanOptions, bool NeedQueryScanOptionsForEachScanSession = true);
    IServiceDialog GetImportEDocumentVersionFromTemplateDialog(object DestEDocument, int DestEDocumentVersionNumber, string TemplateCode, bool NeedUnlock);
    IServiceDialog GetImportEDocumentVersionFromEDocumentDialog(object DestEDocument, int DestEDocumentVersionNumber, object SourceEDocument, int SourceEDocumentVersionNumber, bool NeedUnlock);
    IServiceDialog GetCopyEDocumentDialog(object SourceEDocument, int SourceEDocumentVersionNumber, object Container, bool NeedOpen);
    IServiceDialog GetCreateEDocumentFromFileDialog(string SourceFileName, object Container, string KindCode, string TypeCode, string EditorCode, bool NeedOpen, bool NeedDeleteSourceFile);
    IServiceDialog GetCreateEDocumentFromScannerDialog(object Container, string TypeCode, string KindCode, TImageFileFormat ImageFormat, TImageMode ImageMode, string EditorCode, bool NeedOpen, bool NeedQueryScanOptions, bool NeedPreview, bool NeedQueryScanOptionsForEachScanSession = true);
    IServiceDialog GetCreateEDocumentFromTemplateDialog(string TemplateCode, object Container, string KindCode, string TypeCode, bool NeedOpen);
    IServiceDialog GetCreateEDocumentNewVersionDialog(object EDocument, int VersionNumber, string NewVersionNote, bool NeedOpenNewVersion, TEDocumentVersionState VersionState, bool NeedCreateVersionHidden);
    IEDocumentVersionListDialog GetEDocumentVersionListDialog(object AEDocument);
    IServiceDialog GetEmailMultipleEDocumentsDialog(IContents SourceContents, TEDocumentLockType LockType, TExportedVersionType VersionType, bool InExtendedFormat, bool NeedCompress, TExportedSignaturesType SignaturesType);
    IServiceDialog GetEmailSingleEDocumentDialog(object EDocument, int EDocumentVersionNumber, bool NeedLock, bool InExtendedFormat, bool NeedCompress, TExportedSignaturesType SignaturesType);
    IServiceDialog GetExportMultipleEDocumentDialog(IContents SourceContents, TEDocumentLockType LockType, string DestFolderName, TExportedVersionType VersionType, bool InExtendedFormat, bool NeedCompress, TExportedSignaturesType SignaturesType);
    IServiceDialog GetExportSingleEDocumentDialog(object EDocument, int EDocumentVersionNumber, bool NeedLock, string FileName, bool NeedCompress, TExportedSignaturesType SignaturesType);
    ISelectDialog GetSignatureListDialog(object SignedObject);
    IServiceDialog GetSignEDocumentDialog(object EDocument, int EDocumentVersionNumber, object Certificate, bool IsAnotherUser, IUser User, TSignatureType SignatureType, string Comment);
    IServiceDialog GetSignMultipleObjectsDialog(IContents SourceContents, object Certificate, bool IsAnotherUser, IUser User, string Comment = "", TExportedVersionType EDocumentVersionType = default(TExportedVersionType), TSignatureType SignatureType = default(TSignatureType));
  }

  internal interface ISystemInfo
  {
    string Code { get; }
    string Name { get; }
    string ServerName { get; }
    string DatabaseName { get; }
    string ClientVersion { get; }
    string ServerVersion { get; }
    string CorePath { get; }
    string HelpPath { get; }
    string DirectumCode { get; }
    string ProgID { get; }
    string InstallationID { get; }
  }

  internal interface ITabSheet : IControl
  {
    void Activate();
  }

  internal interface ITask : ICustomWork
  {
    IUserList Observers { get; }
    ITaskRoute Route { get; }
    string AcceptanceCondition { get; set; }
    TWorkRouteType RouteType { get; }
    void Start();
    void Abort();
    void ReInit();
    void LoadStandardRoute(string StandardRouteCode);
    void RunStandardRoute();
    void ClearStandardRoute();
    void Continue();
    void SetupStandardRoute(bool ForceQueryWorkflowParams);
    void ResumeFromPause();
    void ResumeFromPauseByBlockID(int BlockID);
  }

  internal interface ITaskAbortReasonInfo
  {
    TTaskAbortReason AbortReason { get; }
    IException Exception { get; }
  }

  internal interface ITaskCardWizardStep : ICustomObjectWizardStep
  {
  }

  internal interface ITaskDescription : IWorkDescription
  {
    ITask LeaderTask { get; set; }
    ICustomJob LeaderJob { get; set; }
    ITask SourceTask { get; set; }
    string StandardRouteCode { get; set; }
  }

  internal interface ITaskFactory : IEdmsObjectFactory
  {
    DateTime SuspendTime { get; set; }
    DateTime SuspendTimeForUser { get; set; }
    ITask CreateNew();
    ITask CreateSubTaskToTask(ITask LeaderTask);
    ITask CreateSubTaskToJob(ICustomJob LeaderJob);
    ITask Copy(ITask Task);
    IRouteStep CreateRouteStep(int Number, IUser Performer, TJobKind JobKind, object FinalDate, string Note, string StartCondition);
    string ConditionFormat(string Condition, TConditionFormat FormatType);
    void ClearSuspendTime(int ID);
    void ClearSuspendTimeForUser(int ID, IUser User);
    bool NeedPerformByReplication(int TaskID);
    string GetTaskTree(IContents Content, IList EncryptedObjects);
    void ShowTaskTree(IContents Content);
  }

  internal interface ITaskInfo : ICustomWorkInfo
  {
    int LeaderJobID { get; }
    ITask Task { get; }
    bool UserHasInitiatorRights { get; }
    bool TaskAbortDisabled { get; }
  }

  internal interface ITaskRoute : IForEach
  {
    IRouteStep Values { get; }
    void Clear();
    void Add(IRouteStep Step);
    void Delete(int Index);
    int IndexOf(IRouteStep Step);
    void Insert(int Index, IRouteStep Step);
    void BeginUpdate();
    void EndUpdate();
  }

  internal interface ITextCriterion : ISimpleCriterion
  {
    bool UseWordPhorms { get; set; }
    ITextValue Add(string Value, TTextValueType ValueType);
  }

  internal interface ITextRequisite : IRequisite
  {
    string Extension { get; set; }
    string AsMIME { get; set; }
    void SaveToFile(string FileName);
    void LoadFromFile(string FileName);
  }

  internal interface ITextValue : IValue
  {
    TTextValueType ValueType { get; set; }
    string Value { get; set; }
  }

  internal interface ITreeListSelectDialog : IDialog
  {
    IDataSet AvailableItems { get; }
    IDataSet SelectedItems { get; }
    string WindowTitle { get; set; }
    string AvailableItemsTitle { get; set; }
    string SelectedItemsTitle { get; set; }
    bool ImagesEnabled { get; set; }
    string AvailableItemsColumnTitle { get; set; }
    string FormIconName { get; set; }
    bool Execute();
    void AddAvailableItem(object Key, object Parent, string TitleItem, string ImageName, string SelectedImageName, object SelectType);
    void AddSelectedItem(object Key);
  }

  internal interface IUser
  {
    int ID { get; }
    string Name { get; }
    string Code { get; }
    TAccountType AccountType { get; }
    string FullName { get; }
    TUserType UserType { get; }
    bool IsRemote { get; }
  }

  internal interface IUserList : IForEach
  {
    void Clear();
    void Add(IUser User);
    void Delete(IUser User);
    bool IntersectWith(object ASourceList);
    bool Find(IUser User);
  }

  internal interface IValue
  {
    string AsString { get; set; }
    bool IsEmpty { get; set; }
  }

  internal interface IView
  {
    string Code { get; set; }
    IForm MainForm { get; }
    IComponent Component { get; }
    IForm ActiveForm { get; }
    TViewMode ViewMode { get; set; }
    IForm Forms { get; }
    bool SelectionConstrained { get; set; }
    string Name { get; }
    int SelectedRecordCount { get; }
    int SelectedRecordsID { get; }
    bool MultiSelection { get; set; }
    bool CanChangeName { get; }
    string HierarchyName { get; set; }
    string MainHierarchyName { get; }
    bool NeedShowSelectionColumn { get; set; }
    void SelectRecordByID(int RecordID);
    void UnselectRecordByID(int RecordID);
    void SelectAllRecords();
    void UnselectAllRecords();
  }

  internal interface IWebBrowserControl : IControl
  {
    bool NativeMode { get; set; }
    object Document { get; }
    object Application { get; }
    string URL { get; }
    void Navigate(string URL);
    void Refresh();
  }

  internal interface IWizard
  {
    IList Params { get; }
    IList Steps { get; }
    string Title { get; }
    IWizardStep NextStep { get; set; }
    IWizardStep CurrentStep { get; set; }
    IWizardStep PreviousStep { get; }
    string Name { get; }
    IObject Parent { get; }
    void Execute();
    void ExecuteBeforeSelection();
    void ExecuteStart();
    void ExecuteFinish();
    void Save();
    void WriteExecutionToHistory();
  }

  internal interface IWizardAction
  {
    string Name { get; }
    string Title { get; }
    TWizardActionType ActionType { get; }
    bool Enabled { get; set; }
    void Execute();
  }

  internal interface IWizardFactory
  {
    IWizard GetObjectByID(int ID);
    IWizard GetObjectByName(string Name);
    IWizard GetObjectByCode(string Code);
  }

  internal interface IWizardFormElement
  {
    TWizardFormElementType ElementType { get; }
    IWizardParam Parameter { get; }
    string Caption { get; }
    bool Required { get; }
    TWizardFormElementProperty ElementProperty { get; }
    bool ReadOnly { get; set; }
    string Hint { get; set; }
  }

  internal interface IWizardParam
  {
    string Name { get; }
    string Title { get; }
    TWizardParamType ParamType { get; }
    object Value { get; set; }
    bool Required { get; }
    bool IsNull { get; }
  }

  internal interface IWizardPickParam : IWizardParam
  {
    IList PickValues { get; }
  }

  internal interface IWizardReferenceParam : IWizardParam
  {
    string ReferenceName { get; }
    bool ShowOperatingRecordsOnly { get; }
  }

  internal interface IWizardStep
  {
    int Index { get; }
    string Name { get; }
    string Title { get; }
    TWizardStepType StepType { get; }
    TWizardStepResult ExecutionResult { get; set; }
    string Description { get; }
    IList Actions { get; }
    IWizardAction LastExecutedAction { get; set; }
    void ExecuteStart();
    void ExecuteFinish();
  }

  internal interface IWorkAccessRights : IAccessRights
  {
    IUserList Accounts { get; }
    TWorkAccessType AccessType { get; set; }
    TEncodeType EncodeType { get; set; }
    string Password { set; }
    bool IsEmptyPassword { get; }
    bool CanParticipantsReadAttachments(out string ErrorMessage, IAttachmentList Attachments, TDeaAccessRights MinAttachmentAccessType);
    void ChangePasswordTo(string NewPassword);
    void ReEncode();
  }

  internal interface IWorkDescription : IEdmsObjectDescription
  {
    bool IsAttachmentsForFamily { get; set; }
    IList Attachments { get; }
  }

  internal interface IWorkflowAskableParam
  {
    IWorkflowParam Param { get; set; }
    bool Required { get; set; }
    string Hint { get; set; }
  }

  internal interface IWorkflowAskableParams : IList
  {
    int AddParam(IWorkflowParam Param, bool Required);
    int InsertParam(int Index, IWorkflowParam Param, bool Required);
  }

  internal interface IWorkflowBlock : ISchemeBlock
  {
    IList Results { get; }
    IWorkflowAskableParams AskableParams { get; }
    ICustomWork Work { get; }
    IActionList Actions { get; }
  }

  internal interface IWorkflowBlockResult
  {
    string Code { get; }
    string Name { get; }
    string DefaultText { get; }
    string DisplayName { get; }
    bool IsAbort { get; }
    string DefaultDisplayText { get; }
    IWorkflowAskableParams AskableParams { get; }
    bool IsHidden { get; }
  }

  internal interface IWorkflowEnabledMode : IEnabledMode
  {
    bool JobRunning { get; set; }
    bool JobDone { get; set; }
    bool JobAborted { get; set; }
    bool AttachmentSelected { get; set; }
  }

  internal interface IWorkflowParam
  {
    object Value { get; set; }
    int Count { get; }
    object Values { get; set; }
    string Name { get; }
    bool IsCollection { get; }
    string Description { get; }
    TWorkflowDataType ParamType { get; set; }
    IForEach ValueParamNames { get; }
    IAccountSelectionRestrictions AccountSelectionRestrictions { get; }
    bool HideClosedRecords { get; }
  }

  internal interface IWorkflowPickParam : IWorkflowParam
  {
    IList PickValues { get; }
    IList DisplayPickValues { get; }
  }

  internal interface IWorkflowReferenceParam : IWorkflowParam
  {
    string ComponentCode { get; }
    string ComponentName { get; }
    bool SupportsLeaderDriven { get; }
  }

  internal interface IWorkState
  {
    string Name { get; }
    TWorkState Value { get; }
  }

  internal interface IWorkTreeCustomNode
  {
    IList SubTaskNodes { get; }
    string Subject { get; }
    IWorkState State { get; }
    DateTime Started { get; }
    DateTime DeadLine { get; }
    DateTime Executed { get; }
    int WorkID { get; }
  }

  internal interface IWorkTreeJobNode : IWorkTreeCustomNode
  {
    IUser Performer { get; }
    string ExecutionResult { get; }
    bool IsRead { get; }
    TJobKind JobKind { get; }
    IUser ActualPerformer { get; }
  }

  internal interface IWorkTreeTaskNode : IWorkTreeCustomNode
  {
    IList JobNodes { get; }
    IUser Initiator { get; }
    IUser ActualInitiator { get; }
  }

  internal interface IXMLEditorForm : IEditorForm
  {
    string Scheme { get; set; }
  }

  internal interface SBCrypto : ICrypto
  {
  }
}