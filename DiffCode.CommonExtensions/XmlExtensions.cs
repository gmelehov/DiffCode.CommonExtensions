using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;








namespace DiffCode.CommonExtensions
{
  /// <summary>
  /// Статический класс с методами расширения для XML-элементов.
  /// </summary>
  public static class XmlExtensions
  {



    /// <summary>
    /// Возвращает первый найденный дочерний элемент указанного XML-элемента.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>).</param>
    /// <returns></returns>
    public static XElement FirstElement(this XElement elem) => elem?.Elements()?.FirstOrDefault();


    /// <summary>
    /// Возвращает последний найденный дочерний элемент указанного XML-элемента.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>).</param>
    /// <returns></returns>
    public static XElement LastElement(this XElement elem) => elem?.Elements()?.LastOrDefault();


    /// <summary>
    /// Возвращает соседний элемент, расположенный непосредственно перед указанным XML-элементом.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>).</param>
    /// <returns></returns>
    public static XElement PrevElement(this XElement elem) => elem?.ElementsBeforeSelf()?.LastOrDefault();


    /// <summary>
    /// Возвращает соседний элемент, расположенный непосредственно после указанного XML-элемента.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>).</param>
    /// <returns></returns>
    public static XElement NextElement(this XElement elem) => elem?.ElementsAfterSelf()?.FirstOrDefault();











    /// <summary>
    /// Возвращает первый найденный соседний элемент (сиблинг) указанного XML-элемента, имеющий указанное в параметре имя.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>).</param>
    /// <param name="xname">Имя искомого соседнего XML-элемента (сиблинга).</param>
    /// <returns></returns>
    public static XElement Sibling(this XElement elem, XName xname) => elem?.ElementsBeforeSelf().Concat(elem.ElementsAfterSelf()).FirstOrDefault(f => f.Name.Equals(xname));


    /// <summary>
    /// Возвращает первый найденный соседний элемент (сиблинг) указанного XML-элемента, удовлетворяющий заданным в предикате условиям.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>)</param>
    /// <param name="predicate">Выражение-предикат</param>
    /// <returns></returns>
    public static XElement Sibling(this XElement elem, Expression<Func<XElement, bool>> predicate) => elem?.ElementsBeforeSelf().Concat(elem.ElementsAfterSelf()).AsQueryable().FirstOrDefault(predicate);


    /// <summary>
    /// Возвращает все соседние элементы (сиблинги) указанного XML-элемента, имеющие указанные в параметрах имена.
    /// Выборка не включает в себя исходный XML-элемент.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>)</param>
    /// <param name="xnames">Имена соседних элементов (сиблингов), которые нужно включить в выборку</param>
    /// <returns></returns>
    public static IEnumerable<XElement> Siblings(this XElement elem, params XName[] xnames) => elem?.ElementsBeforeSelf().Concat(elem.ElementsAfterSelf()).Where(w => xnames.Contains(w.Name));


    /// <summary>
    /// Возвращает все соседние элементы (сиблинги) указанного XML-элемента, удовлетворяющие заданным в предикате условиям.
    /// Выборка не включает в себя исходный XML-элемент.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>)</param>
    /// <param name="predicate">Выражение-предикат</param>
    /// <returns></returns>
    public static IQueryable<XElement> Siblings(this XElement elem, Expression<Func<XElement, bool>> predicate) => elem?.ElementsBeforeSelf()?.Concat(elem.ElementsAfterSelf()).AsQueryable().Where(predicate);


    /// <summary>
    /// Перегрузка стандартного метода расширения <see cref="XContainer.Element(XName)"/>.
    /// Возвращает первый найденный дочерний элемент 1 уровня (непосредственный потомок) указанного XML-элемента,
    /// удовлетворяющий заданным в предикате условиям.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>)</param>
    /// <param name="predicate">Выражение-предикат</param>
    /// <returns></returns>
    public static XElement Element(this XElement elem, Expression<Func<XElement, bool>> predicate) => elem?.Elements().AsQueryable().FirstOrDefault(predicate);


    /// <summary>
    /// Перегрузка стандартного метода расширения <see cref="XContainer.Elements(XName)"/>.
    /// Возвращает все дочерние элементы 1 уровня (непосредственные потомки) указанного XML-элемента, 
    /// имеющие указанные в параметрах имена.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>)</param>
    /// <param name="xnames">Имена дочерних элементов 1 уровня (непосредственных потомков), которые нужно включить в выборку</param>
    /// <returns></returns>
    public static IEnumerable<XElement> Elements(this XElement elem, params XName[] xnames) => elem?.Elements().Where(w => xnames.Contains(w.Name));


    /// <summary>
    /// Перегрузка стандартного метода расширения <see cref="XContainer.Elements(XName)"/>.
    /// Возвращает все дочерние элементы 1 уровня (непосредственные потомки) указанного XML-элемента, удовлетворяющие заданным в предикате условиям.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>), в котором производится поиск</param>
    /// <param name="predicate">Выражение-предикат</param>
    /// <returns></returns>
    public static IQueryable<XElement> Elements(this XElement elem, Expression<Func<XElement, bool>> predicate) => elem?.Elements().AsQueryable().Where(predicate);


    /// <summary>
    /// Перегрузка стандартного метода расширения <see cref="XContainer.Descendant(XName)"/>.
    /// Возвращает первый найденный потомок указанного XML-элемента, удовлетворяющий заданным в предикате условиям.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>), в котором производится поиск</param>
    /// <param name="predicate">Выражение-предикат</param>
    /// <returns></returns>
    public static XElement Descendant(this XElement elem, Expression<Func<XElement, bool>> predicate) => elem?.Descendants().AsQueryable().FirstOrDefault(predicate);


    /// <summary>
    /// Перегрузка стандартного метода расширения <see cref="XContainer.Descendants(XName)"/>.
    /// Возвращает все найденные потомки указанного XML-элемента, имеющие указанные в параметрах имена.
    /// Выборка включает в себя все имеющиеся дочерние элементы 1 уровня (непосредственные потомки) указанного XML-элемента,
    /// и не включает исходный XML-элемент.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>)</param>
    /// <param name="xnames">Имена потомков, которые нужно включить в выборку</param>
    /// <returns></returns>
    public static IEnumerable<XElement> Descendants(this XElement elem, params XName[] xnames) => elem?.Descendants().Where(w => xnames.Contains(w.Name));


    /// <summary>
    /// Перегрузка стандартного метода расширения <see cref="XContainer.Descendants(XName)"/>.
    /// Возвращает все найденные потомки указанного XML-элемента, удовлетворяющие заданным в предикате условиям.
    /// Выборка включает в себя все имеющиеся дочерние элементы 1 уровня (непосредственные потомки) указанного XML-элемента,
    /// и не включает исходный XML-элемент.
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>)</param>
    /// <param name="predicate">Выражение-предикат</param>
    /// <returns></returns>
    public static IQueryable<XElement> Descendants(this XElement elem, Expression<Func<XElement, bool>> predicate) => elem?.Descendants().AsQueryable().Where(predicate);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IQueryable<XElement> Ancestors(this XElement elem, Expression<Func<XElement, bool>> predicate) => elem?.Ancestors().AsQueryable().Where(predicate);






    /// <summary>
    /// Возвращает <see langword="true"/>, если элемент XML (<see cref="XElement"/>) имеет атрибут с указанным именем.
    /// Возвращает <see langword="false"/>, если атрибут с указанным именем отсутствует.
    /// </summary>
    /// <param name="elem">Проверяемый XML-элемент</param>
    /// <param name="attr">Имя искомого атрибута</param>
    /// <returns></returns>
    public static bool HasAttr(this XElement elem, XName attr) => (elem != null && elem.Attribute(attr) != null);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool HasAttr(this XElement elem, Expression<Func<XAttribute, bool>> predicate) => (elem != null && elem.Attributes().AsQueryable().FirstOrDefault(predicate) != null);


    /// <summary>
    /// Возвращает <see langword="true"/>, если элемент XML (<see cref="XElement"/>) имеет атрибуты с указанными именами.
    /// Возвращает <see langword="false"/>, если хотя бы один атрибут из указанных отсутствует.
    /// </summary>
    /// <param name="elem">Проверяемый XML-элемент</param>
    /// <param name="attrs">Имена искомых атрибутов</param>
    /// <returns></returns>
    public static bool HasAttrs(this XElement elem, params XName[] attrs) => (elem != null) ? attrs.All(elem.HasAttr) : false;


    /// <summary>
    /// Возвращает <see langword="true"/>, если элемент XML (<see cref="XElement"/>) имеет дочерний элемент 1 уровня (непосредственный потомок) с указанным именем.
    /// Возвращает <see langword="false"/>, если дочерний элемент с указанным именем отсутствует.
    /// </summary>
    /// <param name="elem">Проверяемый элемент</param>
    /// <param name="childName">Имя искомого дочернего элемента 1 уровня (непосредственного потомка)</param>
    /// <returns></returns>
    public static bool HasElement(this XElement elem, XName childName) => (elem != null && elem.Element(childName) != null);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool HasElement(this XElement elem, Expression<Func<XElement, bool>> predicate) => (elem != null && elem.Elements().AsQueryable().FirstOrDefault(predicate) != null);


    /// <summary>
    /// Возвращает <see langword="true"/>, если элемент XML (<see cref="XElement"/>) имеет родительский элемент (предок) с указанным именем.
    /// Возвращает <see langword="false"/>, если родительский элемент (предок) с указанным именем отсутствует.
    /// </summary>
    /// <param name="elem">Проверяемый XML-элемент</param>
    /// <param name="ancestorName">Имя искомого родительского элемента (предка)</param>
    /// <returns></returns>
    public static bool HasAncestor(this XElement elem, XName ancestorName = null) => (elem != null) ? (elem.Ancestors(ancestorName) != null && elem.Ancestors(ancestorName).Count() > 0) : false;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool HasAncestor(this XElement elem, Expression<Func<XElement, bool>> predicate) => (elem != null && elem.Ancestors().AsQueryable().FirstOrDefault(predicate) != null);


    /// <summary>
    /// Возвращает <see langword="true"/>, если элемент XML (<see cref="XElement"/>) имеет потомка с указанным именем.
    /// Возвращает <see langword="false"/>, если потомок с указанным именем отсутствует.
    /// </summary>
    /// <param name="elem">Проверяемый XML-элемент</param>
    /// <param name="descendantName">Имя искомого потомка</param>
    /// <returns></returns>
    public static bool HasDescendant(this XElement elem, XName descendantName) => (elem != null) ? (elem.Descendants(descendantName) != null && elem.Descendants(descendantName).Count() > 0) : false;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool HasDescendant(this XElement elem, Expression<Func<XElement, bool>> predicate) => (elem != null && elem.Descendants().AsQueryable().FirstOrDefault(predicate) != null);


    /// <summary>
    /// Возвращает <see langword="true"/>, если элемент XML (<see cref="XElement"/>) имеет соседний элемент (сиблинг) с указанным именем.
    /// Возвращает <see langword="false"/>, если соседний элемент (сиблинг) с указанным именем отсутствует.
    /// </summary>
    /// <param name="elem">Проверяемый XML-элемент</param>
    /// <param name="siblingName">Имя искомого соседнего элемента (сиблинга)</param>
    /// <returns></returns>
    public static bool HasSibling(this XElement elem, XName siblingName) => (elem != null) ? elem.ElementsBeforeSelf(siblingName).Concat(elem.ElementsAfterSelf(siblingName)).Count() > 0 : false;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool HasSibling(this XElement elem, Expression<Func<XElement, bool>> predicate) => (elem != null) ? elem.ElementsBeforeSelf().Concat(elem.ElementsAfterSelf()).AsQueryable().FirstOrDefault(predicate) != null : false;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="xnames"></param>
    /// <returns></returns>
    public static bool HasSiblings(this XElement elem, params XName[] xnames) => (elem != null) ? xnames.All(elem.HasSibling) : false;






    /// <summary>
    /// Возвращает значение указанного атрибута XML-элемента (<see cref="XElement"/>).
    /// </summary>
    /// <param name="elem">Элемент XML (<see cref="XElement"/>)</param>
    /// <param name="attr">Имя атрибута</param>
    /// <returns></returns>
    public static string AttrVal(this XElement elem, XName attr) => (elem != null && elem.HasAttr(attr)) ? elem.Attribute(attr).Value : null;


    /// <summary>
    /// Выбирает всех предков XML-элемента <paramref name="elem"/>, 
    /// от его непосредственного родителя до ближайшего к нему предка, не отвечающего условиям предиката <paramref name="predicate"/>.
    /// </summary>
    /// <param name="elem">Исходный XML-элемент, для которого производится поиск его предков.</param>
    /// <param name="predicate">Условие остановки поиска.</param>
    /// <returns>Возвращает коллекцию предков XML-элемента от его непосредственного родителя до первого его предка, для которого указанный предикат вернет <see langword="false"/>.</returns>
    public static IQueryable<XElement> AncestorsWhile(this XElement elem, Func<XElement, bool> predicate) => elem?.Ancestors().Reverse().SkipWhile(predicate).Reverse().AsQueryable();


    /// <summary>
    /// Выбирает всех потомков XML-элемента <paramref name="elem"/>, 
    /// от первого найденного его дочернего элемента до первого ближайшего к нему потомка, не отвечающего условиям предиката <paramref name="predicate"/>.
    /// </summary>
    /// <param name="elem">Исходный XML-элемент, для которого производится поиск его потомков.</param>
    /// <param name="predicate">Условие остановки поиска.</param>
    /// <returns>Возвращает коллекцию потомков XML-элемента от первого найденного его дочернего элемента 
    /// до первого ближайшего к нему потомка, для которого указанный предикат вернет <see langword="false"/>.</returns>
    public static IQueryable<XElement> DescendantsWhile(this XElement elem, Func<XElement, bool> predicate) => elem?.Descendants().Reverse().SkipWhile(predicate).Reverse().AsQueryable();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static int CountAncestorsTill(this XElement elem, Func<XElement, bool> predicate) => elem?.AncestorsWhile(predicate).Count() ?? 0;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static int CountDescendantsTill(this XElement elem, Func<XElement, bool> predicate) => elem?.DescendantsWhile(predicate).Count() ?? 0;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <returns></returns>
    public static int CountSiblingsBefore(this XElement elem) => elem?.ElementsBeforeSelf().Count() ?? 0;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <returns></returns>
    public static int CountSiblingsAfter(this XElement elem) => elem?.ElementsAfterSelf().Count() ?? 0;


    /// <summary>
    /// Подсчитывает и возвращает общее количество родительских элементов (предков)
    /// Подсчет идет либо до корневого элемента (если не указан параметр countTo), 
    /// либо вплоть до первого появления родительского элемента (предка) с указанным именем countTo
    /// </summary>
    /// <param name="elem">Элемент, для которого подсчитывается количество предков</param>
    /// <param name="countTo">Имя родительского элемента (предка), на котором необходимо остановить подсчет</param>
    /// <returns></returns>
    public static int CountAncestors(this XElement elem, XName countTo = null)
    {
      int ret = 0;
      XDocument xdoc = elem.Document;
      XElement root = xdoc.Root;
      XElement countToElem = null;

      if (countTo != null)
        countToElem = new XElement(countTo);

      if (countToElem != null && countToElem.HasDescendant(elem.Name))
        root = countToElem;

      if (elem != root)
      {
        var parent = elem.Parent;
        ret = ret + 1;
        parent = parent.Parent;
        while (parent != null && parent != root)
        {
          ret = ret + 1;
          parent = parent.Parent;
        };
      };

      return ret;
    }


    /// <summary>
    /// Подсчитывает и возвращает глубину вложенности элемента относительно родительского элемента (предка),
    /// имеющего определенный атрибут topElemAttribute (по умолчанию - атрибут sid)
    /// </summary>
    /// <param name="elem">Элемент, для которого подсчитывается глубина вложенности</param>
    /// <param name="topElemAttribute">Атрибут родительского элемента (предка)</param>
    /// <returns></returns>
    public static int CountDepth(this XElement elem, XName topElemAttribute = null)
    {
      int ret = 0;
      if (elem != null)
      {
        if (topElemAttribute == null)
          topElemAttribute = "sid";

        var mainAncestor = elem.Ancestors().FirstOrDefault(w => w.HasAttr(topElemAttribute));
        if (mainAncestor != null)
        {
          var mainAncestorDepth = mainAncestor.Ancestors().Count();
          var elemDepth = elem.Ancestors().Count();

          return elemDepth - mainAncestorDepth;
        };
      };

      return ret;
    }







    /// <summary>
    /// Подчитывает и возвращает максимальную глубину вложенности дочерних элементов - потомков указанного элемента
    /// </summary>
    /// <param name="xelem">Элемент, для которого подсчитывается максимальная глубина вложенности</param>
    /// <returns></returns>
    public static int CountLevels(this XElement xelem) => xelem.Descendants().Select(d => d.CountAncestors() - xelem.CountAncestors()).Max() + 1 - xelem.CountAncestors();


    /// <summary>
    /// Копирует значение собственного атрибута элемента в другой его атрибут
    /// </summary>
    /// <param name="elem">Обрабатываемый элемент</param>
    /// <param name="sourceAttr">Атрибут-источник</param>
    /// <param name="targetAttr">Атрибут-получатель</param>
    /// <param name="addFirst">В случае true помещает атрибут-получатель первым в списке атрибутов элемента</param>
    /// <returns></returns>
    public static XElement CopySelfAttributes(this XElement elem, XName sourceAttr, XName targetAttr, bool addFirst = false)
    {
      if (elem != null && sourceAttr != null && elem.HasAttr(sourceAttr))
      {
        if (addFirst)
        {
          var newAttrValue = elem.AttrVal(sourceAttr);
          var newAttrs = new List<XAttribute>
          {
            new XAttribute(targetAttr, newAttrValue)
          };
          newAttrs.AddRange(elem.Attributes());
          //var newAttrs = elem.Attributes().Prepend(new XAttribute(targetAttr, newAttrValue));
          elem.ReplaceAttributes(newAttrs);
        }
        else
        {
          elem.SetAttributeValue(targetAttr, elem.AttrVal(sourceAttr));
        };
      };
      return elem;
    }


    /// <summary>
    /// Копирует значение собственного атрибута элемента в другой атрибут другого элемента
    /// </summary>
    /// <param name="elem">Обрабатываемый элемент-источник атрибута</param>
    /// <param name="targetElem">Элемент-получатель атрибута</param>
    /// <param name="srcAttr">Атрибут-источник значения</param>
    /// <param name="targetAttr">Атрибут-получатель значения</param>
    public static void CopyAttrTo(this XElement elem, ref XElement targetElem, XName srcAttr, XName targetAttr)
    {
      if (elem != null && targetElem != null && elem.HasAttr(srcAttr) && srcAttr != null && targetAttr != null)
        targetElem.SetAttributeValue(targetAttr, elem.AttrVal(srcAttr));
    }


    /// <summary>
    /// Удаляет атрибуты элемента, исключая атрибуты из списка exceptAttrs, и включая атрибуты из списка includeAttrs
    /// </summary>
    /// <param name="elem">Обрабатываемый элемент, у которого производится удаление атрибутов</param>
    /// <param name="includeAttrs">Список включаемых атрибутов, которые ДОЛЖНЫ быть удалены</param>
    /// <param name="exceptAttrs">Список исключаемых атрибутов, которые ДОЛЖНЫ быть оставлены без изменения</param>
    public static XElement RemoveAttrs(this XElement elem, IEnumerable<XName> includeAttrs = null, IEnumerable<XName> exceptAttrs = null)
    {
      if (elem != null)
        if (exceptAttrs == null)
          if (includeAttrs == null)
            elem.RemoveAttributes();
          else
            elem.Attributes().Where(w => includeAttrs.Contains(w.Name)).Remove();
        else
          elem.Attributes().Where(at => !exceptAttrs.Contains(at.Name)).ToList().ForEach(a => elem.SetAttributeValue(a.Name, null));

      return elem;
    }


    /// <summary>
    /// Создает новый одиночный XML-элемент с именем/набором атрибутов, 
    /// взятым из текущего XML-элемента (либо первого из его потомков) в соответствии с условиями указанных предикатов.
    /// </summary>
    /// <param name="elem">Исходный XML-элемент, выступающий в качестве прототипа для возвращаемого элемента.</param>
    /// <param name="childPredicate">Условия выбора имени конкретного прототипа для возвращаемого элемента. В случае <see langword="null"/> в качестве прототипа будет использован исходный XML-элемент.</param>
    /// <param name="attrsPredicate">Условия выбора конкретных атрибутов прототипа для возвращаемого элемента. В случае <see langword="null"/> в возвращаемый элемент будут скопированы все атрибуты выбранного прототипа.</param>
    /// <returns></returns>
    public static XElement Cut(this XElement elem, Expression<Func<XElement, bool>> childPredicate = null, Expression<Func<XAttribute, bool>> attrsPredicate = null) =>
      new XElement(
        childPredicate == null ? elem.Name : elem.Descendant(childPredicate).Name,
        (childPredicate == null ? elem : elem.Descendant(childPredicate))
        .Attributes().AsQueryable()
        .Where(attrsPredicate ?? (at => true)));








    public static XElement ProcessAttribute(this XElement elem, Expression<Func<XElement, bool>> predicate, string srcAttr, string targetAttr)
    {
      foreach (var it in elem.Descendants(predicate))
      {
        var itt = it;
        ReplaceAttrName(ref itt, srcAttr, targetAttr);
      };
      return elem;
    }


    public static void ReplaceAttrName(ref XElement srcElem, string srcAttr, string targetAttr)
    {
      if (srcElem.HasAttr(srcAttr))
      {
        srcElem.SetAttributeValue(targetAttr, srcElem.AttrVal(srcAttr));
        srcElem.SetAttributeValue(srcAttr, null);
      };
    }






    /// <summary>
    /// Валидирует XML-документ, используя предоставленный набор XSD-схем
    /// </summary>
    /// <param name="doc">Проверяемый документ</param>
    /// <param name="schemaSet">Набор XSD-схем</param>
    /// <returns></returns>
    public static bool IsValid(this XDocument doc, XmlSchemaSet schemaSet)
    {
      bool errors = false;
      doc.Validate(schemaSet, (o, e) =>
      {
        errors = true;
      });
      return !errors;
    }


    /// <summary>
    /// Проверка на "пустой" элемент.
    /// Элемент считается пустым, если у него отсутствует содержимое (текст, дочерние элементы и т.п.),
    /// а также любые атрибуты, кроме относящихся к декларации пространства имен.
    /// Проверка производится с целью удаления пустых контейнеров содержимого из шаблона (если их нечем заполнять из разметки).
    /// </summary>
    /// <param name="elem">Проверяемый элемент</param>
    /// <returns></returns>
    public static bool IfEmpty(this XElement elem) => (!elem.HasElements && (!elem.HasAttributes || (elem.FirstAttribute.IsNamespaceDeclaration && elem.FirstAttribute == elem.LastAttribute)));





    /// <summary>
    /// Создает один из стандартных контейнеров содержимого и размещает его в определенном порядке внутри элемента.
    /// </summary>
    /// <param name="elem">Обрататываемый элемент, в котором необходимо создать контейнер содержимого</param>
    /// <param name="itemsCont">Имя элемента - контейнера содержимого</param>
    /// <param name="content">Опционально: содержимое, которое необходимо разместить в создаваемом контейнере</param>
    /// <param name="addFirst">Опционально: в случае false создаваемый контейнер размещается внутри элемента-получателя ПЕРВЫМ, ДО всех его дочерних элементов</param>
    public static void CreateItemsNode(this XElement elem, XName itemsCont, object content = null, bool addFirst = false)
    {
      if (!elem.HasElement(itemsCont))
        if (addFirst)
          elem.AddFirst(new XElement(itemsCont, content));
        else
          elem.Add(new XElement(itemsCont, content));
    }


    /// <summary>
    /// Возвращает список дочерних элементов, размещенных в определенном контейнере содержимого
    /// </summary>
    /// <param name="elem">Элемент, чьи дочерние элементы необходимо вернуть в методе</param>
    /// <param name="itemsCont">Имя контейнера содержимого, из которого извлекаются дочерние элементы</param>
    /// <returns></returns>
    public static List<XElement> GetItems(this XElement elem, XName itemsCont)
    {
      return (elem.HasElement(itemsCont)) ?
        elem.Element(itemsCont).Elements().ToList()
        :
        new List<XElement>();
    }


    /// <summary>
    /// Выполняет XSLT-преобразование указанного элемента, используя указанный файл XSLT-стилей, 
    /// с учетом указанных параметров преобразования, имеющихся в стилевом файле.
    /// Выходной формат преобразования - XML
    /// </summary>
    /// <param name="elem">Обрабатываемый элемент</param>
    /// <param name="xslUri">Путь к стилевому файлу-обработчику</param>
    /// <param name="argums">Опционально: список параметров преобразования</param>
    /// <returns></returns>
    public static XDocument XSLTransform(this XElement elem, string xslUri, XsltArgumentList argums = null)
    {
      var ret = new XDocument();
      using (var reader = elem.CreateReader())
      {
        ret = XmlToXDocument(xslUri: xslUri, xml: reader, argums: argums);
      };
      return ret;
    }


    /// <summary>
    /// Выполняет XSLT-преобразование указанного XML-документа, используя указанный файл XSLT-стилей, 
    /// с учетом указанных параметров преобразования, имеющихся в стилевом файле.
    /// Выходной формат преобразования - Text
    /// </summary>
    /// <param name="xdoc">Обрабатываемый XML-документ, содержимое которого необходимо преобразовать в текст</param>
    /// <param name="xslUri">Путь к стилевому файлу-обработчику</param>
    /// <param name="argums">Опционально: список параметров преобразования</param>
    /// <returns></returns>
    public static string XSLTTransformToText(this XDocument xdoc, string xslUri, XsltArgumentList argums = null)
    {
      string ret = "";
      using (var reader = xdoc.CreateReader())
      {
        ret = XmlToText(xslUri, reader, argums);
      };
      return ret;
    }


    /// <summary>
    /// Возвращает список элементов XML-документа, соответствующих указанному выражению XPath.
    /// Используется для построения сложных комбинированных запросов к содержимому XML-документа.
    /// </summary>
    /// <param name="xdoc">Опрашиваемый документ.</param>
    /// <param name="m">Менеджер пространств имен.</param>
    /// <param name="xpathString">Строка, задающая выражение XPath</param>
    /// <returns></returns>
    public static List<XElement> XPathSelect(this XDocument xdoc, XmlNamespaceManager m, string xpathString) => xdoc.Root.XPathSelectElements(xpathString, m).ToList();


    /// <summary>
    /// Производит слияние атрибутов двух элементов.
    /// Возвращает элемент-получатель после слияния атрибутов.
    /// </summary>
    /// <param name="elem">Элемент-источник атрибутов</param>
    /// <param name="other">Элемент-получатель атрибутов</param>
    /// <returns></returns>
    public static XElement MergeAttrs(this XElement elem, XElement other)
    {
      elem.Attributes().Where(a => a.Value != null).ToList().ForEach(f => other.SetAttributeValue(f.Name, f.Value));
      return other;
    }


    /// <summary>
    /// Разрешает ссылку на XML-элемент, имеющий атрибут <paramref name="srcAttrName"/>, 
    /// отвечающий условиям заданного предиката и находящийся в указанном XML-документе.
    /// </summary>
    /// <param name="elem">Исходный XML-элемент, содержащий ссылку на искомый XML-элемент.</param>
    /// <param name="refAttrName">Атрибут исходного XML-элемента, значение которого должно совпадать с целевым атрибутом искомого XML-элемента.</param>
    /// <param name="srcAttrName">Целевой атрибут искомого XML-элемента.</param>
    /// <param name="ofSameName">Опционально: если <see langword="true"/> (по умолчанию), то имена обоих XML-элементов обязаны совпадать.</param>
    /// <param name="predicate">Опционально: предикат для уточнения условий поиска искомого XML-элемента.</param>
    /// <param name="srcXDoc">Опционально: XML-документ, в котором должен выполняться поиск. 
    /// Если параметр не указан, или равен <see langword="null"/>, то поиск будет производиться в документе-родителе исходного XML-элемента <paramref name="elem"/>.</param>
    /// <returns>Возвращает первый найденный XML-элемент, соответствующий всем перечисленным условиям, либо <see langword="null"/>, если таковые XML-элементы не найдены.</returns>
    public static XElement ResolveRef(this XElement elem, XName refAttrName, XName srcAttrName, bool ofSameName = true, Expression<Func<XElement, bool>> predicate = null, XDocument srcXDoc = null)
    {
      if (elem != null && refAttrName.LocalName.IsUseful() && srcAttrName.LocalName.IsUseful() && elem.HasAttr(refAttrName))
      {
        var xdoc = srcXDoc ?? elem.Document;
        var found =
          xdoc.Root.DescendantsAndSelf()
          .Where(
            w =>
            ofSameName == true ? w.Name.Equals(elem.Name) : true
            &&
            w.HasAttr(srcAttrName)
            &&
            w.AttrVal(srcAttrName) == elem.AttrVal(refAttrName)
            );
        if (predicate != null)
          found = found.AsQueryable().Where(predicate);

        return found?.FirstOrDefault();
      }
      else
        return null;
    }


    /// <summary>
    /// Выполняет слияние атрибутов и содержимого двух элементов - получателя и источника.
    /// Логика слияния: создается новый элемент, копирующий атрибуты и содержимое источника.
		/// Далее из элемента-получателя копируются все атрибуты, которыми необходимо заменить шаблон.
		/// Наивысший приоритет при слиянии атрибутов/содержимого - у элемента-получателя 
		/// (его атрибуты/содержимое в случае несовпадения с шаблоном перезаписывают шаблонные значения).
		/// Слияние дочерних элементов производится по каждому из соответствующих контейнеров.
    /// </summary>
    /// <param name="elem">Элемент-получатель содержимого.</param>
    /// <param name="otherElem">Элемент-источник содержимого.</param>
    /// <returns></returns>
    public static XElement MergeWith(this XElement elem, XElement otherElem)
    {
      if (elem != null && otherElem != null)
      {
        var ret = new XElement(otherElem);
        var otherAttrs = otherElem.Attributes();

        elem.Attributes().Where(a => a.Value != null).ToList().ForEach(f => ret.SetAttributeValue(f.Name, f.Value));
        elem.Elements().ToList().ForEach(f =>
        {
          ret.Add(f);
          elem.GetItems(f.Name).ForEach(ff =>
          {
            ret.Element(f.Name).Add(ff);
          });
        });

        return ret;
      }
      else
        return elem;
    }







    /// <summary>
    /// Выполняет преобразование из XML в Text для указанного XML-документа.
    /// </summary>
    /// <param name="xslUri">Путь к стилевому файлу-обработчику</param>
    /// <param name="xmlUri">Путь к обрабатываемому документу</param>
    /// <param name="argums">Опционально: параметры обработки, определенные в стилевом файле</param>
    /// <returns></returns>
    public static string XmlToText(string xslUri, string xmlUri, XsltArgumentList argums = null)
    {
      var xslt = new XslCompiledTransform();
      var sett = new XsltSettings(true, true);
      StringWriter sw = new StringWriter();
      xslt.Load(xslUri, sett, new XmlUrlResolver());
      xslt.Transform(xmlUri, argums, sw);
      sw.Close();
      return sw.ToString();
    }


    /// <summary>
    /// Выполняет преобразование из XML в Text для указанного XML-документа
    /// </summary>
    /// <param name="xsl">Объект XMLReader для стилевого обработчика</param>
    /// <param name="xml">Объект XMLReader для обрабатываемых XML-данных</param>
    /// <param name="argums">Опционально: параметры обработки, определенные в стилевом файле</param>
    /// <returns></returns>
    public static string XmlToText(XmlReader xsl, XmlReader xml, XsltArgumentList argums = null)
    {
      var xslt = new XslCompiledTransform();
      var sett = new XsltSettings(true, true);
      StringWriter sw = new StringWriter();
      xslt.Load(xsl, sett, new XmlUrlResolver());
      xslt.Transform(xml, argums, sw);
      sw.Close();
      return sw.ToString();
    }


    /// <summary>
    /// Выполняет преобразование из XML в Text для указанного XML-документа.
    /// </summary>
    /// <param name="xslUri">Путь к стилевому файлу-обработчику</param>
    /// <param name="xml">Объект XMLReader для обрабатываемых XML-данных</param>
    /// <param name="argums">Опционально: параметры обработки, определенные в стилевом файле</param>
    /// <returns></returns>
    public static string XmlToText(string xslUri, XmlReader xml, XsltArgumentList argums = null)
    {
      var xslt = new XslCompiledTransform();
      var sett = new XsltSettings(true, true);
      StringWriter sw = new StringWriter();
      xslt.Load(xslUri, sett, new XmlUrlResolver());
      xslt.Transform(xml, argums, sw);
      sw.Close();
      return sw.ToString();
    }





    /// <summary>
    /// Выполняет преобразование из XML в Text для указанного XML-элемента.
    /// </summary>
    /// <param name="xslUri">Путь к стилевому файлу-обработчику</param>
    /// <param name="elem">Обрабатываемый XML-элемент</param>
    /// <param name="argums">Опционально: параметры обработки, определенные в стилевом файле</param>
    /// <returns></returns>
    public static string XElementToText(string xslUri, XElement elem, XsltArgumentList argums = null)
    {
      var xslt = new XslCompiledTransform();
      var sett = new XsltSettings(true, true);
      var tempfile = Path.GetTempFileName();
      elem.Save(tempfile);
      StringWriter sw = new StringWriter();
      xslt.Load(xslUri, sett, new XmlUrlResolver());
      xslt.Transform(tempfile, argums, sw);
      sw.Close();
      File.Delete(tempfile);
      return sw.ToString();
    }


    /// <summary>
    /// Записывает указанное текстовое содержимое в файл по указанному пути.
    /// </summary>
    /// <param name="text">Записываемый текст.</param>
    /// <param name="filePath">Путь к целевому файлу.</param>
    public static void TextToFile(string text, string filePath) => File.WriteAllText(filePath, text);




    /// <summary>
    /// Выполняет преобразование из XML в XML для указанного XML-содержимого.
    /// </summary>
    /// <param name="xslUri">Путь к стилевому файлу-обработчику</param>
    /// <param name="xmlUri">Путь к обрабатываемому документу</param>
    /// <param name="argums">Опционально: параметры обработки, определенные в стилевом файле</param>
    /// <returns></returns>
    public static XDocument XmlToXDocument(string xslUri, string xmlUri, XsltArgumentList argums = null)
    {
      var xslt = new XslCompiledTransform();
      var sett = new XsltSettings(true, true);
      StringWriter sw = new StringWriter();
      xslt.Load(xslUri, sett, new XmlUrlResolver());
      xslt.Transform(xmlUri, argums, sw);
      sw.Close();
      return XDocument.Parse(sw.ToString());
    }


    /// <summary>
    /// Выполняет преобразование из XML в XML для указанного XML-содержимого.
    /// </summary>
    /// <param name="xslUri">Путь к стилевому файлу-обработчику</param>
    /// <param name="xml">Объект XMLReader для обрабатываемых XML-данных</param>
    /// <param name="argums">Опционально: параметры обработки, определенные в стилевом файле</param>
    /// <returns></returns>
    public static XDocument XmlToXDocument(string xslUri, XmlReader xml, XsltArgumentList argums = null)
    {
      var xslt = new XslCompiledTransform();
      var sett = new XsltSettings(true, true);
      StringWriter sw = new StringWriter();
      xslt.Load(xslUri, sett, new XmlUrlResolver());
      xslt.Transform(xml, argums, sw);
      sw.Close();
      return XDocument.Parse(sw.ToString());
    }











    /// <summary>
    /// 
    /// </summary>
    /// <param name="xelem"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    public static XElement AddToSelf(this XElement xelem, params XObject[] items)
    {
      xelem.Add(items);
      return xelem;
    }


    /// <summary>
    /// Добавляет указанный контент непосредственно перед первым дочерним элементом целевого XML-элемента.
    /// Возвращает обновленный целевой XML-элемент, если <paramref name="changeScope"/> равен <see langword="false"/>.
    /// Возвращает первый дочерний элемент целевого XML-элемента, если <paramref name="changeScope"/> равен <see langword="true"/>.
    /// </summary>
    /// <param name="xelem">Целевой XML-элемент.</param>
    /// <param name="changeScope">Признак замены возвращаемого элемента с исходного на целевой.</param>
    /// <param name="items">Добавляемый контент</param>
    /// <returns></returns>
    public static XElement AddBeforeFirst(this XElement xelem, bool changeScope = false, params XObject[] items)
    {
      if (xelem.FirstElement() != null)
        xelem.FirstElement()?.AddBeforeSelf(items);
      else
        xelem.AddToSelf(items);

      return changeScope ? xelem.FirstElement() : xelem;
    }


    /// <summary>
    /// Добавляет указанный контент непосредственно после первого дочернего элемента целевого XML-элемента.
    /// Возвращает обновленный целевой XML-элемент, если <paramref name="changeScope"/> равен <see langword="false"/>.
    /// Возвращает первый дочерний элемент целевого XML-элемента, если <paramref name="changeScope"/> равен <see langword="true"/>.
    /// </summary>
    /// <param name="xelem">Целевой XML-элемент.</param>
    /// <param name="changeScope">Признак замены возвращаемого элемента с исходного на целевой.</param>
    /// <param name="items">Добавляемый контент</param>
    /// <returns></returns>
    public static XElement AddAfterFirst(this XElement xelem, bool changeScope = false, params XObject[] items)
    {
      if (xelem.FirstElement() != null)
        xelem.FirstElement().AddAfterSelf(items);
      else
        xelem.AddToSelf(items);

      return changeScope ? xelem.FirstElement() : xelem;
    }


    /// <summary>
    /// Добавляет указанный контент непосредственно перед последним дочерним элементом целевого XML-элемента.
    /// Возвращает обновленный целевой XML-элемент, если <paramref name="changeScope"/> равен <see langword="false"/>.
    /// Возвращает последний дочерний элемент целевого XML-элемента, если <paramref name="changeScope"/> равен <see langword="true"/>.
    /// </summary>
    /// <param name="xelem">Целевой XML-элемент.</param>
    /// <param name="changeScope">Признак замены возвращаемого элемента с исходного на целевой.</param>
    /// <param name="items">Добавляемый контент</param>
    /// <returns></returns>
    public static XElement AddBeforeLast(this XElement xelem, bool changeScope = false, params XObject[] items)
    {
      if (xelem.LastElement() != null)
        xelem.LastElement().AddBeforeSelf(items);
      else
        xelem.AddToSelf(items);

      return changeScope ? xelem.LastElement() : xelem;
    }


    /// <summary>
    /// Добавляет указанный контент непосредственно после последнего дочернего элемента целевого XML-элемента.
    /// Возвращает обновленный целевой XML-элемент, если <paramref name="changeScope"/> равен <see langword="false"/>.
    /// Возвращает последний дочерний элемент целевого XML-элемента, если <paramref name="changeScope"/> равен <see langword="true"/>.
    /// </summary>
    /// <param name="xelem">Целевой XML-элемент.</param>
    /// <param name="changeScope">Признак замены возвращаемого элемента с исходного на целевой.</param>
    /// <param name="items">Добавляемый контент</param>
    /// <returns></returns>
    public static XElement AddAfterLast(this XElement xelem, bool changeScope = false, params XObject[] items)
    {
      if (xelem.LastElement() != null)
        xelem.LastElement().AddAfterSelf(items);
      else
        xelem.AddToSelf(items);

      return changeScope ? xelem.LastElement() : xelem;
    }


    /// <summary>
    /// Добавляет указанный контент непосредственно к первому дочернему элементу целевого XML-элемента.
    /// Возвращает обновленный целевой XML-элемент, если <paramref name="changeScope"/> равен <see langword="false"/>.
    /// Возвращает первый дочерний элемент целевого XML-элемента, если <paramref name="changeScope"/> равен <see langword="true"/>.
    /// </summary>
    /// <param name="xelem">Целевой XML-элемент.</param>
    /// <param name="changeScope">Признак замены возвращаемого элемента с исходного на целевой.</param>
    /// <param name="items">Добавляемый контент</param>
    /// <returns></returns>
    public static XElement AddToFirst(this XElement xelem, bool changeScope = false, params XObject[] items)
    {
      xelem.FirstElement()?.Add(items);
      return changeScope ? xelem.FirstElement() : xelem;
    }


    /// <summary>
    /// Добавляет указанный контент непосредственно к последнему дочернему элементу целевого XML-элемента.
    /// Возвращает обновленный целевой XML-элемент, если <paramref name="changeScope"/> равен <see langword="false"/>.
    /// Возвращает последний дочерний элемент целевого XML-элемента, если <paramref name="changeScope"/> равен <see langword="true"/>.
    /// </summary>
    /// <param name="xelem">Целевой XML-элемент.</param>
    /// <param name="changeScope">Признак замены возвращаемого элемента с исходного на целевой.</param>
    /// <param name="items">Добавляемый контент</param>
    /// <returns></returns>
    public static XElement AddToLast(this XElement xelem, bool changeScope = false, params XObject[] items)
    {
      xelem.LastElement()?.Add(items);
      return changeScope ? xelem.LastElement() : xelem;
    }






    /// <summary>
    /// 
    /// </summary>
    /// <param name="xelem"></param>
    /// <returns></returns>
    public static XElement BeforeLastElement(this XElement xelem) => xelem.LastElement()?.PrevElement();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="xelem"></param>
    /// <returns></returns>
    public static XElement AfterFirstElement(this XElement xelem) => xelem.FirstElement()?.NextElement();








    /// <summary>
    /// Выполняет поиск дочерних элементов, содержащих аннотации указанного типа.
    /// Возвращает список элементов, содержащих аннотации указанного типа.
    /// </summary>
    /// <typeparam name="T">Тип аннотации.</typeparam>
    /// <param name="xelem">Исходный XML-элемент, в котором производится поиск.</param>
    /// <returns></returns>
    public static IEnumerable<XElement> FindAnnotated<T>(this XElement xelem) => xelem.DescendantsAndSelf().Where(w => w.Annotations(typeof(T)).Any(a => a != null));


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xelem"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static XElement FindAnnotated<T>(this XElement xelem, Func<XElement, bool> predicate = null)
    {
      var foundmany = xelem.DescendantsAndSelf().Where(w => w.Annotations(typeof(T)).Any(a => a != null));
      var found = predicate != null ? foundmany.FirstOrDefault(predicate) : foundmany.FirstOrDefault();
      return found;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xelem"></param>
    /// <returns></returns>
    public static IEnumerable<T> GetAnnotations<T>(this XElement xelem) => xelem.FindAnnotated<T>().SelectMany(s => s.Annotations(typeof(T)))?.Cast<T>();


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xelem"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static T GetAnnotation<T>(this XElement xelem, Func<T, bool> predicate = null) => predicate != null ? xelem.GetAnnotations<T>().FirstOrDefault(predicate) : xelem.GetAnnotations<T>().FirstOrDefault();


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xelem"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static XElement ApplyAnnotation<T>(this XElement xelem, Func<T, XElement> transform) => new XElement(transform(xelem.GetAnnotation<T>()));


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="xelem"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static V ApplyAnnotation<T, V>(this XElement xelem, Func<T, V> transform) => transform(xelem.GetAnnotation<T>());
    






    /// <summary>
    /// 
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="predicate"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static XElement FindAndProcess(this XElement elem, Func<XElement, bool> predicate, Action<XElement> action)
    {
      elem.DescendantsAndSelf().Where(predicate).ToList().ForEach(action);
      return elem;
    }







    /// <summary>
    /// 
    /// </summary>
    /// <param name="xelem"></param>
    /// <param name="pattern"></param>
    /// <param name="compareValues"></param>
    /// <param name="strict"></param>
    /// <returns></returns>
    public static bool HasSimilarAttrs(this XElement xelem, XElement pattern, bool compareValues = false, bool strict = false)
    {
      var myAttrs = xelem.Attributes().Except(e => e.IsNamespaceDeclaration).ToList();
      var patternAttrs = pattern.Attributes().Except(e => e.IsNamespaceDeclaration).ToList();

      var result =
        strict
        ?
        patternAttrs
        .Join(myAttrs, p => p, m => m, (pp, mm) => new { pattern = pp, my = mm })
        .All(a => (a.pattern.Name == a.my.Name) && (compareValues ? a.pattern.Value == a.my.Value : true))
        :
        patternAttrs
        .Join(myAttrs, p => p, m => m, (pp, mm) => new { pattern = pp, my = mm })
        .Any(a => (a.pattern.Name == a.my.Name) && (compareValues ? a.pattern.Value == a.my.Value : true));


      return result;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="xelem"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static bool HasSimilarNames(this XElement xelem, XElement pattern) => xelem.Name.LocalName == pattern.Name.LocalName;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="xelem"></param>
    /// <param name="pattern"></param>
    /// <param name="compareAttrs"></param>
    /// <returns></returns>
    public static bool HasSimilarElems(this XElement xelem, XElement pattern, bool compareAttrs = false)
    {
      var myElems = xelem.Elements().ToList();
      var patternElems = pattern.Elements().ToList();

      var result =
        patternElems
        .Join(myElems, p => p, m => m, (pp, mm) => new { pattern = pp, my = mm })
        .All(a => a.pattern.HasSimilarNames(a.my) && a.pattern.HasSimilarAttrs(a.my, compareAttrs));

      return result;
    }







    public static XElement HasThisName(this XElement xelem, XName xname) => xelem.Name == xname ? xelem : null;


    public static XElement HasThisAttr(this XElement xelem, XAttribute attr) => (xelem != null && xelem.HasAttr(attr.Name) && xelem.AttrVal(attr.Name) == attr.Value) ? xelem : null;


    public static XElement HasThisElem(this XElement xelem, XElement child) => (xelem != null && xelem.HasElement(el => el.Name == child.Name && el.HasSimilarAttrs(child, true, true))) ? xelem : null;







    public static bool IsLike(this XElement xelem, XElement pattern, bool omitAttrs, bool omitChilds, bool compareAttrs)
    {
      bool result = true;
      result &= xelem.HasSimilarNames(pattern);

      if (pattern.HasAttributes)
      {
        if (!omitAttrs)
          result &= xelem.HasSimilarAttrs(pattern, compareAttrs);
      };

      if (pattern.HasElements)
      {
        if (!omitChilds)
          result &= xelem.HasSimilarElems(pattern, compareAttrs);
      };

      return result;
    }



  }
}
