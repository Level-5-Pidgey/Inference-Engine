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

        /// <summary>
        /// Function that will start from the query value and work through each clause in the kb to see if it can reach the facts within the kb
        /// </summary>
        /// <returns>Returns a boolean value a backwards chain can be created including the ASK element</returns>
        protected override bool CanFindQuery()
        {
            Element lCurrentElement; //Store currently expanded element here
            List<Element> lFacts = GetFacts();

            fInputChain.Add(new Element(fKB.Query, false, false)); //Start at the query value and expand from there

            while (fInputChain.Count != 0)
            {
                lCurrentElement = fInputChain[0]; //Explore the oldest existing element within the list (for the first loop it will be the ASK query)
                fInputChain.RemoveAt(0);

                fOutputChain.Add(lCurrentElement); //Once explored we can add to the output

                //If the current element is a fact we have reached the bottom of the "clause tree" and no longer need to search
                if (!lFacts.Contains(lCurrentElement))
                {
                    /*
                     * Using a temporary list to store elements found before adding them back into the InputChain next loop to prevent
                     * endless search loops (basically, prevent duplicate elements being added)
                     */
                    List<Element> lTempElements = new List<Element>();

                    //Loop through all clauses in the KB
                    for (int i = 0; i < fKB.ClauseCount; i++)
                    {
                        //Check if the implied element in the curent clause is the same as the current element -- can we evaluate the current element from the elements in this clause
                        //Implied elements are positioned to the right of the implication (=>)
                        if (fKB.Clauses[i].Elements.Any(e => e.IsImplied && e.Name == lCurrentElement.Name)) 
                        {
                            //Since this clause implies the current element being searched, it is an optimal clause to further explore.
                            //Now we need to add all of the elements in the clause to the list so it can be explored in the next loop
                            foreach (Element e in fKB.Clauses[i].Elements)
                            {
                                //If the input chain doesn't already contain the currently explored element, add it to the temp list
                                if (!fInputChain.Contains(e))
                                {
                                    lTempElements.Add(e);
                                }
                            }
                        }
                    }

                    //Now we've collected all the elements to add, we can check if they're duplicates
                    if (lTempElements.Count == 0) 
                    {
                        //If 0 elements were collected then the search is a failure
                        return false;
                    }
                    else
                    {
                        //Otherwise add non-explored (in the output chain already) elements
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

            return true;
        }
    }
}

