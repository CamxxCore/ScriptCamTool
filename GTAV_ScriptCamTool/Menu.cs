using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;
using NativeUI;
using System.Linq;

namespace GTAV_ScriptCamTool
{
    public class Menu : Script
    {
        UIContainer msgBox = new UIContainer();
        string msgText;
        float msgScale;
        Timer msgTimer = new Timer(1000);
        bool fadingMsg = false;

        int nodeDuration = 5000;
        private SplineCamera splineCam;
        private MenuPool activePool;
        private UIMenu mainMenu, cameraOptionsMenu;
        private PositionSelector selector;


        public Menu()
        {
            Tick += OnTick;
            KeyUp += KeyIsUp;
            cameraOptionsMenu = new UIMenu("Camera Options", string.Empty);
            var menuItem = new UIMenuListItem("Speed", Enumerable.Range(0, 100).Cast<dynamic>().ToList<dynamic>(), 1);
            cameraOptionsMenu.AddItem(menuItem);
            menuItem = new UIMenuListItem("Field Of View", Enumerable.Range(0, 100).Cast<dynamic>().ToList<dynamic>(), 50);
            cameraOptionsMenu.AddItem(menuItem);
            var menuItemB = new UIMenuCheckboxItem("Use Player View", false, "(May create smoother terrain rendering when enabled but restricts player movement and is prone to bugs.)");
            cameraOptionsMenu.AddItem(menuItemB);
            menuItemB = new UIMenuCheckboxItem("End At Player", false, "(Camera tracks to player view at end of the scene)");
            cameraOptionsMenu.AddItem(menuItemB);
            cameraOptionsMenu.OnListChange += OnListChanged;
            cameraOptionsMenu.OnCheckboxChange += OnCheckboxChanged;
            mainMenu = new UIMenu("Script Cam Tool", string.Empty);
            var menuItemA = new UIMenuItem("~g~Start Rendering");
            menuItemA.Activated += (sender, item) => StartInterpolatingCam();
            mainMenu.AddItem(menuItemA);
            menuItemA = new UIMenuItem("~r~Stop Rendering");
            menuItemA.Activated += (sender, item) => StopInterpolatingCam();
            mainMenu.AddItem(menuItemA);
            menuItemA = new UIMenuItem("Setup Nodes");
            menuItemA.Activated += (sender, item) => EnterPointSelector();
            mainMenu.AddItem(menuItemA);
            menuItemA = new UIMenuItem("Camera Options");
            mainMenu.BindMenuToItem(cameraOptionsMenu, menuItemA);
            mainMenu.AddItem(menuItemA);
            menuItemA = new UIMenuItem("Reset All Cams");
            menuItemA.Activated += (sender, item) => ResetAllCameras();
            mainMenu.AddItem(menuItemA);
            menuItemA = new UIMenuItem("Close");
            menuItemA.Activated += (sender, item) => activePool.CloseAllMenus();
            mainMenu.AddItem(menuItemA);
            activePool = new MenuPool();
            activePool.Add(mainMenu);
            activePool.Add(cameraOptionsMenu);
            ResetAllCameras();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (fadingMsg)
            {
                if (msgBox.Color.A > 10 || msgBox.Items[0].Color.A > 10)
                {
                    msgBox.Draw();
                    msgBox.Items[0].Draw();
                    msgBox.Color = Color.FromArgb(msgBox.Color.A - 5, msgBox.Color.R, msgBox.Color.G, msgBox.Color.B);
                    msgBox.Items[0].Color = Color.FromArgb(msgBox.Color.A - 5, msgBox.Color.R, msgBox.Color.G, msgBox.Color.B);
                }
                else
                {
                    msgBox = null;
                    fadingMsg = false;
                }
            }

            else if (msgTimer.Enabled)
            {
                if (Game.GameTime > msgTimer.Waiter)
                {
                    msgTimer.Enabled = false;
                    fadingMsg = true;
                }

                else
                {
                    msgBox = new UIContainer(new Point(1100, 0), new Size(150, 34), Color.FromArgb(140, 0, 0, 0));
                    msgBox.Items.Add(new UIText(msgText, new Point(1121, 2), msgScale, Color.White));
                    msgBox.Draw();
                    msgBox.Items[0].Draw();
                }
            }

            splineCam.Update();
            selector.Update();

            if (!activePool.IsAnyMenuOpen())
            {
                if (selector.MainCamera.IsActive)
                {
                    if (Game.IsControlJustPressed(0, GTA.Control.SelectNextWeapon))
                    {
                        nodeDuration -= 100;
                        msgText = string.Format("Duration: {0}", nodeDuration);
                        msgScale = 0.4f;
                        msgTimer.Start();
                    }

                    else if (Game.IsControlJustPressed(0, GTA.Control.SelectPrevWeapon))
                    {
                        nodeDuration += 100;
                        msgText = string.Format("Duration: {0}", nodeDuration);
                        msgScale = 0.4f;
                        msgTimer.Start();
                    }

                    else if (Game.IsControlJustPressed(0, GTA.Control.ScriptRDown))
                    {
                        splineCam.AddNode(selector.MainCamera.Position, selector.MainCamera.Rotation, nodeDuration);
                        msgText = string.Format("~e~New node: {0}", selector.MainCamera.Position);
                        msgScale = 0.3f;
                        msgTimer.Start();
                    }

                    else if (Game.IsControlJustPressed(0, GTA.Control.ScriptRRight))
                    {
                        ExitPointSelector();
                    }
                }
            }
            activePool.ProcessMenus();
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.T)
                ToggleMenu();
        }

        private void ToggleMenu()
        {
            var menus = new List<UIMenu> { mainMenu };
            if (menus.Find(item => item.Visible) != null)
                menus.ForEach(item => { if (item.Visible) item.Visible = false; });
            else
                mainMenu.Visible = !mainMenu.Visible;
        }

        private void OnListChanged(UIMenu sender, UIMenuListItem selectedItem, int index)
        {
            if (sender != cameraOptionsMenu) return;

            if (selectedItem == sender.MenuItems[0])
            {
                //avoid dividing by zero
                var speed = selectedItem.Index;
                speed = speed > 0 ? speed : 1;
                splineCam.Speed = speed;
            }
            else if (selectedItem == sender.MenuItems[1])
            {
                //avoid dividing by zero
                var fov = selectedItem.Index;
                fov = fov > 0 ? fov : 1;
                splineCam.MainCamera.FieldOfView = fov;
            }
        }

        private void OnCheckboxChanged(UIMenu sender, UIMenuCheckboxItem selectedItem, bool isChecked)
        {
            if (sender != cameraOptionsMenu) return;
            if (selectedItem == sender.MenuItems[2])
                splineCam.UsePlayerView = isChecked;
            else if (selectedItem == sender.MenuItems[3])
                splineCam.InterpToPlayer = isChecked;
        }

        private void EnterPointSelector()
        {
            if (selector.MainCamera.IsActive || splineCam.MainCamera.IsActive)
            {
                UI.ShowSubtitle("Camera is Active.");
                return;
            }

            Game.Player.Character.FreezePosition = true;
            selector.EnterCameraView(Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 10f)));
            ToggleMenu();
        }

        private void ExitPointSelector()
        {
            selector.ExitCameraView();
            Game.Player.Character.FreezePosition = false;
        }

        private void StartInterpolatingCam()
        {
            if (splineCam.Nodes.Count < 2)
            {
                UI.ShowSubtitle("Setup camera nodes first!");
                return;
            }

            splineCam.EnterCameraView(Game.Player.Character.GetOffsetInWorldCoords(new Vector3(0, 0, 10f)));
        }

        private void StopInterpolatingCam()
        {
            if (splineCam.MainCamera.IsActive)
            {

                splineCam.ExitCameraView();
            }

            else
                UI.ShowSubtitle("Camera not active.");
        }

        private void ResetAllCameras()
        {
            if (splineCam != null)
            {
                if (splineCam.MainCamera.IsActive)
                    splineCam.ExitCameraView();
                if (splineCam.Nodes.Count > 0)
                    splineCam.Nodes.Clear();
            }
            if (selector != null && selector.MainCamera.IsActive)
                selector.ExitCameraView();

            World.RenderingCamera = null;
            Function.Call(Hash.DESTROY_ALL_CAMS, false);
            splineCam = new SplineCamera();
            selector = new PositionSelector(Vector3.Zero, Vector3.Zero);
        }

        protected override void Dispose(bool A_0)
        {
            if (splineCam != null && splineCam.UsePlayerView)
            {
                Game.Player.Character.IsInvincible = false;
                Game.Player.Character.IsVisible = true;
            }

            World.RenderingCamera = null;
            Function.Call(Hash.CLEAR_FOCUS);

            base.Dispose(A_0);
        }
    }
}
