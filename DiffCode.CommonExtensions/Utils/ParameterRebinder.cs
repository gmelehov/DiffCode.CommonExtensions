using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;





namespace DiffCode.CommonExtensions.Utils
{
  /// <summary>
  /// 
  /// </summary>
  public class ParameterRebinder : ExpressionVisitor
  {
    private readonly Dictionary<ParameterExpression, ParameterExpression> map;




    public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map) => this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();







    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <param name="p"><inheritdoc /></param>
    /// <returns></returns>
    protected override Expression VisitParameter(ParameterExpression p)
    {
      if (map.TryGetValue(p, out ParameterExpression replacement))
        p = replacement;

      return base.VisitParameter(p);
    }








    public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp) => new ParameterRebinder(map).Visit(exp);

  }
}
