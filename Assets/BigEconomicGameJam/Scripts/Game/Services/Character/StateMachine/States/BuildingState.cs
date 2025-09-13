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
        private UISystem _uiSystem = null;
        private Transform _cameraTransform = null;
        private BuildingService _buildingService = null;
        private Material _buildingMaterial = null;
        private Material[] _originalMaterials = null;
        private Vector3 _originPos;
        private Quaternion _originRotation;
        
        private float _rotationSpeed = 90f;
        private float _targetRotation = 0f;
        private float _currentRotation = 0f;
        private bool _isRotating = false;
        private bool _rotateClockwise = false;
        private bool _rotateCounterClockwise = false;
        
        private CharacterService CharacterService => _characterService != null? _characterService: ServiceLocator.GetService<CharacterService>();
        private Transform CameraTransform => _cameraTransform != null? _cameraTransform: ServiceLocator.GetService<CameraService>().Camera.transform;
        private BuildingService BuildingService => _buildingService != null? _buildingService: ServiceLocator.GetService<BuildingService>();
        private UISystem UISystem => _uiSystem != null? _uiSystem: ServiceLocator.GetService<UISystem>();
        
        private Material _greenMaterial;
        private Material _redMaterial;
        private bool _isAllowed;
        private BuildingStateSetting _setting;
        
        public BuildingState(IStateSetting stateSetting) : base(stateSetting)
        {
            _setting = stateSetting as BuildingStateSetting; 
            
            _interactionLayer = _setting.InteractionLayer;
            _interactionDistance = _setting.InteractionDistance;
            
            _greenMaterial = new Material(Shader.Find("Standard"));
            _greenMaterial.color = new Color(0, 1, 0, 0.5f);
            _greenMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _greenMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _greenMaterial.SetInt("_ZWrite", 0);
            _greenMaterial.DisableKeyword("_ALPHATEST_ON");
            _greenMaterial.EnableKeyword("_ALPHABLEND_ON");
            _greenMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            _greenMaterial.renderQueue = 3000;
            
            _redMaterial = new Material(Shader.Find("Standard"));
            _redMaterial.color = new Color(1, 0, 0, 0.5f);
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

                if (_buildedObject.IsPurchased)
                {
                    _originPos = _buildedObject.transform.position;
                    _originRotation = _buildedObject.transform.rotation;
                }
                
                _buildedObject.StartBuilding();
                
                _originalMaterials = new Material[_buildedObject.Renderers.Length];
                for (int i = 0; i < _originalMaterials.Length; i++)
                {
                    _originalMaterials[i] = _buildedObject.Renderers[i].material;
                    _buildedObject.Renderers[i].material = _greenMaterial;
                }

                _currentRotation = _buildedObject.transform.eulerAngles.y;
                _targetRotation = SnapToNearest15(_currentRotation);
                _buildedObject.transform.rotation = Quaternion.Euler(0, _targetRotation, 0);
                _currentRotation = _targetRotation;
                
                SetMaterial(true);
                
                UISystem.OpenWindow(_setting.WindowID);
            }
            else
            {
                Debug.LogError($"object: {obj} is not BaseEquipment");
            }
        }

        public override void Update()
        {
            if (CharacterService.IsPaused) return;
            
            HandleRotationInput();
            
            ApplyRotation();
            
            DetectInteractableObjects();
        }

        private void HandleRotationInput()
        {
            bool qPressed = Input.GetKey(KeyCode.Q);
            bool ePressed = Input.GetKey(KeyCode.E);
            
            if (qPressed && ePressed)
            {
                _isRotating = false;
                _rotateClockwise = false;
                _rotateCounterClockwise = false;
                SnapToNearestRotation();
                return;
            }
            
            if (ePressed && !qPressed)
            {
                _isRotating = true;
                _rotateClockwise = true;
                _rotateCounterClockwise = false;
                _targetRotation = _currentRotation + _rotationSpeed * Time.deltaTime;
            }
            else if (qPressed && !ePressed)
            {
                _isRotating = true;
                _rotateClockwise = false;
                _rotateCounterClockwise = true;
                _targetRotation = _currentRotation - _rotationSpeed * Time.deltaTime;
            }
            else
            {
                if (_isRotating)
                {
                    SnapToNearestRotation();
                }
                _isRotating = false;
            }
        }

        private void ApplyRotation()
        {
            if (_buildedObject == null) return;
            
            if (_isRotating)
            {
                _currentRotation = _targetRotation;
                _buildedObject.transform.rotation = Quaternion.Euler(0, _currentRotation, 0);
            }
        }

        private void SnapToNearestRotation()
        {
            if (_buildedObject == null) return;
            
            _targetRotation = SnapToNearest15(_currentRotation);
            _currentRotation = _targetRotation;
            _buildedObject.transform.rotation = Quaternion.Euler(0, _currentRotation, 0);
        }

        private float SnapToNearest15(float angle)
        {
            angle = angle % 360;
            if (angle < 0) angle += 360;
            
            float snapped = Mathf.Round(angle / 15f) * 15f;
            
            return snapped == 360f ? 0f : snapped;
        }

        private void SetMaterial(bool isAllowed)
        {
            if (_isAllowed == isAllowed)
                return;

            _isAllowed = isAllowed;

            if (isAllowed)
            {
                for (int i = 0; i < _originalMaterials.Length; i++)
                {
                    _buildedObject.Renderers[i].material = _greenMaterial;
                }
            }
            else
            {
                for (int i = 0; i < _originalMaterials.Length; i++)
                {
                    _buildedObject.Renderers[i].material = _redMaterial;
                }
            }
        }

        private void SetOriginMaterial()
        {
            if (_buildedObject != null && _originalMaterials != null)
            {
                for (int i = 0; i < _originalMaterials.Length; i++)
                {
                    _buildedObject.Renderers[i].material = _originalMaterials[i];
                }
            }
        }

        public override void Exit()
        {
            SetOriginMaterial();
            _isAllowed = false;
        }

        private void DetectInteractableObjects()
        {
            if (_buildedObject == null) return;
            
            Ray ray = new Ray(CameraTransform.position, CameraTransform.forward);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, _interactionDistance, _interactionLayer))
            {
                Vector3 buildPosition = new Vector3(hit.point.x, 0, hit.point.z);
                _buildedObject.transform.position = buildPosition;
                SetMaterial(_buildedObject.CanBuild());
            }
            else
            {
                SetMaterial(false);
            }
        }

        public override void HandleClick(MouseClickData clickData)
        {
            if (_buildedObject == null) return;
            
            if (clickData.LeftButtonDown)
            {
                if (_buildedObject.CanBuild())
                {
                    SnapToNearestRotation();
                    _isRotating = false;
                    _rotateClockwise = false;
                    _rotateCounterClockwise = false;
                    
                    SetOriginMaterial();
                    _buildedObject.NotifyBuilt();
                    CharacterService.ResumeGame();
                }
                else
                {
                    Debug.Log("Cannot build here!");
                }
            }
            else if (clickData.RightButtonDown)
            {
                _isRotating = false;
                _rotateClockwise = false;
                _rotateCounterClockwise = false;
                
                if (_buildedObject.IsPurchased)
                {
                    _buildedObject.transform.position = _originPos;
                    _buildedObject.transform.rotation = _originRotation;
                    
                    SetOriginMaterial();
                    _buildedObject.NotifyBuilt();
                }
                else
                {
                    _buildedObject.Destroy();
                }
                
                CharacterService.ResumeGame();
            }
        }
    }
}