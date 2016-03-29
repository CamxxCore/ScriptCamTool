using System;

namespace GTAV_ScriptCamTool.Input_Handlers
{
    public class MouseTouchedEventArgs : EventArgs
    {
        private float _x, _y;

        public MouseTouchedEventArgs(float x, float y)
        {
            this._x = x;
            this._y = y;
        }

        /// <summary>
        /// The amount of force applied on the X axis, from 0 - 254. Neutral position is 127.
        /// </summary>
        public float X { get { return _x; } }

        /// <summary>
        /// The amount of force applied on the Y axis, from 0 - 254. Neutral position is 127.
        /// </summary>
        public float Y { get { return _y; } }
    }
}