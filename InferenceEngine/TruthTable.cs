using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class TruthTable
    {
        public List<Element> Elements
        {
            get;
            set;
        } = new List<Element>();

        public TruthTable(KnowledgeBase kb)
        {
            EvaluateClause(kb.Clauses[0]);
        }

        public List<Element> FindElements(string clause)
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

        public void EvaluateClause(string clause)
        {
            List<Element> clauseElements = FindElements(clause);

            Console.WriteLine("=========================================");
            for (int i = 1; i < Math.Pow(2, clauseElements.Count()) + 1; i++)
            {
                for (int j = clauseElements.Count - 1; j >= 0; j--)
                {
                    //If the remainder of i divided by the significant bit value of the variable is equal to 1, or if the significant bit is already equal to 1, swap the states.
                    if ((i%clauseElements[j].SignificantBit == 1) || clauseElements[j].SignificantBit == 1)
                    {
                        clauseElements[j].SetState();
                    }

                    //Represents the state boolean as a string (so it can be printed to the console!)
                    if (clauseElements[j].State == true)
                    {
                        Console.Write(" | 1");
                    }
                    else
                    {
                        Console.Write(" | 0");
                    }
                }

                //Now we have printed all elements in each state, we can calculate the resulting outcome (depending on the current state of each element in that line)
                Console.Write(" || ");
                ProvideOutput(clauseElements);
                Console.Write(Environment.NewLine);
            }
        }

        public void ProvideOutput(List<Element> clauseElements)
        {
            
        }
    }
}
