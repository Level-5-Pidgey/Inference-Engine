using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InferenceEngine
{
    public class KnowledgeBase
    {
        public List<Clause> Clauses
        {
            get;
            private set;
        } = new List<Clause>();

        public List<Element> Elements
        {
            get;
            private set;
        } = new List<Element>();

        public string Query
        {
            get;
            private set;
        }

        public int ElementsCount
        {
            get
            {
                return Elements.Count();
            }
        }

        public int ClauseCount
        {
            get
            {
                return Clauses.Count();
            }
        }

        public KnowledgeBase(string aFileName)
        {
            LoadFile(aFileName);

            Elements = ProvideUniqueElements();
        }

        /// <summary>
        /// Loads a file from the given fileName and creates a new knowledgebase.
        /// </summary>
        /// <param name="fileName">String form (with no filetype identifier) for the file name to open and load from.</param>
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
                string lLine = reader.ReadLine();
                string lClauseLine = "";

                //read the file in order, line by line
                //all files must start with TELL, signalling the knowledgebase.
                if (lLine != "TELL")
                {
                    throw new FormatException("File is not formatted correctly. Please check the file you are using, consult the README and try again.");
                }
                else
                {
                    lLine = reader.ReadLine();
                    lLine = lLine.Replace(" ", string.Empty);
                    if (lLine.EndsWith(";"))
                    {
                        lLine = lLine.Remove(lLine.Length - 1); //Remove the final ; at the end of the list of clauses/facts 
                    }
                    lClauseLine = lLine; //Save this data and move on
                }

                //Now the "TELL" section should have been completed, we can do the "ASK" section
                lLine = reader.ReadLine();
                if (lLine != "ASK")
                {
                    throw new FormatException("File is not formatted correctly. Please check the file you are using, consult the README and try again.");
                }
                else
                {
                    List<string> clauses = new List<string>();
                    lLine = reader.ReadLine();

                    Query = lLine;

                    //now we know what the query is we can convert the string clauses into objects
                    string[] splitString = lClauseLine.Split(';');
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

        /// <summary>
        /// Creates a list of all unique elements within a knowledgebase.
        /// </summary>
        /// <returns>Returns a list of all unique elements within the KB.</returns>
        private List<Element> ProvideUniqueElements()
        {
            List<Element> lUniqueElements = new List<Element>();

            foreach (Clause c in Clauses) //KB's contain a list of clauses that we need to break down
            {
                foreach (Element e in c.Elements) //Each of these clauses contains an element, each with a name in string format
                {
                    if (!lUniqueElements.Any(e2 => e2.Name == e.Name)) //if an element with a specific name contained in the clauses isn't on the uniqueElements list, add it to that list.
                    {
                        lUniqueElements.Add(e);
                    }
                }
            }

            return lUniqueElements;
        }
    }
}
