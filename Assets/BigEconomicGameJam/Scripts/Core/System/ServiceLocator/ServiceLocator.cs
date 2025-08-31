using System.Collections.Generic;
using UnityEngine;
using System;

namespace CORE
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, IService> _services = new Dictionary<Type, IService>();

        public static System.Action<Type, IService> OnRegisteredService
        {
            get;
            set;
        }

        public static System.Action<Type> OnUnregisteredService
        {
            get;
            set;
        }

        public static void RegisterService<T>(T service) where T : IService
        {
            if(_services.ContainsKey(typeof(T)))
            {
                Debug.LogError($"[{nameof(ServiceLocator)}] " +
                               $"Try to register already exists service {service.GetType().FullName}");
            }

            _services[typeof(T)] = service;

            OnRegisteredService?.Invoke(typeof(T), service);
        }

        public static void RegisterService(this IService service, Type serviceType)
        {
            if(serviceType == null)
            {
                Debug.LogError($"{service} serviceType: {serviceType}");
            }
            
            if(_services.ContainsKey(serviceType))
            {
                Debug.LogError($"[{nameof(ServiceLocator)}] " +
                               $"Try to register already exists service {service.GetType().FullName}");
            }

            _services[serviceType] = service;

            OnRegisteredService?.Invoke(serviceType, service);
        }
        
        public static void UnregisterService<T>() where T : IService
        {
            if(!_services.ContainsKey(typeof(T)))
            {
                return;
            }

            _services.Remove(typeof(T));

            OnUnregisteredService?.Invoke(typeof(T));
        }

        public static void UnregisterService(this IService service, Type serviceType)
        {
            if(!_services.ContainsKey(serviceType))
            {
                return;
            }

            _services.Remove(serviceType);

            OnUnregisteredService?.Invoke(serviceType);
        }
        
        public static T GetService<T>() where T : IService
        {
            if(_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }

            Debug.LogError($"[{nameof(ServiceLocator)}] " +
                           $"Try to get non exists service {service.GetType().FullName}");
            return default;
        }
    }
}