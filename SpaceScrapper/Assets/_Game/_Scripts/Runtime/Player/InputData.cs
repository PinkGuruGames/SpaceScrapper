using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceScrapper
{
    //NOTE: this will need to be converted over to the InputSystem later on.
    public class InputData : MonoBehaviour
    {
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

        [Header("References")]
        [SerializeField] private Camera mainCamera;

        //[Header("Keybinds")]    // These will later be set through the Settings => Keybinds menu
        //[SerializeField] private KeyCode forwardButton = KeyCode.W;
        //[SerializeField] private KeyCode backwardButton = KeyCode.S;
        //[SerializeField] private KeyCode leftwardButton = KeyCode.A;
        //[SerializeField] private KeyCode rightwardButton = KeyCode.D;
        //[SerializeField] private KeyCode relativeModeButton = KeyCode.R;
        //[SerializeField] private KeyCode flightAssistToggleButton = KeyCode.F;
        //[SerializeField] private KeyCode flightAssistHoldButton = KeyCode.LeftShift;
        //[SerializeField] private KeyCode cruiseModeButton = KeyCode.Tab;

        private Vector2 _movement;
        private Vector2 _worldMousePosition;
        private Vector2 _screenMousePosition;
        private bool _relativeMode = false;
        private bool _flightAssist = true;
        private bool _flightAssistOverride = false;
        private bool _cruiseMode = false;

        private static readonly Plane xyPlane = new Plane(Vector3.forward, new Vector3(0, 0, 0));

        //Messages sent from PlayerInput:
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


        //private void Update()
        //{
        //    GetMovementInput();
        //    GetMousePosition(Input.mousePosition);
        //    GetModeInputs();
        //}

        //private void GetMovementInput()
        //{
        //    if (Input.GetKey(forwardButton) == Input.GetKey(backwardButton))    // If both keys are pressed at the same time or if both are released, set input to zero
        //    {
        //        _movement.y = 0;
        //    }
        //    else if (Input.GetKey(forwardButton))
        //    {
        //        _movement.y = 1;
        //    }
        //    else if (Input.GetKey(backwardButton))
        //    {
        //        _movement.y = -1;
        //    }
        //    if (Input.GetKey(leftwardButton) == Input.GetKey(rightwardButton))
        //    {
        //        _movement.x = 0;
        //    }
        //    else if (Input.GetKey(leftwardButton))
        //    {
        //        _movement.x = -1;
        //    }
        //    else if (Input.GetKey(rightwardButton))
        //    {
        //        _movement.x = 1;
        //    }
        //    _movement.Normalize();

        //    // This makes sure that the input works properly even if not all keys are being sent due to keyboard limitations
        //    //Debug.Log($"Keys Pressed - W = {Input.GetKey(forwardButton)} A = {Input.GetKey(leftwardButton)} S = {Input.GetKey(backwardButton)} D = {Input.GetKey(rightwardButton)}\nMovement X = {Movement.x} - Movement Y = {Movement.y}");
        //}

        //private void GetMousePosition(Vector3 screenPosition)
        //{
        //    //simplified version
        //    //_worldMousePosition = mainCamera.WorldToScreenPoint(screenPosition);
        //    Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        //    Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        //    xy.Raycast(ray, out var distance);
        //    _worldMousePosition = ray.GetPoint(distance);
        //}

        //private void GetModeInputs()
        //{
        //    if (Input.GetKeyDown(relativeModeButton))
        //    {
        //        _relativeMode ^= true;
        //    }
        //    if (Input.GetKeyDown(flightAssistToggleButton) || Input.GetKeyDown(flightAssistHoldButton))
        //    {
        //        _flightAssist ^= true;
        //        Debug.Log("Flight Assist toggled!");
        //    }
        //    if (Input.GetKeyUp(flightAssistHoldButton))
        //    {
        //        _flightAssist ^= true;
        //    }
        //    if (Input.GetKeyDown(cruiseModeButton))
        //    {
        //        _cruiseMode ^= true;
        //    }
        //}       
    }
}