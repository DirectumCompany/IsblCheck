using System.Xml.Serialization;

namespace IsblCheck.Context.Development.Package.Models
{
  /// <summary>
  /// Модель детальных разделов.
  /// </summary>
  public class DataSetsModel
  {
    /// <summary>
    /// 1 детальный раздел.
    /// </summary>
    [XmlElement("DetailDataSet1")]
    public DataSetModel DetailDataSet1 { get; set; }

    /// <summary>
    /// 2 детальный раздел.
    /// </summary>
    [XmlElement("DetailDataSet2")]
    public DataSetModel DetailDataSet2 { get; set; }

    /// <summary>
    /// 3 детальный раздел.
    /// </summary>
    [XmlElement("DetailDataSet3")]
    public DataSetModel DetailDataSet3 { get; set; }

    /// <summary>
    /// 4 детальный раздел.
    /// </summary>
    [XmlElement("DetailDataSet4")]
    public DataSetModel DetailDataSet4 { get; set; }

    /// <summary>
    /// 5 детальный раздел.
    /// </summary>
    [XmlElement("DetailDataSet5")]
    public DataSetModel DetailDataSet5 { get; set; }

    /// <summary>
    /// 6 детальный раздел.
    /// </summary>
    [XmlElement("DetailDataSet6")]
    public DataSetModel DetailDataSet6 { get; set; }

    /// <summary>
    /// 7 детальный раздел.
    /// </summary>
    [XmlElement("DetailDataSet7")]
    public DataSetModel DetailDataSet7 { get; set; }

    /// <summary>
    /// 8 детальный раздел.
    /// </summary>
    [XmlElement("DetailDataSet8")]
    public DataSetModel DetailDataSet8 { get; set; }
  }
}
