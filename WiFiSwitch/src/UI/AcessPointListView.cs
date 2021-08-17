using System.Collections.Generic;
using Tizen.Applications;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using WiFiSwitch.Core;

namespace WiFiSwitch.UI
{
    public class AcessPointListView : View
    {
        private List<AceessPointListItemView> mItems;
        private int mSelectedItemIndex = 0;

        public AcessPointListView()
        {
            Initialize();
        }


        public void SetItems(List<AccessPoint> models)
        {
            if (mItems == null)
            {
                mItems = new List<AceessPointListItemView>();

                for (var i = 0; i < models.Count; i++)
                {
                    var model = models[i];
                    var item = new AceessPointListItemView()
                    {
                        Model = model,
                        Selected = model.Connected
                    };

                    if (item.Selected)
                    {
                        mSelectedItemIndex = i;
                    }

                    item.Update();

                    mItems.Add(item);
                    Add(item);
                }
            }
        }


        private void Initialize()
        {
            HeightResizePolicy = ResizePolicyType.FitToChildren;
            WidthSpecification = LayoutParamPolicies.MatchParent;

            Layout = new LinearLayout()
            {
                CellPadding = new Size2D(0, 20),
                LinearOrientation = LinearLayout.Orientation.Vertical,
            };
        }

        public void SelectNextItem()
        {
            int count = mItems.Count;

            if (count > 0)
            {
                if (mSelectedItemIndex + 1 <= count - 1)
                {
                    SetSelectItem(mSelectedItemIndex + 1);
                }
                else
                {
                    SetSelectItem(0);
                }
            }
        }

        public void SelectPrevItem()
        {
            int count = mItems.Count;

            if (count > 0)
            {
                if (mSelectedItemIndex - 1 >= 0)
                {
                    SetSelectItem(mSelectedItemIndex - 1);
                }
                else
                {
                    SetSelectItem(count - 1);
                }
            }
        }

        public void SetSelectItem(int index)
        {
            for (var i = 0; i < mItems.Count; i++)
            {
                var item = mItems[i];

                item.Selected = i == index;
                item.Update();
            }

            mSelectedItemIndex = index;
        }

        public AccessPoint GetSelectedItem()
        {
            for (var i = 0; i < mItems.Count; i++)
            {
                var item = mItems[i];
                if (item.Selected)
                {
                    return item.Model;
                }
            }

            return null;
        }

        public void Update()
        {
            for (var i = 0; i < mItems.Count; i++)
            {
                mItems[i].Update();
            }
        }
    }

    public class AceessPointListItemView : VisualView
    {
        public AccessPoint Model
        {
            get;
            set;
        }

        public bool Selected
        {
            get;
            set;
        }

        private NPatchVisual mHighlightVisual;
        private TextLabel mNameLabel;
        private ImageView mConnectionStatus;

        public AceessPointListItemView()
        {
            Initialize();
            Update();
        }

        public void Update()
        {
            if (Model != null)
            {
                mNameLabel.Text = Model.Name;

                if (Model.Connected)
                {
                    mConnectionStatus.Show();
                }
                else
                {
                    mConnectionStatus.Hide();
                }
            }
            mHighlightVisual.Opacity = Selected ? 1 : 0;
        }

        private void Initialize()
        {
            HeightResizePolicy = ResizePolicyType.FitToChildren;
            WidthSpecification = LayoutParamPolicies.MatchParent;
            Padding = new Extents(0, 0, 40, 40);

            Layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal,
                LinearAlignment = LinearLayout.Alignment.CenterVertical,
            };

            mNameLabel = new TextLabel()
            {
                TextColor = Color.Black,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightResizePolicy = ResizePolicyType.UseNaturalSize,
                HorizontalAlignment = HorizontalAlignment.Begin,
                Weight = 0.8f,
                Margin = new Extents(40, 0, 0, 0),
            };
            mConnectionStatus = new ImageView(Application.Current.DirectoryInfo.SharedResource + "connected.png")
            {
                Size2D = new Size2D(65, 65),
                WidthResizePolicy = ResizePolicyType.UseAssignedSize,
                HeightResizePolicy = ResizePolicyType.UseAssignedSize,
                Margin = new Extents(0, 40, 0, 0)
            };
            mHighlightVisual = new NPatchVisual()
            {
                URL = Application.Current.DirectoryInfo.SharedResource + "highlight.9.png",
            };

            Add(mNameLabel);
            Add(mConnectionStatus);
            AddVisual("Highlight", mHighlightVisual);
        }
    }
}
