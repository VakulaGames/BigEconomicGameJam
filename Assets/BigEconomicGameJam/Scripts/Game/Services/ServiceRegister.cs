using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CORE;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class ServiceRegister: MonoBehaviour
    {
        [SerializeField] private AbstractMonoService[] _monoServices = null;

        private List<IUpdatable> _updatableServices = null;

        public async Task RegisterServises()
        {
            _updatableServices = new List<IUpdatable>();
            
            foreach (var service in _monoServices)
            {
                service.RegisterService(service.RegisterType);
                
                if (service is IInitializable initializable)
                    initializable.Init();
                
                if (service is IUpdatable updatable)
                    _updatableServices.Add(updatable);
            }
        }

        private void Update()
        {
            if (_updatableServices == null)
                return;
            
            foreach (var updatableService in _updatableServices)
            {
                updatableService.OnUpdate();
            }
        }

        private void OnDestroy()
        {
            
        }
    }
}