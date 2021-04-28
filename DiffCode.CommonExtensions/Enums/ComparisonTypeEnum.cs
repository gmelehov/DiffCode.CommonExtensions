namespace DiffCode.CommonExtensions.Enums
{
  /// <summary>
  /// Типы операций сравнения.
  /// </summary>
  public enum ComparisonTypeEnum
  {

    /// <summary>
    /// Равно.
    /// </summary>
    Equals = 0,

    /// <summary>
    /// Не равно.
    /// </summary>
    NotEquals = 1,

    /// <summary>
    /// Содержит/включает в себя.
    /// </summary>
    Contains,

    /// <summary>
    /// Не содержит/не включает в себя.
    /// </summary>
    NotContains,

    /// <summary>
    /// Начинается с.
    /// </summary>
    StartsWith,
    
    /// <summary>
    /// Не начинается с.
    /// </summary>
    NotStartsWith,

    /// <summary>
    /// Заканчивается на.
    /// </summary>
    EndsWith,

    /// <summary>
    /// Не заканчивается на.
    /// </summary>
    NotEndsWith,

    /// <summary>
    /// Меньше, чем.
    /// </summary>
    LesserThan,

    /// <summary>
    /// Меньше или равно.
    /// </summary>
    LesserOrEquals,
    
    /// <summary>
    /// Больше, чем.
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Больше или равно.
    /// </summary>
    GreaterOrEquals,





    /// <summary>
    /// Находится в списке значений.
    /// </summary>
    IsIn,

    /// <summary>
    /// Месяц даты находится в списке значений.
    /// </summary>
    MonthIsIn,







  }
}
