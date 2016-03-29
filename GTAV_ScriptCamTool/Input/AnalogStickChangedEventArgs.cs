using System;

namespace GTAV_ScriptCamTool.Input
{
    public class AnalogStickChangedEventArgs : EventArgs
    {
        private int _x, _y;

        public AnalogStickChangedEventArgs(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        /// <summary>
        /// The amount of force applied on the X axis, from 0 - 254. Neutral position is 127.
        /// </summary>
        public int X { get { return _x; } }

        /// <summary>
        /// The amount of force applied on the Y axis, from 0 - 254. Neutral position is 127.
        /// </summary>
        public int Y { get { return _y; } }
    }
}
