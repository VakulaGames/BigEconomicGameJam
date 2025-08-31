using System.Collections.Generic;
using UnityEngine;

namespace CORE.CONST_SELECTOR
{
    public class ConstSelectorWindow: StringSelectorWindow
    {
        public static void Open(System.Action<string> onClose, string initValue, List<string> values)
        {
            Window = GetWindow<ConstSelectorWindow>();
            Window.minSize = new Vector2(250, 600);
            Window.titleContent.text = "Search Value";
            Window.OnCloseAction = onClose;
            Window.CurValue = initValue;
            Window.Load(values);
        }
    }
}