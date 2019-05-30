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
            Element lCurrentElement;
            List<Element> lFacts = GetFacts();

            fInputChain.Add(new Element(fKB.Query, false, false));

            while (fInputChain.Count != 0)
            {
                lCurrentElement = fInputChain[0];
                fInputChain.RemoveAt(0);

                fOutputChain.Add(lCurrentElement);

                if (!lFacts.Contains(lCurrentElement))
                {
                    /*
                     * Using a temporary list to store elements found before adding them back into the InputChain next loop to prevent
                     * endless search loops (basically, prevent duplicate elements being added)
                     */
                    List<Element> lTempElements = new List<Element>();

                    for (int i = 0; i < fKB.ClauseCount; i++)
                    {
                        if(fKB.Clauses[i].Elements.Any(e => e.IsImplied && e.Name == lCurrentElement.Name))
                        {
                            foreach (Element e in fKB.Clauses[i].Elements)
                            {
                                if (!fInputChain.Contains(e))
                                {
                                    lTempElements.Add(e);
                                }
                            }
                        }
                    }

                    if (lTempElements.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (Element e in lTempElements)
                        {
                            if (!fOutputChain.Contains(e))
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

