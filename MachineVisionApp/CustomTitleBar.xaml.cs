using System.Windows;
using System.Windows.Controls;

namespace MachineVisionApp
{
    public partial class CustomTitleBar : UserControl
    {
        public CustomTitleBar()
        {
            InitializeComponent();
            Loaded += (s, e) => UpdateButtonIcon();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window == null) return;

            // 使用系统原生最大化/还原功能
            window.WindowState = window.WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;

            UpdateButtonIcon();
        }

        private void UpdateButtonIcon()
        {
            var window = Window.GetWindow(this);
            if (window == null) return;

            // 根据窗口状态更新图标
            MaximizeButton.Content = window.WindowState == WindowState.Maximized
                ? "\uE923"  // Segoe MDL2 Assets 还原图标
                : "\uE922"; // Segoe MDL2 Assets 最大化图标
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (e.ClickCount == 2 && window.ResizeMode != ResizeMode.NoResize)
            {
                // 支持双击标题栏切换最大化状态
                MaximizeButton_Click(sender, e);
            }
            else
            {
                window?.DragMove();
            }
        }
    }
}