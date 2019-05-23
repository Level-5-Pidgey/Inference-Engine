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

        public bool IsImplied
        {
            get;
            set;
        }

        public Element(string name, bool state = false, bool implied = false)
        {
            Name = name;
            IsImplied = implied;
            State = state;
        }

        public void SetState()
        {
            State = !State;
        }

        public override string ToString()
        {
            return Name + " : " + State.ToString(); 
        }
    }
}
