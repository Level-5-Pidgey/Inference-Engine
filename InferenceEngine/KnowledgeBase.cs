using System;
using System.Collections.Generic;
using System.IO;

namespace InferenceEngine
{
    public class KnowledgeBase
    {
        public List<Clause> Clauses
        {
            get;
            private set;
        } = new List<Clause>();

        public List<string> Facts //Can represent as strings since we're only using them for comparison
        {
            get;
            private set;
        } = new List<string>();

        public string Query
        {
            get;
            private set;
        }

        public KnowledgeBase(string fileName)
        {
            LoadFile(fileName);

            EstablishFacts();
        }

        private void LoadFile(string fileName)
        {
            //Example file knowledge base + query:
            //TELL
            //p2 => p3; p3 => p1; c => e; b & e => f; f & g => h; p1 => d; p1 & p3 => c; a; b; p2;
            //ASK
            //d

            StreamReader reader = new StreamReader(fileName);
            try
            {
                string line = reader.ReadLine();
                string clauseLine = "";
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
                    clauseLine = line; //Save this data and move on
                }

                //Now the "TELL" section should have been completed, we can do the "ASK" section
                line = reader.ReadLine();
                if (line != "ASK")
                {
                    throw new FormatException("File is not formatted correctly. Please check the file you are using, consult the README and try again.");
                }
                else
                {
                    List<string> clauses = new List<string>();
                    line = reader.ReadLine();

                    Query = line;

                    //now we know what the query is we can convert the string clauses into objects
                    string[] splitString = clauseLine.Split(';');
                    foreach (string s in splitString)
                    {
                        clauses.Add(s);
                    }

                    for (int i = 0; i < clauses.Count; i++)
                    {
                        Clauses.Add(new Clause(clauses[i], Query));
                    }
                }
            }
            finally
            {
                reader.Close();
            }
        }

        private void EstablishFacts()
        {
            //We want to find all clauses that are facts and make all elements that are facts be placed on a list
            foreach (Clause c in Clauses)
            {
                if(c.IsFact)
                {
                    if(!Facts.Contains(c.Elements[0].Name))
                    {
                        Facts.Add(c.Elements[0].Name);
                    }
                }
            }
        }
    }
}
