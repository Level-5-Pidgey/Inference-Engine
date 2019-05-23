using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class TruthTable
    {
        private KnowledgeBase _kb;
        private List<Element> _uniqueElements;
        private List<List<Element>> _stateGrid = new List<List<Element>>();
        private List<bool> _modelResult = new List<bool>();

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
                result = "YES (" + _kb.Query + "): " + ASKCount.ToString();
            }
            else
            {
                result = "NO (" + _kb.Query + ")";
            }

            return result;
        }

        public TruthTable(KnowledgeBase kb)
        {
            //Counter for the number of states where the ASK section returns true
            ASKCount = 0;
            _kb = kb;
            _uniqueElements = ProvideUniqueElements(kb.Clauses);

            PopulateTruthTable();
            EvaluateStates();
        }

        private void PopulateTruthTable()
        {
            for(int i = 0; i < Math.Pow(2, _uniqueElements.Count); i++)
            {
                //Add one new row for each element there is in the knowledgebase
                _stateGrid.Add(new List<Element>());
                _modelResult.Add(true); //To start, we can assume each outcome is true (and will change these to false later when evaluating states)

                for (int j = 0; j < _uniqueElements.Count; j++)
                {
                    //Credit to Dhass on this CareerCup post on how to populate truth table grids easily:
                    //https://www.careercup.com/question?id=17632666

                    int k = i & 1 << _uniqueElements.Count - 1 - j;

                    _stateGrid[i].Add(new Element(_uniqueElements[j].Name, (k == 0 ? true : false)));
                }
            }
        }

        private void EvaluateStates()
        {
            //Check the state of all elements in each potential model
            for (int i = 0; i < Math.Pow(2, _uniqueElements.Count); i++)
            {
                for(int j = 0; j < _uniqueElements.Count; j++)
                {
                    if(_modelResult[i]) //This lets us skip models that we have evaluated to false
                    {
                        if((_stateGrid[i][j].Name == _kb.Query) && (!_stateGrid[i][j].State)) //is this element within the grid the query?
                        {
                            _modelResult[i] = false; //If the ASK element is false, the model cannot be true/optimal

                            break; //Now we know this model is not optimal we can move on and skip evaluating the rest of the elements in this model
                        }
                    }
                }
            }

            for (int i = 0; i < Math.Pow(2, _uniqueElements.Count); i++)
            {
                if (_modelResult[i])
                {
                    //Within this model we want to:
                    //Set the elements within the clause to match the states in the current model
                    //Then, evaluate the clause and see if it results in a truth
                    //if the result is false, change _modelResult[i] to false also
                    //After iterating through the list, any models where the query element is true can be left true

                    foreach (Clause c in _kb.Clauses)
                    {
                        c.MatchStates(_stateGrid[i]);
                        c.ResolveClauseStates();
                    }

                    bool optimalModel = true;
                    foreach(Clause c in _kb.Clauses)
                    {
                        if(!c.Resolution)
                        {
                            optimalModel = false;
                            break;
                        }
                    }

                    if(optimalModel)
                    {
                        ASKCount++;
                    }
                }
            }
        }

        private void RemoveFalseFacts()
        {
            //Any models where a fact is false is automatically not going to be an optimal model and therefore can be removed from the potential models of the truth table
            //Speeds up processing time by removing models that are never going to result in an optimal model

            for (int i = 0; i < _stateGrid.Count; i++)
            {
                foreach (Element e in _stateGrid[i])
                {
                    if ((!e.State) && _kb.Facts.Any(n1 => n1 == e.Name))
                    {
                        _stateGrid.RemoveAt(i);
                    }
                }
            }
        }

        private List<Element> ProvideUniqueElements(List<Clause> clauses)
        {
            List<Element> uniqueElements = new List<Element>();

            foreach (Clause c in clauses) //KB's contain a list of clauses that we need to break down
            {
                foreach (Element e in c.Elements) //Each of these clauses contains an element, each with a name in string format
                {
                    if (!uniqueElements.Any(e2 => e2.Name == e.Name)) //if an element with a specific name contained in the clauses isn't on the uniqueElements list, add it to that list.
                    {
                        uniqueElements.Add(e);
                    }
                }
            }

            return uniqueElements;
        }
    }
}
