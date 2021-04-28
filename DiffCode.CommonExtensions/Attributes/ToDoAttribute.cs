using System;
using System.Collections.Generic;
using System.Text;





namespace DiffCode.CommonExtensions.Attributes
{
  /// <summary>
  /// 
  /// </summary>
  [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
  public class ToDoAttribute : Attribute
  {
    public ToDoAttribute(string text) => Text = text;




    public string Text { get; set; }
  }
}
