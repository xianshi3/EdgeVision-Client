﻿using System;
using System.Threading.Tasks;
using System.Windows;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace MachineVisionApp
{
    /// <summary>
    /// 主窗口类，用于捕捉视频并进行处理
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private VideoCapture? _capture;
        private Mat? _frame;
        private Mat? _edges;
        private Mat? _grayFrame;
        private CascadeClassifier? _faceCascade;
        private bool _isRunning;

        /// <summary>
        /// 初始化主窗口，设置摄像头、图像矩阵和人脸检测器
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _capture = new VideoCapture();
            _frame = new Mat();
            _edges = new Mat();
            _grayFrame = new Mat();
            _faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml"); // 确保路径正确
            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
        }

        /// <summary>
        /// 当窗口加载时初始化摄像头并启动视频捕获和处理
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">包含事件数据的RoutedEventArgs</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 初始化摄像头
            _capture = new VideoCapture(0); // 0 表示默认摄像头
            if (!_capture.IsOpened())
            {
                MessageBox.Show("无法打开摄像头！");
                return;
            }

            _isRunning = true;

            // 启动视频捕获和处理
            Task.Run(() => CaptureAndProcess());
        }

        /// <summary>
        /// 当窗口关闭时停止摄像头并释放资源
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">包含事件数据的EventArgs</param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            // 停止摄像头
            _isRunning = false;
            _capture?.Release();
        }

        /// <summary>
        /// 捕获视频帧并进行处理：转换为灰度图像，检测边缘，检测人脸，并在原始图像上绘制矩形
        /// </summary>
        private void CaptureAndProcess()
        {
            while (_isRunning)
            {
                // 捕获一帧
                _capture.Read(_frame);
                if (_frame.Empty())
                    continue;

                // 转换为灰度图像
                Cv2.CvtColor(_frame, _grayFrame, ColorConversionCodes.BGR2GRAY);

                // 使用 Canny 边缘检测
                Cv2.Canny(_grayFrame, _edges, 100, 200);

                // 人脸检测
                OpenCvSharp.Rect[] faces = _faceCascade.DetectMultiScale(_grayFrame);

                // 在原始图像上绘制矩形
                foreach (OpenCvSharp.Rect face in faces)
                {
                    Cv2.Rectangle(_frame, face, Scalar.Red, 2);
                }

                // 更新界面
                Dispatcher.Invoke(() =>
                {
                    OriginalImage.Source = BitmapSourceConverter.ToBitmapSource(_frame);
                    EdgeImage.Source = BitmapSourceConverter.ToBitmapSource(_edges);
                });
            }
        }
    }
}
