using GTA;
using GTA.Math;
using GTA.Native;
using System.Drawing;
using GTAV_ScriptCamTool.Input;

namespace GTAV_ScriptCamTool
{
    public class PositionSelector
    {
        private Timer _renderSceneTimer;
        private float _currentLerpTime;
        private Scaleform _instructionalButtons;
        private Vector3 _previousPos;
        private Camera _mainCamera;

        private readonly float LerpTime = 0.5f;
        private readonly float RotationSpeed = 0.7f;

        public readonly GamepadHandler GamepadHandler;

        public Camera MainCamera { get { return _mainCamera; } }
  
        public PositionSelector(Vector3 position, Vector3 rotation)
        {
            this.GamepadHandler = new GamepadHandler();
            this.GamepadHandler.LeftStickChanged += LeftStickChanged;
            this.GamepadHandler.RightStickChanged += RightStickChanged;
            this.GamepadHandler.LeftStickPressed += LeftStickPressed;
            this._instructionalButtons = new Scaleform(Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, "instructional_buttons"));
            this._mainCamera = World.CreateCamera(position, rotation, 50f);
            this._mainCamera.IsActive = false;
            this._renderSceneTimer = new Timer(5000);
            this._renderSceneTimer.Start();
        }

        private void LeftStickChanged(object sender, AnalogStickChangedEventArgs e)
        {
            if (e.X > sbyte.MaxValue)
                _previousPos -= Utils.RotationToDirection(MainCamera.Rotation).RightVector(new Vector3(0, 0, 1f)) * 
                    (Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 218) * -3f);
            if (e.X < sbyte.MaxValue)
                _previousPos += Utils.RotationToDirection(MainCamera.Rotation).LeftVector(new Vector3(0, 0, 1f)) * 
                    (Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 218) * -3f);
            if (e.Y != sbyte.MaxValue)
                _previousPos += Utils.RotationToDirection(MainCamera.Rotation) * 
                    (Function.Call<float>(Hash.GET_CONTROL_NORMAL, 0, 8) * -5f);

            _currentLerpTime += 0.02f;

            if (_currentLerpTime > LerpTime)
                _currentLerpTime = LerpTime;

            float amount = _currentLerpTime / LerpTime;

            _mainCamera.Position = Vector3.Lerp(MainCamera.Position, _previousPos, amount);
        }

        private void RightStickChanged(object sender, AnalogStickChangedEventArgs e)
        {
            MainCamera.Rotation += new Vector3(Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 221) * -4f, 0, 
                Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 220) * -5f) * RotationSpeed;      
        }

        private void LeftStickPressed(object sender, ButtonPressedEventArgs e)
        {
            _previousPos += Utils.RotationToDirection(MainCamera.Rotation) * 
                (Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 230) * -5f);
        }

        public void EnterCameraView(Vector3 position)
        {
            Function.Call(Hash.DO_SCREEN_FADE_OUT, 1200);
            Script.Wait(1100);
            MainCamera.Position = position;
            MainCamera.IsActive = true;
            World.RenderingCamera = MainCamera;
            Script.Wait(100);
            Function.Call(Hash.DO_SCREEN_FADE_IN, 800);
        }

        public void ExitCameraView()
        {
            Function.Call(Hash.DO_SCREEN_FADE_OUT, 1200);
            Script.Wait(1100);
            MainCamera.IsActive = false;
            World.RenderingCamera = null;
            Script.Wait(100);
            Function.Call(Hash.CLEAR_FOCUS);
            Function.Call(Hash.DO_SCREEN_FADE_IN, 800);
        }

        public void Update()
        {
            if (MainCamera.IsActive)
            {
                if (_renderSceneTimer.Enabled && Game.GameTime > _renderSceneTimer.Waiter)
                {
                    Function.Call(Hash._0x0923DBF87DFF735E, _mainCamera.Position.X, _mainCamera.Position.Y, _mainCamera.Position.Z);
                    _renderSceneTimer.Reset();
                }

                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.VehicleCinCam, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.MultiplayerInfo, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.MeleeAttackLight, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.MeleeAttackAlternate, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.MeleeAttack2, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.Phone, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.VehicleLookBehind, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.FrontendRs, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.FrontendLs, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.FrontendX, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.ReplayShowhotkey, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.ReplayTools, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.ScriptPadDown, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.FrontendDown, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.PhoneDown, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.HUDSpecial, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.SniperZoomOutSecondary, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.CharacterWheel, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.ReplayNewmarker, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.ReplayStartStopRecording, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.ReplayStartStopRecordingSecondary, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.ReplayPause, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.MoveUpDown, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.MoveLeftRight, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.MoveLeftOnly, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.MoveRightOnly, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.MoveUpOnly, true);
                Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, (int)Control.MoveDownOnly, true);

                Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 18);

                //render local scene

                var lastPos = Vector3.Subtract(_mainCamera.Position, _previousPos);
                Function.Call(Hash._SET_FOCUS_AREA, _mainCamera.Position.X, _mainCamera.Position.Y, _mainCamera.Position.Z, lastPos.X, lastPos.Y, lastPos.Z);

                _previousPos = _mainCamera.Position;

                RenderEntityPosition();

                GamepadHandler.Update();
                RenderIntructionalButtons();

                if (_currentLerpTime > 0) _currentLerpTime -= 0.01f;
            }
        }

        private void RenderEntityPosition()
        {
            var pos = Game.Player.Character.Position + Game.Player.Character.UpVector * 1.8f;
            var dir = Vector3.WorldDown;
            var rot = new Vector3(90f, 0f, 0f);
            var scale3D = new Vector3(2.0f,2.0f, 2.0f);
            var color = Color.Yellow;
            DrawMarker(20, pos, dir, rot, scale3D, color, true, false, false);        
        }

        private void DrawMarker(int type, Vector3 position, Vector3 direction, Vector3 rotation, Vector3 scale3D, Color color, bool animate = false, bool faceCam = false, bool rotate = false)
        {
            Function.Call(Hash.DRAW_MARKER, type, position.X, position.Y, position.Z, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, scale3D.X, scale3D.Y, scale3D.Z, color.R, color.G, color.B, color.A, animate, faceCam, 2, rotate, 0, 0, 0);
        }

        private void RenderIntructionalButtons()
        {
            _instructionalButtons.CallFunction("CLEAR_ALL");
            _instructionalButtons.CallFunction("TOGGLE_MOUSE_BUTTONS", false);
            string str = Function.Call<string>(Hash._0x0499D7B09FC9B407, 2, 24, 0);
            _instructionalButtons.CallFunction("SET_DATA_SLOT", 4, str, "Select Position");
            str = Function.Call<string>(Hash._0x0499D7B09FC9B407, 3, 17, 0);
            _instructionalButtons.CallFunction("SET_DATA_SLOT", 3, str, "Increase Duration");
            str = Function.Call<string>(Hash._0x0499D7B09FC9B407, 1, 16, 0);
            _instructionalButtons.CallFunction("SET_DATA_SLOT", 2, str, "Decrease Duration");
            str = Function.Call<string>(Hash._0x0499D7B09FC9B407, 2, 25, 0);
            _instructionalButtons.CallFunction("SET_DATA_SLOT", 1, str, "Exit");
            string[] args = new string[] {
                    Function.Call<string>(Hash._0x0499D7B09FC9B407, 2, 32, 0),
                    Function.Call<string>(Hash._0x0499D7B09FC9B407, 2, 34, 0),
                    Function.Call<string>(Hash._0x0499D7B09FC9B407, 2, 33, 0),
                    Function.Call<string>(Hash._0x0499D7B09FC9B407, 2, 35, 0)
                };
            _instructionalButtons.CallFunction("SET_DATA_SLOT", 0, args[3], args[2], args[1], args[0], "Move");
            _instructionalButtons.CallFunction("SET_BACKGROUND_COLOUR", 0, 0, 0, 80);
            _instructionalButtons.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", 0);
            _instructionalButtons.Render2D();
        }
    }
}
