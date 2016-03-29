using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;

namespace GTAV_ScriptCamTool
{
    public class SplineCamera
    {
        private bool _usePlayerView;
        private Timer _renderSceneTimer;
        private Camera _mainCamera;
        private List<Tuple<Vector3, Vector3>> _nodes;
        private Timer _replayTimer;
        private Vector3 _startPos, _previousPos;
        public Camera MainCamera { get { return _mainCamera; } }

        public bool InterpToPlayer { get; set; }

        public bool UsePlayerView
        {
            get
            {
                return _usePlayerView;
            }
            set
            {
                if (value)
                {
                    _startPos = Game.Player.Character.Position;
                    Game.Player.Character.IsInvincible = true;
                    Game.Player.Character.IsVisible = false;
                }

                else
                {
                    if (_startPos != null)
                    Game.Player.Character.Position = _startPos;
                    Game.Player.Character.IsInvincible = false;
                    Game.Player.Character.IsVisible = true;
                }

                this._usePlayerView = value;
            }
        }

        public int Speed { set { Function.Call(Hash.SET_CAM_SPLINE_DURATION, _mainCamera.Handle,  100 / value * 1000 ); } }

        public List<Tuple<Vector3, Vector3>> Nodes {  get { return _nodes; } }

        public SplineCamera()
        {
            this._mainCamera = new Camera(Function.Call<int>(Hash.CREATE_CAM, "DEFAULT_SPLINE_CAMERA", 0));
            this._nodes = new List<Tuple<Vector3, Vector3>>();
            this._replayTimer = new Timer(1100);
            this._renderSceneTimer = new Timer(5000);
            this._renderSceneTimer.Start();
        }

        public void AddNode(Vector3 position, Vector3 rotation, int duration)
        {
            _nodes.Add(new Tuple<Vector3, Vector3>(position, rotation));
            Function.Call(Hash.ADD_CAM_SPLINE_NODE, _mainCamera.Handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, duration, 3, 2);
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
            if (UsePlayerView)
                UsePlayerView = false;
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

                Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, 18);
        
                _previousPos = _mainCamera.Position;

                if (_replayTimer.Enabled && Game.GameTime > _replayTimer.Waiter)
                {
                    Function.Call(Hash.SET_CAM_SPLINE_PHASE, _mainCamera.Handle, 0f);
                    _replayTimer.Enabled = false;
                }

                if (!_mainCamera.IsInterpolating)
                {
                    if (Function.Call<float>(Hash.GET_CAM_SPLINE_PHASE, _mainCamera.Handle) > 0.001f)
                    {
                        if (InterpToPlayer)
                        {
                            Function.Call(Hash.RENDER_SCRIPT_CAMS, 0, 1, 3000, 1, 1, 1);

                            Function.Call(Hash.CLEAR_FOCUS);
                            MainCamera.IsActive = false;
                        }
                    }

                    if (!_replayTimer.Enabled)
                        _replayTimer.Start();
                }

                else
                {
                    if (UsePlayerView)
                        Game.Player.Character.Position = MainCamera.Position;

                    else
                    {
                        //render local scene
                        var lastPos = Vector3.Subtract(_mainCamera.Position, _previousPos);
                        Function.Call(Hash._SET_FOCUS_AREA, _mainCamera.Position.X, _mainCamera.Position.Y, _mainCamera.Position.Z, lastPos.X, lastPos.Y, lastPos.Z);
                    }
                }
            }
        }
    }
}

