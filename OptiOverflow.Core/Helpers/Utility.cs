namespace OptiOverflow.Core.Helpers;

public class Utility
{
    public static bool SearchStringArray(in string[] values, in string input)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            return true;
        var stringInput = input.ToLower().Trim();

        var stringValues = values
            .Select(value => value?.ToLower().Trim())
            .Where(value => !string.IsNullOrWhiteSpace(value) || !string.IsNullOrEmpty(value))
            .ToList();

        return stringValues.Count > 0 && stringValues.Any(value => value != null && value.Contains(stringInput));
    }
}