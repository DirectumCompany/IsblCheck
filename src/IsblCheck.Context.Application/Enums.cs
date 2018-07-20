/* ------------------------------------------------------------------------------------
 * Файл сгенерирован утилитой GenerateSbrteClasses. 
 * Для актуализации файла нужно обновить исходные данные и библиотеки утилиты
 * с последнего билда IS-Builder 7, запустить утилиту и обновить сгенерированные файлы.
 * ------------------------------------------------------------------------------------ 
 */

// ReSharper disable InconsistentNaming

namespace IsblCheck.Context.Application.Enums
{
  internal enum TAccountType
  {
    atUser,
    atGroup,
    atRole
  }

  internal enum TActionEnabledMode
  {
    aemEnabledAlways,
    aemDisabledAlways,
    aemEnabledOnBrowse,
    aemEnabledOnEdit,
    aemDisabledOnBrowseEmpty
  }

  internal enum TAddPosition
  {
    apBegin,
    apEnd
  }

  internal enum TAlignment
  {
    alLeft,
    alRight
  }

  internal enum TAreaShowMode
  {
    asmNever,
    asmNoButCustomize,
    asmAsLastTime,
    asmYesButCustomize,
    asmAlways
  }

  internal enum TCertificateInvalidationReason
  {
    cirCommon,
    cirRevoked
  }

  internal enum TCertificateType
  {
    ctSignature,
    ctEncode,
    ctSignatureEncode
  }

  internal enum TCheckListBoxItemState
  {
    clbUnchecked,
    clbChecked,
    clbGrayed
  }

  internal enum TCloseOnEsc
  {
    ceISB,
    ceAlways,
    ceNever
  }

  internal enum TCompType
  {
    ctDocument,
    ctReference,
    ctScript,
    ctUnknown,
    ctReport,
    ctDialog,
    ctFunction,
    ctFolder,
    ctEDocument,
    ctTask,
    ctJob,
    ctNotice,
    ctControlJob
  }

  internal enum TConditionFormat
  {
    cfInternal,
    cfDisplay
  }

  internal enum TConnectionIntent
  {
    ciUnspecified,
    ciWrite,
    ciRead
  }

  internal enum TContentKind
  {
    ckFolder,
    ckEDocument,
    ckTask,
    ckJob,
    ckComponentToken,
    ckAny,
    ckReference,
    ckScript,
    ckReport,
    ckDialog
  }

  internal enum TControlType
  {
    ctISBLEditor,
    ctBevel,
    ctButton,
    ctCheckListBox,
    ctComboBox,
    ctComboEdit,
    ctGrid,
    ctDBCheckBox,
    ctDBComboBox,
    ctDBEdit,
    ctDBEllipsis,
    ctDBMemo,
    ctDBNavigator,
    ctDBRadioGroup,
    ctDBStatusLabel,
    ctEdit,
    ctGroupBox,
    ctInplaceHint,
    ctMemo,
    ctPanel,
    ctListBox,
    ctRadioButton,
    ctRichEdit,
    ctTabSheet,
    ctWebBrowser,
    ctImage,
    ctHyperLink,
    ctLabel,
    ctDBMultiEllipsis,
    ctRibbon,
    ctRichView,
    ctInnerPanel,
    ctPanelGroup,
    ctBitButton
  }

  internal enum TCriterionContentType
  {
    cctDate,
    cctInteger,
    cctNumeric,
    cctPick,
    cctReference,
    cctString,
    cctText
  }

  internal enum TCultureType
  {
    cltInternal,
    cltPrimary,
    cltGUI
  }

  internal enum TDataSetEventType
  {
    dseBeforeOpen,
    dseAfterOpen,
    dseBeforeClose,
    dseAfterClose,
    dseOnValidDelete,
    dseBeforeDelete,
    dseAfterDelete,
    dseAfterDeleteOutOfTransaction,
    dseOnDeleteError,
    dseBeforeInsert,
    dseAfterInsert,
    dseOnValidUpdate,
    dseBeforeUpdate,
    dseOnUpdateRatifiedRecord,
    dseAfterUpdate,
    dseAfterUpdateOutOfTransaction,
    dseOnUpdateError,
    dseAfterScroll,
    dseOnOpenRecord,
    dseOnCloseRecord,
    dseBeforeCancel,
    dseAfterCancel,
    dseOnUpdateDeadlockError,
    dseBeforeDetailUpdate,
    dseOnPrepareUpdate,
    dseOnAnyRequisiteChange
  }

  internal enum TDataSetState
  {
    dssEdit,
    dssInsert,
    dssBrowse,
    dssInActive
  }

  internal enum TDateFormatType
  {
    dftDate,
    dftShortDate,
    dftDateTime,
    dftTimeStamp
  }

  internal enum TDateOffsetType
  {
    dotDays,
    dotHours,
    dotMinutes,
    dotSeconds
  }

  internal enum TDateTimeKind
  {
    dtkndLocal,
    dtkndUTC
  }

  internal enum TDeaAccessRights
  {
    arNone,
    arView,
    arEdit,
    arFull
  }

  internal enum TDocumentDefaultAction
  {
    ddaView,
    ddaEdit
  }

  internal enum TEditMode
  {
    emLock,
    emEdit,
    emSign,
    emExportWithLock,
    emImportWithUnlock,
    emChangeVersionNote,
    emOpenForModify,
    emChangeLifeStage,
    emDelete,
    emCreateVersion,
    emImport,
    emUnlockExportedWithLock,
    emStart,
    emAbort,
    emReInit,
    emMarkAsReaded,
    emMarkAsUnreaded,
    emPerform,
    emAccept,
    emResume,
    emChangeRights,
    emEditRoute,
    emEditObserver,
    emRecoveryFromLocalCopy,
    emChangeWorkAccessType,
    emChangeEncodeTypeToCertificate,
    emChangeEncodeTypeToPassword,
    emChangeEncodeTypeToNone,
    emChangeEncodeTypeToCertificatePassword,
    emChangeStandardRoute,
    emGetText,
    emOpenForView,
    emMoveToStorage,
    emCreateObject,
    emChangeVersionHidden,
    emDeleteVersion,
    emChangeLifeCycleStage,
    emApprovingSign,
    emExport,
    emContinue,
    emLockFromEdit,
    emUnLockForEdit,
    emLockForServer,
    emUnlockFromServer,
    emDelegateAccessRights,
    emReEncode,
    emAddTimestamp,
    emAddValidationData
  }

  internal enum TEditorCloseObservType
  {
    ecotFile,
    ecotProcess
  }

  internal enum TEdmsApplicationAction
  {
    eaGet,
    eaCopy,
    eaCreate,
    eaCreateStandardRoute
  }

  internal enum TEDocumentLockType
  {
    edltAll,
    edltNothing,
    edltQuery
  }

  internal enum TEDocumentStepShowMode
  {
    essmText,
    essmCard
  }

  internal enum TEDocumentStepVersionType
  {
    esvtLast,
    esvtLastActive,
    esvtSpecified
  }

  internal enum TEDocumentStorageFunction
  {
    edsfExecutive,
    edsfArchive
  }

  internal enum TEDocumentStorageType
  {
    edstSQLServer,
    edstFile
  }

  internal enum TEDocumentVersionSourceType
  {
    edvstNone,
    edvstEDocumentVersionCopy,
    edvstFile,
    edvstTemplate,
    edvstScannedFile
  }

  internal enum TEDocumentVersionState
  {
    vsDefault,
    vsDesign,
    vsActive,
    vsObsolete
  }

  internal enum TEncodeType
  {
    etNone,
    etCertificate,
    etPassword,
    etCertificatePassword
  }

  internal enum TExceptionCategory
  {
    ecException,
    ecWarning,
    ecInformation
  }

  internal enum TExportedSignaturesType
  {
    estAll,
    estApprovingOnly
  }

  internal enum TExportedVersionType
  {
    evtLast,
    evtLastActive,
    evtQuery
  }

  internal enum TFieldDataType
  {
    fdtString,
    fdtNumeric,
    fdtInteger,
    fdtDate,
    fdtText,
    fdtUnknown,
    fdtWideString,
    fdtLargeInteger
  }

  internal enum TFolderType
  {
    ftInbox,
    ftOutbox,
    ftFavorites,
    ftCommonFolder,
    ftUserFolder,
    ftComponents,
    ftQuickLaunch,
    ftShortcuts,
    ftSearch
  }

  internal enum TGridRowHeight
  {
    grhAuto,
    grhX1,
    grhX2,
    grhX3
  }

  internal enum THashType
  {
    htUnknown,
    htGost3411,
    htGost3411_2012_256,
    htGost3411_2012_512,
    htSha1,
    htSha256,
    htSha384,
    htSha512,
    htMD5
  }

  internal enum THyperlinkType
  {
    hltText,
    hltRTF,
    hltHTML
  }

  internal enum TImageFileFormat
  {
    iffBMP,
    iffJPEG,
    iffMultiPageTIFF,
    iffSinglePageTIFF,
    iffTIFF,
    iffPNG
  }

  internal enum TImageMode
  {
    im8bGrayscale,
    im24bRGB,
    im1bMonochrome
  }

  internal enum TImageType
  {
    itBMP,
    itJPEG,
    itWMF,
    itPNG
  }

  internal enum TInplaceHintKind
  {
    ikhInformation,
    ikhWarning,
    ikhError,
    ikhNoIcon
  }

  internal enum TISBLContext
  {
    icUnknown,
    icScript,
    icFunction,
    icIntegratedReport,
    icAnalyticReport,
    icDataSetEventHandler,
    icActionHandler,
    icFormEventHandler,
    icLookUpEventHandler,
    icRequisiteChangeEventHandler,
    icBeforeSearchEventHandler,
    icRoleCalculation,
    icSelectRouteEventHandler,
    icBlockPropertyCalculation,
    icBlockQueryParamsEventHandler,
    icChangeSearchResultEventHandler,
    icBlockEventHandler,
    icSubTaskInitEventHandler = 18,
    icEDocDataSetEventHandler,
    icEDocLookUpEventHandler,
    icEDocActionHandler,
    icEDocFormEventHandler,
    icEDocRequisiteChangeEventHandler,
    icStructuredConversionRule,
    icStructuredConversionEventBefore,
    icStructuredConversionEventAfter,
    icWizardEventHandler,
    icWizardFinishEventHandler,
    icWizardStepEventHandler,
    icWizardStepFinishEventHandler,
    icWizardActionEnableEventHandler,
    icWizardActionExecuteEventHandler,
    icCreateJobsHandler,
    icCreateNoticesHandler,
    icBeforeLookUpEventHandler,
    icAfterLookUpEventHandler,
    icTaskAbortEventHandler,
    icWorkflowBlockActionHandler,
    icDialogDataSetEventHandler,
    icDialogActionHandler,
    icDialogLookUpEventHandler,
    icDialogRequisiteChangeEventHandler,
    icDialogFormEventHandler,
    icDialogValidCloseEventHandler,
    icBlockFormEventHandler,
    icTaskFormEventHandler,
    icReferenceMethod,
    icEDocMethod,
    icDialogMethod,
    icProcessMessageHandler,
    icFolderActionHandler,
    icFolderBeforeSearchEventHandler,
    icFolderMethod
  }

  internal enum TItemShow
  {
    isShow,
    isHide,
    isByUserSettings
  }

  internal enum TJobKind
  {
    jkJob,
    jkNotice,
    jkControlJob
  }

  internal enum TJoinType
  {
    jtInner,
    jtLeft,
    jtRight,
    jtFull,
    jtCross
  }

  internal enum TLabelPos
  {
    lbpAbove,
    lbpBelow,
    lbpLeft,
    lbpRight
  }

  internal enum TLicensingType
  {
    eltPerConnection,
    eltPerUser
  }

  internal enum TLifeCycleStageFontColor
  {
    sfcUndefined,
    sfcBlack,
    sfcGreen,
    sfcRed,
    sfcBlue,
    sfcOrange,
    sfcLilac
  }

  internal enum TLifeCycleStageFontStyle
  {
    sfsItalic,
    sfsStrikeout,
    sfsNormal
  }

  internal enum TLockableDevelopmentComponentType
  {
    ldctStandardRoute,
    ldctWizard,
    ldctScript,
    ldctFunction,
    ldctRouteBlock,
    ldctIntegratedReport,
    ldctAnalyticReport,
    ldctReferenceType,
    ldctEDocumentType,
    ldctDialog,
    ldctServerEvents
  }

  internal enum TMaxRecordCountRestrictionType
  {
    mrcrtNone,
    mrcrtUser,
    mrcrtMaximal,
    mrcrtCustom
  }

  internal enum TRangeValueType
  {
    vtEqual,
    vtGreaterOrEqual,
    vtLessOrEqual,
    vtRange
  }

  internal enum TRelativeDate
  {
    rdYesterday,
    rdToday,
    rdTomorrow,
    rdThisWeek,
    rdThisMonth,
    rdThisYear,
    rdNextMonth,
    rdNextWeek,
    rdLastWeek,
    rdLastMonth
  }

  internal enum TReportDestination
  {
    rdWindow,
    rdFile,
    rdPrinter
  }

  internal enum TReqDataType
  {
    rdtString,
    rdtNumeric,
    rdtInteger,
    rdtDate,
    rdtReference,
    rdtAccount,
    rdtText,
    rdtPick,
    rdtUnknown,
    rdtLargeInteger,
    rdtDocument
  }

  internal enum TRequisiteEventType
  {
    reOnChange,
    reOnChangeValues
  }

  internal enum TSBTimeType
  {
    ttGlobal,
    ttLocal,
    ttUser,
    ttSystem
  }

  internal enum TSearchShowMode
  {
    ssmBrowse,
    ssmSelect,
    ssmMultiSelect,
    ssmBrowseModal
  }

  internal enum TSelectMode
  {
    smSelect,
    smLike,
    smCard
  }

  internal enum TSignatureType
  {
    stNone,
    stAuthenticating,
    stApproving
  }

  internal enum TSignerContentType
  {
    sctString,
    sctStream
  }

  internal enum TStringsSortType
  {
    sstAnsiSort,
    sstNaturalSort
  }

  internal enum TStringValueType
  {
    svtEqual,
    svtContain
  }

  internal enum TStructuredObjectAttributeType
  {
    soatString,
    soatNumeric,
    soatInteger,
    soatDatetime,
    soatReferenceRecord,
    soatText,
    soatPick,
    soatBoolean,
    soatEDocument,
    soatAccount,
    soatIntegerCollection,
    soatNumericCollection,
    soatStringCollection,
    soatPickCollection,
    soatDatetimeCollection,
    soatBooleanCollection,
    soatReferenceRecordCollection,
    soatEDocumentCollection,
    soatAccountCollection,
    soatContents,
    soatUnknown
  }

  internal enum TTaskAbortReason
  {
    tarAbortByUser,
    tarAbortByWorkflowException
  }

  internal enum TTextValueType
  {
    tvtAllWords,
    tvtExactPhrase,
    tvtAnyWord
  }

  internal enum TUserObjectStatus
  {
    usNone,
    usCompleted,
    usRedSquare,
    usBlueSquare,
    usYellowSquare,
    usGreenSquare,
    usOrangeSquare,
    usPurpleSquare,
    usFollowUp
  }

  internal enum TUserType
  {
    utUnknown,
    utUser,
    utDeveloper,
    utAdministrator,
    utSystemDeveloper,
    utDisconnected
  }

  internal enum TValuesBuildType
  {
    btAnd,
    btDetailAnd,
    btOr,
    btNotOr,
    btOnly
  }

  internal enum TVariableDataType
  {
    vdtUnknown,
    vdtNull,
    vdtInteger,
    vdtFloat,
    vdtDate,
    vdtString,
    vdtObject,
    vdtBoolean,
    vdtVariant,
    vdtArray
  }

  internal enum TViewMode
  {
    vmView,
    vmSelect,
    vmNavigation
  }

  internal enum TViewSelectionMode
  {
    vsmSingle,
    vsmMultiple,
    vsmMultipleCheck,
    vsmNoSelection
  }

  internal enum TWizardActionType
  {
    wfatPrevious,
    wfatNext,
    wfatCancel,
    wfatFinish
  }

  internal enum TWizardFormElementProperty
  {
    wfepUndefined,
    wfepText3,
    wfepText6,
    wfepText9,
    wfepSpinEdit,
    wfepDropDown,
    wfepRadioGroup,
    wfepFlag,
    wfepText12,
    wfepText15,
    wfepText18,
    wfepText21,
    wfepText24,
    wfepText27,
    wfepText30,
    wfepRadioGroupColumn1,
    wfepRadioGroupColumn2,
    wfepRadioGroupColumn3
  }

  internal enum TWizardFormElementType
  {
    wfetQueryParameter,
    wfetText,
    wfetDelimiter,
    wfetLabel
  }

  internal enum TWizardParamType
  {
    wptString,
    wptInteger,
    wptNumeric,
    wptBoolean,
    wptDateTime,
    wptPick,
    wptText,
    wptUser,
    wptUserList,
    wptEDocumentInfo,
    wptEDocumentInfoList,
    wptReferenceRecordInfo,
    wptReferenceRecordInfoList,
    wptFolderInfo,
    wptTaskInfo,
    wptContents,
    wptFileName,
    wptDate
  }

  internal enum TWizardStepResult
  {
    wsrComplete,
    wsrGoNext,
    wsrGoPrevious,
    wsrCustom,
    wsrCancel,
    wsrGoFinal
  }

  internal enum TWizardStepType
  {
    wstForm,
    wstEDocument,
    wstTaskCard,
    wstReferenceRecordCard,
    wstFinal
  }

  internal enum TWorkAccessType
  {
    waAll,
    waPerformers,
    waManual
  }

  internal enum TWorkflowBlockType
  {
    wsbStart,
    wsbFinish,
    wsbNotice,
    wsbStep,
    wsbDecision,
    wsbWait,
    wsbMonitor,
    wsbScript,
    wsbConnector,
    wsbSubTask,
    wsbLifeCycleStage,
    wsbPause
  }

  internal enum TWorkflowDataType
  {
    wdtInteger,
    wdtFloat,
    wdtString,
    wdtPick,
    wdtDateTime,
    wdtBoolean,
    wdtTask,
    wdtJob,
    wdtFolder,
    wdtEDocument,
    wdtReferenceRecord,
    wdtUser,
    wdtGroup,
    wdtRole,
    wdtIntegerCollection,
    wdtFloatCollection,
    wdtStringCollection,
    wdtPickCollection,
    wdtDateTimeCollection,
    wdtBooleanCollection,
    wdtTaskCollection,
    wdtJobCollection,
    wdtFolderCollection,
    wdtEDocumentCollection,
    wdtReferenceRecordCollection,
    wdtUserCollection,
    wdtGroupCollection,
    wdtRoleCollection,
    wdtContents,
    wdtUserList,
    wdtSearchDescription,
    wdtDeadLine,
    wdtPickSet,
    wdtAccountCollection
  }

  internal enum TWorkImportance
  {
    wiLow,
    wiNormal,
    wiHigh
  }

  internal enum TWorkRouteType
  {
    wrtSoft,
    wrtHard
  }

  internal enum TWorkState
  {
    wsInit,
    wsRunning,
    wsDone,
    wsControlled,
    wsAborted,
    wsContinued
  }

  internal enum TWorkTextBuildingMode
  {
    wtmFull,
    wtmFromCurrent,
    wtmOnlyCurrent
  }
}