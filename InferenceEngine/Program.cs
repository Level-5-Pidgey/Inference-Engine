using System;
using System.Diagnostics;
using System.IO;

namespace InferenceEngine
{
    class Program
    {
        static void Main(string[] aArgs)
        {
            //Variable assignment
            KnowledgeBase fKB;

            if (aArgs.Length != 2)
            {
                throw new ArgumentException("2 arguments are expected. Please re-try the program with appropriate arguments.");
            }

            string lFileDir = aArgs[1] + ".txt";
            string lEngineMethod = aArgs[0];

            if (!File.Exists(lFileDir))
            {
                throw new FileNotFoundException("Could not find the file you are looking for.");
            }
            else
            {
                fKB = new KnowledgeBase(lFileDir);
            }

            switch (lEngineMethod.ToLower())
            {
                case "forwardchaining":
                case "forwardchain":
                case "fchaining":
                case "fchain":
                case "fc":
                    {
                        Console.WriteLine("+======================================+");
                        Console.WriteLine("+= Initializing Forward Chaining...   =+");
                        Console.WriteLine("+======================================+");
                        Console.WriteLine("");

                        var algorithmTimer = new Stopwatch();
                        ForwardChaining fc = new ForwardChaining(fKB);
                        algorithmTimer.Start();
                        Console.WriteLine(fc.OutputQuery());
                        algorithmTimer.Stop();

                        Console.WriteLine("Time elapsed: " + algorithmTimer.ElapsedMilliseconds + "ms");
                        break;
                    }
                case "backwardchaining":
                case "backwardchain":
                case "bchaining":
                case "bchain":
                case "bc":
                    {
                        Console.WriteLine("+======================================+");
                        Console.WriteLine("+= Initializing Backwards Chaining... =+");
                        Console.WriteLine("+======================================+");
                        Console.WriteLine("");

                        var algorithmTimer = new Stopwatch();
                        BackwardsChaining bc = new BackwardsChaining(fKB);
                        algorithmTimer.Start();
                        Console.WriteLine(bc.OutputQuery());
                        algorithmTimer.Stop();

                        Console.WriteLine("Time elapsed: " + algorithmTimer.ElapsedMilliseconds + "ms");
                        break;
                    }
                case "truthtable":
                case "tt":
                case "trutht":
                case "ttable":
                    {
                        Console.WriteLine("+======================================+");
                        Console.WriteLine("+= Initializing Truth Table Gen. ...  =+");
                        Console.WriteLine("+======================================+");
                        Console.WriteLine("");

                        var algorithmTimer = new Stopwatch();
                        algorithmTimer.Start();
                        TruthTable tt = new TruthTable(fKB);
                        Console.WriteLine(tt.OutputQueryResult());
                        algorithmTimer.Stop();

                        Console.WriteLine("Time elapsed: " + algorithmTimer.ElapsedMilliseconds + "ms");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid/Improper pathfinding type provided. Consult the README for additional information. The program will now exit.");
                        break;
                    }
            }
        }
    }
}
