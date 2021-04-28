using DiffCode.CommonExtensions.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;






namespace DiffCode.CommonExtensions.Utils
{
  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public sealed class Spec<T> where T : class
  {
    public Spec(Expression<Func<T, bool>> expr)
    {
      Expression = expr;
      if (expr == null) throw new ArgumentNullException(nameof(expr));
    }







    public IQueryable<T> Apply(IQueryable<T> query) => query.Where(Expression);







    public Expression<Func<T, bool>> Expression { get; set; }






    public bool IsSatisfiedBy(T obj) => Expression.Compile()(obj);








    public static implicit operator Expression<Func<T, bool>>(Spec<T> spec) => spec.Expression;

    public static bool operator false(Spec<T> spec) => false;

    public static bool operator true(Spec<T> spec) => true;

    public static Spec<T> operator &(Spec<T> spec1, Spec<T> spec2) => new Spec<T>(spec1.Expression.And(spec2.Expression));

    public static Spec<T> operator |(Spec<T> spec1, Spec<T> spec2) => new Spec<T>(spec1.Expression.Or(spec2.Expression));

    public static Spec<T> operator !(Spec<T> spec) => new Spec<T>(spec.Expression.Not());


  }
}
