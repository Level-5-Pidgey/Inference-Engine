using System;
using System.Collections.Generic;
using System.IO;

namespace InferenceEngine
{
    public class KnowledgeBase
    {
        public List<string> Facts
        {
            get;
            set;
        } = new List<string>();
        public List<string> Clauses
        {
            get;
            set;
        } = new List<string>();
        public string Query
        {
            get;
            private set;
        }

        public KnowledgeBase(string fileName)
        {
            LoadFile(fileName);
        }

        private void LoadFile(string fileName)
        {
            //Example file knowledge base + query:
            //TELL
            //p2 => p3; p3 => p1; c => e; b & e => f; f & g => h; p1 => d; p1 & p3 => c; a; b; p2;
            //ASK
            //d
            List<string> clauses = new List<string>();

            StreamReader reader = new StreamReader(fileName);
            try
            {
                string line = reader.ReadLine();
                //read the file in order, line by line
                //all files must start with TELL, signalling the knowledgebase.
                if (line != "TELL")
                {
                    throw new FormatException("File is not formatted correctly. Please check the file you are using, consult the README and try again.");
                }
                else
                {
                    line = reader.ReadLine();
                    line = line.Replace(" ", string.Empty);
                    if (line.EndsWith(";"))
                    {
                        line = line.Remove(line.Length - 1); //Remove the final ; at the end of the list of clauses/facts 
                    }
                    string[] splitString = line.Split(';');
                    foreach (string s in splitString)
                    {
                        clauses.Add(s);
                    }

                    for (int i = 0; i < clauses.Count; i++)
                    {
                        if (clauses[i].Contains("=>"))
                        {
                            //These are statements where "if A is true, B is true" or "if A and B are true, C is true"
                            Clauses.Add(clauses[i]);
                        }
                        else
                        {
                            Facts.Add(clauses[i]);
                        }
                    }
                }

                //Now the "TELL" section should have been completed, we can do the "ASK" section
                line = reader.ReadLine();
                if (line != "ASK")
                {
                    throw new FormatException("File is not formatted correctly. Please check the file you are using, consult the README and try again.");
                }
                else
                {
                    line = reader.ReadLine();

                    Query = line;
                }
            }
            finally
            {
                reader.Close();
            }
        }
    }
}
