using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace CORE
{
    public class UISystem: AbstractMonoService
    {
        [SerializeField] private Transform _uiRoot;
        [SerializeField] private UIWindow[] _windows;
        [SerializeField] private UIWindow[] _popups;
        
        public override Type RegisterType => typeof(UISystem);
        
        
        private Dictionary<string, UIWindow> _windowsDic = null;
        private Dictionary<string, UIWindow> _popupsDic = null;
        private UIWindow _currentOpenWindow = null;

        public Dictionary<string, UIWindow> Windows
        {
            get
            {
                if (_windowsDic == null)
                {
                    _windowsDic = new Dictionary<string, UIWindow>();
                    
                    foreach (var uiWindow in _windows)
                    {
                        _windowsDic[uiWindow.ID] = uiWindow;
                    }
                }
                
                return _windowsDic;
            }
        }
        public Dictionary<string, UIWindow> Popups
        {
            get
            {
                if (_popupsDic == null)
                {
                    _popupsDic = new Dictionary<string, UIWindow>();
                    
                    foreach (var popup in _popups)
                    {
                        _popupsDic[popup.ID] = popup;
                    }
                }
                
                return _popupsDic;
            }
        }
        
        public void OpenWindow(string id, Object obj = null, Action onComplete = null)
        {
            if (_currentOpenWindow != null)
            {
                if (_currentOpenWindow.ID == id)
                    return;
                
                _currentOpenWindow.Hide(() => { ShowWindow(id, obj, onComplete); });
            }
            else
            {
                ShowWindow(id, obj, onComplete);
            }
        }

        public void OpenPopup(string id, Object obj = null, Action onComplete = null)
        {
            Popups[id].Show(obj,onComplete);
        }

        public void HideAllPopups()
        {
            foreach (var popup in Popups.Values)
            {
                popup.Hide();
            }
        }
        
        private void ShowWindow(string id, Object obj = null, Action onComplete = null)
        {
            _currentOpenWindow = Windows[id];
            _currentOpenWindow.Show(obj, () => { onComplete?.Invoke(); });
        }

        public void SetEnable(bool enable)
        {
            _uiRoot.gameObject.SetActive(enable);
        }
    }
}