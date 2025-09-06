using System.Collections.Generic;
using BigEconomicGameJam.Events;
using CORE;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class CursorIconView : MonoBehaviour
    {
        [SerializeField] private SelectedIconSetting[] _settings;

        [SerializeField] private GameObject _default;

        private EventBinding<EventSelectInteractable> _eSelect = null;
        private EventBinding<EventUnSelectInteractable> _eUnSelect = null;

        private void Start()
        {
            _eSelect = new EventBinding<EventSelectInteractable>(SetIcon);
            EventBus<EventSelectInteractable>.Register(_eSelect);

            _eUnSelect = new EventBinding<EventUnSelectInteractable>(SetDefaultIcon);
            EventBus<EventUnSelectInteractable>.Register(_eUnSelect);

            _default.SetActive(true);
        }

        private void SetIcon(EventSelectInteractable obj)
        {
            _default.SetActive(false);

            foreach (var setting in _settings)
            {
                setting.GameObject.SetActive(setting.Id == obj.ID);
            }
        }

        private void SetDefaultIcon(EventUnSelectInteractable obj)
        {
            foreach (var setting in _settings)
            {
                setting.GameObject.SetActive(false);
            }
            
            _default.SetActive(true);
        }

        private void OnDestroy()
        {
            EventBus<EventSelectInteractable>.Unregister(_eSelect);
            EventBus<EventUnSelectInteractable>.Unregister(_eUnSelect);
        }
    }
}