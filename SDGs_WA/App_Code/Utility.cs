
public static class Utility
{
    public static string StringCrop(this string text, int maxLength)
    {
        if (text == null) return string.Empty;

        if (text.Length < maxLength) return text;

        return text.Substring(0, maxLength) + "...";
    }
}