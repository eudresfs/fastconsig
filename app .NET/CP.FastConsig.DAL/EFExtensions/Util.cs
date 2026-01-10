//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Microsoft.Data.Extensions
{
    /// <summary>
    /// General purpose helper methods.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Foreach element in the input, runs the specified action. Yields input elements.
        /// </summary>
        /// <typeparam name="TResult">Element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="action">Action to perform on each element of the source.</param>
        /// <returns>Source elements.</returns>
        public static IEnumerable<TResult> ForEach<TResult>(this IEnumerable<TResult> source, Action<TResult> action)
        {
            Utility.CheckArgumentNotNull(source, "source");
            Utility.CheckArgumentNotNull(action, "action");

            return ForEachIterator(source, action);
        }

        private static IEnumerable<TResult> ForEachIterator<TResult>(IEnumerable<TResult> source, Action<TResult> action)
        {
            foreach (TResult element in source)
            {
                action(element);
                yield return element;
            }
        }

        /// <summary>
        /// Determines whether a generic type definition is assignable from a type given some
        /// generic type arguments. For instance, <code>typeof(IEnumerable&lt;&gt;).IsGenericAssignableFrom(typeof(List&lt;int&gt;), out genericArguments)</code>
        /// returns true with generic arguments { typeof(int) }.
        /// </summary>
        /// <param name="toType">Target generic type definition (to which the value would be assigned).</param>
        /// <param name="fromType">Source type (instance of which is being assigned)</param>
        /// <param name="genericArguments">Returns generic type arguments required for the assignment to succeed
        /// or null if no such assignment exists.</param>
        /// <returns>true if the type can be assigned; otherwise false</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
        public static bool IsGenericAssignableFrom(this Type toType, Type fromType, out Type[] genericArguments)
        {
            Utility.CheckArgumentNotNull(toType, "toType");
            Utility.CheckArgumentNotNull(fromType, "fromType");

            if (!toType.IsGenericTypeDefinition ||
                fromType.IsGenericTypeDefinition)
            {
                // if 'toType' is not generic or 'fromType' is generic, the assignment pattern 
                // is not matched (e.g. toType<genericArguments>.IsAssignableFrom(fromType)
                // cannot be satisfied)
                genericArguments = null;
                return false;
            }

            if (toType.IsInterface)
            {
                // if the toType is an interface, simply look for the interface implementation in fromType
                foreach (Type interfaceCandidate in fromType.GetInterfaces())
                {
                    if (interfaceCandidate.IsGenericType && interfaceCandidate.GetGenericTypeDefinition() == toType)
                    {
                        genericArguments = interfaceCandidate.GetGenericArguments();
                        return true;
                    }
                }
            }
            else
            {
                // if toType is not an interface, check hierarchy for match
                while (fromType != null)
                {
                    if (fromType.IsGenericType && fromType.GetGenericTypeDefinition() == toType)
                    {
                        genericArguments = fromType.GetGenericArguments();
                        return true;
                    }
                    fromType = fromType.BaseType;
                }
            }
            genericArguments = null;
            return false;
        }

        internal static void CheckArgumentNotNull<T>(T argumentValue, string argumentName)
            where T : class
        {
            if (null == argumentValue)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
