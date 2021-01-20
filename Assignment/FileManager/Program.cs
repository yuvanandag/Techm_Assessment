using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please provide valid input file path");
            GenerateFile(@"E:\A_Tech_Interview\Attended\Microsoft\File\inputFile.txt");
        }

        static void GenerateFile(string inputFilePath)
        {
            try
            {
                var vowels = new HashSet<Char> { 'a', 'e', 'i', 'o', 'u' };
                if (File.Exists(inputFilePath))
                {
                    var inputSentence = File.ReadAllText(inputFilePath);
                    var SentenceList = inputSentence.Split('.');
                    List<(int, string)> vowelSentenseList = new List<(int, string)>();
                    foreach(var sentense in SentenceList)
                    {
                        if (!string.IsNullOrWhiteSpace(sentense))
                        {
                            int totalVowelsCount = sentense.ToLower().Count(a => vowels.Contains(a));
                            vowelSentenseList.Add((totalVowelsCount, sentense));
                        }
                    }
                    GenerateOutputFile(vowelSentenseList, inputFilePath);
                }
                else
                {
                    Console.WriteLine("Please provide valid file path");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        static void GenerateOutputFile(List<(int, string)> vowelSentenseList, string inputFilePath)
        {
            var resultSet = vowelSentenseList.OrderByDescending(a => a.Item1).ToList();
            StringBuilder sbOutput = new StringBuilder();
            foreach(var result in resultSet)
            {
                sbOutput.Append(result.Item2 + ".");
            }

            FileInfo file = new FileInfo(inputFilePath);
            var directoryName = file.DirectoryName;
            var outputFilePath = Path.Combine(directoryName, "OutputFile.txt");

            File.WriteAllText(outputFilePath, sbOutput.ToString());
            Console.WriteLine("Outpu file is generated");
        }
    }
}
