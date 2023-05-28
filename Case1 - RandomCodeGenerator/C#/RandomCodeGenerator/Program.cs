using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace RamdomCodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            CodeGenerator generator = new CodeGenerator();
            List<string> codes = generator.GenerateCodes(1000);

            foreach (string code in codes)
            {
                Console.WriteLine($"Kod {codes.IndexOf(code)}: {code}");
            }
        }

    }
    public class CodeGenerator
    {
        private const string allowedCharacters = "ACDEFGHKLMNPRTXYZ234579";
        private const int codeLength = 8;

        public List<string> GenerateCodes(int countOfCode)
        {
            var generatedCodes = new List<string>();

            for (int i = 0; i < countOfCode; i++)
            {
                string code = GenerateUniqueCode(generatedCodes);
                generatedCodes.Add(code);
            }

            return generatedCodes;
        }

        private string GenerateUniqueCode(List<string> existingCodes)
        {
            string code = GenerateRandomCode();


            while (existingCodes.Contains(code))
            {
                code = GenerateRandomCode();
            }

            return code;
        }

        private string GenerateRandomCode()
        {
            var random = new Random();
            char[] code = new char[codeLength];

            for (int i = 0; i < codeLength; i++)
            {
                code[i] = allowedCharacters[random.Next(0, allowedCharacters.Length)];
            }

            return new string(code);
        }
    }
}
