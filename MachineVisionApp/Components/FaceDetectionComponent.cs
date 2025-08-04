using OpenCvSharp;

namespace MachineVisionApp.Components
{
    public class FaceDetectionComponent
    {
        private CascadeClassifier _faceCascade;
        private Scalar _rectangleColor;
        private int _rectangleThickness;

        public FaceDetectionComponent(string cascadePath)
        {
            _faceCascade = new CascadeClassifier(cascadePath);
            _rectangleColor = Scalar.Red;
            _rectangleThickness = 2;
        }

        public int DetectFaces(Mat grayFrame, Mat originalFrame)
        {
            OpenCvSharp.Rect[] faces = _faceCascade.DetectMultiScale(grayFrame);
            foreach (OpenCvSharp.Rect face in faces)
            {
                Cv2.Rectangle(originalFrame, face, _rectangleColor, _rectangleThickness);
            }
            return faces.Length;
        }
    }
}
