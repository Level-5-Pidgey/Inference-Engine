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

        protected override bool CanFindQuery()
        {
            Element lCurrentElement;

            foreach (Clause c in fClauses)
            {
                if (c.IsFact)
                {
                    c.SkipInChaining = true;
                }

                foreach (Element e in c.Elements)
                {
                    if (e.IsImplied)
                    {
                        e.State = true;
                    }
                }
            }

            while (fInputChain.Count != 0)
            {
                lCurrentElement = fInputChain[0];
                fInputChain.RemoveAt(0);

                fOutputChain.Add(lCurrentElement);

                if (lCurrentElement.Name == fKB.Query)
                {
                    return true;
                }

                for (int i = 0; i < fClauses.Count; i++)
                {
                    if (!fClauses[i].SkipInChaining)
                    {
                        for (int j = 0; j < fClauses[i].ElementsCount; j++)
                        {
                            if (!fClauses[i].Elements[j].State) //Skip elements marked as true
                            {
                                if (lCurrentElement.Equals(fClauses[i].Elements[j]))
                                {
                                    fClauses[i].Elements[j].State = true;
                                }
                            }
                        }

                        if (!fClauses[i].SkipInChaining)
                        {
                            if (fClauses[i].Elements.All(e => e.State))
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
