using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class TruthTable
    {
        private KnowledgeBase fKB;
        private List<List<Element>> fStateGrid = new List<List<Element>>();
        private List<bool> fModelResult = new List<bool>();

        public int ASKCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Formats the ASKCount nicely to display to the user on console
        /// </summary>
        /// <returns>String containing the amount of times the ASK Query element can result true in all possible models.</returns>
        public string OutputQueryResult()
        {
            string result;

            if(ASKCount > 0)
            {
                result = "YES (" + fKB.Query + "): " + ASKCount.ToString();
            }
            else
            {
                result = "NO (" + fKB.Query + ")";
            }

            return result;
        }

        public TruthTable(KnowledgeBase aKB)
        {
            //Counter for the number of states where the ASK section returns true
            ASKCount = 0;
            fKB = aKB;

            PopulateTruthTable();
            EvaluateStates();
        }

        /// <summary>
        /// Populates the truth table with every possible state of the elements in the KB
        /// O(2^n) time complexity(?) : the test KB has a 2048 (2^11) different possible states for the elements
        /// </summary>
        private void PopulateTruthTable()
        {
            for (int i = 0; i < Math.Pow(2, fKB.ElementsCount); i++)
            {
                //Add one new row for each element there is in the knowledgebase
                fStateGrid.Add(new List<Element>());
                fModelResult.Add(true); //To start, we can assume each outcome is true (and will change these to false later when evaluating states)

                for (int j = 0; j < fKB.ElementsCount; j++)
                {
                    //Credit to Dhass on this CareerCup post on how to populate truth table grids easily:
                    //https://www.careercup.com/question?id=17632666

                    int k = i & 1 << fKB.ElementsCount - 1 - j;

                    fStateGrid[i].Add(new Element(fKB.Elements[j].Name, (k == 0 ? true : false)));
                }
            }
        }

        /// <summary>
        /// Evaluates the generated states of the elements in the KB alters and how many models result in the ASK Query element being true
        /// </summary>
        private void EvaluateStates()
        {
            //Check the state of all elements in each potential model
            for (int i = 0; i < Math.Pow(2, fKB.ElementsCount); i++)
            {
                for(int j = 0; j < fKB.ElementsCount; j++)
                {
                    if(fModelResult[i]) //This lets us skip models that we have evaluated to false
                    {
                        if((fStateGrid[i][j].Name == fKB.Query) && (!fStateGrid[i][j].State)) //is this element within the grid the query?
                        {
                            fModelResult[i] = false; //If the ASK element is false, the model cannot be true/optimal

                            break; //Now we know this model is not optimal we can move on and skip evaluating the rest of the elements in this model
                        }
                    }
                }
            }

            for (int i = 0; i < Math.Pow(2, fKB.ElementsCount); i++)
            {
                if (fModelResult[i])
                {
                    //Within this model we want to:
                    //(1) Set the elements within the clause to match the states in the current model
                    //(2) Then, evaluate the clause and see if it results in a truth
                    //(3) if the result is false, then this clause is not part of the optimal model
                    //(4) After iterating through the list, any models where the query element is true can be left true

                    foreach (Clause c in fKB.Clauses)
                    {
                        c.MatchStates(fStateGrid[i]); //(1) Set the elements within the clause to match the states in the current model
                        c.ResolveClauseStates(); //(2) Then, evaluate the clause and see if it results in a truth
                    }

                    bool lOptimalModel = true;
                    foreach(Clause c in fKB.Clauses)
                    {
                        if(!c.Resolution)
                        {
                            lOptimalModel = false; //(3) if the result is false, then this clause is not part of the optimal model
                            break;
                        }
                        //(4) After iterating through the list, any models where the query element is true can be left true
                    }

                    if (lOptimalModel)
                    {
                        ASKCount++;
                    }
                }
            }
        }
    }
}
