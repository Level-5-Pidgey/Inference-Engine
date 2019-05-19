using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class Clause
    {
        public string StringForm
        {
            get;
            private set;
        }

        public List<Element> Elements
        {
            get;
            set;
        } = new List<Element>();

        public int ElementsCount
        {
            get;
            private set;
        }

        public bool ContainsConjunction
        {
            get;
            private set;
        }

        public bool IsFact
        {
            get;
            private set;
        }

        public bool ContainsImplication
        {
            get;
            private set;
        }

        public bool ContainsASK
        {
            get;
            private set;
        }

        public Clause(string clauseString, string query)
        {
            //Format the object within constructor
            //Setting properties of the object
            if (clauseString.Contains(query))
            {
                ContainsASK = true;
            }

            if(clauseString.Contains("=>"))
            {
                ContainsImplication = true;
            }

            if(clauseString.Contains("&"))
            {
                ContainsConjunction = true;
            }

            //More complex properties -- string form and elements
            StringForm = clauseString;
            Elements = FindElements(clauseString);

            ElementsCount = Elements.Count(); //So that other methods don't need to use the .Count() method multiple times (increases performance theoretically)
            if (ElementsCount == 1)
            {
                IsFact = true;
            }
        }

        private List<Element> FindElements(string clause)
        {
            string[] dividedclauses = Regex.Split(clause, "=>");
            List<Element> result = new List<Element>();

            //Go through all of the strings passed to the method to start comparison
            int count = 1;
            foreach (string s in dividedclauses)
            {
                //Need to further expand this by checking for OR statements and for elements within brackets (as these may be provided as elements to be made into a truth table)
                //Check if this statement contains one or two elements
                if (s.Contains("&"))
                {
                    string[] conditionals = s.Split('&');
                    foreach (string c in conditionals)
                    {
                        if (!result.Any(q => q.Name == c))
                        {
                            result.Add(new Element(c, count, true));
                            count *= 2;
                        }
                    }
                }
                else
                {
                    if (!result.Any(q => q.Name == s))
                    {
                        result.Add(new Element(s, count, true));
                        count *= 2;
                    }
                }
            }

            return result;
        }

        public bool CalculateConjunctionAndImplication()
        {
            //The clause is a ^ b -> c
            if (
                //(clause.Elements[0].State && clause.Elements[1].State && clause.Elements[2].State) ||
                //!(clause.Elements[0].State && clause.Elements[1].State && clause.Elements[2].State) ||
                //(!clause.Elements[0].State && !clause.Elements[1].State && clause.Elements[2].State) ||
                //((!clause.Elements[0].State || !clause.Elements[1].State) && !clause.Elements[2].State) ||
                //((!clause.Elements[0].State || !clause.Elements[1].State) && clause.Elements[2].State)

                !(Elements[0].State && Elements[1].State && !Elements[2].State)
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CalculateImplication()
        {
            //The clause is a -> b
            if (
                (!Elements[0].State && !Elements[1].State) ||
                (!Elements[0].State && Elements[1].State) ||
                (Elements[0].State && Elements[1].State)
               )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CalculateConjunction()
        {
            //The clause is a ^ b
            if (Elements[0].State && Elements[1].State)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return StringForm;
        }
    }
}
