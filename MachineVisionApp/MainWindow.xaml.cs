using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using OpenCvSharp;

namespace MachineVisionApp
{
    public partial class MainWindow : System.Windows.Window
    {
        private Components.VideoCaptureComponent _videoCaptureComponent;
        private Components.ImageDisplayComponent _imageDisplayComponent;
        private Components.ThresholdParameterComponent _thresholdParameterComponent;
        private Components.FaceDetectionComponent _faceDetectionComponent;
        private Components.EdgeDetectionComponent _edgeDetectionComponent;
        private int _threshold1 = 100;
        private int _threshold2 = 200;

        public MainWindow()
        {
            InitializeComponent();
            _faceDetectionComponent = new Components.FaceDetectionComponent("haarcascade_frontalface_default.xml");
            _edgeDetectionComponent = new Components.EdgeDetectionComponent();
            _videoCaptureComponent = new Components.VideoCaptureComponent();
            _imageDisplayComponent = new Components.ImageDisplayComponent(OriginalImage, EdgeImage);
            _thresholdParameterComponent = new Components.ThresholdParameterComponent(Threshold1TextBox, Threshold2TextBox, ApplyThresholdsButton);
            _videoCaptureComponent.OnFrameCaptured += ProcessFrame;
            _thresholdParameterComponent.OnThresholdsChanged += UpdateThresholds;
            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _videoCaptureComponent.StopCapture();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            _videoCaptureComponent.StopCapture();
        }

        private void ProcessFrame(Mat originalFrame, Mat grayFrame)
        {
            try
            {
                Mat edges = _edgeDetectionComponent.DetectEdges(grayFrame, _threshold1, _threshold2);
                int faceCount = _faceDetectionComponent.DetectFaces(grayFrame, originalFrame);
                _imageDisplayComponent.UpdateImages(originalFrame, edges);
                FaceCountTextBlock.Dispatcher.Invoke(() => FaceCountTextBlock.Text = $"检测到的人脸数量: {faceCount}");

                // 更新摄像头数据
                UpdateCameraData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理帧时出错: {ex.Message}");
            }
        }

        private void UpdateThresholds(int threshold1, int threshold2)
        {
            _threshold1 = threshold1;
            _threshold2 = threshold2;
        }

        private void ApplyThresholdsButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(Threshold1TextBox.Text, out int threshold1) && int.TryParse(Threshold2TextBox.Text, out int threshold2))
            {
                _threshold1 = threshold1;
                _threshold2 = threshold2;
            }
            else
            {
                MessageBox.Show("请输入有效的整数作为阈值！");
            }
        }

        private void StartCameraButton_Click(object sender, RoutedEventArgs e)
        {
            _videoCaptureComponent.StartCapture();
        }

        private void StopCameraButton_Click(object sender, RoutedEventArgs e)
        {
            _videoCaptureComponent.StopCapture();
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp|所有文件|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    Mat image = Cv2.ImRead(openFileDialog.FileName);
                    if (image.Empty())
                    {
                        MessageBox.Show("无法加载图片！");
                        return;
                    }
                    Mat grayImage = new Mat();
                    Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);
                    Mat edges = _edgeDetectionComponent.DetectEdges(grayImage, _threshold1, _threshold2);
                    int faceCount = _faceDetectionComponent.DetectFaces(grayImage, image);
                    _imageDisplayComponent.UpdateImages(image, edges);
                    FaceCountTextBlock.Dispatcher.Invoke(() => FaceCountTextBlock.Text = $"检测到的人脸数量: {faceCount}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载图片或处理图像时出错: {ex.Message}");
                }
            }
        }

        // 更新摄像头数据
        private void UpdateCameraData()
        {
            try
            {
                double frameRate = _videoCaptureComponent.GetFrameRate();
                System.Windows.Size resolution = _videoCaptureComponent.GetResolution();

                CameraDataTextBlock.Dispatcher.Invoke(() => CameraDataTextBlock.Text = $"摄像头数据: 帧率={frameRate} FPS, 分辨率={resolution.Width}x{resolution.Height}");
            }
            catch (Exception ex)
            {
                CameraDataTextBlock.Dispatcher.Invoke(() => CameraDataTextBlock.Text = $"摄像头数据: 无法获取数据 - {ex.Message}");
            }
        }
    }
}
