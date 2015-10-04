using System.Runtime.CompilerServices;
using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.OData.Core.UriParser.TreeNodeKinds;

namespace Moon.OData.Validators
{
    /// <summary>
    /// Represents a validator used to validate a $filter query option value.
    /// </summary>
    public class FilterQueryValidator
    {
        /// <summary>
        /// Validates the parsed $filter query option.
        /// </summary>
        /// <param name="filter">The parsed $filter query option.</param>
        /// <param name="settings">The validation settings.</param>
        public virtual void Validate(FilterClause filter, ValidationSettings settings)
        {
            Requires.NotNull(filter, nameof(filter));
            Requires.NotNull(settings, nameof(settings));

            ValidateQueryNode(filter.Expression, settings);
        }

        void ValidateQueryNode(QueryNode node, ValidationSettings settings)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();

            var singleNode = node as SingleValueNode;
            var collectionNode = node as CollectionNode;

            if (singleNode != null)
            {
                ValidateSingleValueNode(singleNode, settings);
            }
            else if (collectionNode != null)
            {
                ValidateCollectionNode(collectionNode, settings);
            }
        }

        void ValidateSingleValueNode(SingleValueNode node, ValidationSettings settings)
        {
            switch (node.Kind)
            {
                case QueryNodeKind.Convert:
                    ValidateQueryNode((node as ConvertNode).Source, settings);
                    break;

                case QueryNodeKind.BinaryOperator:
                    ValidateBinaryOperatorNode(node as BinaryOperatorNode, settings);
                    break;

                case QueryNodeKind.UnaryOperator:
                    ValidateUnaryOperatorNode(node as UnaryOperatorNode, settings);
                    break;

                case QueryNodeKind.SingleValuePropertyAccess:
                    ValidateQueryNode((node as SingleValuePropertyAccessNode).Source, settings);
                    break;

                case QueryNodeKind.SingleValueFunctionCall:
                    ValidateSingleValueFunctionCallNode(node as SingleValueFunctionCallNode, settings);
                    break;

                case QueryNodeKind.Any:
                    ValidateAnyNode(node as AnyNode, settings);
                    break;

                case QueryNodeKind.SingleEntityCast:
                    ValidateQueryNode((node as SingleEntityCastNode).Source, settings);
                    break;

                case QueryNodeKind.All:
                    ValidateAllNode(node as AllNode, settings);
                    break;

                case QueryNodeKind.SingleEntityFunctionCall:
                    ValidateSingleEntityFunctionCallNode(node as SingleEntityFunctionCallNode, settings);
                    break;
            }
        }

        void ValidateCollectionNode(CollectionNode node, ValidationSettings settings)
        {
            switch (node.Kind)
            {
                case QueryNodeKind.CollectionPropertyAccess:
                    ValidateQueryNode((node as CollectionPropertyAccessNode).Source, settings);
                    break;

                case QueryNodeKind.EntityCollectionCast:
                    ValidateQueryNode((node as EntityCollectionCastNode).Source, settings);
                    break;
            }
        }

        void ValidateBinaryOperatorNode(BinaryOperatorNode node, ValidationSettings settings)
        {
            switch (node.OperatorKind)
            {
                case BinaryOperatorKind.Equal:
                case BinaryOperatorKind.NotEqual:
                case BinaryOperatorKind.And:
                case BinaryOperatorKind.GreaterThan:
                case BinaryOperatorKind.GreaterThanOrEqual:
                case BinaryOperatorKind.LessThan:
                case BinaryOperatorKind.LessThanOrEqual:
                case BinaryOperatorKind.Or:
                case BinaryOperatorKind.Has:
                    ValidateLogicalOperator(node, settings);
                    break;

                default:
                    ValidateArithmeticOperator(node, settings);
                    break;
            }
        }

        void ValidateUnaryOperatorNode(UnaryOperatorNode node, ValidationSettings settings)
        {
            ValidateQueryNode(node.Operand, settings);

            switch (node.OperatorKind)
            {
                case UnaryOperatorKind.Negate:
                case UnaryOperatorKind.Not:
                    if (!settings.AllowedOperators.HasFlag(AllowedOperators.Not))
                    {
                        throw new ODataException($"The '{node.OperatorKind}' logical operator is not allowed.");
                    }
                    break;
            }
        }

        void ValidateSingleValueFunctionCallNode(SingleValueFunctionCallNode node, ValidationSettings settings)
        {
            ValidateFunction(node.Name, settings);

            foreach (var argumentNode in node.Parameters)
            {
                ValidateQueryNode(argumentNode, settings);
            }
        }

        void ValidateSingleEntityFunctionCallNode(SingleEntityFunctionCallNode node, ValidationSettings settings)
        {
            ValidateFunction(node.Name, settings);

            foreach (var argumentNode in node.Parameters)
            {
                ValidateQueryNode(argumentNode, settings);
            }
        }

        void ValidateNavigationPropertyNode(QueryNode node, ValidationSettings settings)
        {
            if (node != null)
            {
                ValidateQueryNode(node, settings);
            }
        }

        void ValidateAnyNode(AnyNode node, ValidationSettings settings)
        {
            ValidateFunction("any", settings);
            ValidateQueryNode(node.Source, settings);

            if (node.Body != null && node.Body.Kind != QueryNodeKind.Constant)
            {
                ValidateQueryNode(node.Body, settings);
            }
        }

        void ValidateAllNode(AllNode node, ValidationSettings settings)
        {
            ValidateFunction("all", settings);
            ValidateQueryNode(node.Source, settings);
            ValidateQueryNode(node.Body, settings);
        }

        void ValidateLogicalOperator(BinaryOperatorNode node, ValidationSettings settings)
        {
            var op = ToLogicalOperator(node);

            if (!settings.AllowedOperators.HasFlag(op))
            {
                throw new ODataException($"The '{op}' logical operator is not allowed.");
            }

            ValidateQueryNode(node.Left, settings);
            ValidateQueryNode(node.Right, settings);
        }

        void ValidateArithmeticOperator(BinaryOperatorNode node, ValidationSettings settings)
        {
            var op = ToArithmeticOperator(node);

            if (!settings.AllowedOperators.HasFlag(op))
            {
                throw new ODataException($"The '{op}' arithmetic operator is not allowed.");
            }

            ValidateQueryNode(node.Left, settings);
            ValidateQueryNode(node.Right, settings);
        }

        void ValidateFunction(string functionName, ValidationSettings settings)
        {
            var function = ToFunction(functionName);

            if (!settings.AllowedFunctions.HasFlag(function))
            {
                throw new ODataException($"The '{functionName}' function is not allowed.");
            }
        }

        AllowedOperators ToLogicalOperator(BinaryOperatorNode binaryNode)
        {
            var result = AllowedOperators.None;

            switch (binaryNode.OperatorKind)
            {
                case BinaryOperatorKind.Equal:
                    result = AllowedOperators.Equal;
                    break;

                case BinaryOperatorKind.NotEqual:
                    result = AllowedOperators.NotEqual;
                    break;

                case BinaryOperatorKind.GreaterThan:
                    result = AllowedOperators.GreaterThan;
                    break;

                case BinaryOperatorKind.GreaterThanOrEqual:
                    result = AllowedOperators.GreaterThanOrEqual;
                    break;

                case BinaryOperatorKind.LessThan:
                    result = AllowedOperators.LessThan;
                    break;

                case BinaryOperatorKind.LessThanOrEqual:
                    result = AllowedOperators.LessThanOrEqual;
                    break;

                case BinaryOperatorKind.And:
                    result = AllowedOperators.And;
                    break;

                case BinaryOperatorKind.Or:
                    result = AllowedOperators.Or;
                    break;

                case BinaryOperatorKind.Has:
                    result = AllowedOperators.Has;
                    break;
            }

            return result;
        }

        AllowedOperators ToArithmeticOperator(BinaryOperatorNode binaryNode)
        {
            var result = AllowedOperators.None;

            switch (binaryNode.OperatorKind)
            {
                case BinaryOperatorKind.Add:
                    result = AllowedOperators.Add;
                    break;

                case BinaryOperatorKind.Subtract:
                    result = AllowedOperators.Subtract;
                    break;

                case BinaryOperatorKind.Multiply:
                    result = AllowedOperators.Multiply;
                    break;

                case BinaryOperatorKind.Divide:
                    result = AllowedOperators.Divide;
                    break;

                case BinaryOperatorKind.Modulo:
                    result = AllowedOperators.Modulo;
                    break;
            }

            return result;
        }

        AllowedFunctions ToFunction(string functionName)
        {
            var result = AllowedFunctions.None;

            switch (functionName)
            {
                case "contains":
                    result = AllowedFunctions.Contains;
                    break;

                case "endswith":
                    result = AllowedFunctions.EndsWith;
                    break;

                case "startswith":
                    result = AllowedFunctions.StartsWith;
                    break;

                case "length":
                    result = AllowedFunctions.Length;
                    break;

                case "indexof":
                    result = AllowedFunctions.IndexOf;
                    break;

                case "substring":
                    result = AllowedFunctions.Substring;
                    break;

                case "tolower":
                    result = AllowedFunctions.ToLower;
                    break;

                case "toupper":
                    result = AllowedFunctions.ToUpper;
                    break;

                case "trim":
                    result = AllowedFunctions.Trim;
                    break;

                case "concat":
                    result = AllowedFunctions.Concat;
                    break;

                case "year":
                    result = AllowedFunctions.Year;
                    break;

                case "month":
                    result = AllowedFunctions.Month;
                    break;

                case "day":
                    result = AllowedFunctions.Day;
                    break;

                case "hour":
                    result = AllowedFunctions.Hour;
                    break;

                case "minute":
                    result = AllowedFunctions.Minute;
                    break;

                case "second":
                    result = AllowedFunctions.Second;
                    break;

                case "fractionalseconds":
                    result = AllowedFunctions.FractionalSeconds;
                    break;

                case "date":
                    result = AllowedFunctions.Date;
                    break;

                case "time":
                    result = AllowedFunctions.Time;
                    break;

                case "totaloffsetminutes":
                    result = AllowedFunctions.TotalOffsetMinutes;
                    break;

                case "now":
                    result = AllowedFunctions.Now;
                    break;

                case "maxdatetime":
                    result = AllowedFunctions.MaxDateTime;
                    break;

                case "mindatetime":
                    result = AllowedFunctions.MinDateTime;
                    break;

                case "totalseconds":
                    result = AllowedFunctions.TotalSeconds;
                    break;

                case "round":
                    result = AllowedFunctions.Round;
                    break;

                case "floor":
                    result = AllowedFunctions.Floor;
                    break;

                case "ceiling":
                    result = AllowedFunctions.Ceiling;
                    break;

                case "isof":
                    result = AllowedFunctions.IsOf;
                    break;

                case "cast":
                    result = AllowedFunctions.Cast;
                    break;

                case "any":
                    result = AllowedFunctions.Any;
                    break;

                case "all":
                    result = AllowedFunctions.All;
                    break;
            }

            return result;
        }
    }
}