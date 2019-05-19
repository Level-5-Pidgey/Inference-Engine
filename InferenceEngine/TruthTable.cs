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

        public int QueryResults
        {
            get;
            private set;
        }

        public string OutputQueryResult()
        {
            string result;

            if(QueryResults > 0)
            {
                result = "YES (" + _kb.Query + "): " + QueryResults.ToString();
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
            QueryResults = 0;
            _kb = kb;

            foreach (Clause c in kb.Clauses)
            {
                EvaluateClause(c);
                Debug.WriteLine("");
            }
        }

        public void EvaluateClause(Clause clause)
        {
            //Write all of the elements within the truth table so we know what variables are involved
            #region
            string elements = "";
            string breakerLine = "";

            Debug.WriteLine(clause.ToString());
            foreach (Element e in clause.Elements)
            {
                if (elements == "")
                {
                    if (e.Name.Length > 1)
                    {
                        elements = " |" + e.Name;
                    }
                    else
                    {
                        elements = " | " + e.Name;
                    }
                }
                else
                {
                    if (e.Name.Length > 1)
                    {
                        elements += " |" + e.Name;
                    }
                    else
                    {
                        elements += " | " + e.Name;
                    }
                }

                for (int i = 0; i < e.Name.Length; i++)
                {
                    breakerLine += "=";
                }
                breakerLine += "====";
            }
            breakerLine += "======";
            Debug.WriteLine(elements + " || RESULT");
            Debug.WriteLine(breakerLine);
            #endregion

            //Now write all states of the elements within the clause
            for (int i = 1; i < Math.Pow(2, clause.ElementsCount) + 1; i++)
            {
                for (int j = clause.ElementsCount - 1; j >= 0; j--)
                {
                    //If the remainder of i divided by the significant bit value of the variable is equal to 1, or if the significant bit is already equal to 1, swap the states.
                    if (i%clause.Elements[j].SignificantBit == 1 || clause.Elements[j].SignificantBit == 1)
                    {
                        clause.Elements[j].SetState();
                    }

                }

                //Now we have printed all elements in each state, we can calculate the resulting outcome (depending on the current state of each element in that line)
                foreach(Element e in clause.Elements)
                {
                    Debug.Write(" | " + e.ToString());
                }

                Debug.Write(" || ");
                
                bool outputResult = ProvideOutput(clause);
                if(outputResult)
                {
                    Debug.Write("T");
                    if(clause.ContainsASK)
                    {
                        if(clause.ElementsCount == 2 && clause.Elements[1].Name == _kb.Query)
                        {
                            QueryResults++;
                        }
                        else if (clause.ElementsCount == 3 && clause.Elements[2].Name == _kb.Query)
                        {
                            QueryResults++;
                        }
                        //Since this state results in true and contains the ASK statement
                        
                    }
                }
                else
                {
                    Debug.Write("F");
                }

                Debug.Write(Environment.NewLine);
            }
        }

        public bool ProvideOutput(Clause clause)
        {
            if (clause.IsFact)
            {
                if (clause.Elements[0].State)
                {
                    return true;
                }
            }
            else
            {
                if (clause.ContainsImplication)
                {
                    if (clause.ContainsConjunction)
                    {
                        //The clause is a ^ b -> c
                        return clause.CalculateConjunctionAndImplication();
                    }
                    else
                    {
                        //The clause is a -> b
                        return clause.CalculateImplication();
                    }
                }
                else
                {
                    //The clause is a ^ b
                    return clause.CalculateConjunction();
                }
            }

            return false; //unless explicitly stated we can assume the result is false
        }
    }
}
