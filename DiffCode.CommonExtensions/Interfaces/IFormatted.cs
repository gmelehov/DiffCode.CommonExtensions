using System;
using System.Collections.Generic;
using System.Text;





namespace DiffCode.CommonExtensions.Interfaces
{
  /// <summary>
	/// Интерфейс для объекта, имеющего форматированное строковое представление.
	/// </summary>
	public interface IFormatted
  {

    /// <summary>
    /// Строка отступа.
    /// </summary>
    string IndentString { get; }

    /// <summary>
    /// Разделитель дочерних элементов.
    /// </summary>
    string ItemsDivider { get; }

    /// <summary>
    /// Форматированное строковое представление объекта.
    /// </summary>
    string Formatted { get; }

  }
}
