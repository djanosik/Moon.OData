using System;

namespace Moon.OData
{
    /// <summary>
    /// Logical and arithmetic operators to allow for querying using $filter.
    /// </summary>
    [Flags]
    public enum AllowedOperators
    {
        /// <summary>
        /// A value that corresponds to allowing no logical operators in $filter.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// A value that corresponds to allowing 'Equal' logical operator in $filter.
        /// </summary>
        Equal = 0x1,

        /// <summary>
        /// A value that corresponds to allowing 'NotEqual' logical operator in $filter.
        /// </summary>
        NotEqual = 0x2,

        /// <summary>
        /// A value that corresponds to allowing 'GreaterThan' logical operator in $filter.
        /// </summary>
        GreaterThan = 0x4,

        /// <summary>
        /// A value that corresponds to allowing 'GreaterThanOrEqual' logical operator in $filter.
        /// </summary>
        GreaterThanOrEqual = 0x8,

        /// <summary>
        /// A value that corresponds to allowing 'LessThan' logical operator in $filter.
        /// </summary>
        LessThan = 0x10,

        /// <summary>
        /// A value that corresponds to allowing 'LessThanOrEqual' logical operator in $filter.
        /// </summary>
        LessThanOrEqual = 0x20,

        /// <summary>
        /// A value that corresponds to allowing 'And' logical operator in $filter.
        /// </summary>
        And = 0x40,

        /// <summary>
        /// A value that corresponds to allowing 'Or' logical operator in $filter.
        /// </summary>
        Or = 0x80,

        /// <summary>
        /// A value that corresponds to allowing 'Not' logical operator in $filter.
        /// </summary>
        Not = 0x100,

        /// <summary>
        /// A value that corresponds to allowing 'Has' logical operator in $filter.
        /// </summary>
        Has = 0x200,

        /// <summary>
        /// A value that corresponds to allowing 'Add' arithmetic operator in $filter.
        /// </summary>
        Add = 0x400,

        /// <summary>
        /// A value that corresponds to allowing 'Subtract' arithmetic operator in $filter.
        /// </summary>
        Subtract = 0x800,

        /// <summary>
        /// A value that corresponds to allowing 'Multiply' arithmetic operator in $filter.
        /// </summary>
        Multiply = 0x1000,

        /// <summary>
        /// A value that corresponds to allowing 'Divide' arithmetic operator in $filter.
        /// </summary>
        Divide = 0x2000,

        /// <summary>
        /// A value that corresponds to allowing 'Modulo' arithmetic operator in $filter.
        /// </summary>
        Modulo = 0x4000,

        /// <summary>
        /// A value that corresponds to allowing all logical operators in $filter.
        /// </summary>
        Logical = Equal | NotEqual | GreaterThan | GreaterThanOrEqual | LessThan | LessThanOrEqual | And | Or | Not | Has,

        /// <summary>
        /// A value that corresponds to allowing all arithmetic operators in $filter.
        /// </summary>
        Arithmetic = Add | Subtract | Multiply | Divide | Modulo,

        /// <summary>
        /// A value that corresponds to allowing all operators in $filter.
        /// </summary>
        All = Logical | Arithmetic
    }
}