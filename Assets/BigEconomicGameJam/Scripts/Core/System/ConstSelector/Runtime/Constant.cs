using UnityEngine;

namespace CORE.CONST_SELECTOR
{
    public class Constant: PropertyAttribute
    {
        public string ID;

        public Constant(string id)
        {
            ID = id;
        }
    }
}