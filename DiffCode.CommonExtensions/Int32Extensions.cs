using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;






namespace DiffCode.CommonExtensions
{
  /// <summary>
  /// Статический класс с методами расширения для типа <see cref="int"/>.
  /// </summary>
  public static class Int32Extensions
  {



    /// <summary>
    /// 
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static IEnumerable<int> SplitByPowersOfTwo(this int val)
    {
      var bin = Convert.ToString(Math.Abs(val), 2);
      return bin.Select((s, i) => (bin.Length - 1 - i, int.Parse(s.ToString()))).Where(w => w.Item2 == 1).Select(ss => (int)Math.Pow(2, ss.Item1));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="val"></param>
    /// <returns></returns>
    public static T ToFlaggedEnum<T>(this int val) where T : struct, Enum
    {
      var splittedVal = val.SplitByPowersOfTwo();
      var vals =
        Enum
        .GetNames(typeof(T))
        .Select(s =>
        {
          bool success = Enum.TryParse<T>(s, out T res);
          return (s, success, Convert.ToInt32(res));
        });

      var maxVal = vals.Max(m => m.Item3);

      if (val <= maxVal)
      {
        var stringVals = vals.Where(w => w.success == true && splittedVal.Contains(w.Item3)).Select(ss => ss.s);
        var joinedVals = string.Join(", ", stringVals);
        bool success = Enum.TryParse<T>(joinedVals, out T result);
        return success ? result : default;
      }
      else
      {
        return default;
      }
    }










  }
}
