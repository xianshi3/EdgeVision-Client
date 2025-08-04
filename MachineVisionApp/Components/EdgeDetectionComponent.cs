using OpenCvSharp;

namespace MachineVisionApp.Components
{
    public class EdgeDetectionComponent
    {
        public Mat DetectEdges(Mat grayFrame, int threshold1, int threshold2)
        {
            Mat edges = new Mat();
            Cv2.Canny(grayFrame, edges, threshold1, threshold2);
            return edges;
        }
    }
}
