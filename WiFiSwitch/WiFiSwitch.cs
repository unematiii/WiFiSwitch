using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using System;
using System.Linq;
using WiFiSwitch.UI;
using WiFiSwitch.Core;

namespace WiFiSwitch
{
    class Program : NUIApplication
    {
        private AccessPointManager mAccessPointManager;
        private AcessPointListView mAccessPointListView;
        private LoaderView mLoaderView;
        private View mView;

        protected override void OnCreate()
        {
            base.OnCreate();
            Initialize();
        }

        void Initialize()
        {
            CreateView();

            Window.Instance.BackgroundColor = Color.White;
            Window.Instance.KeyEvent += OnKeyEvent;

            mAccessPointManager = new AccessPointManager();
            mAccessPointManager.AcessPointsLoaded += OnAcessPointsLoaded;
            mAccessPointManager.Connecting += OnConnecting;
            mAccessPointManager.Connected += OnConnected;

            mAccessPointManager.Scan();
        }

        public void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down)
            {

                if (e.Key.KeyPressedName == "Up")
                {
                    mAccessPointListView.SelectPrevItem();
                }

                if (e.Key.KeyPressedName == "Down")
                {
                    mAccessPointListView.SelectNextItem();
                }

                if (e.Key.KeyPressedName == "Return")
                {
                    ActivateSelectedAP();
                }

                if (e.Key.KeyPressedName == "XF86Back" || e.Key.KeyPressedName == "Escape")
                {
                    Exit();
                }
            }
        }

        private void ActivateSelectedAP()
        {
            mAccessPointManager.Connect(mAccessPointListView.GetSelectedItem().Name);
        }

        private void CreateAcessPointsListView()
        {
            mAccessPointListView = new AcessPointListView();
            mView.Add(mAccessPointListView);
        }

        private void CreateLoaderView()
        {
            var layer = new Layer();
            Window.Instance.AddLayer(layer);

            mLoaderView = new LoaderView();
            layer.Add(mLoaderView);
        }

        private void CreateView()
        {
            mView = new View
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                Layout = new LinearLayout()
                {
                    LinearAlignment = LinearLayout.Alignment.CenterVertical,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                }
            };

            Window.Instance.GetDefaultLayer().Add(mView);

            CreateLoaderView();
            CreateAcessPointsListView();
        }


        private void RenderAccessPoints()
        {
            try
            {
                var accessPoints = mAccessPointManager.AccessPoints.Values.ToList();

                if (accessPoints.Count > 0)
                {
                    mAccessPointListView.SetItems(accessPoints);
                }
                else
                {
                    throw new Exception("No configurations...");
                }
            }
            catch (Exception e)
            {
                HandleExecption(e);
            }
        }

        private void ToggleLoader(bool isLoading)
        {
            mLoaderView.ToggleLoading(isLoading);
        }

        private void UpdateAccessPoints()
        {
            mAccessPointListView.Update();
        }

        private void HandleExecption(Exception e)
        {
            TextLabel text = new TextLabel(e.Message)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextColor = Color.Black,
                PointSize = 60.0f,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthResizePolicy = ResizePolicyType.FillToParent
            };

            mView.Remove(mAccessPointListView);
            mView.Add(text);
        }

        private void OnAcessPointsLoaded(object sender, EventArgs args)
        {
            RenderAccessPoints();
            ToggleLoader(false);
        }

        private void OnConnecting(object sender, EventArgs args)
        {
            UpdateAccessPoints();
            ToggleLoader(true);
        }

        private void OnConnected(object sender, EventArgs args)
        {
            UpdateAccessPoints();
            ToggleLoader(false);
        }

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
