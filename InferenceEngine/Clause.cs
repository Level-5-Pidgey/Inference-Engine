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
            get
            {
                return Elements.Count();
            }
        }

        public bool IsFact
        {
            get;
            private set;
        }

        public bool SkipInChaining //Used for the purpose of FC/BC to delegate if this clause should be iterated over
        {
            get;
            set;
        }

        public bool ContainsASK
        {
            get;
            private set;
        }

        public bool Resolution
        {
            get;
            set;
        }

        public Clause(string aClauseString, string aQuery)
        {
            //Format the object within constructor
            //Setting properties of the object
            if (aClauseString.Contains(aQuery))
            {
                ContainsASK = true;
            }

            //More complex properties -- string form and elements
            StringForm = aClauseString;
            Elements = FindElements(aClauseString);

            if (ElementsCount == 1)
            {
                IsFact = true;
                SkipInChaining = true;
            }
            else
            {
                SkipInChaining = false;
            }
        }

        /// <summary>
        /// Uses string splitting and regex to find all of the elements within a string-form clause and create a list of elements.
        /// </summary>
        /// <param name="aClause">The clause in string form to find elements from.</param>
        /// <returns>Returns a list of elements with their states set to false.</returns>
        private List<Element> FindElements(string aClause)
        {
            string[] lDividedClauses = Regex.Split(aClause, "=>");
            List<Element> lResult = new List<Element>();

            //Go through all of the strings passed to the method to start comparison
            foreach (string s in lDividedClauses)
            {
                //Check if this statement contains one or two elements
                if (s.Contains("&"))
                {
                    string[] fConditionals = s.Split('&');
                    foreach (string c in fConditionals)
                    {
                        lResult.Add(new Element(c));
                    }
                }
                else
                {
                    if(lDividedClauses.Last() == s)
                    {
                        lResult.Add(new Element(s, false, true));
                    }
                    else
                    {
                        lResult.Add(new Element(s));
                    }
                }
            }

            return lResult;
        }

        /// <summary>
        /// Ensures all states within the given list are the same on both sides.
        /// </summary>
        /// <param name="aElements">List of elements to match the states of.</param>
        public void MatchStates(List<Element> aElements)
        {
            foreach (Element e1 in aElements)
            {
                foreach (Element e2 in Elements)
                {
                    if(e1.Equals(e2))
                    {
                        e2.State = e1.State;
                    }
                }
            }
        }

        /// <summary>
        /// Evaluates if the clause, based on the current states of all elements within the clause, resolves to be true.
        /// </summary>
        public void ResolveClauseStates()
        {
            //In the case of this assignment, Horn clauses only contain implications (=>) and conjunctions (&). Therefore, if all left-allocated elements are true, the right must be true for the result to be true.
            //e.g. a ^ b:
            //a = T, b = T, result = T
            //any other combination would result in F.
            //For implications and conjunctions:
            //a^b => c
            //a = T, b = T, c = F, result = F
            //since both left-associated elements are true, c HAS to be true, making the statement false. If the right-most element is false then the statement can't be correct.
            //any other arrangement of values means that the result is true, as either the conjunction is not met and therefore the value cannot be implied or the implication results in true
            Resolution = true;

            if (!IsFact)
            {
                if (IsAllTrue (Elements.FindAll(e => !e.IsImplied)) && !IsAllTrue(Elements.FindAll(e => e.IsImplied)))
                {
                    Resolution = false;
                }
            }
            else
            {
                if(!Elements[0].State) //since Facts only have 1 element it'll always be index 0
                {
                    Resolution = false;
                }
            }
        }

        /// <summary>
        /// Checks if all elements have their states set to true within a given list. Used for the resolution process of states.
        /// </summary>
        /// <param name="elements">List of elements to check if all have their states set to true.</param>
        /// <returns>Returns true if all elements in the list have true states</returns>
        private bool IsAllTrue(List<Element> elements)
        {
            bool result = true;

            foreach (Element e in elements)
            {
                if (!e.State)
                {
                    result = false;
                }
            }

            return result;
        }

        public override string ToString()
        {
            return StringForm;
        }
    }
}
