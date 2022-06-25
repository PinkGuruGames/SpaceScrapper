using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceScrapper
{
    /// <summary>
    /// Handles most of the input (manages PlayerInput messages and active action maps)
    /// </summary>
    public class InputData : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] 
        private Camera mainCamera;
        [Header("Pause Menu")] //this is temporary for the demo.
        [SerializeField]
        private PauseMenu pauseMenu;
        [Header("Input Settings")]
        [SerializeField]
        private PlayerInput playerInput;
        [SerializeField]
        private InputActionAsset inputAsset;
        [SerializeField, HideInInspector]
        private string gameplayActionMapName;
        [SerializeField, HideInInspector]
        private string pauseMenuActionMapName;
        [SerializeField, HideInInspector]
        private string dialogueScreenActionMapName;


        //runtime buffers for input values.
        private Vector2 _movement;
        private Vector2 _screenMousePosition;
        private bool _relativeMode = false;
        private bool _flightAssist = true;
        private bool _flightAssistOverride = false;
        private bool _cruiseMode = false;
        
        //heleper for screenposition of mouse
        private static readonly Plane xyPlane = new Plane(Vector3.forward, new Vector3(0, 0, 0));

        //public properties
        public Vector2 Movement => _movement;
        public Vector3 WorldMousePosition
        {
            get
            {
                Ray ray = mainCamera.ScreenPointToRay(_screenMousePosition);
                xyPlane.Raycast(ray, out var distance);
                return ray.GetPoint(distance);
            }
        }
        public bool RelativeMode => _relativeMode;
        public bool FlightAssist => _flightAssist | _flightAssistOverride;
        public bool CruiseMode => _cruiseMode;

        //Methods that activate the different action maps for the different modes of player interaction
        public void ActivateGameplayControls()
            => playerInput.SwitchCurrentActionMap(gameplayActionMapName);
        public void ActivateDialogueControls()
            => playerInput.SwitchCurrentActionMap(dialogueScreenActionMapName);
        public void ActivatePauseMenuControls()
            => playerInput.SwitchCurrentActionMap(pauseMenuActionMapName);
        
        //Messages sent from PlayerInput:
#pragma warning disable IDE0051 // Remove unused private members
        private void OnFlightAssistToggle()
        {
            _flightAssist ^= true;
        }

        private void OnFlightAssistHold()
        {
            _flightAssistOverride ^= true;
        }

        private void OnFlightModeSwitch()
        {
            _cruiseMode ^= true;
        }

        private void OnHorizontalMovementAxis(InputValue value)
        {
            _movement.x = value.Get<float>();
        }

        private void OnVerticalMovementAxis(InputValue value)
        {
            _movement.y = value.Get<float>();
        }

        private void OnMousePosition(InputValue value)
        {
            _screenMousePosition = value.Get<Vector2>();
        }

        private void OnMenu()
        {
            if(pauseMenu.gameObject.activeSelf)
            {
                pauseMenu.Resume();
                ActivateGameplayControls();
            }
            else
            {
                pauseMenu.Pause();
                ActivatePauseMenuControls();
            }
        }
#pragma warning restore IDE0051 // Remove unused private members
    }
}