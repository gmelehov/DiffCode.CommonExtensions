using System.Linq;

using DiffCode.CommonExtensions.Enums;





namespace DiffCode.CommonExtensions.Utils
{
  /// <summary>
  /// Конфигуратор фильтра-предиката для типов, реализующих интерфейс <see cref="IQueryable{T}"/>.
  /// </summary>
  public class QFilter
  {

    /// <summary>
    /// Конструктор, вызываемый при создании фильтра-предиката 
    /// для простых (не составных) типов данных, не имеющих определений свойств.
    /// </summary>
    /// <param name="comparison">Тип операции сравнения.</param>
    /// <param name="val">Объект, с которым сравнивается значение элементов списка.</param>
    public QFilter(ComparisonTypeEnum comparison, object val)
    {
      PropName = null;
      Comparison = comparison;
      Value = val;
    }

    /// <summary>
    /// Конструктор, вызываемый при создании фильтра-предиката
    /// для составных типов данных, содержащих определение свойства <paramref name="propName"/>.
    /// </summary>
    /// <param name="propName">Наименование свойства объекта типа <see cref="{T}"/>, по которому необходимо выполнить фильтрацию.</param>
    /// <param name="comparison">Тип операции сравнения.</param>
    /// <param name="val">Объект, с которым сравнивается значение свойства <paramref name="propName"/>.</param>
    public QFilter(string propName, ComparisonTypeEnum comparison, object val)
    {
      PropName = propName;
      Comparison = comparison;
      Value = val;
    }












    /// <summary>
    /// Наименование свойства объекта типа <see cref="{T}"/>,
    /// по которому необходимо выполнить фильтрацию.
    /// </summary>
    public string PropName { get; }

    /// <summary>
    /// Тип сравнения.
    /// </summary>
    public ComparisonTypeEnum Comparison { get; }

    /// <summary>
    /// Объект, с которым сравнивается значение свойства <see cref="PropName"/>.
    /// </summary>
    public object Value { get; }

  }
}
