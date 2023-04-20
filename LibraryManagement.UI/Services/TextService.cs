using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibraryManagement.UI.Services
{
    public class TextService
    {
        public string ConvertToUnSign(string s)
        {
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            var temp = s.Normalize(NormalizationForm.FormD);
            var textUnSign = regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            return RemoveSpaces(textUnSign);
        }
        public string RemoveSpaces(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            while (text.Contains("  "))
                text = text.Replace("  ", " ");
            return text;
        }
    }
}
