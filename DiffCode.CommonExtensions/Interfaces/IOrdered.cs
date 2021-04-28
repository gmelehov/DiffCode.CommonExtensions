namespace DiffCode.CommonExtensions.Interfaces
{
  /// <summary>
  /// Интерфейс для объекта, имеющего установленный порядок следования
  /// </summary>
  public interface IOrdered
  {

    /// <summary>
    /// Порядок следования объекта.
    /// </summary>
    int Order { get; set; }


    /// <summary>
    /// Обновляет порядок следования объекта.
    /// </summary>
    void ReOrder();

  }
}
