namespace DiffCode.CommonExtensions.Interfaces
{
  /// <summary>
  /// Интерфейс для объекта, имеющего строго типизированное значение указанного типа.
  /// </summary>
  /// <typeparam name="T">Тип значения.</typeparam>
  public interface IHasValue<T>
  {

    /// <summary>
    /// Метод, возвращающий собственное значение объекта.
    /// </summary>
    T Value { get; }
  }
}
