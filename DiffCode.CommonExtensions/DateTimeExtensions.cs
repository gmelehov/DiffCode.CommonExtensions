using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




namespace DiffCode.CommonExtensions
{
  /// <summary>
  /// 
  /// </summary>
  public static class DateTimeExtensions
  {


    /// <summary>
    /// Возвращает дату, на которую приходится первый в году указанный день недели.
    /// Год извлекается из указанной исходной даты.
    /// </summary>
    /// <param name="date">Исходная дата, из которой извлекается год.</param>
    /// <param name="dayOfWeek">День недели.</param>
    /// <returns></returns>
    public static DateTime GetYearFirstWeekDay(this DateTime date, DayOfWeek dayOfWeek)
    {
      var firstDay = new DateTime(date.Year, 1, 1);
      return firstDay.AddDays(Enumerable.Range(0, 7).FirstOrDefault(f => firstDay.AddDays(f).DayOfWeek.Equals(dayOfWeek)));
    }

    /// <summary>
    /// Возвращает дату, на которую приходится понедельник первой недели в году.
    /// Год извлекается из указанной исходной даты.
    /// </summary>
    /// <param name="date">Исходная дата, из которой извлекается год.</param>
    /// <returns></returns>
    public static DateTime GetYearFirstWeekMonday(this DateTime date)
    {
      var firstSunday = date.GetYearFirstWeekDay(DayOfWeek.Sunday);
      return firstSunday.DayOfYear <= 3 ? new DateTime(date.Year, 1, firstSunday.DayOfYear + 1) : new DateTime(date.Year, 1, firstSunday.DayOfYear + 1).AddDays(-7);
    }

    /// <summary>
    /// Возвращает дату, на которую приходится понедельник указанной недели в году.
    /// Год извлекается из указанной исходной даты.
    /// </summary>
    /// <param name="date">Исходная дата, из которой извлекается год.</param>
    /// <param name="weekOfYear">Номер недели в году.</param>
    /// <returns></returns>
    public static DateTime GetWeekOfYearMonday(this DateTime date, int weekOfYear) => date.GetYearFirstWeekMonday().AddDays((Math.Abs(weekOfYear) - 1) * 7);

    /// <summary>
    /// Возвращает номер недели в году, на которую приходится указанная дата.
    /// Год извлекается из указанной исходной даты.
    /// </summary>
    /// <param name="date">Исходная дата, для которой вычисляется номер соответствующей недели в году.</param>
    /// <returns></returns>
    public static int GetWeekOfYear(this DateTime date)
    {
      var firstMonday = date.GetYearFirstWeekMonday();
      var diff = date.Subtract(firstMonday).Days;
      return diff % 7 > 0 ? diff / 7 + 1 : diff / 7;
    }

    /// <summary>
    /// Возвращает признак переходящей недели, на которую приходится указанная дата.
    /// Год извлекается из указанной исходной даты.
    /// </summary>
    /// <param name="date">Исходная дата, для которой вычисляется признак принадлежности к переходящей неделе.</param>
    /// <returns></returns>
    public static bool IsTransitionWeek(this DateTime date)
    {
      var weekMonday = date.GetWeekOfYearMonday(date.GetWeekOfYear());
      var weekSunday = weekMonday.AddDays(6);
      return weekSunday.Month > weekMonday.Month;
    }

    /// <summary>
    /// Возвращает все даты, приходящиеся на указанный день каждой недели в указанном году.
    /// Год извлекается из указанной исходной даты.
    /// </summary>
    /// <param name="date">Исходная дата, из которой извлекается год.</param>
    /// <param name="dayOfWeek">День недели.</param>
    /// <returns></returns>
    public static IEnumerable<DateTime> GetYearWeekDaysOfKind(this DateTime date, DayOfWeek dayOfWeek)
    {
      var firstMonday = date.GetYearFirstWeekMonday();
      var diff = (int)dayOfWeek - 1 >= 0 ? (int)dayOfWeek - 1 : 6;
      var firstDay = firstMonday.AddDays(diff);
      return Enumerable.Range(0, 53).Select(s => firstDay.AddDays(s * 7));
    }








  }
}
