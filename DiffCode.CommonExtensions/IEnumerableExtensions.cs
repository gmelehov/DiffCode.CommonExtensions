using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;






namespace DiffCode.CommonExtensions
{
  /// <summary>
  /// Статический класс с методами расширения для типов, реализующих интерфейс <see cref="IEnumerable{T}"/>.
  /// </summary>
  public static class IEnumerableExtensions
  {


    /// <summary>
		/// Возвращает выборку элементов списка, расположенных между элементом from и элементом to.
		/// Включает в выборку граничные элементы (from и to), если параметр includeFromTo равен <see langword="true"/> (по умолчанию).
		/// Исключает граничные элементы из выборки, если параметр includeFromTo равен <see langword="false"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">Исходный список элементов</param>
		/// <param name="from">Граничный элемент, обозначающий начало выборки</param>
		/// <param name="to">Граничный элемент, обозначающий конец выборки</param>
		/// <param name="includeFromTo">Признак включения/исключения граничных элементов из результирующей выборки</param>
		/// <returns></returns>
		public static IEnumerable<T> TakeBetween<T>(this IEnumerable<T> source, T from, T to, bool includeFromTo = true)
    {
      return (from != null && to != null && source.Contains(from) && source.Contains(to)) ?
        source
        .Reverse()
        .SkipWhile(s => !s.Equals(to))
        .Skip(includeFromTo ? 0 : 1)
        .Reverse()
        .SkipWhile(s => !s.Equals(from))
        .Skip(includeFromTo ? 0 : 1)
        :
        source;
    }


    /// <summary>
    /// Возвращает выборку элементов списка, расположенных между граничными элементами, заданными с помощью указанных функций-селекторов.
    /// Включает в выборку граничные элементы, если параметр includeFromTo равен <see langword="true"/> (по умолчанию).
    /// Исключает граничные элементы из выборки, если параметр includeFromTo равен <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">Исходный список элементов</param>
    /// <param name="fromSelector">Функция-селектор, обозначающая начало выборки</param>
    /// <param name="toSelector">Функция-селектор, обозначающая конец выборки</param>
    /// <param name="includeFromTo">Признак включения/исключения граничных элементов из результирующей выборки</param>
    /// <returns></returns>
    public static IEnumerable<T> TakeBetween<T>(this IEnumerable<T> source, Func<T, bool> fromSelector, Func<T, bool> toSelector, bool includeFromTo = true)
    {
      return (fromSelector != null && toSelector != null) ?
        source
        .Reverse()
        .SkipWhile(toSelector)
        .Skip(includeFromTo ? 0 : 1)
        .Reverse()
        .SkipWhile(fromSelector)
        .Skip(includeFromTo ? 0 : 1)
        :
        source;
    }


    /// <summary>
    /// Возвращает выборку элементов списка, расположенных между указанным начальным элементом и 
    /// конечным элементом, заданным с помощью указанной функции-селектора.
    /// Включает в выборку граничные элементы, если параметр includeFromTo равен <see langword="true"/> (по умолчанию).
    /// Исключает граничные элементы из выборки, если параметр includeFromTo равен <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">Исходный список элементов</param>
    /// <param name="from">Граничный элемент, обозначающий начало выборки</param>
    /// <param name="toSelector">Функция-селектор, обозначающая конец выборки</param>
    /// <param name="includeFromTo">Признак включения/исключения граничных элементов из результирующей выборки</param>
    /// <returns></returns>
    public static IEnumerable<T> TakeBetween<T>(this IEnumerable<T> source, T from, Func<T, bool> toSelector, bool includeFromTo = true)
    {
      return (from != null && toSelector != null && source.Contains(from)) ?
        source
        .Reverse()
        .SkipWhile(toSelector)
        .Skip(includeFromTo ? 0 : 1)
        .Reverse()
        .SkipWhile(s => !s.Equals(from))
        .Skip(includeFromTo ? 0 : 1)
        :
        source;
    }


    /// <summary>
    /// Разделяет исходный список source на блоки, 
    /// используя указанный предикат splitterSelector для определения элементов-разделителей
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">Исходный список</param>
    /// <param name="splitterSelector">Предикат для выбора разделителей списка</param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, Func<T, bool> splitterSelector)
    {
      return
        source
        .GroupBy(
          z => splitterSelector(z) || z.Equals(source.First()),
          h =>
            source
            .SkipWhile(t => !t.Equals(h))
            .Skip(h.Equals(source.First()) ? 0 : 1)
            .TakeWhile(j => !splitterSelector(j)))
        .Where(ww => ww.Key)
        .SelectMany(x => x.Select(xx => xx.Select(xxx => xxx)));
    }


    /// <summary>
    /// Возвращает выборку всех элементов списка, кроме его последнего элемента.
    /// </summary>
    /// <typeparam name="T">Тип элементов списка.</typeparam>
    /// <param name="source">Исходный список.</param>
    /// <returns></returns>
    public static IEnumerable<T> ExceptLast<T>(this IEnumerable<T> source) => source.Reverse().Skip(1).Reverse();


    /// <summary>
    /// Возвращает выборку всех элементов списка, кроме его первого элемента.
    /// </summary>
    /// <typeparam name="T">Тип элементов списка.</typeparam>
    /// <param name="source">Исходный список.</param>
    /// <returns></returns>
    public static IEnumerable<T> ExceptFirst<T>(this IEnumerable<T> source) => source.Skip(1);


    /// <summary>
    /// Возвращает выборку всех элементов списка, кроме соответствующих указанному предикату predicate.
    /// </summary>
    /// <typeparam name="T">Тип элементов списка.</typeparam>
    /// <param name="source">Исходный список.</param>
    /// <param name="predicate">Предикат для определения элементов списка, не участвующих в результирующей выборке.</param>
    /// <returns></returns>
    public static IEnumerable<T> Except<T>(this IEnumerable<T> source, Expression<Func<T, bool>> predicate) => source.AsQueryable().Where(predicate.Not());


    /// <summary>
    /// Возвращает выборку всех элементов списка, 
    /// кроме указанного количества элементов из начала списка 
    /// и указанного количества элементов из конца списка.
    /// По умолчанию количество пропускаемых элементов в начале и конце списка
    /// одинаково и равно единице.
    /// </summary>
    /// <typeparam name="T">Тип элементов списка.</typeparam>
    /// <param name="source">Исходный список.</param>
    /// <param name="trimLeft">Количество пропускаемых в начале списка элементов.</param>
    /// <param name="trimRight">Количество пропускаемых в конце списка элементов.</param>
    /// <returns></returns>
    public static IEnumerable<T> Trim<T>(this IEnumerable<T> source, int trimLeft = 1, int trimRight = 1) => source.Reverse().Skip(trimRight).Reverse().Skip(trimLeft);










    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="total"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> src, int total, int page) => total > 0 ? src.Skip((page - 1) * total).Take(total) : src;




  }
}
