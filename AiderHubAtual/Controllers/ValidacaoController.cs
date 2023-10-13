using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AiderHubAtual.Controllers
{
    public class ValidacaoController : Controller
    {
        public static bool ValidateCPF(string cpf)
        {
            // Remove caracteres não numéricos do CPF
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Verifica se o CPF possui 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Calcula o dígito verificador
            int[] cpfArray = cpf.Select(c => int.Parse(c.ToString())).ToArray();
            int sum = 0;
            int factor = 10;

            for (int i = 0; i < 9; i++)
            {
                sum += cpfArray[i] * factor;
                factor--;
            }

            int remainder = sum % 11;
            int firstDigit = (remainder < 2) ? 0 : 11 - remainder;

            // Verifica o primeiro dígito verificador
            if (cpfArray[9] != firstDigit)
                return false;

            // Calcula o segundo dígito verificador
            sum = 0;
            factor = 11;

            for (int i = 0; i < 10; i++)
            {
                sum += cpfArray[i] * factor;
                factor--;
            }

            remainder = sum % 11;
            int secondDigit = (remainder < 2) ? 0 : 11 - remainder;

            // Verifica o segundo dígito verificador
            return cpfArray[10] == secondDigit;
        }


        public static bool ValidateCNPJ(string cnpj)
        {
            // Remove caracteres não numéricos do CNPJ
            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            // Verifica se o CNPJ possui 14 dígitos
            if (cnpj.Length != 14)
                return false;

            // Calcula o primeiro dígito verificador
            int[] cnpjArray = cnpj.Select(c => int.Parse(c.ToString())).ToArray();
            int[] weights = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum += cnpjArray[i] * weights[i];
            }

            int remainder = sum % 11;
            int firstDigit = (remainder < 2) ? 0 : 11 - remainder;

            // Verifica o primeiro dígito verificador
            if (cnpjArray[12] != firstDigit)
                return false;

            // Calcula o segundo dígito verificador
            sum = 0;
            weights = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int i = 0; i < 13; i++)
            {
                sum += cnpjArray[i] * weights[i];
            }

            remainder = sum % 11;
            int secondDigit = (remainder < 2) ? 0 : 11 - remainder;

            // Verifica o segundo dígito verificador
            return cnpjArray[13] == secondDigit;
        }
    }
}
