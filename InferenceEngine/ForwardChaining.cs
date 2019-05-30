using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class ForwardChaining : Chaining
    {
        private List<Clause> fClauses = new List<Clause>();

        public ForwardChaining(KnowledgeBase aKB) : base(aKB)
        {
            fInputChain = GetFacts();
            fClauses = fKB.Clauses;
        }

        /// <summary>
        /// Iterates through all elements within a knowledgebase and sees if it can reach the query starting from the KB's facts
        /// </summary>
        /// <returns></returns>
        protected override bool CanFindQuery()
        {
            Element lCurrentElement;

            foreach (Clause c in fClauses)
            {
                //Facts have already been added in the constructor to the input chain, so they should be marked to be skipped hereafter
                if (c.IsFact)
                {
                    c.SkipInChaining = true;
                }

                //If an element is on the right of an implication it also does not need to be searched/added to the input chain so it can be marked to be skipped
                foreach (Element e in c.Elements)
                {
                    if (e.IsImplied)
                    {
                        e.State = true;
                    }
                }
            }

            //Loop through the input chain until we either exhaust all possibilities or find the query
            while (fInputChain.Count != 0)
            {
                lCurrentElement = fInputChain[0]; //Explore the oldest existing element within the list (for the first loop it will be the ASK query)
                fInputChain.RemoveAt(0);

                fOutputChain.Add(lCurrentElement); //Once explored we can add to the output

                //If the current element is the same as the query, then the search is satisfied
                if (lCurrentElement.Name == fKB.Query)
                {
                    return true;
                }

                //Since this element is not the one being searched for, we can search for other possible elements
                for (int i = 0; i < fClauses.Count; i++)
                {
                    if (!fClauses[i].SkipInChaining) //Don't bother searching the elements of exhausted clauses
                    {
                        for (int j = 0; j < fClauses[i].ElementsCount; j++) //Loop through all elements within non-exhausted clauses
                        {
                            if (!fClauses[i].Elements[j].State) //Skip elements marked as true
                            {
                                if (lCurrentElement.Equals(fClauses[i].Elements[j])) //If the current element is found within this clause, set it as true (so it can be skipped)
                                {
                                    fClauses[i].Elements[j].State = true;
                                }
                            }
                        }

                        if (!fClauses[i].SkipInChaining) //For all non-skippable clauses...
                        {
                            if (fClauses[i].Elements.All(e => e.State)) //If all of the non-implied elements are marked to be skipped, we can abandon this clause and add the implied element to be searched for
                            {
                                fInputChain.Add(fClauses[i].Elements.Find(e => e.IsImplied));
                                fClauses[i].SkipInChaining = true;
                            }
                        }
                    }
                }
            }

            return false; //We've exhausted all possible elements, so there is no way to reach the query element
        }
    }
}
