using System;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Windows;

namespace MachineVisionApp.Components
{
    public class VideoCaptureComponent
    {
        private VideoCapture _capture;
        private Mat _frame;
        private Mat _grayFrame;
        private bool _isRunning;

        public VideoCaptureComponent()
        {
            _capture = new VideoCapture();
            _frame = new Mat();
            _grayFrame = new Mat();
        }

        public void StartCapture()
        {
            try
            {
                _capture = new VideoCapture(0); // 0 表示默认摄像头
                if (!_capture.IsOpened())
                {
                    MessageBox.Show("无法打开摄像头！");
                    return;
                }
                _isRunning = true;
                Task.Run(() => CaptureAndProcess());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开摄像头时出错: {ex.Message}");
            }
        }

        public void StopCapture()
        {
            _isRunning = false;
            _capture?.Release();
        }

        private void CaptureAndProcess()
        {
            try
            {
                while (_isRunning)
                {
                    _capture.Read(_frame);
                    if (_frame.Empty())
                        continue;

                    Cv2.CvtColor(_frame, _grayFrame, ColorConversionCodes.BGR2GRAY);
                    OnFrameCaptured?.Invoke(_frame, _grayFrame);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"捕获和处理图像时出错: {ex.Message}");
            }
            finally
            {
                _capture?.Release();
            }
        }

        // 获取摄像头帧率
        public double GetFrameRate()
        {
            return _capture.Get(VideoCaptureProperties.Fps);
        }

        // 获取摄像头分辨率
        public System.Windows.Size GetResolution()
        {
            int width = (int)_capture.Get(VideoCaptureProperties.FrameWidth);
            int height = (int)_capture.Get(VideoCaptureProperties.FrameHeight);
            return new System.Windows.Size(width, height);
        }

        public event Action<Mat, Mat> OnFrameCaptured;
    }
}
