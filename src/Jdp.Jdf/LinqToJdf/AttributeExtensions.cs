﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace Jdp.Jdf.LinqToJdf
{
    /// <summary>
    /// JDF LINQ helpers for attributes
    /// </summary>
    public static class AttributeExtensions
    {
        ///<summary>
        /// Is Attribute Value empty?
        ///</summary>
        ///<param name="source"></param>
        ///<returns></returns>
        public static bool IsEmpty(this XAttribute source)
        {
            Contract.Requires(source != null);

            return String.IsNullOrEmpty(source.Value);
        }

        /// <summary>
        /// Gets the value of the given attribute
        /// </summary>
        /// <param name="element">The element containing the attribute</param>
        /// <param name="attributeName">The name of the attribute</param>
        /// <returns>The string value of the attribute or null if it does not exist.</returns>
        public static string GetAttributeValueOrNull(this XElement element, XName attributeName)
        {
            Contract.Requires(element != null);
            Contract.Requires(attributeName != null);

            if (element.Attribute(attributeName) == null) return null;
            return element.Attribute(attributeName).Value;
        }

        /// <summary>
        /// Gets the value of the given attribute
        /// </summary>
        /// <param name="element">The element containing the attribute</param>
        /// <param name="attributeName">The name of the attribute</param>
        /// <returns>The string value of the attribute or empty string if it does not exist.</returns>
        public static string GetAttributeValueOrEmpty(this XElement element, XName attributeName)
        {
            Contract.Requires(element != null);
            Contract.Requires(attributeName != null);

            if (element.Attribute(attributeName) == null) return string.Empty;
            return element.Attribute(attributeName).Value;
        }

        /// <summary>
        /// Gets the value of the given attribute as a double
        /// </summary>
        /// <param name="element">The element containing the attribute</param>
        /// <param name="attributeName">The name of the attribute</param>
        /// <returns>The double value of the attribute or null if it does not exist or cannot be parsed to a double.</returns>
        public static double? GetAttributeValueAsDoubleOrNull(this XElement element, XName attributeName)
        {
            Contract.Requires(element != null);
            Contract.Requires(attributeName != null);

            var doubleString = element.GetAttributeValueOrNull(attributeName);
            if (doubleString == null) return null;

            double doubleVal = 0;
            return double.TryParse(doubleString, out doubleVal) ? (double?)doubleVal : null;
        }

        /// <summary>
        /// Gets the value of the given attribute as an int
        /// </summary>
        /// <param name="element">The element containing the attribute</param>
        /// <param name="attributeName">The name of the attribute</param>
        /// <returns>The int value of the attribute or null if it does not exist or cannot be parsed to an int.</returns>
        public static int? GetAttributeValueAsIntOrNull(this XElement element, XName attributeName)
        {
            Contract.Requires(element != null);
            Contract.Requires(attributeName != null);

            var intString = element.GetAttributeValueOrNull(attributeName);
            if (intString == null) return null;

            int intVal = 0;
            return int.TryParse(intString, out intVal) ? (int?)intVal : null;
        }

        /// <summary>
        /// Returns Attributes found within the supplied element.
        /// </summary>
        /// <typeparam name="T">The type of objects in source constrained to an XNode</typeparam>
        /// <param name="source">An IEnumerable containing XNode decendants that will be operated upon.</param>
        /// <param name="name">The name of the attribute to find</param>
        public static IEnumerable<XAttribute> GetAttributesByAttributeName<T>(this IEnumerable<T> source, XName name) where T : XNode
        {
            Contract.Requires(source != null);
            Contract.Requires(name != null);

            IEnumerable<XElement> item = (from t in
                                              ((IEnumerable<XElement>)source).Where(n => n.Attributes().Where(a => a.Name == name).Count() > 0)
                                          select t
                                );
            return item.FirstOrDefault().Attributes().Where(a => a.Name == name);
        }

        /// <summary>
        /// Replaces the attribute specified by the oldAttrName.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="newAttr">The new attr.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static XElement AddOrReplaceAttribute(this XElement source, XAttribute newAttr)
        {
            Contract.Requires(source != null);
            Contract.Requires(newAttr != null);

            var foundAttr = source.Attributes(newAttr.Name.LocalName).FirstOrDefault();
            if (foundAttr != null)
            {
                foundAttr.Remove();
            }
            source.Add(newAttr);
            return source;
        }

        /// <summary>
        /// Gets the string value of a span or null if a specific value is not found.
        /// </summary>
        /// <param name="source">The element that contains the span</param>
        /// <param name="attributeName">The local name of the span (assumes it is in the JDF namespace)</param>
        /// <returns>The value in the actual attribute if found.  If not, returns the value in preferred.  If that is not
        /// found either, returns null.</returns>
        public static string GetSpanAttributeActualPreferredOrNull(this XElement source, string attributeName)
        {
            Contract.Requires(source != null);
            Contract.Requires(attributeName != null);

            return GetSpanAttributeActualPreferredOrNull(source,
                                                         XName.Get(attributeName, Globals.Namespace.ToString()));
        }

        /// <summary>
        /// Gets the string value of a span or null if a specific value is not found.
        /// </summary>
        /// <param name="source">The element that contains the span</param>
        /// <param name="attributeName">The fully-qualified XName of the span </param>
        /// <returns>The value in the actual attribute if found.  If not, returns the value in preferred.  If that is not
        /// found either, returns null.</returns>
        public static string GetSpanAttributeActualPreferredOrNull(this XElement source, XName attributeName)
        {
            Contract.Requires(source != null);
            Contract.Requires(attributeName != null);

            return source.Descendants(attributeName).FirstOrDefault().GetActualPreferredOrNull();
        }

        /// <summary>
        /// Get the actual or preferred attribute of the first element
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetActualPreferredOrNull(this XElement source)
        {
            Contract.Requires(source != null);

            return source == null
                       ? null
                       : source.GetAttributeValueOrNull("Actual") ?? source.GetAttributeValueOrNull("Preferred");
        }
    }

}