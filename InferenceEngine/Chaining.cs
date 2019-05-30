using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public abstract class Chaining
    {
        protected KnowledgeBase fKB;
        protected List<Element> fOutputChain = new List<Element>();
        protected List<Element> fInputChain = new List<Element>();

        public Chaining(KnowledgeBase aKB)
        {
            fKB = aKB;
        }

        protected abstract bool CanFindQuery();

        public string OutputQuery()
        {
            string fOutput;
            if (CanFindQuery())
            {
                fOutput = "YES (" + fKB.Query + "): ";

                fOutputChain.Sort(); //Sort the output chain for a more aesthetic presentation of results

                for (int i = 0; i < fOutputChain.Count; i++)
                {
                    fOutput += fOutputChain[i].Name;

                    if (i < fOutputChain.Count - 1) //Add a comma after the element name, except if it's the last element
                    {
                        fOutput += ", ";
                    }
                }

                fOutput += ";"; //Add a semicolon at the end to signify that it's the end of the output chain
            }
            else
            {
                fOutput = "NO (" + fKB.Query + ");";
            }

            return fOutput;
        }

        protected List<Element> GetFacts()
        {
            List<Element> lFactsList = new List<Element>();

            foreach (Clause c in fKB.Clauses)
            {
                if (c.IsFact)
                {
                    lFactsList.Add(c.Elements[0]);
                }
            }

            return lFactsList;
        }
    }
}
