# Edge-Detection
A Windows Forms App which performs Canny Edge Detection on the user given image

## Description
- The USER can input an image file with the file dialoge from the "Open_Image" button
- The "Detect_Edges" button will begin the Edge Detection
- > The input image will be converted to greyscale
- > Each pixel will have a smoothing algorithm applied
- > The gradient of each pixel will be calculated
- > Non-maximal suppression of the gradients will be applied
- > Maximal gradients that surpass the given threshold will be rendered as an edge
- > A black image with white edges will be returned in a new window

The C# code is in Form1.cs
