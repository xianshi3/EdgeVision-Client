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

        public event Action<Mat, Mat> OnFrameCaptured;
        public event Action<string?> OnCaptureStopped; // 关闭事件，参数为异常消息

        public VideoCaptureComponent()
        {
            _capture = new VideoCapture();
            _frame = new Mat();
            _grayFrame = new Mat();
            OnFrameCaptured = (frame, grayFrame) => { };
            OnCaptureStopped = (reason) => { };
        }

        public void StartCapture()
        {
            try
            {
                _capture = new VideoCapture(0);
                if (!_capture.IsOpened())
                {
                    MessageBox.Show("无法打开摄像头！");
                    OnCaptureStopped?.Invoke("无法打开摄像头");
                    return;
                }
                _isRunning = true;
                Task.Run(() => CaptureAndProcess());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开摄像头时出错: {ex.Message}");
                OnCaptureStopped?.Invoke($"打开摄像头时异常: {ex.Message}");
            }
        }

        public void StopCapture()
        {
            _isRunning = false;
            _capture?.Release();
            OnCaptureStopped?.Invoke(null); // 正常停止，原因为空
        }

        private void CaptureAndProcess()
        {
            try
            {
                while (_isRunning)
                {
                    if (!_capture.Read(_frame) || _frame.Empty())
                    {
                        OnCaptureStopped?.Invoke("摄像头停止工作或关闭");
                        break;
                    }

                    Cv2.CvtColor(_frame, _grayFrame, ColorConversionCodes.BGR2GRAY);
                    OnFrameCaptured?.Invoke(_frame, _grayFrame);
                }
            }
            catch (Exception ex)
            {
                OnCaptureStopped?.Invoke($"捕获异常: {ex.Message}");
            }
            finally
            {
                _capture?.Release();
                _isRunning = false;
            }
        }

        public double GetFrameRate()
        {
            return _capture.Get(VideoCaptureProperties.Fps);
        }

        public System.Windows.Size GetResolution()
        {
            int width = (int)_capture.Get(VideoCaptureProperties.FrameWidth);
            int height = (int)_capture.Get(VideoCaptureProperties.FrameHeight);
            return new System.Windows.Size(width, height);
        }
    }
}
