
using CORE;
using UnityEngine;
using Object = System.Object;

namespace BigEconomicGameJam
{
    public class BuildingState : BaseCharacterState
    {
        private LayerMask _interactionLayer;
        private float _interactionDistance;
        private BaseEquipment _buildedObject = null;
        private CharacterService _characterService = null;
        private Transform _cameraTransform = null;
        private BuildingService _buildingService = null;
        private Material _buildingMaterial = null;
        private Material _originalMaterial = null;
        
        private CharacterService CharacterService => _characterService != null? _characterService: ServiceLocator.GetService<CharacterService>();
        private Transform CameraTransform => _cameraTransform != null? _cameraTransform: ServiceLocator.GetService<CameraService>().Camera.transform;
        private BuildingService BuildingService => _buildingService != null? _buildingService: ServiceLocator.GetService<BuildingService>();
        
        // Материалы для подсветки
        private Material _greenMaterial;
        private Material _redMaterial;
        
        public BuildingState(IStateSetting stateSetting) : base(stateSetting)
        {
            var setting = stateSetting as BuildingStateSetting; 
            
            _interactionLayer = setting.InteractionLayer;
            _interactionDistance = setting.InteractionDistance;
            
            // Создаем материалы для подсветки
            _greenMaterial = new Material(Shader.Find("Standard"));
            _greenMaterial.color = new Color(0, 1, 0, 0.5f); // Зеленый полупрозрачный
            _greenMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _greenMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _greenMaterial.SetInt("_ZWrite", 0);
            _greenMaterial.DisableKeyword("_ALPHATEST_ON");
            _greenMaterial.EnableKeyword("_ALPHABLEND_ON");
            _greenMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            _greenMaterial.renderQueue = 3000;
            
            _redMaterial = new Material(Shader.Find("Standard"));
            _redMaterial.color = new Color(1, 0, 0, 0.5f); // Красный полупрозрачный
            _redMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _redMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _redMaterial.SetInt("_ZWrite", 0);
            _redMaterial.DisableKeyword("_ALPHATEST_ON");
            _redMaterial.EnableKeyword("_ALPHABLEND_ON");
            _redMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            _redMaterial.renderQueue = 3000;
        }

        public override void Enter(Object obj)
        {
            if (obj == null)
                return;
            
            if (obj is BaseEquipment equipment)
            {
                _buildedObject = equipment;
                
                // Сохраняем оригинальный материал и делаем объект прозрачным
                var renderer = _buildedObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    _originalMaterial = renderer.material;
                    renderer.material = _greenMaterial;
                }
                
                // Делаем объект кинематическим, чтобы не падал
                var rigidbody = _buildedObject.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.isKinematic = true;
                }
            }
            else
            {
                Debug.LogError($"object: {obj} is not BaseEquipment");
            }
        }

        public override void Update()
        {
            if (CharacterService.IsPaused) return;
            DetectInteractableObjects();
        }

        public override void Exit()
        {
            // Восстанавливаем оригинальный материал при выходе из состояния
            if (_buildedObject != null && _originalMaterial != null)
            {
                var renderer = _buildedObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = _originalMaterial;
                }
                
                // Включаем физику обратно
                var rigidbody = _buildedObject.GetComponent<Rigidbody>();
                if (rigidbody != null)
                {
                    rigidbody.isKinematic = false;
                }
            }
        }

        private void DetectInteractableObjects()
        {
            if (_buildedObject == null) return;
            
            Ray ray = new Ray(CameraTransform.position, CameraTransform.forward);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, _interactionDistance, _interactionLayer))
            {
                // Получаем позицию для строительства (с нулевой Y координатой)
                Vector3 buildPosition = new Vector3(hit.point.x, 0, hit.point.z);
                
                // Перемещаем объект в точку попадания луча
                _buildedObject.transform.position = buildPosition;
                
                // Проверяем, можно ли построить в этой позиции
                bool canBuild = BuildingService.CanBuildAtPosition(buildPosition);
                
                // Меняем материал в зависимости от возможности строительства
                var renderer = _buildedObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = canBuild ? _greenMaterial : _redMaterial;
                }
            }
            else
            {
                // Если луч не попал никуда, скрываем объект или показываем красным
                var renderer = _buildedObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = _redMaterial;
                }
            }
        }

        public override void HandleClick(MouseClickData clickData)
        {
            if (_buildedObject == null) return;
            
            if (clickData.LeftButtonDown)
            {
                // Проверяем, можно ли построить в текущей позиции
                Vector3 currentPosition = new Vector3(_buildedObject.transform.position.x, 0, _buildedObject.transform.position.z);
                bool canBuild = BuildingService.CanBuildAtPosition(currentPosition);
                
                if (canBuild)
                {
                    // Сообщаем сервису о постройке
                    BuildingService.RegisterBuilding(_buildedObject, currentPosition);
                    
                    // Восстанавливаем оригинальный материал
                    var renderer = _buildedObject.GetComponent<Renderer>();
                    if (renderer != null && _originalMaterial != null)
                    {
                        renderer.material = _originalMaterial;
                    }
                    
                    // Включаем физику обратно
                    var rigidbody = _buildedObject.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        rigidbody.isKinematic = false;
                    }
                    
                    // Выходим из состояния строительства
                    CharacterService.SetState(typeof(EmptyHandsState));
                }
                else
                {
                    Debug.Log("Cannot build here!");
                }
            }
        }
    }
}