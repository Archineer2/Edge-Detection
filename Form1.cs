using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project03
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Open_Image_Click(object sender, EventArgs e)
        // Opens a File Dialog that accepts only Image Files to open into the Picutre Box
        {
            OpenFileDialog imagefileopen = new OpenFileDialog();
            imagefileopen.Filter = "Image Files(*.jpg;*.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg; *.gif; *.bmp; *.png";
            if (imagefileopen.ShowDialog() == DialogResult.OK)
            {
                OpenImageDisplay.Image = new Bitmap(imagefileopen.FileName);
                OpenImageDisplay.Size = OpenImageDisplay.Image.Size;
            }
        }

        private Bitmap MakeGrayScale(Bitmap original)
        // Creates a Gray Scale Image of the passed Image
        {
            try
            {
                Bitmap newBitmap = new Bitmap(original.Width, original.Height);
                for (int i = 0; i < original.Width; i++)
                {
                    for (int j = 0; j < original.Height; j++)
                    {
                        // Get pixel of original image
                        Color originalColor = original.GetPixel(i, j);

                        // Create Grey Scale of each pixel
                        int grayScale = (int)((originalColor.R * 0.3) + (originalColor.G * .59) + (originalColor.B * .11));
                        Color newColor = Color.FromArgb(grayScale, grayScale, grayScale);
                        newBitmap.SetPixel(i, j, newColor);
                    }
                }
                return newBitmap;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        private Bitmap MakeSmoothImage(Bitmap original)
        // Makes a Smoothed Image from the passed Image
        {
            try
            {
                // Create new Bitmap which will be returned
                Bitmap newBitmap = new Bitmap(original.Width, original.Height);
                for (int i = 0; i < original.Width; i++)
                {
                    for (int j = 0; j < original.Height; j++)
                    {
                        // Get the 5x5 pixel area for each pixel in original
                        int[] SmoothArray = new int[25];
                        int SmoothArrayIndex = 0;
                        // If at edges, Mirror missing pixels from Target Pixel
                        for (int y = -2; y <= 2; y++)
                        {
                            for (int x = -2; x <= 2; x++)
                            {
                                // If the Pixel is on the Left/Right Edges
                                if ((i + x < 0) || (i + x >= original.Width))
                                {
                                    // If the Pixel is on the Top/Bottom Edges
                                    if ((j + y < 0) || (j + y >= original.Height))
                                    {
                                        // Obtain the value of the Double Mirrored Pixel
                                        Color originalColor = original.GetPixel((i - x), (j - y));
                                        SmoothArray[SmoothArrayIndex] = originalColor.R;
                                    }
                                    // If the Pixel is not near the Top/Bottom Edges
                                    else
                                    {
                                        // Obtain the value of the Horizontal Mirrored Pixel
                                        Color originalColor = original.GetPixel((i - x), j);
                                        SmoothArray[SmoothArrayIndex] = originalColor.R;
                                    }
                                }
                                // If the Pixel is not near the Left/Right Edges
                                else
                                {
                                    // If the Pixel is on the Top/Bottom Edges
                                    if ((j + y < 0) || (j + y > original.Height))
                                    {
                                        // Obtain the value of the Vertical Mirrored Pixel
                                        Color originalColor = original.GetPixel(i, (j - y));
                                        SmoothArray[SmoothArrayIndex] = originalColor.R;
                                    }
                                    // If the Pixel is not near the Top/Bottom Edges
                                    else
                                    {
                                        // Obtain the value of the Original Pixel
                                        Color originalColor = original.GetPixel(i, j);
                                        SmoothArray[SmoothArrayIndex] = originalColor.R;
                                    }
                                }
                                // Increment the array location from [0] - [24];
                                SmoothArrayIndex++;
                            }
                        }

                        // Multiply by Smoothing Matrix and find Sum
                        int SmoothValue = (SmoothArray[0] + SmoothArray[4] + SmoothArray[20] + SmoothArray[24]);
                        SmoothValue += (4 * (SmoothArray[1] + SmoothArray[3] + SmoothArray[5] + SmoothArray[9] + SmoothArray[15] + SmoothArray[19] + SmoothArray[21] + SmoothArray[13]));
                        SmoothValue += (7 * (SmoothArray[2] + SmoothArray[10] + SmoothArray[14] + SmoothArray[21]));
                        SmoothValue += (16 * (SmoothArray[6] + SmoothArray[8] + SmoothArray[16] + SmoothArray[18]));
                        SmoothValue += (26 * (SmoothArray[7] + SmoothArray[11] + SmoothArray[13] + SmoothArray[17]));
                        SmoothValue += (41 * SmoothArray[12]);

                        // Add 137 and Divide by 273 for Integer Arthimetic
                        SmoothValue += 137;
                        SmoothValue = SmoothValue / 273;

                        // Create Smooth Version of each pixel
                        Color newColor = Color.FromArgb(SmoothValue, SmoothValue, SmoothValue);
                        newBitmap.SetPixel(i, j, newColor);
                    }
                }
                return newBitmap;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        private Bitmap CalculateGradient(Bitmap original)
        {
            try
            {
                // Create array for Gradient(x), Gradient(y), and Manhattan Norm
                int[,,] GradientArray3D = new int[original.Width, original.Height, 3];
                for (int i = 0; i < original.Width; i++)
                {
                    for (int j = 0; j < original.Height; j++)
                    {
                        // Calculate the gradient for x
                        if((i - 1 < 0) || (i + 1 >= original.Width))
                        {
                            // If at edges, Gradient(x) should be 0
                            GradientArray3D[i, j, 0] = 0;
                        }
                        else
                        {
                            // Gradient(x) = [Y(i+1, j) - Y(i-1, j)] / 2
                            Color leftColor = original.GetPixel(i - 1, j);
                            Color rightColor = original.GetPixel(i + 1, j);
                            GradientArray3D[i, j, 0] = (rightColor.R - leftColor.R + 1) / 2;
                        }

                        // Calculate the gradient for y
                        if ((j - 1 < 0) || (j + 1 >= original.Height))
                        {
                            // If at edges, Gradient(y) should be 0
                            GradientArray3D[i, j, 1] = 0;
                        }
                        else
                        {
                            // Gradient(y) = [Y(i+1, j) - Y(i-1, j)] / 2
                            Color topColor = original.GetPixel(i, j - 1);
                            Color bottomColor = original.GetPixel(i, j + 1);
                            GradientArray3D[i, j, 1] = (bottomColor.R - topColor.R + 1) / 2;
                        }

                        // Calculate the Manhattan Norm
                        int tempX = GradientArray3D[i, j, 0];
                        int tempY = GradientArray3D[i, j, 1];
                        if (tempX < 0)
                        {
                            tempX = -tempX;
                        }
                        if (tempY < 0)
                        {
                            tempY = -tempY;
                        }
                        GradientArray3D[i, j, 2] = tempX + tempY;
                    }
                }

                // Obtain Non-Maximal Suppression and find edges with Thresholds
                
                Bitmap newBitmap = new Bitmap(original.Width, original.Height);
                newBitmap = NonMaximal(original, GradientArray3D);

                return newBitmap;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        private Bitmap NonMaximal(Bitmap original, int[, ,] GradientArray)
        {
            try
            {
                int[,] maximalArray = new int[original.Width, original.Height];
                int min = GradientArray[0, 0, 2];
                int max = GradientArray[0, 0, 2];
                
                for (int i = 0; i < original.Width; i++)
                {
                    for(int j = 0; j < original.Height; j++)
                    {
                        if(GradientArray[i, j, 0] == 0)
                        {
                            // if Grad(x) = 0, Grad(y) = 0
                            if(GradientArray[i, j, 1] == 0)
                            {
                                maximalArray[i, j] = 0;
                            }
                            // if Grad(x) = 0, Grad(y) is POS
                            else if (GradientArray[i, j, 1] > 0)
                            {
                                int before = GradientArray[i, j - 1, 2];
                                int here = GradientArray[i, j, 2];
                                int after = GradientArray[i, j + 1, 2];

                                // Check if Maximal
                                if((here > before) && (here > after))
                                {
                                    maximalArray[i, j] = 1;
                                }
                                else
                                {
                                    maximalArray[i, j] = 0;
                                }
                            }
                            // if Grad(x) = 0, Grad(y) is NEG
                            else
                            {
                                int before = GradientArray[i, j + 1, 2];
                                int here = GradientArray[i, j, 2];
                                int after = GradientArray[i, j - 1, 2];

                                // Check if Maximal
                                if ((here > before) && (here > after))
                                {
                                    maximalArray[i, j] = 1;
                                }
                                else
                                {
                                    maximalArray[i, j] = 0;
                                }
                            }
                        }
                        else if(GradientArray[i, j, 0] > 0)
                        {
                            // if Grad(x) is POS, Grad(y) = 0
                            if (GradientArray[i, j, 1] == 0)
                            {
                                int before = GradientArray[i - 1, j, 2];
                                int here = GradientArray[i, j, 2];
                                int after = GradientArray[i + 1, j, 2];

                                // Check if Maximal
                                if ((here > before) && (here > after))
                                {
                                    maximalArray[i, j] = 1;
                                }
                                else
                                {
                                    maximalArray[i, j] = 0;
                                }
                            }
                            // if Grad(x) is POS, Grad(y) is POS (Quadrant I)
                            else if (GradientArray[i, j, 1] > 0)
                            {
                                int before = 0;
                                int here = GradientArray[i, j, 2];
                                int after = 0;

                                // if 45 Degrees
                                if(GradientArray[i, j, 0] == GradientArray[i, j, 1])
                                {
                                    before = GradientArray[i - 1, j + 1, 2];
                                    after = GradientArray[i + 1, j - 1, 2];
                                }
                                // if 0 - 45 Degrees
                                else if(GradientArray[i, j, 0] > GradientArray[i, j, 1])
                                {
                                    before = (GradientArray[i - 1, j, 2] + GradientArray[i - 1, j + 1, 2] + 1) / 2;
                                    after = (GradientArray[i + 1, j, 2] + GradientArray[i + 1, j - 1, 2] + 1) / 2;
                                }
                                // if 45 - 90 Degrees
                                else
                                {
                                    before = (GradientArray[i, j + 1, 2] + GradientArray[i - 1, j + 1, 2] + 1) / 2;
                                    before = (GradientArray[i, j - 1, 2] + GradientArray[i + 1, j - 1, 2] + 1) / 2;
                                }

                                // Check if Maximal
                                if ((here > before) && (here > after))
                                {
                                    maximalArray[i, j] = 1;
                                }
                                else
                                {
                                    maximalArray[i, j] = 0;
                                }
                            }
                            // if Grad(x) is POS, Grad(y) is NEG (Quadrant IV)
                            else
                            {
                                int before = 0;
                                int here = GradientArray[i, j, 2];
                                int after = 0;

                                // if 315 Degrees
                                if (GradientArray[i, j, 0] == -GradientArray[i, j, 1])
                                {
                                    before = GradientArray[i - 1, j - 1, 2];
                                    after = GradientArray[i + 1, j + 1, 2];
                                }
                                // if 315 - 360 Degrees
                                else if (GradientArray[i, j, 0] > -GradientArray[i, j, 1])
                                {
                                    before = (GradientArray[i - 1, j, 2] + GradientArray[i - 1, j - 1, 2] + 1) / 2;
                                    after = (GradientArray[i + 1, j, 2] + GradientArray[i + 1, j + 1, 2] + 1) / 2;
                                }
                                // if 270 - 315 Degrees
                                else
                                {
                                    before = (GradientArray[i, j - 1, 2] + GradientArray[i - 1, j - 1, 2] + 1) / 2;
                                    before = (GradientArray[i, j + 1, 2] + GradientArray[i + 1, j + 1, 2] + 1) / 2;
                                }

                                // Check if Maximal
                                if ((here > before) && (here > after))
                                {
                                    maximalArray[i, j] = 1;
                                }
                                else
                                {
                                    maximalArray[i, j] = 0;
                                }
                            }
                        }
                        else
                        {
                            // if Grad(x) is NEG, Grad(y) = 0
                            if (GradientArray[i, j, 1] == 0)
                            {
                                int before = GradientArray[i + 1, j, 2];
                                int here = GradientArray[i, j, 2];
                                int after = GradientArray[i - 1, j, 2];

                                // Check if Maximal
                                if ((here > before) && (here > after))
                                {
                                    maximalArray[i, j] = 1;
                                }
                                else
                                {
                                    maximalArray[i, j] = 0;
                                }
                            }
                            // if Grad(x) is NEG, Grad(y) is POS (Quadrant II)
                            else if (GradientArray[i, j, 1] > 0)
                            {
                                int before = 0;
                                int here = GradientArray[i, j, 2];
                                int after = 0;

                                // if 135 Degrees
                                if (GradientArray[i, j, 0] == GradientArray[i, j, 1])
                                {
                                    before = GradientArray[i + 1, j + 1, 2];
                                    after = GradientArray[i - 1, j - 1, 2];
                                }
                                // if 135 - 180 Degrees
                                else if (-GradientArray[i, j, 0] > GradientArray[i, j, 1])
                                {
                                    before = (GradientArray[i + 1, j, 2] + GradientArray[i + 1, j + 1, 2] + 1) / 2;
                                    after = (GradientArray[i - 1, j, 2] + GradientArray[i - 1, j - 1, 2] + 1) / 2;
                                }
                                // if 90 - 135 Degrees
                                else
                                {
                                    before = (GradientArray[i, j + 1, 2] + GradientArray[i + 1, j + 1, 2] + 1) / 2;
                                    before = (GradientArray[i, j - 1, 2] + GradientArray[i - 1, j - 1, 2] + 1) / 2;
                                }

                                // Check if Maximal
                                if ((here > before) && (here > after))
                                {
                                    maximalArray[i, j] = 1;
                                }
                                else
                                {
                                    maximalArray[i, j] = 0;
                                }
                            }
                            // if Grad(x) is NEG, Grad(y) is NEG (Quadrant III)
                            else
                            {
                                int before = 0;
                                int here = GradientArray[i, j, 2];
                                int after = 0;

                                // if 225 Degrees
                                if (-GradientArray[i, j, 0] == -GradientArray[i, j, 1])
                                {
                                    before = GradientArray[i + 1, j - 1, 2];
                                    after = GradientArray[i - 1, j + 1, 2];
                                }
                                // if 180 - 225 Degrees
                                else if (-GradientArray[i, j, 0] > -GradientArray[i, j, 1])
                                {
                                    before = (GradientArray[i + 1, j, 2] + GradientArray[i + 1, j - 1, 2] + 1) / 2;
                                    after = (GradientArray[i - 1, j, 2] + GradientArray[i - 1, j + 1, 2] + 1) / 2;
                                }
                                // if 225 - 270 Degrees
                                else
                                {
                                    before = (GradientArray[i, j - 1, 2] + GradientArray[i + 1, j - 1, 2] + 1) / 2;
                                    before = (GradientArray[i, j + 1, 2] + GradientArray[i - 1, j + 1, 2] + 1) / 2;
                                }

                                // Check if Maximal
                                if ((here > before) && (here > after))
                                {
                                    maximalArray[i, j] = 1;
                                }
                                else
                                {
                                    maximalArray[i, j] = 0;
                                }
                            }
                        }

                        if(GradientArray[i, j, 2] < min)
                        {
                            min = GradientArray[i, j, 2];
                        }
                        if(GradientArray[i, j, 2] > max)
                        {
                            max = GradientArray[i, j, 2];
                        }
                    }
                }

                int range = max - min;
                int threshold = ((range * 9) + 5) / 10;
                
                Bitmap newBitmap = new Bitmap(original.Width, original.Height);

                for(int i = 0; i < original.Width; i++)
                {
                    for(int j = 0; j < original.Height; j++)
                    {
                        if(maximalArray[i, j] == 1)
                        {
                            if (GradientArray[i, j, 2] > max - threshold)
                            {
                                maximalArray[i, j] = 2;
                            }
                            else if (GradientArray[i, j, 2] < min + threshold)
                            {
                                maximalArray[i, j] = 0;
                            }
                        }
                    }
                }

                for (int i = 0; i < original.Width; i++)
                {
                    for (int j = 0; j < original.Height; j++)
                    {
                        if (maximalArray[i, j] == 2)
                        {
                            Threshold(newBitmap, maximalArray, i, j);
                        }
                    }
                }

                Color whiteColor = Color.FromArgb(255, 255, 255);
                Color blackColor = Color.FromArgb(0, 0, 0);

                for (int i = 0; i < original.Width; i++)
                {
                    for (int j = 0; j < original.Height; j++)
                    {
                        if(maximalArray[i, j] == 2)
                        {
                            newBitmap.SetPixel(i, j, whiteColor);
                        }
                        else
                        {
                            newBitmap.SetPixel(i, j, blackColor);
                        }
                    }
                }

                return newBitmap;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        private void Threshold(Bitmap newBitmap, int[,] maximalArray, int i, int j)
        {
            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    if((i + x >= 0) && (i + x <= newBitmap.Width - 1) && (j + y >= 0) && (j + y <= newBitmap.Height - 1))
                    {
                        if(maximalArray[i + x, j + y] == 1)
                        {
                            maximalArray[i + x, j + y] = 2;
                            Threshold(newBitmap, maximalArray, i + x, j + y);
                        }
                    }
                }
            }
        }

        private void Detect_Edges_Click(object sender, EventArgs e)
        // Creates an Image that shows the edges of the Image in the Picture Box
        {
            Form form2 = new Detect_Edges();
            Bitmap imageInstance = (Bitmap)OpenImageDisplay.Image;
            Bitmap imageInstance1 = new Bitmap(imageInstance.Width, imageInstance.Height);
            Bitmap imageInstance2 = new Bitmap(imageInstance.Width, imageInstance.Height);
            Bitmap imageInstance3 = new Bitmap(imageInstance.Width, imageInstance.Height);
            if (imageInstance != null)
            {
                imageInstance1 = MakeGrayScale(imageInstance);
                imageInstance2 = MakeSmoothImage(imageInstance1);
                imageInstance3 = CalculateGradient(imageInstance2);
                PictureBox tempPict = new PictureBox();
                tempPict.Size = imageInstance3.Size;
                form2.Controls.Add(tempPict);
                tempPict.Image = imageInstance3;
                form2.Show();
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        // Exits the current application
        {
            Application.Exit();
        }
    }
}
