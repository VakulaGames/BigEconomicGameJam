using System.Collections.Generic;

namespace CORE
{
    public static class EventBus<T> where T : IEvent
    {
        public const int CAPACITY_RATE = 3;

        static readonly List<IEventBinding<T>> bindings = new List<IEventBinding<T>>();

        public static void Register(EventBinding<T> binding) => Add(binding);
        public static void Unregister(EventBinding<T> binding) => Remove(binding);

        public static bool HasBinding() => bindings.Count > 0;

        public static bool Contains(EventBinding<T> binding)
        {
            return bindings.Contains(binding);
        }

        public static void Raise(T @event)
        {
            for (int i = 0; i < bindings.Count; i++)
            {
                var binding = bindings[i];

                if (binding == null)
                {
#if DEBUG_LOG
                    Debug.LogError($"try to rise non exist binding");
#endif
                    continue;
                }

                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }

        private static void Add(EventBinding<T> binding)
        {
            if (bindings.Capacity - bindings.Count <= 1)
                bindings.Capacity = bindings.Count + CAPACITY_RATE;

            bindings.Add(binding);
        }

        private static void Remove(EventBinding<T> binding)
        {
            bindings.Remove(binding);

            if (bindings.Capacity - bindings.Count >= CAPACITY_RATE)
                bindings.Capacity = bindings.Count;
        }

        static void Clear()
        {
#if DEBUG_LOG
            Debug.Log($"Clearing {typeof(T).Name} bindings");
#endif
            bindings.Clear();
        }
    }
}
