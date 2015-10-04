using System.Text;

namespace Moon.OData.Sql
{
    static class StringBuilderExteinsions
    {
        public static StringBuilder AppendWithSpace(this StringBuilder builder, string value)
        {
            if (value.Length > 0)
            {
                builder.Append($" {value}");
            }

            return builder;
        }

        public static StringBuilder AppendArgument(this StringBuilder builder, int number)
            => builder.Append($"@p{number}");
    }
}