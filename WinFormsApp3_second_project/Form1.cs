using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
//using NAudio.Wave;
//using MathNet.Numerics;

using Emgu.CV.Util;
using System.Diagnostics;



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
            string imgpath1 =
            //"D:\\chrome stuff\\multi\\hand edited\\photo_2025-05-22_08-51-28.jpg";
            //"D:/chrome stuff/multi/hand edited/img1.png";

            //@"D:\chrome stuff\multi\jeeda\1.jpg";
            //@"D:\chrome stuff\multi\jeeda\2.jpg";
            @"D:\chrome stuff\multi\jeeda\3.jpg";
            string imgpath2 =
            //string imgpath2 = "D:\\chrome stuff\\multi\\hand edited\\photo_2025-05-22_08-51-28me.jpg";
            //"D:/chrome stuff/multi/hand edited/img1 edited circles.png";

            //@"D:\chrome stuff\multi\jeeda\1_e.jpg";
            //@"D:\chrome stuff\multi\jeeda\2_e.jpg";
            @"D:\chrome stuff\multi\jeeda\3_e.jpg";


            //read images
            Mat img1 = CvInvoke.Imread(imgpath1);
            Mat img2 = CvInvoke.Imread(imgpath2);



            //subrtaction
            Mat subimage = new Mat();
            CvInvoke.AbsDiff(img1, img2, subimage);
            //subimage.Save("D:/chrome stuff/multi/hand edited/subimg.png");
            subimage.Save(@"D:\chrome stuff\multi\jeeda\sub.jpg");




            //convert to binary
            Mat grayscalimg = new Mat();
            CvInvoke.CvtColor(subimage, grayscalimg, ColorConversion.Bgr2Gray);

            Mat binaryimg = new Mat();
            CvInvoke.Threshold(grayscalimg, binaryimg, 20, 255, ThresholdType.Binary);
            binaryimg.Save(@"D:\chrome stuff\multi\jeeda\binary.jpg");



            //finding all contours
            VectorOfVectorOfPoint allContours = new VectorOfVectorOfPoint(); //contour is a list of points, VectorOfVectorOfPoint is a list of countours
            CvInvoke.FindContours(binaryimg, allContours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple); //null means dont save the hirearchy anywhere//use external when you only care about the main blocks not what's inside them //chain..: contour point storage method
            //Debug.WriteLine($"length issssssssssssssssss {allContours.Size}");


            ////draw boxes
            //for (int i = 0; i < allContours.Size; i++)
            //{
            //    //Debug.WriteLine($"i is heeeeeeeeeeere :{i} ");

            //    Rectangle rect = CvInvoke.BoundingRectangle(allContours[i]);
            //    CvInvoke.Rectangle(img2, rect, new MCvScalar(0,255,0), 2); //color BGR, and thikness

            //    //var circle = CvInvoke.MinEnclosingCircle(allContours[i]);
            //    //CvInvoke.Circle(img2, Point.Round(circle.Center), (int)(circle.Radius * 1.2), new MCvScalar(0, 0, 255), 2);//point is needed and it is pointF ,so we rounded to point

            //}

            //CvInvoke.Imwrite(@"D:\chrome stuff\multi\jeeda\sq.jpg", img2);
            //CvInvoke.Imwrite(@"D:\chrome stuff\multi\jeeda\circles.jpg", img2);





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
            this.Controls.Add(pictureBox1); // this referes to Form1, controls is the collection of UI elements like buttons lables .. placed on the form,Add(bla) adds bla to te collection so it shows when the form is shown
            this.Controls.Add(pictureBox2);

            //resize whole form to fit both images
            this.ClientSize = new Size(pictureBox2.Right + 10, Math.Max(pictureBox1.Height, pictureBox2.Height) + 20);


            //click handedling
            void box_click(object sender, MouseEventArgs e) // sender is the object that triggered the event — in this case, one of your two PictureBox controls.
            {
                PictureBox clickedBox = sender as PictureBox;
                if (clickedBox != null)
                {
                    Debug.WriteLine($"Clickedddddddddddd at ({e.X}, {e.Y}) in {clickedBox.Name}");
                    Point clickedPoint = e.Location;

                    for (int i = 0; i < allContours.Size; i++)
                    {
                        var contour = allContours[i];
                        Rectangle rect = CvInvoke.BoundingRectangle(contour);

                        int xpad = (int)(rect.Width * 0.15);
                        int ypad = (int)(rect.Height * 0.15);

                        Rectangle largRec = new Rectangle(rect.X - xpad, rect.Y - ypad, rect.Width + 2 * xpad, rect.Height + 2 * ypad);
                        largRec.Intersect(new Rectangle(0, 0, img2.Width, img2.Height));

                        if (largRec.Contains(clickedPoint))
                        {
                            CvInvoke.Rectangle(img2, largRec, new MCvScalar(0,255,0), 2 );
                            pictureBox2.Image = img2.ToBitmap();
                        }
                    }
                }
            }
        }
    }
}
