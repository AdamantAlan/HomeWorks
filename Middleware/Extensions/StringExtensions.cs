namespace Middleware.Extensions
{
    public static class StringExtensions
    {
        public static string GetHiddenPan(this string pan) => pan.Replace(pan.Substring(0, pan.Length - 4), new string('*', pan.Length - 4));

        public static string GetHiddenCvc(this string cvc) => "***";
    }
}
