using System.Text;

namespace Futurum.Microsoft.Extensions.DependencyInjection.Generator.Extensions;

public static class EnumerableExtensions
{
    public static string ToDelimitedString<T>(this IEnumerable<T> values) =>
        values.ToDelimitedString(",");

    public static string ToDelimitedString<T>(this IEnumerable<T>? values, string delimiter)
    {
        if (values is null)
            return string.Empty;

        var sb = new StringBuilder();
        foreach (var value in values)
        {
            if (sb.Length > 0)
                sb.Append(delimiter ?? ",");
            sb.Append(value);
        }

        return sb.ToString();
    }

    public static string ToDelimitedString(this IEnumerable<string> values) =>
        values.ToDelimitedString(",");

    public static string ToDelimitedString(this IEnumerable<string> values, string delimiter) =>
        values.ToDelimitedString(delimiter, null);

    public static string ToDelimitedString(this IEnumerable<string>? values, string delimiter, Func<string, string>? escapeDelimiter)
    {
        if (values is null)
            return string.Empty;

        var stringBuilder = new StringBuilder();
        foreach (var value in values)
        {
            if (stringBuilder.Length > 0)
                stringBuilder.Append(delimiter);

            var v = escapeDelimiter != null
                ? escapeDelimiter(value ?? string.Empty)
                : value ?? string.Empty;

            stringBuilder.Append(v);
        }

        return stringBuilder.ToString();
    }
}