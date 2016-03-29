#define DEBUG
using System.Windows.Forms;
using GTA.Native;
using GTA;



namespace GTAV_GamepadHandler
{
    public delegate void KeyPressedEventHandler();

    public sealed class KeyboardHandler : Script
    {
        #region Events
        /// <summary>
        /// Called when the user presses the W key.
        /// </summary>
        public event ButtonPressedEventHandler WKeyPressed;

        /// <summary>
        /// Called when the user presses the A key.
        /// </summary>
        public event ButtonPressedEventHandler AKeyPressed;

        /// <summary>
        /// Called when the user presses the S key.
        /// </summary>
        public event ButtonPressedEventHandler SKeyPressed;

        /// <summary>
        /// Called when the user presses the D key.
        /// </summary>
        public event ButtonPressedEventHandler DKeyPressed;

        /// <summary>
        /// Called when the user presses the T key.
        /// </summary>
        /// 
#if DEBUG
        public event ButtonPressedEventHandler TKeyPressed;
#endif

        #endregion

        public KeyboardHandler()
        {
            this.KeyDown += KeyIsDown;
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (WKeyPressed != null && e.KeyCode == Keys.W)
                WKeyPressed();
            if (AKeyPressed != null && e.KeyCode == Keys.A)
                AKeyPressed();
            if (SKeyPressed != null && e.KeyCode == Keys.S)
                SKeyPressed();
            if (DKeyPressed != null && e.KeyCode == Keys.D)
                DKeyPressed();
            if (TKeyPressed != null && e.KeyCode == Keys.T)
                TKeyPressed();
        }
    }
}
