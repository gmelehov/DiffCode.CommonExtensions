namespace DiffCode.CommonExtensions.Interfaces
{
  /// <summary>
  /// Интерфейс для объекта, имеющего строго типизированный префикс.
  /// </summary>
  /// <typeparam name="T">Тип префикса.</typeparam>
  public interface IHasPrefix<T>
  {

    /// <summary>
    /// Префикс.
    /// </summary>
    T Prefix { get; }
  }
}
