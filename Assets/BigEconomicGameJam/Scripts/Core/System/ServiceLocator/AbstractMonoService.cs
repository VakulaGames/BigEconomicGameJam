using System;
using UnityEngine;

namespace CORE
{
    public abstract class AbstractMonoService: MonoBehaviour, IService
    {
        public abstract Type RegisterType { get; }
    }
}