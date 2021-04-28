using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;






namespace DiffCode.CommonExtensions
{
  /// <summary>
  /// Методы расширения для строк.
  /// </summary>
  public static class StringExtensions
  {


    /// <summary>
    /// Переводит первый символ строки в верхний регистр.
    /// </summary>
    /// <param name="str">Исходная строка.</param>
    /// <returns></returns>
    public static string CapitalizeFirstChar(this string str) => !string.IsNullOrWhiteSpace(str) ? $"{str[0].ToString().ToUpper()}{str.Substring(1)}" : "";


    /// <summary>
    /// Переводит первый символ строки в нижний регистр.
    /// </summary>
    /// <param name="str">Исходная строка.</param>
    /// <returns></returns>
    public static string DecapitalizeFirstChar(this string str) => !string.IsNullOrWhiteSpace(str) ? $"{str[0].ToString().ToLower()}{str.Substring(1)}" : "";


    /// <summary>
    /// Форматирует строку в kebab-case.
    /// </summary>
    /// <param name="str">Исходная строка.</param>
    /// <returns></returns>
    public static string ToKebabCase(this string str)
    {
      string pattern = "([A-Z]?[a-z]+)*";
      var splt = Regex.Matches(str, pattern)[0].Groups[1].Captures;
      var ret = new List<string>();
      foreach (Capture s in splt)
        ret.Add(s.Value.ToLower());

      return string.Join("-", ret);
    }


    /// <summary>
    /// Форматирует строку в snake_case.
    /// </summary>
    /// <param name="str">Исходная строка.</param>
    /// <returns></returns>
    public static string ToSnakeCase(this string str)
    {
      string pattern = "([A-Z]?[a-z]+)*";
      var splt = Regex.Matches(str, pattern)[0].Groups[1].Captures;
      var ret = new List<string>();
      foreach (Capture s in splt)
        ret.Add(s.Value.ToLower());

      return string.Join("_", ret);
    }


    /// <summary>
    /// Форматирует строку в camelCase.
    /// </summary>
    /// <param name="str">Исходная строка.</param>
    /// <returns></returns>
    public static string ToCamelCase(this string str)
    {
      var arr = str?.Split(new char[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.CapitalizeFirstChar()).ToArray();
      if (arr != null && arr.Count() > 0)
        arr[0] = arr[0].DecapitalizeFirstChar();
      return string.Join("", arr);
    }


    /// <summary>
    /// Форматирует строку в CamelCase.
    /// </summary>
    /// <param name="str">Исходная строка.</param>
    /// <returns></returns>
    public static string ToUpperCamelCase(this string str)
    {
      var arr = str?.Split(new char[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.CapitalizeFirstChar()).ToArray();
      if (arr != null && arr.Count() > 0)
        arr[0] = arr[0].CapitalizeFirstChar();
      return string.Join("", arr);
    }


    /// <summary>
    /// Возвращает <see langword="true"/>, если исходная строка имеет "полезное" содержимое:
    /// не пустое и не равное <see langword="null"/>.
    /// </summary>
    /// <param name="str">Исходная строка.</param>
    /// <returns></returns>
    public static bool IsUseful(this string str) => !string.IsNullOrWhiteSpace(str);


    /// <summary>
    /// Повторяет строку указанное количество раз.
    /// Если указанное количество повторений меньше или равно нулю,
    /// возвращается пустая строка (ноль повторений шаблонной строки).
    /// Если количество повторений не указано, то оно принимается равным единице
    /// и возвращается одно повторение шаблонной строки.
    /// </summary>
    /// <param name="str">Исходная строка.</param>
    /// <param name="amount">Количество повторений.</param>
    /// <returns></returns>
    public static string Repeat(this string str, int amount = 1) => amount <= 0 ? "" : string.Join("", Enumerable.Range(1, amount).Select(s => str += str));


    /// <summary>
    /// Проверяет возможность извлечения булевого значения из исходной строки, приведенной к нижнему регистру 
    /// и очищенной от начальных и конечных пробелов, кавычек, апострофов, символов перевода строки и табуляции.
    /// Возвращает <see langword="true"/>, если обработанная таким образом строка содержит значение true.
    /// Возвращает <see langword="false"/>, если обработанная таким образом строка содержит значение false.
    /// Возвращает <see langword="null"/>, если обработанная таким образом строка отличается от true/false,
    /// либо если исходная строка была пуста либо равна <see langword="null"/>.
    /// </summary>
    /// <param name="str">Исходная строка.</param>
    /// <returns></returns>
    public static bool? AsBool(this string str)
    {
      if (str.IsUseful())
      {
        var res = str.ToLower().Trim('\'', '"', ' ', '\r', '\n', '\t');
        return res == "true" ? true as bool? : res == "false" ? false as bool? : null;
      }
      else
        return null;
    }







    public static bool IsJSIdentifier(this string str)
    {
      var pattern = "[$|_|A-Z|a-z][$|_|A-Z|a-z|0-9]*";
      var match = Regex.Match(str, pattern).Value == str;
      return match;
    }






  }
}
