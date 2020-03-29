namespace SpaghettiInstaller
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.quitButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.instructionLabel = new System.Windows.Forms.Label();
            this.finalDoodle = new System.Windows.Forms.PictureBox();
            this.alphyDoodle = new System.Windows.Forms.PictureBox();
            this.thankyouLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.finalDoodle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphyDoodle)).BeginInit();
            this.SuspendLayout();
            // 
            // quitButton
            // 
            this.quitButton.Location = new System.Drawing.Point(12, 327);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(75, 23);
            this.quitButton.TabIndex = 1;
            this.quitButton.Text = "Quit";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(502, 327);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(75, 23);
            this.nextButton.TabIndex = 2;
            this.nextButton.Text = "Next >";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // prevButton
            // 
            this.prevButton.Location = new System.Drawing.Point(421, 327);
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size(75, 23);
            this.prevButton.TabIndex = 3;
            this.prevButton.Text = "< Back";
            this.prevButton.UseVisualStyleBackColor = true;
            this.prevButton.Click += new System.EventHandler(this.prevButton_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(305, 12);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(191, 50);
            this.titleLabel.TabIndex = 4;
            this.titleLabel.Text = "Undertale Together\r\nby Depa";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // instructionLabel
            // 
            this.instructionLabel.AutoSize = true;
            this.instructionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instructionLabel.Location = new System.Drawing.Point(228, 93);
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.Size = new System.Drawing.Size(361, 160);
            this.instructionLabel.TabIndex = 5;
            this.instructionLabel.Text = resources.GetString("instructionLabel.Text");
            // 
            // finalDoodle
            // 
            this.finalDoodle.Image = global::SpaghettiInstaller.Properties.Resources.finalDoodle;
            this.finalDoodle.Location = new System.Drawing.Point(281, 65);
            this.finalDoodle.Name = "finalDoodle";
            this.finalDoodle.Size = new System.Drawing.Size(231, 130);
            this.finalDoodle.TabIndex = 6;
            this.finalDoodle.TabStop = false;
            this.finalDoodle.Visible = false;
            // 
            // alphyDoodle
            // 
            this.alphyDoodle.Image = global::SpaghettiInstaller.Properties.Resources.alphyDoodle;
            this.alphyDoodle.InitialImage = global::SpaghettiInstaller.Properties.Resources.alphyDoodle;
            this.alphyDoodle.Location = new System.Drawing.Point(12, 12);
            this.alphyDoodle.Name = "alphyDoodle";
            this.alphyDoodle.Size = new System.Drawing.Size(210, 308);
            this.alphyDoodle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.alphyDoodle.TabIndex = 0;
            this.alphyDoodle.TabStop = false;
            // 
            // thankyouLabel
            // 
            this.thankyouLabel.AutoSize = true;
            this.thankyouLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.thankyouLabel.Location = new System.Drawing.Point(228, 198);
            this.thankyouLabel.Name = "thankyouLabel";
            this.thankyouLabel.Size = new System.Drawing.Size(195, 91);
            this.thankyouLabel.TabIndex = 7;
            this.thankyouLabel.Text = "Success! Now, launch your game.\r\n\r\n$REPLACETEXT$\r\n\r\nMod by: Depa the Dog\r\nSpaghet" +
    "tiInstaller by: Nik the Cat\r\nArrivederci! (that\'s \"Goodbye!\" in Italian)";
            this.thankyouLabel.Visible = false;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 359);
            this.Controls.Add(this.thankyouLabel);
            this.Controls.Add(this.finalDoodle);
            this.Controls.Add(this.instructionLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.prevButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.alphyDoodle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(605, 398);
            this.MinimumSize = new System.Drawing.Size(605, 398);
            this.Name = "mainForm";
            this.Text = "Undertale Together\'s little C# Installer";
            ((System.ComponentModel.ISupportInitialize)(this.finalDoodle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphyDoodle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox alphyDoodle;
        private System.Windows.Forms.Button quitButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button prevButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label instructionLabel;
        private System.Windows.Forms.PictureBox finalDoodle;
        private System.Windows.Forms.Label thankyouLabel;
    }
}

