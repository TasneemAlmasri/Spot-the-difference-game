using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

//using NAudio.Wave;
using MathNet.Numerics;

namespace WinFormsApp3_second_project
{
    public partial class Form1 : Form
    {
        PictureBox pictureBox1;
        PictureBox pictureBox2;
        public Form1()
        {
            InitializeComponent();
            //paths
            string imgpath1 = "D:\\chrome stuff\\multi\\hand edited\\photo_2025-05-22_08-51-28.jpg";
            //"D:/chrome stuff/multi/hand edited/img1.png";
            //string imgpath2 = "D:/chrome stuff/multi/hand edited/img1 edited.png";
            string imgpath2 = "D:\\chrome stuff\\multi\\hand edited\\photo_2025-05-22_08-51-28me.jpg";
            //"D:/chrome stuff/multi/hand edited/img1 edited circles.png";



            //read images
            Mat img1 = CvInvoke.Imread(imgpath1);
            Mat img2 = CvInvoke.Imread(imgpath2);



            //subrtaction
            Mat subimage = new Mat();
            CvInvoke.AbsDiff(img1, img2, subimage);
            //subimage.Save("D:/chrome stuff/multi/hand edited/subimg.png");



            //convert to binary
            Mat grayscalimg = new Mat();
            CvInvoke.CvtColor(subimage, grayscalimg, ColorConversion.Bgr2Gray);

            Mat binaryimg = new Mat();
            CvInvoke.Threshold(grayscalimg, binaryimg, 30, 255, ThresholdType.Binary);
            binaryimg.Save("D:/chrome stuff/multi/hand edited/tuqa.png");



            //finding all contours
            VectorOfVectorOfPoint allContours = new VectorOfVectorOfPoint(); //contour is a list of points, VectorOfVectorOfPoint is a list of countours
            CvInvoke.FindContours(binaryimg, allContours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple); //null means dont save the hirearchy anywhere//use external when you only care about the main blocks not what's inside them //chain..: contour point storage method



            //////draw boxes
            //for (int i = 0; i < allContours.Size; i++)
            //{
            //    Debug.WriteLine($"i is heeeeeeeeeeere :{i} ");

            //    //Rectangle rect = CvInvoke.BoundingRectangle(allContours[i]);
            //    //CvInvoke.Rectangle(img2, rect, new MCvScalar(0, 0, 0), 2); //color BGR, and thikness

            //    var circle = CvInvoke.MinEnclosingCircle(allContours[i]);
            //    CvInvoke.Circle(img2, Point.Round(circle.Center), (int)(circle.Radius * 1.2), new MCvScalar(0, 0, 0), 2);//point is needed and it is pointF ,so we rounded to point

            //}

            //CvInvoke.Imwrite("D:\\chrome stuff\\multi\\hand edited\\highlighted.png", img2);
            //CvInvoke.Imwrite("D:\\chrome stuff\\multi\\hand edited\\highlightedCircle.png", img2);




            //PictureBox1
            Bitmap img1bmp = img1.ToBitmap();
            pictureBox1 = new PictureBox();
            pictureBox1.Image = img1bmp;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.Location = new Point(10, 10);
            pictureBox1.MouseClick += box_click;

            //PictureBox2
            Bitmap img2bmp = img2.ToBitmap();
            pictureBox2 = new PictureBox();
            pictureBox2.Image = img2bmp;
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox2.Location = new Point(pictureBox1.Right + 10, 10);
            pictureBox2.MouseClick += box_click;

            //add to form
            this.Controls.Add(pictureBox1); // this referes to Form1, Controls is the collection of UI elements like buttons lables .. placed on the form,Add(bla) adds bla to te collection so it shows when the form is shown
            this.Controls.Add(pictureBox2);

            //resize whole form to fit both images
            this.ClientSize = new Size(pictureBox2.Right + 10, Math.Max(pictureBox1.Height, pictureBox2.Height) + 20);


            //click handedling
            void box_click(object sender, MouseEventArgs e)
            {
                PictureBox clickedBox = sender as PictureBox;
                if (clickedBox != null)
                {
                    Debug.WriteLine($"Clickedddddddddddd at ({e.X}, {e.Y}) in {clickedBox.Name}");

                    Point clickedPoint = e.Location;

                    for (int i = 0; i < allContours.Size; i++)
                    {
                        var contour = allContours[i];

                        double isInside = CvInvoke.PointPolygonTest(contour, new PointF(clickedPoint.X, clickedPoint.Y), false);

                        if (isInside > 0)
                        {
                            var circle = CvInvoke.MinEnclosingCircle(contour);
                            CvInvoke.Circle(img2, Point.Round(circle.Center), (int)(circle.Radius * 1.2), new MCvScalar(0, 0, 0), 2);
                            pictureBox2.Image = img2.ToBitmap();

                        }



                    }
                }


            }
        }
    }
}
