using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class Element
    {
        public bool State
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int SignificantBit
        {
            get;
            set;
        }

        public Element(string name, int significantBit, bool state)
        {
            Name = name;
            SignificantBit = significantBit;
            State = state;
        }

        public void SetState()
        {
            State = !State;
        }
    }
}
