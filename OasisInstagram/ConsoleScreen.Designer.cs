namespace OasisInstagram
{
    partial class ConsoleScreen
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
            this.components = new System.ComponentModel.Container();
            this.consoleList = new System.Windows.Forms.ListView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // consoleList
            // 
            this.consoleList.BackColor = System.Drawing.SystemColors.Desktop;
            this.consoleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleList.ForeColor = System.Drawing.SystemColors.Window;
            this.consoleList.HideSelection = false;
            this.consoleList.Location = new System.Drawing.Point(0, 0);
            this.consoleList.Name = "consoleList";
            this.consoleList.Size = new System.Drawing.Size(800, 450);
            this.consoleList.TabIndex = 0;
            this.consoleList.UseCompatibleStateImageBehavior = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ConsoleScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.consoleList);
            this.Name = "ConsoleScreen";
            this.Text = "ConsoleScreen";
            this.Load += new System.EventHandler(this.ConsoleScreen_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView consoleList;
        private System.Windows.Forms.Timer timer1;
    }
}