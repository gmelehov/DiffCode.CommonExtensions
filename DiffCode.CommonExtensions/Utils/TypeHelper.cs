using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;






namespace DiffCode.CommonExtensions.Utils
{
  /// <summary>
  /// Вспомогательные методы для работы с классом <see cref="Type"/>.
  /// </summary>
  public static class TypeHelper
  {

    public static Type FindIEnumerable(Type seqType)
    {
      if (seqType == null || seqType == typeof(string))
        return null;

      if (seqType.IsArray)
        return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());

      if (seqType.IsGenericType)
      {
        foreach (Type arg in seqType.GetGenericArguments())
        {
          Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
          if (ienum.IsAssignableFrom(seqType))
            return ienum;
        }
      };

      Type[] ifaces = seqType.GetInterfaces();
      if (ifaces != null && ifaces.Length > 0)
      {
        foreach (Type iface in ifaces)
        {
          Type ienum = FindIEnumerable(iface);
          if (ienum != null)
            return ienum;
        }
      };

      if (seqType.BaseType != null && seqType.BaseType != typeof(object))
        return FindIEnumerable(seqType.BaseType);

      return null;
    }
    public static Type GetSequenceType(Type elementType) => typeof(IEnumerable<>).MakeGenericType(elementType);
    public static Type GetElementType(Type seqType)
    {
      Type ienum = FindIEnumerable(seqType);

      if (ienum == null)
        return seqType;

      return ienum.GetGenericArguments()[0];
    }
    public static bool IsNullableType(Type type) => type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    public static bool IsNullAssignable(Type type) => !type.IsValueType || IsNullableType(type);
    public static Type GetNonNullableType(Type type) => IsNullableType(type) ? type.GetGenericArguments()[0] : type;
    public static Type GetNullAssignableType(Type type) => !IsNullAssignable(type) ? typeof(Nullable<>).MakeGenericType(type) : type;
    public static ConstantExpression GetNullConstant(Type type) => Expression.Constant(null, GetNullAssignableType(type));
    public static Type GetMemberType(MemberInfo mi)
    {
      FieldInfo fi = mi as FieldInfo;

      if (fi != null)
        return fi.FieldType;

      PropertyInfo pi = mi as PropertyInfo;

      if (pi != null)
        return pi.PropertyType;

      EventInfo ei = mi as EventInfo;

      if (ei != null)
        return ei.EventHandlerType;

      MethodInfo meth = mi as MethodInfo;

      if (meth != null)
        return meth.ReturnType;

      return null;
    }
    public static object GetDefault(Type type)
    {
      bool isNullable = !type.IsValueType || TypeHelper.IsNullableType(type);
      if (!isNullable)
        return Activator.CreateInstance(type);

      return null;
    }
    public static bool IsReadOnly(MemberInfo member)
    {
      switch (member.MemberType)
      {
        case MemberTypes.Field:
          return (((FieldInfo)member).Attributes & FieldAttributes.InitOnly) != 0;
        case MemberTypes.Property:
          PropertyInfo pi = (PropertyInfo)member;
          return !pi.CanWrite || pi.GetSetMethod() == null;
        default:
          return true;
      }
    }
    public static bool IsInteger(Type type)
    {
      Type nnType = GetNonNullableType(type);
      switch (Type.GetTypeCode(nnType))
      {
        case TypeCode.SByte:
        case TypeCode.Int16:
        case TypeCode.Int32:
        case TypeCode.Int64:
        case TypeCode.Byte:
        case TypeCode.UInt16:
        case TypeCode.UInt32:
        case TypeCode.UInt64:
          return true;


        default:
          return false;
      };
    }
    public static bool IsNumericNonInteger(Type type)
    {
      Type nnType = GetNonNullableType(type);
      switch (Type.GetTypeCode(nnType))
      {
        case TypeCode.Decimal:
        case TypeCode.Double:
          return true;


        default:
          return false;
      };
    }
    public static bool IsNumeric(Type type) => IsInteger(type) || IsNumericNonInteger(type);
    public static bool IsDateTime(Type type)
    {
      Type nnType = GetNonNullableType(type);
      switch (Type.GetTypeCode(nnType))
      {
        case TypeCode.DateTime:
          return true;

        default:
          return false;
      };
    }
    public static bool IsArrayOfInts(Type type) => type.IsArray ? IsInteger(type.GetElementType()) : false;
    public static bool IsArrayOfStrings(Type type) => type.IsArray ? Type.GetTypeCode(type.GetElementType()) == TypeCode.String : false;




    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAttr"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetAttributedProperties<TAttr>(this Type type) where TAttr : Attribute => type.GetProperties().Where(w => Attribute.GetCustomAttribute(w, typeof(TAttr)) != null);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAttr"></typeparam>
    /// <param name="type"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetAttributedProperties<TAttr>(this Type type, Func<TAttr, bool> predicate) where TAttr : Attribute =>
      type.GetProperties().Where(w => Attribute.GetCustomAttribute(w, typeof(TAttr)) != null && predicate.Invoke((TAttr)Attribute.GetCustomAttribute(w, typeof(TAttr))));












  }
}
