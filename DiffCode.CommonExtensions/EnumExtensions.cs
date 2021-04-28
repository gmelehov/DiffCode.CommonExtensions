using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;





namespace DiffCode.CommonExtensions
{
  /// <summary>
  /// Статический класс с методами расширения для типов <see cref="Enum"/>.
  /// </summary>
  public static class EnumExtensions
  {


    /// <summary>
    /// 
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetNamesFrom(this Enum enumValue) => enumValue.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static IEnumerable<long> GetValuesFrom(this Enum enumValue) => GetNamesFrom(enumValue).Select(s => (long)Enum.Parse(enumValue.GetType(), s));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static IEnumerable<Enum> GetEnumsFrom(this Enum enumValue) => GetNamesFrom(enumValue).Select(s => (Enum)Enum.Parse(enumValue.GetType(), s));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumValue"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static T AddFlags<T>(this T enumValue, params string[] flags) where T : struct, Enum
    {
      var names = string.Join(", ", enumValue.GetNamesFrom().Concat(flags).Distinct());
      bool success = Enum.TryParse<T>(names, out T result);
      return success ? result : enumValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumValue"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static T AddFlags<T>(this T enumValue, IEnumerable<string> flags) where T : struct, Enum => enumValue.AddFlags(flags.ToArray());

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumValue"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static T RemoveFlags<T>(this T enumValue, params string[] flags) where T : struct, Enum
    {
      var names = string.Join(", ", enumValue.GetNamesFrom().Except(flags));
      if(!names.IsUseful())
      {
        names = default(T).ToString();
      };

      bool success = Enum.TryParse(names, out T result);
      return success ? result : enumValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumValue"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static T RemoveFlags<T>(this T enumValue, IEnumerable<string> flags) where T : struct, Enum => enumValue.RemoveFlags(flags.ToArray());






    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAttr"></typeparam>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static IEnumerable<TAttr> GetAttributeFrom<TAttr>(this Enum enumValue) where TAttr : Attribute => GetEnumsFrom(enumValue).Select(s => (TAttr)Attribute.GetCustomAttribute(s.GetType().GetField(s.ToString()), typeof(TAttr)));









  }
}
