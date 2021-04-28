using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DiffCode.CommonExtensions.Utils;
using DiffCode.CommonExtensions.Enums;
using System.Reflection;





namespace DiffCode.CommonExtensions
{
  /// <summary>
  /// Статический класс с методами расширения для типов, реализующих интерфейс <see cref="IQueryable{T}"/>.
  /// </summary>
  public static class IQueryableExtensions
  {



    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="propName"></param>
    /// <param name="ascending"></param>
    /// <param name="isFirst"></param>
    /// <returns></returns>
    public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> src, string propName, bool ascending = true, bool isFirst = true)
    {
      var expr = PropertyAccessorCache<T>.Lambdas[propName];
      if (expr == null)
        return src;


      var methodName = isFirst ? ascending ? "OrderBy" : "OrderByDescending" : ascending ? "ThenBy" : "ThenByDescending";


      MethodCallExpression res = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(T), expr.ReturnType }, src.Expression, Expression.Quote(expr));
      return src.Provider.CreateQuery<T>(res);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="sorter"></param>
    /// <param name="isFirst"></param>
    /// <returns></returns>
    public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> src, QSorter sorter, bool isFirst = true) => src.ApplyOrderBy(sorter.PropName, sorter.IsAscending, isFirst);


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="sorters"></param>
    /// <returns></returns>
    public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> src, params QSorter[] sorters)
    {
      src = src.ApplyOrderBy(sorters[0].PropName, sorters[0].IsAscending, true);
      foreach (var sorter in sorters.Skip(1))
      {
        src = src.ApplyOrderBy(sorter.PropName, sorter.IsAscending, false);
      };
      return src;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="sorters"></param>
    /// <returns></returns>
    public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> src, IEnumerable<QSorter> sorters) => src.ApplyOrderBy(sorters.ToArray());


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="sorters"></param>
    /// <returns></returns>
    public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> src, params (string, bool)[] sorters)
    {
      src = src.ApplyOrderBy(sorters[0].Item1, sorters[0].Item2, true);
      foreach(var sorter in sorters.Skip(1))
      {
        src = src.ApplyOrderBy(sorter.Item1, sorter.Item2, false);
      };
      return src;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="propName"></param>
    /// <param name="comparison"></param>
    /// <param name="val"></param>
    /// <returns></returns>
    public static IQueryable<T> ApplyFilterBy<T>(this IQueryable<T> src, string propName, ComparisonTypeEnum comparison, object val) 
    {

      var expr = string.IsNullOrWhiteSpace(propName) ? PropertyAccessorCache<T>.SimpleLambda : PropertyAccessorCache<T>.Lambdas[propName];
      if (expr == null)
        return src;

      Type valType = val.GetType();
      Expression compare = null;
      MethodInfo method = null;
      var left = string.IsNullOrWhiteSpace(propName) ? expr.Parameters[0] : expr.Body;
      Expression right = null;


      // Если значение свойства сравнивается со строкой...
      if (Type.GetTypeCode(valType).Equals(TypeCode.String) && Type.GetTypeCode(left.Type).Equals(TypeCode.String))
      {
        right = Expression.Constant(val.ToString(), typeof(string));

        switch (comparison) 
        {
          case ComparisonTypeEnum.Equals:    
            compare = Expression.Equal(left, right);
            break;

          case ComparisonTypeEnum.NotEquals:
            compare = Expression.NotEqual(left, right);
            break;

          case ComparisonTypeEnum.Contains:
            method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            compare = Expression.Call(left, method, Expression.Constant(val.ToString(), typeof(string)));
            break;

          case ComparisonTypeEnum.NotContains:
            method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            compare = Expression.Not(Expression.Call(left, method, Expression.Constant(val.ToString(), typeof(string))));
            break;

          case ComparisonTypeEnum.StartsWith:
            method = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
            compare = Expression.Call(left, method, Expression.Constant(val.ToString(), typeof(string)));
            break;

          case ComparisonTypeEnum.NotStartsWith:
            method = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
            compare = Expression.Not(Expression.Call(left, method, Expression.Constant(val.ToString(), typeof(string))));
            break;

          case ComparisonTypeEnum.EndsWith:
            method = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
            compare = Expression.Call(left, method, Expression.Constant(val.ToString(), typeof(string)));
            break;

          case ComparisonTypeEnum.NotEndsWith:
            method = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
            compare = Expression.Not(Expression.Call(left, method, Expression.Constant(val.ToString(), typeof(string))));
            break;



          default:
            return src;
        };

        var lambda = Expression.Lambda<Func<T, bool>>(compare, expr.Parameters);
        return src.Where(lambda);
      }
      // Иначе, если значение свойства сравнивается с булевой константой...
      else if (Type.GetTypeCode(valType).Equals(TypeCode.Boolean) && Type.GetTypeCode(left.Type).Equals(TypeCode.Boolean))
      {
        var valAsBool = Boolean.Parse(val.ToString());
        right = Expression.Constant(valAsBool, typeof(Boolean));

        switch (comparison)
        {
          case ComparisonTypeEnum.Equals:
            compare = Expression.Equal(left, right);
            break;

          case ComparisonTypeEnum.NotEquals:
            compare = Expression.NotEqual(left, right);
            break;


          default:
            return src;
        };

        var lambda = Expression.Lambda<Func<T, bool>>(compare, expr.Parameters);
        return src.Where(lambda);
      }
      // Иначе, если значение свойства сравнивается с числом или датой...
      else if ((TypeHelper.IsNumeric(valType) && TypeHelper.IsNumeric(left.Type)) || (TypeHelper.IsDateTime(valType) && TypeHelper.IsDateTime(left.Type)))
      {
        switch (Type.GetTypeCode(expr.ReturnType))
        {
          case TypeCode.DateTime: right = Expression.Constant((DateTime)val, typeof(DateTime)); break;

          case TypeCode.Byte: right = Expression.Constant((byte)Decimal.Parse(val.ToString()), left.Type); break;
          case TypeCode.Decimal: right = Expression.Constant(Decimal.Parse(val.ToString()), left.Type); break;
          case TypeCode.Double: right = Expression.Constant(Double.Parse(val.ToString()), left.Type); break;
          case TypeCode.Int16: right = Expression.Constant((Int16)Decimal.Parse(val.ToString()), left.Type); break;
          case TypeCode.Int32: right = Expression.Constant((Int32)Decimal.Parse(val.ToString()), left.Type); break;
          case TypeCode.Int64: right = Expression.Constant((Int64)Decimal.Parse(val.ToString()), left.Type); break;
          case TypeCode.SByte: right = Expression.Constant((SByte)Decimal.Parse(val.ToString()), left.Type); break;
          case TypeCode.Single: right = Expression.Constant((Single)Decimal.Parse(val.ToString()), left.Type); break;
          case TypeCode.UInt16: right = Expression.Constant((UInt16)Decimal.Parse(val.ToString()), left.Type); break;
          case TypeCode.UInt32: right = Expression.Constant((UInt32)Decimal.Parse(val.ToString()), left.Type); break;
          case TypeCode.UInt64: right = Expression.Constant((UInt64)Decimal.Parse(val.ToString()), left.Type); break;


          default: break;
        };


        switch (comparison)
        {
          case ComparisonTypeEnum.Equals:
            compare = Expression.Equal(left, right);
            break;

          case ComparisonTypeEnum.NotEquals:
            compare = Expression.NotEqual(left, right);
            break;

          case ComparisonTypeEnum.LesserThan:
            compare = Expression.LessThan(left, right);
            break;

          case ComparisonTypeEnum.LesserOrEquals:
            compare = Expression.LessThanOrEqual(left, right);
            break;

          case ComparisonTypeEnum.GreaterThan:
            compare = Expression.GreaterThan(left, right);
            break;

          case ComparisonTypeEnum.GreaterOrEquals:
            compare = Expression.GreaterThanOrEqual(left, right);
            break;


          default:
            return src;
        };

        var lambda = Expression.Lambda<Func<T, bool>>(compare, expr.Parameters);
        return src.Where(lambda);
      }

      else if (TypeHelper.IsArrayOfInts(valType))
      {
        switch (comparison)
        {
          case ComparisonTypeEnum.MonthIsIn:
            if (TypeHelper.IsDateTime(left.Type))
            {
              left = Expression.Property(expr.Body, "Month");
              right = Expression.Constant(val, typeof(int[]));
              compare = Expression.Call(typeof(Enumerable), "Contains", new Type[] { typeof(int) }, right, left);
            };
            break;

          case ComparisonTypeEnum.IsIn:
            if (TypeHelper.IsNumeric(left.Type))
            {
              right = Expression.Constant(val, typeof(int[]));
              compare = Expression.Call(typeof(Enumerable), "Contains", new Type[] { typeof(int) }, right, left);
            };
            break;


          default: break;
        };

        var lambda = Expression.Lambda<Func<T, bool>>(compare, expr.Parameters);
        return src.Where(lambda);
      }
      else if (TypeHelper.IsArrayOfStrings(valType) && Type.GetTypeCode(left.Type).Equals(TypeCode.String))
      {
        switch (comparison)
        {
          case ComparisonTypeEnum.IsIn:
            right = Expression.Constant(val, typeof(string[]));
            compare = Expression.Call(typeof(Enumerable), "Contains", new Type[] { typeof(string) }, right, left);
            break;


          default: break;
        };

        var lambda = Expression.Lambda<Func<T, bool>>(compare, expr.Parameters);
        return src.Where(lambda);
      }
      // В любом другом случае...
      else
      {
        return src;
      };
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="filters"></param>
    /// <returns></returns>
    public static IQueryable<T> ApplyFilterBy<T>(this IQueryable<T> src, IEnumerable<(string, ComparisonTypeEnum, object)> filters)
    {
      foreach(var filter in filters)
      {
        src = src.ApplyFilterBy(filter.Item1, filter.Item2, filter.Item3);
      };
      return src;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="filters"></param>
    /// <returns></returns>
    public static IQueryable<T> ApplyFilterBy<T>(this IQueryable<T> src, params (string, ComparisonTypeEnum, object)[] filters) => src.ApplyFilterBy(filters);


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public static IQueryable<T> ApplyFilterBy<T>(this IQueryable<T> src, QFilter filter) => src.ApplyFilterBy(filter.PropName, filter.Comparison, filter.Value);


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="filters"></param>
    /// <returns></returns>
    public static IQueryable<T> ApplyFilterBy<T>(this IQueryable<T> src, params QFilter[] filters)
    {
      foreach(var filter in filters)
      {
        src = src.ApplyFilterBy(filter);
      };
      return src;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="filters"></param>
    /// <returns></returns>
    public static IQueryable<T> ApplyFilterBy<T>(this IQueryable<T> src, IEnumerable<QFilter> filters) => src.ApplyFilterBy(filters.ToArray());
    

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="src"></param>
    /// <param name="total"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public static IQueryable<T> Paginate<T>(this IQueryable<T> src, int total, int page) => total > 0 ? src.Skip((page - 1) * total).Take(total) : src;








    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TParam"></typeparam>
    /// <param name="src"></param>
    /// <param name="prop"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    public static IQueryable<T> Where<T, TParam>(this IQueryable<T> src, Expression<Func<T, TParam>> prop, Expression<Func<TParam, bool>> where) => src.Where(prop.Compose(where));








  }
}
