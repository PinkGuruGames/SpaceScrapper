using UnityEngine;

namespace SpaceScrapper
{
    public class InputHandler : MonoBehaviour
    {
        public bool Forward { get; private set; }
        public bool Backward { get; private set; }
        public bool Leftward { get; private set; }
        public bool Rightward { get; private set; }
        public bool RelativeMode { get; private set; }
        public bool FlightAssist { get; private set; }
        public bool CruiseMode { get; private set; }

        [Header("Keybinds")]    // These will later be set through the Settings => Keybinds menu
        [SerializeField] private KeyCode forwardButton = KeyCode.W;
        [SerializeField] private KeyCode backwardButton = KeyCode.S;
        [SerializeField] private KeyCode leftwardButton = KeyCode.A;
        [SerializeField] private KeyCode rightwardButton = KeyCode.D;
        [SerializeField] private KeyCode relativeModeButton = KeyCode.R;
        [SerializeField] private KeyCode flightAssistToggleButton = KeyCode.F;
        [SerializeField] private KeyCode flightAssistHoldButton = KeyCode.LeftShift;
        [SerializeField] private KeyCode cruiseModeButton = KeyCode.Tab;

        private void Update()
        {
            GetMovementInput();
            GetModeInputs();
        }

        private void GetMovementInput()
        {
            Forward = Input.GetKey(forwardButton);
            Backward = Input.GetKey(backwardButton);
            Leftward = Input.GetKey(leftwardButton);
            Rightward = Input.GetKey(rightwardButton);
        }

        private void GetModeInputs()
        {
            if (Input.GetKeyDown(relativeModeButton))
            {
                RelativeMode ^= true;
            }
            if (Input.GetKeyDown(flightAssistToggleButton) || Input.GetKeyDown(flightAssistHoldButton))
            {
                FlightAssist ^= true;
            }
            if (Input.GetKeyUp(flightAssistHoldButton))
            {
                FlightAssist ^= true;
            }
            if (Input.GetKeyDown(cruiseModeButton))
            {
                CruiseMode ^= true;
            }
        }
    }
}