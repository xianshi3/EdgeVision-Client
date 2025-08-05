using System;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Windows;

namespace MachineVisionApp.Components
{
    public class VideoCaptureComponent
    {
        private VideoCapture? _capture;
        private Mat? _frame;
        private Mat? _grayFrame;
        private bool _isRunning = false;
        private readonly object _lock = new();

        public event Action<Mat, Mat> OnFrameCaptured = delegate { };
        public event Action<string?> OnCaptureError = delegate { };
        public event Action<string?> OnCaptureStopped = delegate { };

        public void StartCapture()
        {
            lock (_lock)
            {
                if (_isRunning)
                    return; // 已运行，忽略重复调用

                try
                {
                    _capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW); // 用DirectShow提高稳定性
                    if (!_capture.IsOpened())
                    {
                        MessageBox.Show("无法打开摄像头！");
                        OnCaptureError?.Invoke("无法打开摄像头");
                        _capture.Release();
                        _capture = null;
                        return;
                    }

                    _frame = new Mat();
                    _grayFrame = new Mat();
                    _isRunning = true;
                    _ = CaptureAndProcessAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"打开摄像头时出错: {ex.Message}");
                    OnCaptureError?.Invoke($"打开摄像头异常: {ex.Message}");
                    _isRunning = false;
                }
            }
        }

        public void StopCapture()
        {
            lock (_lock)
            {
                if (!_isRunning)
                    return;

                _isRunning = false;
                try
                {
                    _capture?.Release();
                    _capture = null;
                }
                catch { }

                OnCaptureStopped?.Invoke("摄像头已停止");
            }
        }

        private async Task CaptureAndProcessAsync()
        {
            try
            {
                while (_isRunning)
                {
                    if (_capture == null || _frame == null || _grayFrame == null)
                    {
                        await Task.Delay(50);
                        continue;
                    }

                    bool readSuccess = _capture.Read(_frame);
                    if (!readSuccess || _frame.Empty())
                    {
                        OnCaptureError?.Invoke("摄像头读取失败或帧为空，正在重试...");
                        await Task.Delay(100);
                        continue;
                    }

                    Cv2.CvtColor(_frame, _grayFrame, ColorConversionCodes.BGR2GRAY);
                    OnFrameCaptured?.Invoke(_frame, _grayFrame);

                    await Task.Delay(30);
                }
            }
            catch (Exception ex)
            {
                OnCaptureError?.Invoke($"捕获线程异常：{ex.Message}");
            }
            finally
            {
                try
                {
                    _capture?.Release();
                    _capture = null;
                }
                catch { }

                _isRunning = false;
                OnCaptureStopped?.Invoke("摄像头已停止");
            }
        }

        public double GetFrameRate()
        {
            if (_capture == null)
                return 0;
            return _capture.Get(VideoCaptureProperties.Fps);
        }

        public System.Windows.Size GetResolution()
        {
            if (_capture == null)
                return new System.Windows.Size(0, 0);
            int width = (int)_capture.Get(VideoCaptureProperties.FrameWidth);
            int height = (int)_capture.Get(VideoCaptureProperties.FrameHeight);
            return new System.Windows.Size(width, height);
        }
    }
}
