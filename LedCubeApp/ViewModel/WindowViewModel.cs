using System.Windows;
using System.Windows.Input;

namespace LedCubeApp
{
    /// <summary>
    /// The viewmodel for the custom Window
    /// </summary>
    public class WindowViewModel : BaseViewModel
    {
        #region Private Fields

        /// <summary>
        /// The window this ViewModel controls
        /// </summary>
        private Window mWindow;
        /// <summary>
        /// The Margin around the window to allow for a drop shadow
        /// </summary>
        private int mOuterMarginSize = 10;
        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int mWindowRadius = 10;

        #endregion
        #region Commands
        /// <summary>
        /// The command for minimizing a window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command for maximizing a window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The Command for closing the window 
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command for the drop down menu on the Icon
        /// </summary>
        public ICommand MenuCommand { get; set; }
        #endregion
        #region Public Properties

        /// <summary>
        /// The smallest width the window can go to
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 600;
        /// <summary>
        /// The smallest height the window can go to
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;

        /// <summary>
        /// The Window is border less or not
        /// </summary>
        public bool Borderless => (mWindow.WindowState == WindowState.Maximized);

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder { get; set; } = 6;

        /// <summary>
        /// The size of the resize border around the window, taking account of the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness => new Thickness(ResizeBorder + OuterMarginSize);

        /// <summary>
        /// The Margin around the window to allow for a drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            get => Borderless ? 0 : mOuterMarginSize;
            set => mOuterMarginSize = value;
        }

        /// <summary>
        /// The Margin Thickness around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness => new Thickness(OuterMarginSize);

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius
        {
            get => Borderless ? 0 : mWindowRadius;
            set => mWindowRadius = value;
        }
        /// <summary>
        /// The Corner radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public int TitleHeight { get; set; } = 30;

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);


        /// <summary>
        /// The Padding of the inner content of the main window
        /// </summary>
        public Thickness InnerContentPadding { get; set; } = new Thickness(6);
        #endregion

        #region Constructor
        /// <summary>
        /// The Default constructor
        /// </summary>
        /// <param name="window"></param>
        public WindowViewModel(Window window)
        {
            mWindow = window;

            //Listen out for the window resizing
            mWindow.StateChanged += (sender, e) =>
            {
                //Fire of all event for all properties that are affected by resize
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };
            //Create the commands
            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => mWindow.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, mWindow.PointToScreen(Mouse.GetPosition(mWindow))));

            var resizer = new WindowResizer(mWindow);
        } 
        #endregion
    }
}
