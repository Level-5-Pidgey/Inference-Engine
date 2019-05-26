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
        List<Element> Facts = new List<Element>();
        List<Clause> Clauses = new List<Clause>();

        public ForwardChaining(KnowledgeBase kb)
        {
            _kb = kb;

            Facts = GetFacts();
            Clauses = _kb.Clauses;
            InputChain = Facts;
        }

        public bool FindQuery()
        {
            while (InputChain.Count != 0)
            {
                Element currentElement = InputChain[0];
                InputChain.RemoveAt(0);

                OutputChain.Add(currentElement);

                if (currentElement.Name == _kb.Query)
                {
                    return true;
                }

                for (int i = 0; i < Clauses.Count; i++)
                {
                    for (int j = 0; j < _kb.Elements.Count; j++)
                    {
                        if (currentElement.Name == Clauses[i].Elements[j].Name)
                        {
                            Clauses[i].Elements[j]
                        }
                    }

                    foreach (Element e in Clauses.ElementAt(i))
                }

                foreach (Clause c in Clauses)
                {
                    foreach(Element e in c.Elements)
                    {
                        if (currentElement.Name == e.Name)
                        {

                        }
                    }
                }
            }
        }

        public List<Element> GetFacts()
        {
            List<Element> factsList = new List<Element>();

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
