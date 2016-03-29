using System;

namespace GTAV_ScriptCamTool.Input
{
    public class TriggerChangedEventArgs : EventArgs
    {
        private int _value;

        public TriggerChangedEventArgs(int value)
        {
            _value = value;
        }

        /// <summary>
        /// The amount of force applied to the trigger, from 127 - 254.
        /// </summary>
        public int Value { get { return _value; } }
    }
}
