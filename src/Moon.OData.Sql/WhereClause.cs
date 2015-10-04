using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.OData.Core.UriParser.TreeNodeKinds;
using Moon.OData.Edm;

namespace Moon.OData.Sql
{
    /// <summary>
    /// The <c>WHERE</c> SQL clause builder.
    /// </summary>
    public class WhereClause
    {
        readonly string oprator;
        readonly IList<object> arguments;
        readonly IODataOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhereClause" /> class.
        /// </summary>
        /// <param name="oprator">The operator to start with.</param>
        /// <param name="arguments">The list where to store values of SQL arguments.</param>
        /// <param name="options">The OData query options.</param>
        public WhereClause(string oprator, IList<object> arguments, IODataOptions options)
        {
            Requires.NotNull(oprator, nameof(oprator));
            Requires.NotNull(arguments, nameof(arguments));
            Requires.NotNull(options, nameof(options));

            this.oprator = oprator;
            this.arguments = arguments;
            this.options = options;
        }

        /// <summary>
        /// Gets or sets a function used to resolve column names.
        /// </summary>
        public Func<PropertyInfo, string> ResolveColumn { get; set; } = p => $"[{p.Name}]";

        /// <summary>
        /// Builds a <c>WHERE</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="startWith">The operator to start with.</param>
        /// <param name="arguments">The list where to store values of SQL arguments.</param>
        /// <param name="options">The OData query options.</param>
        public static string Build(string startWith, IList<object> arguments, IODataOptions options)
            => Build(startWith, arguments, options, null);

        /// <summary>
        /// Builds a <c>WHERE</c> SQL clause using the given OData query options.
        /// </summary>
        /// <param name="startWith">The operator to start with.</param>
        /// <param name="arguments">The list where to store values of SQL arguments.</param>
        /// <param name="options">The OData query options.</param>
        /// <param name="resolveColumn">A function used to resolve column names.</param>
        public static string Build(string startWith, IList<object> arguments, IODataOptions options, Func<PropertyInfo, string> resolveColumn)
        {
            var clause = new WhereClause(startWith, arguments, options);

            if (resolveColumn != null)
            {
                clause.ResolveColumn = resolveColumn;
            }

            return clause.Build();
        }

        /// <summary>
        /// Builds a <c>WHERE</c> SQL clause. The method returns an empty string when $filter option
        /// is not defined.
        /// </summary>
        public string Build()
        {
            var builder = new StringBuilder();

            if (options.Filter != null)
            {
                builder.Append($"{oprator.ToUpper()} ");
                AppendQueryNode(builder, options.Filter.Expression);
            }

            return builder.ToString();
        }

        void AppendQueryNode(StringBuilder builder, QueryNode node)
        {
            switch (node.Kind)
            {
                case QueryNodeKind.Convert:
                    AppendQueryNode(builder, (node as ConvertNode).Source);
                    return;

                case QueryNodeKind.Constant:
                    AppendConstantNode(builder, node as ConstantNode);
                    return;

                case QueryNodeKind.BinaryOperator:
                    AppendBinaryOperatorNode(builder, node as BinaryOperatorNode);
                    return;

                case QueryNodeKind.UnaryOperator:
                    AppendUnaryOperatorNode(builder, node as UnaryOperatorNode);
                    return;

                case QueryNodeKind.SingleValuePropertyAccess:
                    AppendSingleValuePropertyAccessNode(builder, node as SingleValuePropertyAccessNode);
                    return;

                case QueryNodeKind.SingleValueFunctionCall:
                    AppendSingleValueFunctionCallNode(builder, node as SingleValueFunctionCallNode);
                    break;

                default:
                    throw new ODataException($"The '{node.GetType().Name}' query node is not supported.");
            }
        }

        void AppendConstantNode(StringBuilder builder, ConstantNode node)
        {
            if (node.Value == null)
            {
                builder.Append("NULL");
            }
            else
            {
                builder.AppendArgument(arguments.Count);
                arguments.Add(node.Value);
            }
        }

        void AppendBinaryOperatorNode(StringBuilder builder, BinaryOperatorNode node)
        {
            builder.Append("(");

            AppendQueryNode(builder, node.Left);

            if (!IsMethodCall(node.Left))
            {
                var constantNode = GetConstantNode(node.Right);

                if (constantNode != null && constantNode.Value == null)
                {
                    if (node.OperatorKind == BinaryOperatorKind.Equal)
                    {
                        builder.Append(" IS ");
                    }
                    else if (node.OperatorKind == BinaryOperatorKind.NotEqual)
                    {
                        builder.Append(" IS NOT ");
                    }
                }
                else
                {
                    builder.Append($" {ToSqlOperator(node.OperatorKind)} ");
                }

                AppendQueryNode(builder, node.Right);
            }

            builder.Append(")");
        }

        void AppendUnaryOperatorNode(StringBuilder builder, UnaryOperatorNode node)
        {
            builder.Append($"{ToSqlOperator(node.OperatorKind)} ");
            AppendQueryNode(builder, node.Operand);
        }

        void AppendSingleValuePropertyAccessNode(StringBuilder builder, SingleValuePropertyAccessNode node)
        {
            var property = node.Property as EdmClrProperty;

            if (property == null)
            {
                throw new ODataException($"The '{property.GetType().Name}' property is not supported.");
            }

            var column = ResolveColumn(property.Property);

            if (column == null)
            {
                throw new ODataException("The column name is invalid.");
            }

            builder.Append(column);
        }

        void AppendSingleValueFunctionCallNode(StringBuilder builder, SingleValueFunctionCallNode node)
        {
            var parameters = node.Parameters.ToList();

            switch (node.Name)
            {
                case "contains":
                    AppendQueryNode(builder, parameters[0]);
                    builder.Append(" LIKE ('%' + ");
                    AppendQueryNode(builder, parameters[1]);
                    builder.Append(" + '%')");
                    break;

                case "endswith":
                    AppendQueryNode(builder, parameters[0]);
                    builder.Append(" LIKE ('%' + ");
                    AppendQueryNode(builder, parameters[1]);
                    builder.Append(")");
                    break;

                case "startswith":
                    AppendQueryNode(builder, parameters[0]);
                    builder.Append(" LIKE (");
                    AppendQueryNode(builder, parameters[1]);
                    builder.Append(" + '%')");
                    break;

                case "indexof":
                    builder.Append("CHARINDEX(");
                    AppendQueryNode(builder, parameters[1]);
                    builder.Append(", ");
                    AppendQueryNode(builder, parameters[0]);
                    builder.Append(")");
                    break;

                case "trim":
                    builder.Append("LTRIM(RTRIM(");
                    AppendQueryNode(builder, parameters[0]);
                    builder.Append("))");
                    break;

                case "hour":
                case "minute":
                case "second":
                    builder.Append($"DATEPART({node.Name}, ");
                    AppendQueryNode(builder, parameters[0]);
                    builder.Append(")");
                    break;

                case "date":
                case "time":
                    builder.Append("CAST(");
                    AppendQueryNode(builder, parameters[0]);
                    builder.Append($" AS {node.Name})");
                    break;

                case "totaloffsetminutes":
                    builder.Append("DATEPART(TZoffset, ");
                    AppendQueryNode(builder, parameters[0]);
                    builder.Append(")");
                    break;

                case "totalseconds":
                    builder.Append("DATEDIFF(second, 0, ");
                    AppendQueryNode(builder, parameters[0]);
                    builder.Append(")");
                    break;

                case "length":
                case "substring":
                case "tolower":
                case "toupper":
                case "concat":
                case "year":
                case "month":
                case "day":
                case "now":
                case "round":
                case "floor":
                case "ceiling":
                    builder.Append($"{ToSqlFunction(node.Name)}(");

                    for (int i = 0; i < parameters.Count; i++)
                    {
                        AppendQueryNode(builder, parameters[i]);

                        if (i < parameters.Count - 1)
                        {
                            builder.Append(", ");
                        }
                    }

                    builder.Append(")");
                    break;

                default:
                    throw new ODataException($"The function '{node.Name}' is not supported.");
            }
        }

        bool IsMethodCall(SingleValueNode node)
        {
            var callNode = node as SingleValueFunctionCallNode;

            if (callNode != null)
            {
                return callNode.Name == "contains"
                    || callNode.Name == "endswith"
                    || callNode.Name == "startswith";
            }

            return false;
        }

        ConstantNode GetConstantNode(SingleValueNode node)
        {
            if (node is ConvertNode)
            {
                return GetConstantNode((node as ConvertNode).Source);
            }

            return node as ConstantNode;
        }

        string ToSqlOperator(BinaryOperatorKind operatorKind)
        {
            switch (operatorKind)
            {
                case BinaryOperatorKind.Or:
                    return "OR";

                case BinaryOperatorKind.And:
                    return "AND";

                case BinaryOperatorKind.Equal:
                    return "=";

                case BinaryOperatorKind.NotEqual:
                    return "<>";

                case BinaryOperatorKind.GreaterThan:
                    return ">";

                case BinaryOperatorKind.GreaterThanOrEqual:
                    return ">=";

                case BinaryOperatorKind.LessThan:
                    return "<";

                case BinaryOperatorKind.LessThanOrEqual:
                    return "<=";

                case BinaryOperatorKind.Add:
                    return "+";

                case BinaryOperatorKind.Subtract:
                    return "-";

                case BinaryOperatorKind.Multiply:
                    return "*";

                case BinaryOperatorKind.Divide:
                    return "/";

                case BinaryOperatorKind.Modulo:
                    return "%";

                default:
                    throw new ODataException($"The operator '{operatorKind}' is not supported.");
            }
        }

        string ToSqlOperator(UnaryOperatorKind operatorKind)
        {
            if (operatorKind != UnaryOperatorKind.Not)
            {
                throw new ODataException($"The operator '{operatorKind}' is not supported.");
            }

            return "NOT";
        }

        string ToSqlFunction(string functionName)
        {
            switch (functionName)
            {
                case "length":
                    return "LEN";

                case "tolower":
                    return "LOWER";

                case "toupper":
                    return "UPPER";

                case "now":
                    return "GETUTCDATE";

                default:
                    return functionName.ToUpperInvariant();
            }
        }
    }
}