namespace ControleDeBar.WebApp.Extensions
{
    public static class StringExtensions
    {
        public static string FormatarCPF(this string cpf)
        {
            if (string.IsNullOrEmpty(cpf) || cpf.Length != 11)
                return cpf;

            return System.Text.RegularExpressions.Regex.Replace(cpf, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");
        }
    }
}
