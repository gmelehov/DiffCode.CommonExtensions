using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;






namespace DiffCode.CommonExtensions.Interfaces
{
  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface ISpecification<T>
  {

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Expression<Func<T, bool>> IsSatisfiedBy();
  }
}
