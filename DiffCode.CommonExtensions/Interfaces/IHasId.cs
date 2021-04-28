namespace DiffCode.CommonExtensions.Interfaces
{
  /// <summary>
  /// Интерфейс для объекта, имеющего строго типизированный идентификатор указанного типа.
  /// </summary>
  /// <typeparam name="T">Тип идентификатора.</typeparam>
  public interface IHasId<T>
  {

    /// <summary>
    /// Строго типизированный идентификатор объекта.
    /// </summary>
    T Id { get; set; }
  }
}
