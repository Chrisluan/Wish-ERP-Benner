using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wish.ERP.Benner.Services
{
    public static class Validator
    {
        public static bool IsCPFValid(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11)
                return false;
            if (cpf.Distinct().Count() == 1)
                return false;
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += (cpf[i] - '0') * (10 - i);

            int remainder = sum % 11;
            int firstDigit = (remainder < 2) ? 0 : 11 - remainder;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += (cpf[i] - '0') * (11 - i);

            remainder = sum % 11;
            int secondDigit = (remainder < 2) ? 0 : 11 - remainder;
            return (cpf[9] - '0' == firstDigit) && (cpf[10] - '0' == secondDigit);
        }
        public static bool IsFieldsFilled(Dictionary<object, object> keyValues, out List<object> emptyFields)
        {
            emptyFields = new List<object>();

            if (keyValues == null)
                return false;

            foreach (var kv in keyValues)
            {
                bool isEmpty = false;

                switch (kv.Value)
                {
                    case TextBox tb:
                        isEmpty = string.IsNullOrWhiteSpace(tb.Text);
                        break;

                    case ComboBox cb:
                        isEmpty = cb.SelectedItem == null || string.IsNullOrWhiteSpace(cb.Text);
                        break;

                    case CheckBox chk:
                        isEmpty = chk.IsChecked == false;
                        break;

                    default:
                        isEmpty = kv.Value == null || kv.Value.Equals("");
                        break;
                }

                if (isEmpty)
                    emptyFields.Add(kv.Key);
            }

            return emptyFields.Count == 0;
        }


    }
}
