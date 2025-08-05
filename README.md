<h1 align="center">🧠 WPF + OpenCV Machine Vision</h1>

<p align="center">
  A modern desktop application for real-time video processing, edge detection, and face recognition.
  <br/>
  Built with ❤️ using <strong>WPF</strong> and <strong>OpenCV</strong>.
</p>

---

## 🚀 Features

- 🎥 **Live Video Capture** – Stream and process webcam frames in real-time  
- 🧍 **Face Detection** – Detect faces using Haar cascade classifiers  
- 🪞 **Edge Detection** – Apply Canny filters with adjustable thresholds  
- 🖼 **Image Analysis** – Load static images and apply the same pipeline  
- 🌈 **Grayscale Conversion** – Enhance clarity and detection precision  
- 🧩 **Minimal UI** – Responsive and user-friendly layout

---

## 🖥️ User Interface Preview

<p align="center">
  <img src="https://github.com/user-attachments/assets/60e1da5e-7809-44d6-a1a6-0c28bb53dc48" alt="Live Preview" width="600"/>
</p>

---

## ⚙️ How It Works

1. **Start Camera** – Begin video capture and real-time processing  
2. **Stop Camera** – Release webcam and halt processing  
3. **Load Image** – Import an image from your computer  
4. **Apply Thresholds** – Fine-tune the Canny edge detector for custom output  

Processing runs asynchronously to keep the UI responsive and flicker-free.

---

## 📁 Code Structure

- `MainWindow.xaml.cs`  
  Core logic for:
  - `VideoCapture` – Webcam stream
  - `Mat` – Image frames
  - `CascadeClassifier` – Face detection  
  - Event handlers for buttons and window events  
  - `CaptureAndProcess()` – Video frame pipeline  
  - `LoadImageButton_Click()` – Image file processing

---

## 🛠 Requirements

- ✅ [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- ✅ Windows with WPF support
- ✅ NuGet Packages:
  - `OpenCvSharp4`
  - `OpenCvSharp4.runtime.windows`
  - `OpenCvSharp4.WpfExtensions`

---

## 📦 Tech Stack

| Technology     | Description                     |
|----------------|---------------------------------|
| `WPF`          | UI framework for Windows apps   |
| `OpenCvSharp`  | .NET wrapper for OpenCV         |
| `C#`           | Primary programming language    |
| `XAML`         | UI design and layout            |

---

