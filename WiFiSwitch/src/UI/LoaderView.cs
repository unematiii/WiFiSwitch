using Tizen.Applications;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace WiFiSwitch.UI
{
    public class LoaderView : View
    {
        private Animation mLoadingAnimation;
        private ImageView mLoaderImageView;

        public LoaderView()
        {
            Initialize();
        }

        private void Initialize()
        {
            HeightResizePolicy = ResizePolicyType.FillToParent;
            WidthResizePolicy = ResizePolicyType.FillToParent;
            BackgroundColor = new Color(1, 1, 1, 0.45f);

            Layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal,
                LinearAlignment = LinearLayout.Alignment.Center
            };

            mLoaderImageView = new ImageView(Application.Current.DirectoryInfo.SharedResource + "loader.png")
            {
                Size2D = new Size2D(125, 125),
                WidthResizePolicy = ResizePolicyType.UseAssignedSize,
                HeightResizePolicy = ResizePolicyType.UseAssignedSize,
            };
            Add(mLoaderImageView);

            mLoadingAnimation = new Animation()
            {
                Duration = 750,
                Looping = true
            };
            mLoadingAnimation.AnimateBy(mLoaderImageView, "Orientation", new Rotation(new Radian(new Degree(360.0f)), PositionAxis.Z));

            StartLoading();
        }

        public void StartLoading()
        {
            Show();
            mLoadingAnimation.Play();
        }

        public void StopLoading()
        {
            Hide();
            mLoadingAnimation.Stop();
        }

        public void ToggleLoading(bool loading)
        {
            if (loading)
            {
                StartLoading();
            }
            else
            {
                StopLoading();
            }
        }
    }
}
