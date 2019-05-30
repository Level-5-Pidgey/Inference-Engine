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

                    fStateGrid[i].Add(new Element(fKB.Elements[j].Name, k == 0 ? true : false));
                }
            }
        }

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

            for (int i = 0; i < Math.Pow(2, fKB.Elements.Count); i++)
            {
                if (fModelResult[i])
                {
                    //Within this model we want to:
                    //Set the elements within the clause to match the states in the current model
                    //Then, evaluate the clause and see if it results in a truth
                    //if the result is false, change _modelResult[i] to false also
                    //After iterating through the list, any models where the query element is true can be left true

                    foreach (Clause c in fKB.Clauses)
                    {
                        c.MatchStates(fStateGrid[i]);
                        c.ResolveClauseStates();
                    }

                    bool lOptimalModel = true;
                    foreach(Clause c in fKB.Clauses)
                    {
                        if(!c.Resolution)
                        {
                            lOptimalModel = false;
                            break;
                        }
                    }

                    if(lOptimalModel)
                    {
                        ASKCount++;
                    }
                }
            }
        }
    }
}
