using System;
using CORE;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class CameraService: AbstractMonoService
    {
        public override Type RegisterType => typeof(CameraService);

        private Camera _camera;

        public Camera Camera => _camera != null ? _camera : _camera = Camera.main;
    }
}