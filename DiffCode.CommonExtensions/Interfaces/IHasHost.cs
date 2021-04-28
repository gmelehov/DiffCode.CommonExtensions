namespace DiffCode.CommonExtensions.Interfaces
{
  /// <summary>
  /// Интерфейс для объекта, имеющего родительский узел.
  /// </summary>
  /// <typeparam name="T">Тип родительского узла.</typeparam>
  public interface IHasHost<T>
  {

    /// <summary>
    /// Родительский узел.
    /// </summary>
    T Host { get; }

    /// <summary>
    /// Метод для проверки существования родительского узла.
    /// </summary>
    /// <returns></returns>
    bool HasHost();

  }
}
