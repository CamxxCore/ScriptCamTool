using GTA.Native;

namespace GTAV_ScriptCamTool.Input
{
    public delegate void ButtonPressedEventHandler(object sender, ButtonPressedEventArgs e);

    public delegate void TriggerChangedEventHandler(object sender, TriggerChangedEventArgs e);

    public delegate void AnalogStickChangedEventHandler(object sender, AnalogStickChangedEventArgs e);

    public class GamepadHandler
    {
        #region Declare Events
        /// <summary>
        /// Called when the user presses the A button.
        /// </summary>
        public event ButtonPressedEventHandler AButtonPressed;

        /// <summary>
        /// Called when the user presses the B button.
        /// </summary>
        public event ButtonPressedEventHandler BButtonPressed;

        /// <summary>
        /// Called when the user presses the X button.
        /// </summary>
        public event ButtonPressedEventHandler XButtonPressed;

        /// <summary>
        /// Called when the user presses the Y button.
        /// </summary>
        public event ButtonPressedEventHandler YButtonPressed;

        /// <summary>
        /// Called when the user presses the right trigger.
        /// </summary>
        public event TriggerChangedEventHandler RightTriggerChanged;

        /// <summary>
        /// Called when the user presses the left trigger.
        /// </summary>
        public event TriggerChangedEventHandler LeftTriggerChanged;

        /// <summary>
        /// Called when the user presses the right bumper.
        /// </summary>
        public event ButtonPressedEventHandler RightBumperPressed;

        /// <summary>
        /// Called when the user presses the left bumper.
        /// </summary>
        public event ButtonPressedEventHandler LeftBumperPressed;

        /// <summary>
        /// Called when the user presses left on the Dpad.
        /// </summary>
        public event ButtonPressedEventHandler DpadLeftPressed;

        /// <summary>
        /// Called when the user presses down on the DPad.
        /// </summary>
        public event ButtonPressedEventHandler DpadDownPressed;

        /// <summary>
        /// Called when the user presses right on the Dpad.
        /// </summary>
        public event ButtonPressedEventHandler DpadRightPressed;

        /// <summary>
        /// Called when the user presses up on the Dpad.
        /// </summary>
        public event ButtonPressedEventHandler DpadUpPressed;

        /// <summary>
        /// Called when the user uses the analog stick.
        /// </summary>
        public event AnalogStickChangedEventHandler LeftStickChanged;

        /// <summary>
        /// Called when the user uses the analog stick.
        /// </summary>
        public event AnalogStickChangedEventHandler RightStickChanged;

        /// <summary>
        /// Called when the user uses the analog stick.
        /// </summary>
        public event ButtonPressedEventHandler LeftStickPressed;

        /// <summary>
        /// Called when the user uses the analog stick.
        /// </summary>
        public event ButtonPressedEventHandler RightStickPressed;


        #endregion

        public GamepadHandler()
        {
        }

        public void Update()
        {
            if (GetControlValue(220) != 127 || GetControlValue(221) != 127)
                OnRightStickChanged(new AnalogStickChangedEventArgs(GetControlValue(220), GetControlValue(221)));
            if (GetControlValue(218) != 127 || GetControlValue(219) != 127)
                OnLeftStickChanged(new AnalogStickChangedEventArgs(GetControlValue(218), GetControlValue(219)));

            if (GetControlInput(230))
                OnLeftStickPressed(new ButtonPressedEventArgs(GetControlValue(230)));
            if (GetControlInput(231))
                OnRightStickPressed(new ButtonPressedEventArgs(GetControlValue(231)));

            if (GetControlValue(229) > 127)
                OnRightTriggerChanged(new TriggerChangedEventArgs(GetControlValue(229)));
            if (GetControlValue(228) > 127)
                OnLeftTriggerChanged(new TriggerChangedEventArgs(GetControlValue(228)));

            if (GetControlInput(222))
                OnYPressed(new ButtonPressedEventArgs(GetControlValue(222)));
            if (GetControlInput(223))
                OnAPressed(new ButtonPressedEventArgs(GetControlValue(223)));
            if (GetControlInput(224))
                OnXPressed(new ButtonPressedEventArgs(GetControlValue(224)));
            if (GetControlInput(225))
                OnBPressed(new ButtonPressedEventArgs(GetControlValue(225)));
            if (GetControlInput(226))
                OnLBPressed(new ButtonPressedEventArgs(GetControlValue(226)));
            if (GetControlInput(227))
                OnRBPressed(new ButtonPressedEventArgs(GetControlValue(227)));
        }

        #region Event Handlers

        protected virtual void OnAPressed(ButtonPressedEventArgs e)
        {
            AButtonPressed?.Invoke(this, e);
        }

        protected virtual void OnBPressed(ButtonPressedEventArgs e)
        {
            BButtonPressed?.Invoke(this, e);
        }

        protected virtual void OnXPressed(ButtonPressedEventArgs e)
        {
            XButtonPressed?.Invoke(this, e);
        }

        protected virtual void OnYPressed(ButtonPressedEventArgs e)
        {
            YButtonPressed?.Invoke(this, e);
        }

        protected virtual void OnLBPressed(ButtonPressedEventArgs e)
        {
            LeftBumperPressed?.Invoke(this, e);
        }

        protected virtual void OnRBPressed(ButtonPressedEventArgs e)
        {
            RightBumperPressed?.Invoke(this, e);
        }

        protected virtual void OnLeftTriggerChanged(TriggerChangedEventArgs e)
        {
            LeftTriggerChanged?.Invoke(this, e);
        }

        protected virtual void OnRightTriggerChanged(TriggerChangedEventArgs e)
        {
            RightTriggerChanged?.Invoke(this, e);
        }

        protected virtual void OnLeftStickChanged(AnalogStickChangedEventArgs e)
        {
            LeftStickChanged?.Invoke(this, e);
        }

        protected virtual void OnRightStickChanged(AnalogStickChangedEventArgs e)
        {
            RightStickChanged?.Invoke(this, e);
        }

        protected virtual void OnLeftStickPressed(ButtonPressedEventArgs e)
        {
            LeftStickPressed?.Invoke(this, e);
        }

        protected virtual void OnRightStickPressed(ButtonPressedEventArgs e)
        {
            RightStickPressed?.Invoke(this, e);
        }

        #endregion

        private bool GetControlInput(int control)
        {
            return Function.Call<bool>(Hash.IS_CONTROL_JUST_PRESSED, 0, control);
        }

        private int GetControlValue(int control)
        {
            return Function.Call<int>(Hash.GET_CONTROL_VALUE, 0, control);
        }
    }
}
