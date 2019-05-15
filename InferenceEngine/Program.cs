using System;
using System.IO;

namespace InferenceEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            //Variable assignment
            KnowledgeBase kb;
            TruthTable testTruth;

            if (args.Length != 2)
            {
                throw new ArgumentException("2 arguments are expected. Please re-try the program with appropriate arguments.");
            }

            string fileDir = args[1] + ".txt";
            string engineMethod = args[0];

            if (!File.Exists(fileDir))
            {
                throw new FileNotFoundException("Could not find the file you are looking for.");
            }
            else
            {
                kb = new KnowledgeBase(fileDir);
                testTruth = new TruthTable(kb);
            }
        }
    }
}
