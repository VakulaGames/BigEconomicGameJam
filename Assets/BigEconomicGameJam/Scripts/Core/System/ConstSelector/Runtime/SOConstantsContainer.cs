using System.Collections.Generic;
using UnityEngine;

namespace CORE.CONST_SELECTOR
{
    [CreateAssetMenu(fileName = nameof(SOConstantsContainer), menuName = "SOConstantsContainer")]
    public class SOConstantsContainer: ScriptableObject
    {
        public List<ConstantsList> ConstantsList;

        public List<string> GetConsts(string groupID)
        {
            List<string> res = new List<string>();

            if(string.IsNullOrEmpty(groupID))
            {
                foreach(var list in ConstantsList)
                {
                    res.AddRange(list.Constants);
                }
            }
            else
            {
                foreach (var list in ConstantsList)
                {
                    if (list.ID == groupID)
                    {
                        res.AddRange(list.Constants);
                    }
                }
            }

            return res;
        }
    }
}