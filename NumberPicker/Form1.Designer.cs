namespace NumberPicker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            rollingNumberLabel = new Label();
            startStopButton = new Button();
            minValueTextBox = new TextBox();
            maxValueTextBox = new TextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            title = new Label();
            SuspendLayout();
            // 
            // rollingNumberLabel
            // 
            rollingNumberLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rollingNumberLabel.Font = new Font("微软雅黑", 48F, FontStyle.Bold, GraphicsUnit.Point);
            rollingNumberLabel.Location = new Point(360, 180);
            rollingNumberLabel.Name = "rollingNumberLabel";
            rollingNumberLabel.Size = new Size(600, 380);
            rollingNumberLabel.TabIndex = 0;
            rollingNumberLabel.Text = "null";
            rollingNumberLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // startStopButton
            // 
            startStopButton.Location = new Point(130, 500);
            startStopButton.Name = "startStopButton";
            startStopButton.Size = new Size(160, 90);
            startStopButton.TabIndex = 1;
            startStopButton.Text = "start";
            startStopButton.UseVisualStyleBackColor = true;
            startStopButton.Click += startStopButton_Click;
            // 
            // minValueTextBox
            // 
            minValueTextBox.Location = new Point(120, 180);
            minValueTextBox.Name = "minValueTextBox";
            minValueTextBox.Size = new Size(200, 100);
            minValueTextBox.TabIndex = 2;
            // 
            // maxValueTextBox
            // 
            maxValueTextBox.Location = new Point(120, 350);
            maxValueTextBox.Name = "maxValueTextBox";
            maxValueTextBox.Size = new Size(200, 100);
            maxValueTextBox.TabIndex = 3;
            // 
            // title
            // 
            title.Dock = DockStyle.Top;
            title.Font = new Font("微软雅黑", 48F, FontStyle.Bold, GraphicsUnit.Point);
            title.Location = new Point(0, 0);
            title.Name = "title";
            title.Size = new Size(1024, 180);
            title.TabIndex = 4;
            title.Text = "Number Picker";
            title.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            ClientSize = new Size(1024, 720);
            Controls.Add(title);
            Controls.Add(maxValueTextBox);
            Controls.Add(minValueTextBox);
            Controls.Add(startStopButton);
            Controls.Add(rollingNumberLabel);
            Name = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label rollingNumberLabel;
        private Button startStopButton;
        private TextBox minValueTextBox;
        private TextBox maxValueTextBox;
        private System.Windows.Forms.Timer timer1;
        private Label title;
    }
}