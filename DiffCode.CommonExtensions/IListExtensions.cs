using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;





namespace DiffCode.CommonExtensions
{
  /// <summary>
  /// Статический класс с методами расширения для типов, реализующих интерфейс <see cref="IList{T}"/>.
  /// </summary>
  public static class IListExtensions
  {


    /// <summary>
    /// Возвращает исходный список, в конец которого добавлен новый элемент.
    /// </summary>
    /// <typeparam name="T">Тип элементов списка.</typeparam>
    /// <param name="list">Исходный список.</param>
    /// <param name="item">Добавляемый элемент.</param>
    /// <returns></returns>
    public static IList<T> Add<T>(this IList<T> list, T item)
    {
      list.Add(item);
      return list;
    }


    /// <summary>
    /// Возвращает исходный список, в конец которого добавлены новые элементы.
    /// </summary>
    /// <typeparam name="T">Тип элементов списка.</typeparam>
    /// <param name="list">Исходный список.</param>
    /// <param name="items">Добавляемые элементы.</param>
    /// <returns></returns>
    public static IList<T> AddRange<T>(this IList<T> list, IEnumerable<T> items)
    {
      foreach (T it in items)
      {
        list.Add(it);
      };
      return list;
    }


    /// <summary>
    /// Преобразует одиночный объект в типизированный список объектов,
    /// возвращает итоговый список.
    /// </summary>
    /// <typeparam name="T">Тип объекта.</typeparam>
    /// <param name="item">Исходный объект.</param>
    /// <returns></returns>
    public static IList<T> ToMany<T>(this T item) => new List<T> { item };


    /// <summary>
    /// Преобразует одиночный объект в типизированный список объектов,
    /// добавляет к нему указанный объект того же типа,
    /// возвращает итоговый список.
    /// </summary>
    /// <typeparam name="T">Тип объекта.</typeparam>
    /// <param name="item">Исходный объект.</param>
    /// <param name="other">Добавляемый объект.</param>
    /// <returns></returns>
    public static IList<T> ToMany<T>(this T item, T other)
    {
      IList<T> ret = new List<T> { item };
      return ret.Add<T>(other);
    }


    /// <summary>
    /// Преобразует одиночный объект в типизированный список объектов,
    /// добавляет к нему указанную коллекцию объектов того же типа,
    /// возвращает итоговый список.
    /// </summary>
    /// <typeparam name="T">Тип объекта.</typeparam>
    /// <param name="item">Исходный объект.</param>
    /// <param name="items">Коллекция добавляемых объектов.</param>
    /// <returns></returns>
    public static IList<T> ToMany<T>(this T item, IEnumerable<T> items)
    {
      IList<T> ret = new List<T> { item };
      return ret.AddRange(items);
    }


  }
}
