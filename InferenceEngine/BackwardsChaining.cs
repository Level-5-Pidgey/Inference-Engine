using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class BackwardsChaining : Chaining
    {
        public BackwardsChaining(KnowledgeBase aKB) : base(aKB) { }

        protected override bool CanFindQuery()
        {
            Element currentElement;
            List<Element> Facts = GetFacts();

            fInputChain.Add(new Element(fKB.Query, false, false));

            while (fInputChain.Count != 0)
            {
                currentElement = fInputChain[0];
                fInputChain.RemoveAt(0);

                fOutputChain.Add(currentElement);

                if (!Facts.Any(f => f.Name == currentElement.Name))
                {
                    /*
                     * Using a temporary list to store elements found before adding them back into the InputChain next loop to prevent
                     * endless search loops (basically, prevent duplicate elements being added)
                     */
                    List<Element> tempElements = new List<Element>();

                    for (int i = 0; i < fKB.Clauses.Count; i++)
                    {
                        if(fKB.Clauses[i].Elements.Any(e => e.IsImplied && e.Name == currentElement.Name))
                        {
                            foreach (Element e in fKB.Clauses[i].Elements)
                            {
                                if (!fInputChain.Any(ic => ic.Name == e.Name))
                                {
                                    tempElements.Add(e);
                                }
                            }
                        }
                    }

                    if (tempElements.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (Element e in tempElements)
                        {
                            if (!fOutputChain.Any(o => o.Name == e.Name))
                            {
                                fInputChain.Add(e);
                            }
                        }
                    }
                }
            }

            return true; //We've exhausted all possible elements, so there is no way to reach the query element
        }
    }
}

