using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;





namespace DiffCode.CommonExtensions.Utils
{
  /// <summary>
  /// 
  /// </summary>
  public static class PropertyAccessorCache<T>
  {
    static PropertyAccessorCache()
    {
      Type t = typeof(T);
      ParameterExpression param = Expression.Parameter(t, "p");

      try
      {
        foreach (var prop in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
          var propAccess = Expression.MakeMemberAccess(param, prop);
          var lambda = Expression.Lambda(propAccess, param);
          Lambdas[prop.Name] = lambda;
        }
      }
      catch (Exception ex)
      {
        
      };

    }




    public static LambdaExpression SimpleLambda => Expression.Lambda(Expression.Variable(typeof(T), "p"), Expression.Parameter(typeof(T), "p"));



    public static Dictionary<string, LambdaExpression> Lambdas = new Dictionary<string, LambdaExpression>();

  }
}
