## Overview
This project is a Windows-based machine vision application developed using WPF (Windows Presentation Foundation) and OpenCV. The primary functions of this application include video capture, image processing, face detection, and edge detection.

![QQ20250202-005723](https://github.com/user-attachments/assets/60e1da5e-7809-44d6-a1a6-0c28bb53dc48)

## Features
- **Video Capture**: The application captures video frames from the default camera and processes them in real-time.
- **Image Conversion**: Converts captured video frames to grayscale images for better edge detection and face recognition.
- **Edge Detection**: Utilizes the Canny edge detection algorithm with adjustable thresholds to identify and highlight edges in the captured frames.
- **Face Detection**: Detects faces in the grayscale images using a pre-trained Haar Cascade classifier and draws rectangles around detected faces.
- **Image Loading**: Allows users to load images from their local storage, perform similar processing as the video frames, and display the results.
- **User Interface**: Provides a simple user interface to control the camera and adjust edge detection thresholds.

## How It Works
- When the application starts, the camera is turned off by default.
- Users can start video capture by clicking the 'Start Camera' button. The application then continuously captures video frames 和 processes them in a separate task to avoid blocking the UI thread.
- Users can stop video capture 和 release the camera resources by clicking the 'Stop Camera' button.
- Users can load an image by clicking the 'Load Image' button 和 select an image file from their local storage. The application will then process the image 和 display the results.
- The 'Apply Thresholds' button allows users to set custom thresholds for the Canny edge detector, enhancing the flexibility of edge detection.

## Code Structure
- The code is organized into a single class, `MainWindow`, which inherits from `System.Windows.Window`.
- It initializes the necessary components such as `VideoCapture` for video capturing, `Mat` objects for storing image frames, and a `CascadeClassifier` for face detection.
- Event handlers (`MainWindow_Loaded`, `MainWindow_Closed`, `StartCameraButton_Click`, `StopCameraButton_Click`, `LoadImageButton_Click`, `ApplyThresholdsButton_Click`) manage the application's lifecycle and user interactions.
- 该 `CaptureAndProcess` method contains the core logic for processing video frames, while similar processing is done for loaded images in the `LoadImageButton_Click` method.

## Requirements
- .NET8
- OpenCvSharp library for .NET
- OpenCvSharp.WpfExtensions for displaying images in WPF
- WPF

## Usage
1. Open the application.
2. Click 'Start Camera' to begin capturing 和 processing video frames.
3. Adjust the Canny edge detection thresholds using the provided textboxes 和 click 'Apply Thresholds'.
4. Click 'Stop Camera' to stop capturing video frames.
5. Click 'Load Image' to select 和 process an image from your local storage.
6. Observe the processed images 和 the number of detected faces in the UI.
