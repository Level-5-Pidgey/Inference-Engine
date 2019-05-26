using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class ForwardChaining
    {
        KnowledgeBase _kb;
        List<Element> InputChain = new List<Element>();
        HashSet<Element> OutputChain = new HashSet<Element>();
        HashSet<Element> Facts = new HashSet<Element>();

        public ForwardChaining(KnowledgeBase kb)
        {
            _kb = kb;

            Facts = GetFacts();
        }

        public bool FindQuery()
        {
            while (InputChain.Count == 0)
            {
                Element currentElement = InputChain[0];
                InputChain.RemoveAt(0);

                OutputChain.Add(currentElement);

                if (currentElement.Name == _kb.Query)
                {
                    return true;
                }

                for (int i = 0; i < _kb.Clauses.Count; i++)
                {
                    for(int j = 0; j < _kb.Elements.Count; j++)
                    {
                        if (currentElement.Name == _kb.Clauses[i].Elements[j].Name)
                        {

                        }
                    }
                }
            }
        }

        public HashSet<Element> GetFacts()
        {
            HashSet<Element> factsList = new HashSet<Element>();

            foreach(Clause c in _kb.Clauses)
            {
                if(c.IsFact)
                {
                    factsList.Add(c.Elements[0]);
                }
            }

            return factsList;
        }
    }
}
