using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using DiffCode.CommonExtensions.Utils;
using DiffCode.CommonExtensions.Interfaces;







namespace DiffCode.CommonExtensions
{
  /// <summary>
  /// Статический класс с методами расширения для деревьев выражений <see cref="Expression"/>.
  /// </summary>
  public static class ExpressionExtensions
  {





    public static Spec<T> AsSpec<T>(this Expression<Func<T, bool>> expr) where T : class => new Spec<T>(expr);










    public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
    {
      var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
      var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
      return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
    }
    public static Expression<T> Compose<T>(this Expression<T> first, Func<Expression, Expression> merge) => Expression.Lambda<T>(merge(first.Body), first.Parameters);
    public static Expression<Func<X, Z>> Compose<X, Y, Z>(this Expression<Func<X, Y>> fn, Expression<Func<Y, Z>> func)
    {
      var x = Expression.Parameter(typeof(X), "x");
      var invokedFn = Expression.Invoke(fn, x);
      var invokedFunc = Expression.Invoke(func, invokedFn);
      return Expression.Lambda<Func<X, Z>>(invokedFunc, x);
    }






    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) => first.Compose(second, Expression.AndAlso);
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) => first.Compose(second, Expression.OrElse);
    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> first) => first.Compose(Expression.Not);
    public static Expression<Func<T1, T2, bool>> And<T1, T2>(this Expression<Func<T1, T2, bool>> first, Expression<Func<T1, T2, bool>> second) => first.Compose(second, Expression.AndAlso);
    public static Expression<Func<T1, T2, bool>> Or<T1, T2>(this Expression<Func<T1, T2, bool>> first, Expression<Func<T1, T2, bool>> second) => first.Compose(second, Expression.OrElse);
    public static Expression<Func<T1, T2, bool>> Not<T1, T2>(this Expression<Func<T1, T2, bool>> first) => first.Compose(Expression.Not);
    public static Expression<Func<T1, T2, T3, bool>> And<T1, T2, T3>(this Expression<Func<T1, T2, T3, bool>> first, Expression<Func<T1, T2, T3, bool>> second) => first.Compose(second, Expression.AndAlso);
    public static Expression<Func<T1, T2, T3, bool>> Or<T1, T2, T3>(this Expression<Func<T1, T2, T3, bool>> first, Expression<Func<T1, T2, T3, bool>> second) => first.Compose(second, Expression.OrElse);
    public static Expression<Func<T1, T2, T3, bool>> Not<T1, T2, T3>(this Expression<Func<T1, T2, T3, bool>> first) => first.Compose(Expression.Not);



    public static Expression<Func<T1, T2, T3, bool>> And<T1, T2, T3>(this Expression<Func<T1, bool>> first, Expression<Func<T1, T2, T3, bool>> second)
    {
      var prm1 = Expression.Parameter(typeof(T1), "prm1");
      var prm2 = Expression.Parameter(typeof(T2), "prm2");
      var prm3 = Expression.Parameter(typeof(T3), "prm3");
      Expression<Func<T1, T2, T3, bool>> lambda = Expression.Lambda<Func<T1, T2, T3, bool>>(first, prm1, prm2, prm3);
      return lambda.And(second);
    }







    public static Expression<Func<T, TResult>> ApplyTo<T, TResult>(this Expression<Func<T, TResult>> func, Expression<Func<T, T>> converter)
    {
      var prm = Expression.Parameter(typeof(T), "prm");
      var converterInvoked = Expression.Invoke(converter, prm);
      var funcInvoked = Expression.Invoke(func, converterInvoked);
      return Expression.Lambda<Func<T, TResult>>(funcInvoked, prm);
    }
    public static Expression<Func<T1, T2, TResult>> ApplyTo<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> func, Expression<Func<T1, T1>> converter)
    {
      var prm = Expression.Parameter(typeof(T1), "prm");
      var converterInvoked = Expression.Invoke(converter, prm);
      var funcInvoked = Expression.Invoke(func, converterInvoked);
      return Expression.Lambda<Func<T1, T2, TResult>>(funcInvoked, prm);
    }
    public static Expression<Func<T1, T2, TResult>> ApplyTo<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> func, Expression<Func<T2, T2>> converter)
    {
      var prm = Expression.Parameter(typeof(T2), "prm");
      var converterInvoked = Expression.Invoke(converter, prm);
      var funcInvoked = Expression.Invoke(func, converterInvoked);
      return Expression.Lambda<Func<T1, T2, TResult>>(funcInvoked, prm);
    }
    public static Expression<Func<T1, T2, T3, TResult>> ApplyTo<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> func, Expression<Func<T1, T1>> converter)
    {
      var prm = Expression.Parameter(typeof(T1), "prm");
      var converterInvoked = Expression.Invoke(converter, prm);
      var funcInvoked = Expression.Invoke(func, converterInvoked);
      return Expression.Lambda<Func<T1, T2, T3, TResult>>(funcInvoked, prm);
    }
    public static Expression<Func<T1, T2, T3, TResult>> ApplyTo<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> func, Expression<Func<T2, T2>> converter)
    {
      var prm = Expression.Parameter(typeof(T2), "prm");
      var converterInvoked = Expression.Invoke(converter, prm);
      var funcInvoked = Expression.Invoke(func, converterInvoked);
      return Expression.Lambda<Func<T1, T2, T3, TResult>>(funcInvoked, prm);
    }
    public static Expression<Func<T1, T2, T3, TResult>> ApplyTo<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> func, Expression<Func<T3, T3>> converter)
    {
      var prm = Expression.Parameter(typeof(T3), "prm");
      var converterInvoked = Expression.Invoke(converter, prm);
      var funcInvoked = Expression.Invoke(func, converterInvoked);
      return Expression.Lambda<Func<T1, T2, T3, TResult>>(funcInvoked, prm);
    }










    public static Expression<Func<TResult>> PApply<T1, TResult>(this Expression<Func<T1, TResult>> func, T1 arg)
    {
      var const_ = Expression.Constant(arg, typeof(T1));
      var partInvoked = Expression.Invoke(func, const_);
      return Expression.Lambda<Func<TResult>>(partInvoked);
    }
    public static Expression<Func<TResult>> PApply<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> func, T1 arg1, T2 arg2)
    {
      var const1 = Expression.Constant(arg1, typeof(T1));
      var const2 = Expression.Constant(arg2, typeof(T2));
      var partInvoked = Expression.Invoke(func, const1, const2);
      return Expression.Lambda<Func<TResult>>(partInvoked);
    }
    public static Expression<Func<TResult>> PApply<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> func, T1 arg1, T2 arg2, T3 arg3)
    {
      var const1 = Expression.Constant(arg1, typeof(T1));
      var const2 = Expression.Constant(arg2, typeof(T2));
      var const3 = Expression.Constant(arg3, typeof(T3));
      var partInvoked = Expression.Invoke(func, const1, const2, const3);
      return Expression.Lambda<Func<TResult>>(partInvoked);
    }
    public static Expression<Func<T1, TResult>> PApply<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> func, T2 arg)
    {
      var prm = Expression.Parameter(typeof(T1), "prm");
      var const_ = Expression.Constant(arg, typeof(T2));
      var partInvoked = Expression.Invoke(func, prm, const_);
      return Expression.Lambda<Func<T1, TResult>>(partInvoked, prm);
    }
    public static Expression<Func<T2, TResult>> PApply<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> func, T1 arg)
    {
      var prm = Expression.Parameter(typeof(T2), "prm");
      var const_ = Expression.Constant(arg, typeof(T1));
      var partInvoked = Expression.Invoke(func, const_, prm);
      return Expression.Lambda<Func<T2, TResult>>(partInvoked, prm);
    }
    public static Expression<Func<T2, T3, TResult>> PApply<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> func, T1 arg)
    {
      var prm2 = Expression.Parameter(typeof(T2), "prm2");
      var prm3 = Expression.Parameter(typeof(T3), "prm3");
      var const_ = Expression.Constant(arg, typeof(T1));
      var partInvoked = Expression.Invoke(func, const_, prm2, prm3);
      return Expression.Lambda<Func<T2, T3, TResult>>(partInvoked, prm2, prm3);
    }
    public static Expression<Func<T1, T3, TResult>> PApply<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> func, T2 arg)
    {
      var prm1 = Expression.Parameter(typeof(T1), "prm1");
      var prm3 = Expression.Parameter(typeof(T3), "prm3");
      var const_ = Expression.Constant(arg, typeof(T2));
      var partInvoked = Expression.Invoke(func, prm1, const_, prm3);
      return Expression.Lambda<Func<T1, T3, TResult>>(partInvoked, prm1, prm3);
    }
    public static Expression<Func<T1, T2, TResult>> PApply<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> func, T3 arg)
    {
      var prm1 = Expression.Parameter(typeof(T1), "prm1");
      var prm2 = Expression.Parameter(typeof(T2), "prm2");
      var const_ = Expression.Constant(arg, typeof(T3));
      var partInvoked = Expression.Invoke(func, prm1, prm2, const_);
      return Expression.Lambda<Func<T1, T2, TResult>>(partInvoked, prm1, prm2);
    }
    public static Expression<Func<T1, TResult>> PApply<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> func, T2 arg2, T3 arg3)
    {
      var prm1 = Expression.Parameter(typeof(T1), "prm1");
      var const2 = Expression.Constant(arg2, typeof(T2));
      var const3 = Expression.Constant(arg3, typeof(T3));
      var partInvoked = Expression.Invoke(func, prm1, const2, const3);
      return Expression.Lambda<Func<T1, TResult>>(partInvoked, prm1);
    }
    public static Expression<Func<T2, TResult>> PApply<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> func, T1 arg1, T3 arg3)
    {
      var prm2 = Expression.Parameter(typeof(T2), "prm2");
      var const1 = Expression.Constant(arg1, typeof(T1));
      var const3 = Expression.Constant(arg3, typeof(T3));
      var partInvoked = Expression.Invoke(func, const1, prm2, const3);
      return Expression.Lambda<Func<T2, TResult>>(partInvoked, prm2);
    }
    public static Expression<Func<T3, TResult>> PApply<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> func, T1 arg1, T2 arg2)
    {
      var prm3 = Expression.Parameter(typeof(T3), "prm3");
      var const1 = Expression.Constant(arg1, typeof(T1));
      var const2 = Expression.Constant(arg2, typeof(T2));
      var partInvoked = Expression.Invoke(func, const1, const2, prm3);
      return Expression.Lambda<Func<T3, TResult>>(partInvoked, prm3);
    }






    public static Expression<Func<TEntity, TResult>> WithField<TEntity, TResult>(this Expression<Func<TEntity, TResult>> func, TEntity entity, string fieldName)
    {
      var type = typeof(TEntity);
      var field = type.GetField(fieldName);
      if (field != null)
      {
        var prm = Expression.Parameter(typeof(TEntity), "prm");
        var call = Expression.Invoke(Expression.Field(null, field), prm);
        Expression<Func<TEntity, TResult>> lambda = Expression.Lambda<Func<TEntity, TResult>>(call, prm);
        return lambda;
      }
      else
      {
        return func;
      };
    }





    public static Expression<Action<T1, T2>> AddParameter<T1, T2>(this Expression<Action<T1>> action)
    {
      var prm1 = Expression.Parameter(typeof(T1), "prm1");
      var prm2 = Expression.Parameter(typeof(T2), "prm2");
      var body = Expression.Block(action);
      Expression<Action<T1, T2>> lambda = Expression.Lambda<Action<T1, T2>>(body, prm1, prm2);
      return lambda;
    }






    public static Expression<Action> Then(this Expression<Action> first, Expression<Action> second)
    {
      var firstInvoked = Expression.Invoke(first);
      var secondInvoked = Expression.Invoke(second);
      var body = Expression.Block(firstInvoked, secondInvoked);
      Expression<Action> lambda = Expression.Lambda<Action>(body);
      return lambda;
    }
    public static Expression<Action<T>> Then<T>(this Expression<Action> first, Expression<Action<T>> second)
    {
      var prm = Expression.Parameter(typeof(T), "prm");
      var firstInvoked = Expression.Invoke(first);
      var secondInvoked = Expression.Invoke(second, prm);
      var body = Expression.Block(firstInvoked, secondInvoked);
      Expression<Action<T>> lambda = Expression.Lambda<Action<T>>(body, prm);
      return lambda;
    }
    public static Expression<Action<T>> Then<T>(this Expression<Action<T>> first, Expression<Action<T>> second)
    {
      var prm = Expression.Parameter(typeof(T), "prm");
      var firstInvoked = Expression.Invoke(first, prm);
      var secondInvoked = Expression.Invoke(second, prm);
      var body = Expression.Block(firstInvoked, secondInvoked);
      Expression<Action<T>> lambda = Expression.Lambda<Action<T>>(body, prm);
      return lambda;
    }
    public static Expression<Action<T>> ThenWith<T>(this Expression<Action<T>> first, Expression<Action<T>> second, Expression<Func<T, T>> converter)
    {
      var prm = Expression.Parameter(typeof(T), "prm");
      var firstInvoked = Expression.Invoke(first, prm);
      var converterInvoked = Expression.Invoke(converter, prm);
      var secondInvoked = Expression.Invoke(second, converterInvoked);
      var body = Expression.Block(firstInvoked, secondInvoked);
      Expression<Action<T>> lambda = Expression.Lambda<Action<T>>(body, prm);
      return lambda;
    }






    public static Expression<Func<T1, Func<T2, TResult>>> Curry<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> func) => t1 => t2 => func.Compile()(t1, t2);
    public static Expression<Func<T1, Func<T2, Func<T3, TResult>>>> Curry<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> func) => t1 => t2 => t3 => func.Compile()(t1, t2, t3);
    public static Expression<Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>>> Curry<T1, T2, T3, T4, TResult>(this Expression<Func<T1, T2, T3, T4, TResult>> func) => t1 => t2 => t3 => t4 => func.Compile()(t1, t2, t3, t4);





    public static Expression<Func<T1, T2, TResult>> UnCurry<T1, T2, TResult>(this Expression<Func<T1, Func<T2, TResult>>> func) => (t1, t2) => func.Compile()(t1)(t2);
    public static Expression<Func<T1, T2, T3, TResult>> UnCurry<T1, T2, T3, TResult>(this Expression<Func<T1, Func<T2, Func<T3, TResult>>>> func) => (t1, t2, t3) => func.Compile()(t1)(t2)(t3);
    public static Expression<Func<T1, T2, T3, T4, TResult>> UnCurry<T1, T2, T3, T4, TResult>(this Expression<Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>>> func) => (t1, t2, t3, t4) => func.Compile()(t1)(t2)(t3)(t4);




    public static Expression<Func<T, bool>> Union<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) => first.Or(second);
    public static Expression<Func<T, bool>> ISect<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second) => first.And(second);
    public static Expression<Func<T1, T2, bool>> Cartesian<T1, T2>(this Expression<Func<T1, bool>> first, Expression<Func<T2, bool>> second)
    {
      var tt1 = Expression.Parameter(typeof(T1), "t1");
      var tt2 = Expression.Parameter(typeof(T2), "t2");
      var body1 = Expression.Invoke(first, tt1);
      var body2 = Expression.Invoke(second, tt2);
      Expression<Func<T1, T2, bool>> lambda = Expression.Lambda<Func<T1, T2, bool>>(Expression.And(body1, body2), tt1, tt2);
      return lambda;
    }











    public static Expression Equal(this Expression expression1, Expression expression2)
    {
      ConvertExpressions(ref expression1, ref expression2);
      return Expression.Equal(expression1, expression2);
    }
    public static Expression NotEqual(this Expression expression1, Expression expression2)
    {
      ConvertExpressions(ref expression1, ref expression2);
      return Expression.NotEqual(expression1, expression2);
    }
    public static Expression GreaterThan(this Expression expression1, Expression expression2)
    {
      ConvertExpressions(ref expression1, ref expression2);
      return Expression.GreaterThan(expression1, expression2);
    }
    public static Expression GreaterThanOrEqual(this Expression expression1, Expression expression2)
    {
      ConvertExpressions(ref expression1, ref expression2);
      return Expression.GreaterThanOrEqual(expression1, expression2);
    }
    public static Expression LessThan(this Expression expression1, Expression expression2)
    {
      ConvertExpressions(ref expression1, ref expression2);
      return Expression.LessThan(expression1, expression2);
    }
    public static Expression LessThanOrEqual(this Expression expression1, Expression expression2)
    {
      ConvertExpressions(ref expression1, ref expression2);
      return Expression.LessThanOrEqual(expression1, expression2);
    }
    public static Expression And(this Expression expression1, Expression expression2)
    {
      ConvertExpressions(ref expression1, ref expression2);
      return Expression.And(expression1, expression2);
    }
    public static Expression Or(this Expression expression1, Expression expression2)
    {
      ConvertExpressions(ref expression1, ref expression2);
      return Expression.Or(expression1, expression2);
    }
    public static Expression Binary(this Expression expression1, ExpressionType op, Expression expression2)
    {
      ConvertExpressions(ref expression1, ref expression2);
      return Expression.MakeBinary(op, expression1, expression2);
    }
    private static void ConvertExpressions(ref Expression expression1, ref Expression expression2)
    {
      if (expression1.Type != expression2.Type)
      {
        var isNullable1 = TypeHelper.IsNullableType(expression1.Type);
        var isNullable2 = TypeHelper.IsNullableType(expression2.Type);
        if (isNullable1 || isNullable2)
        {
          if (TypeHelper.GetNonNullableType(expression1.Type) == TypeHelper.GetNonNullableType(expression2.Type))
          {
            if (!isNullable1)
            {
              expression1 = Expression.Convert(expression1, expression2.Type);
            }
            else if (!isNullable2)
            {
              expression2 = Expression.Convert(expression2, expression1.Type);
            }
          }
        }
      }
    }
    public static Expression[] Split(this Expression expression, params ExpressionType[] binarySeparators)
    {
      var list = new List<Expression>();
      Split(expression, list, binarySeparators);
      return list.ToArray();
    }
    private static void Split(Expression expression, List<Expression> list, ExpressionType[] binarySeparators)
    {
      if (expression != null)
      {
        if (binarySeparators.Contains(expression.NodeType))
        {
          var bex = expression as BinaryExpression;
          if (bex != null)
          {
            Split(bex.Left, list, binarySeparators);
            Split(bex.Right, list, binarySeparators);
          }
        }
        else
        {
          list.Add(expression);
        }
      }
    }
    public static Expression Join(this IEnumerable<Expression> list, ExpressionType binarySeparator)
    {
      if (list != null)
      {
        var array = list.ToArray();
        if (array.Length > 0)
        {
          return array.Aggregate((x1, x2) => Expression.MakeBinary(binarySeparator, x1, x2));
        }
      }
      return null;
    }




    public static Expression<Func<object, object>> CreateGetter<T>(this T entity, string propName)
    {
      var param = Expression.Parameter(typeof(object), "prm");
      var body = Expression.PropertyOrField(Expression.TypeAs(param, entity.GetType()), propName);
      var getterExpression = Expression.Lambda<Func<object, object>>(body, param);
      return getterExpression;
    }

  }
}
