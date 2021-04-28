using System.Xml.Linq;




namespace DiffCode.CommonExtensions.Interfaces
{
  /// <summary>
  /// Интерфейс объекта, имеющего XML-представление.
  /// </summary>
  public interface IAsXml
  {

    /// <summary>
    /// XML-представление объекта.
    /// </summary>
    XObject AsXml { get; }

  }
}
