
namespace GameCaro
{
    partial class StartGame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartGame));
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.PictureBox();
            this.prbLoading = new System.Windows.Forms.ProgressBar();
            this.txtLoading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStart)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Image = global::GameCaro.Properties.Resources.close;
            this.btnClose.Location = new System.Drawing.Point(572, 597);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 50);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnClose.TabIndex = 1;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.pcbClose_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnStart.Image = global::GameCaro.Properties.Resources.start1;
            this.btnStart.Location = new System.Drawing.Point(291, 597);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(150, 50);
            this.btnStart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnStart.TabIndex = 0;
            this.btnStart.TabStop = false;
            this.btnStart.Click += new System.EventHandler(this.pcbStart_Click);
            // 
            // prbLoading
            // 
            this.prbLoading.Location = new System.Drawing.Point(237, 614);
            this.prbLoading.Name = "prbLoading";
            this.prbLoading.Size = new System.Drawing.Size(552, 18);
            this.prbLoading.TabIndex = 2;
            // 
            // txtLoading
            // 
            this.txtLoading.AutoSize = true;
            this.txtLoading.BackColor = System.Drawing.Color.Transparent;
            this.txtLoading.Font = new System.Drawing.Font("VNI-Franko", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.txtLoading.Location = new System.Drawing.Point(467, 576);
            this.txtLoading.Name = "txtLoading";
            this.txtLoading.Size = new System.Drawing.Size(84, 20);
            this.txtLoading.TabIndex = 3;
            this.txtLoading.Text = "Loading: 0%";
            // 
            // StartGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GameCaro.Properties.Resources.king_of_sky_1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1026, 751);
            this.Controls.Add(this.txtLoading);
            this.Controls.Add(this.prbLoading);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1048, 802);
            this.MinimumSize = new System.Drawing.Size(1048, 802);
            this.Name = "StartGame";
            this.Text = "StartGame";
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnStart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox btnStart;
        private System.Windows.Forms.ProgressBar prbLoading;
        private System.Windows.Forms.Label txtLoading;
    }
}