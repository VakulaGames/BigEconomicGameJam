using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace CORE.CONST_SELECTOR
{
    public class StringSelectorWindow: SearchableEditorWindow
    {
        protected static StringSelectorWindow Window = null;
        
        public System.Action<string> OnCloseAction = null;
        
        public string CurValue;
        
        private static string filter = "";
        
        private List<string> valuesStr = new List<string>();
        
        private StyleSheet _styleSheet;

        private VisualElement _searchPanel;
        private VisualElement _scrollPanel;

        private TextField _textFieldSearch;
        
        
        private List<string> _items = new List<string>();
        private string _oldDataFilter;
        
        private void OnGUI()
        {
            filter = _textFieldSearch.text;

            Search();

             if (filter != _oldDataFilter)
             {
                _scrollPanel.Clear();
                
                 ReloadScroll();
                
                 _oldDataFilter = filter;
            }
        }

        public override void OnEnable()
        {
            DrawInterface();
        }

        private void DrawInterface()
        {
            rootVisualElement.Add( SearchPanel());
            rootVisualElement.Add(ScrollPanel());
        }

        private VisualElement SearchPanel()
        {
            _searchPanel = new VisualElement();
            
            _searchPanel.AddToClassList("search-panel-container");
            
            _textFieldSearch = new TextField("Search:");
            
            _textFieldSearch.AddToClassList("search-panel-textField");
            
            _searchPanel.Add(_textFieldSearch);

            return _searchPanel;
        }

        private VisualElement ScrollPanel()
        {
            _scrollPanel = new ScrollView();
            
            _scrollPanel.AddToClassList("scroll-panel-container");

            foreach (var item in _items)
                _scrollPanel.Add(ScrollElement(item));

            return _scrollPanel;
        }

        private void ReloadScroll()
        {
            foreach (var item in _items)
                _scrollPanel.Add(ScrollElement(item));
        }

        private VisualElement ScrollElement(string element)
        {
            Button button = new Button(() =>
            {
                CurValue = element;
                Close();
            });

            button.text = element;
            
            button.AddToClassList("scroll-element");
            
            return button;
        }

        private void Search()
        {
            List<string> items = new List<string>();
            
            foreach(var item in valuesStr)
            {
                if(!item.ToLower().Contains(filter.ToLower()))
                {
                    continue;
                }

                items.Add(item);
            }

            _items = items;
        }
        
        public void Load(List<string> values)
        {
            foreach(string str in values)
            {
                valuesStr.Add(str);
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();

            OnCloseAction?.Invoke(CurValue);
            OnCloseAction = null;
        }
    }
}