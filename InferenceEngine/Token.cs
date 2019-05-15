using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    public class Token
    {
        public enum TokenCategory
        {
            Undefined,
            Bool,
            Operator,
            LParent,
            RParent
        };

        public int? Priority
        {
            get;
            private set;
        }

        public TokenCategory Category
        {
            get;
            private set;
        }

        public bool IsVar
        {
            get;
            private set;
        }

        public bool IsLeftAssociated
        {
            get;
            private set;
        }

        public bool BoolValue
    }
}
