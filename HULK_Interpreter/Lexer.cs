﻿using System.Text.RegularExpressions;

namespace HULK_Interpreter
{
    public class Lexer
    {
        public static void Main(string[] args)
        {

        }
        public static void Tokenize(string SourceCode)
        {

            //expresiones regulares para los tokens
            //string keywordPattern = @"\b(if|else|true|false|boolean|number)\b";
            string operatorPattern = @"[+\-]";
            string symbolPattern = @"[()]";
            //string identifierPattern = @"\b\w+\b";
            string numberPattern = @"\b\d+(\.\d+)?\b";

            //Creando la regex
            string pattern = $"{operatorPattern}|{symbolPattern}|{numberPattern}";
            Regex regex = new Regex(pattern);

            //Tokenizar
            MatchCollection matches = regex.Matches(SourceCode);

            // Imprimir los tokens encontrados
            foreach (Match match in matches)
            {
                Console.WriteLine(match.Value);
            }
        }
    }
    
}