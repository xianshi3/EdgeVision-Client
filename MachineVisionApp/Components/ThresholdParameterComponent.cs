using System.Windows;
using System.Windows.Controls;

namespace MachineVisionApp.Components
{
    public class ThresholdParameterComponent
    {
        private TextBox _threshold1TextBox;
        private TextBox _threshold2TextBox;
        private Button _applyThresholdsButton;

        public ThresholdParameterComponent(TextBox threshold1TextBox, TextBox threshold2TextBox, Button applyThresholdsButton)
        {
            _threshold1TextBox = threshold1TextBox;
            _threshold2TextBox = threshold2TextBox;
            _applyThresholdsButton = applyThresholdsButton;
            _applyThresholdsButton.Click += ApplyThresholdsButton_Click;
        }

        private void ApplyThresholdsButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(_threshold1TextBox.Text, out int threshold1) && int.TryParse(_threshold2TextBox.Text, out int threshold2))
            {
                OnThresholdsChanged?.Invoke(threshold1, threshold2);
            }
            else
            {
                MessageBox.Show("请输入有效的整数作为阈值！");
            }
        }

        public event Action<int, int> OnThresholdsChanged;
    }
}
