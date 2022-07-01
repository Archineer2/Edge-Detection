namespace Project03
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Open_Image = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.OpenImageDisplay = new System.Windows.Forms.PictureBox();
            this.Detect_Edges = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OpenImageDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // Open_Image
            // 
            this.Open_Image.Location = new System.Drawing.Point(13, 13);
            this.Open_Image.Name = "Open_Image";
            this.Open_Image.Size = new System.Drawing.Size(100, 23);
            this.Open_Image.TabIndex = 0;
            this.Open_Image.Text = "Open_Image";
            this.Open_Image.UseVisualStyleBackColor = true;
            this.Open_Image.Click += new System.EventHandler(this.Open_Image_Click);
            // 
            // Exit
            // 
            this.Exit.Location = new System.Drawing.Point(12, 71);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(100, 23);
            this.Exit.TabIndex = 0;
            this.Exit.Text = "Exit";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // OpenImageDisplay
            // 
            this.OpenImageDisplay.Location = new System.Drawing.Point(120, 13);
            this.OpenImageDisplay.Name = "OpenImageDisplay";
            this.OpenImageDisplay.Size = new System.Drawing.Size(585, 300);
            this.OpenImageDisplay.TabIndex = 1;
            this.OpenImageDisplay.TabStop = false;
            // 
            // Detect_Edges
            // 
            this.Detect_Edges.Location = new System.Drawing.Point(12, 42);
            this.Detect_Edges.Name = "Detect_Edges";
            this.Detect_Edges.Size = new System.Drawing.Size(100, 23);
            this.Detect_Edges.TabIndex = 0;
            this.Detect_Edges.Text = "Detect_Edges";
            this.Detect_Edges.UseVisualStyleBackColor = true;
            this.Detect_Edges.Click += new System.EventHandler(this.Detect_Edges_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 325);
            this.Controls.Add(this.OpenImageDisplay);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.Detect_Edges);
            this.Controls.Add(this.Open_Image);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.OpenImageDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Open_Image;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.PictureBox OpenImageDisplay;
        private System.Windows.Forms.Button Detect_Edges;
    }
}

