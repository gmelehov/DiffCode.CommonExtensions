using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DiffCode.CommonExtensions.Enums;





namespace DiffCode.CommonExtensions.Utils
{
  /// <summary>
  /// Конфигуратор сортировки для типов, реализующих интерфейс <see cref="IQueryable{T}"/>.
  /// </summary>
  public class QSorter
  {
    public QSorter(string propName, bool isAscending = true)
    {
      PropName = propName;
      IsAscending = isAscending;
    }







    /// <summary>
    /// Наименование свойства объекта типа <see cref="{T}"/>,
    /// по которому необходимо выполнить сортировку.
    /// </summary>
    public string PropName { get; }

    /// <summary>
    /// Направление сортировки.
    /// В случае <see langword="true"/> производится сортировка по возрастанию,
    /// в случае <see langword="false"/> - по убыванию.
    /// По умолчанию равно <see langword="true"/>.
    /// </summary>
    public bool IsAscending { get; }

  }
}
