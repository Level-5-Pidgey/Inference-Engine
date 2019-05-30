using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class ForwardChaining
    {
        KnowledgeBase _kb;
        List<Element> InputChain = new List<Element>();
        List<Element> OutputChain = new List<Element>();
        List<Element> Facts = new List<Element>();
        List<Clause> Clauses = new List<Clause>();

        public ForwardChaining(KnowledgeBase kb)
        {
            _kb = kb;

            Facts = GetFacts();
            Clauses = _kb.Clauses;
            InputChain = Facts;
        }

        private bool CanFindQuery()
        {
            while (InputChain.Count != 0)
            {
                Element currentElement = InputChain[0];
                InputChain.RemoveAt(0);

                OutputChain.Add(currentElement);

                if (currentElement.Name == _kb.Query)
                {
                    return true;
                }

                foreach (Clause c in Clauses)
                {
                    if (!c.SkipInChaining && c.ElementsCount > 1)
                    {
                        foreach (Element e in c.Elements)
                        {
                            if ((currentElement.Name == e.Name) && !e.State)
                            {
                                e.SetState(); //If the element's state is true, it's already been addressed and doesn't need to be anymore
                            }
                        }
                    }
                    else if (!c.SkipInChaining)
                    {
                        InputChain.Add(c.Elements[0]);
                        c.SkipInChaining = true;
                    }
                    else
                    {
                        c.SkipInChaining = true; //This clause is just a fact, and since we have already added all facts to the InputChain we can disregard it
                    }
                }
            }

            return false; //We've exhausted all possible elements, so there is no way to reach the query element
        }

        public string OutputQuery()
        {
            string output;
            if (CanFindQuery())
            {
                output = "YES (" + _kb.Query + "): ";

                for (int i = 0; i < OutputChain.Count; i++)
                {
                    output += OutputChain[i].Name;

                    if (i < OutputChain.Count - 1) //Add a comma after the element name, except if it's the last element
                    {
                        output += ", "; 
                    }
                }

                output += ";"; //Add a semicolon at the end to signify that it's the end of the output chain
            }
            else
            {
                output = "NO (" + _kb.Query + ");";
            }

            return output;
        }

        private List<Element> GetFacts()
        {
            List<Element> factsList = new List<Element>();

            foreach(Clause c in _kb.Clauses)
            {
                if(c.IsFact)
                {
                    factsList.Add(c.Elements[0]);
                }
            }

            return factsList;
        }
    }
}
