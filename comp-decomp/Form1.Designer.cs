namespace comp_decomp
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btncompress = new System.Windows.Forms.Button();
            this.btndecompress = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.tbinfo = new System.Windows.Forms.TextBox();
            this.pbdecompress = new System.Windows.Forms.PictureBox();
            this.pbprimary = new System.Windows.Forms.PictureBox();
            this.btnchoose = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tbinput = new System.Windows.Forms.TextBox();
            this.tboutput = new System.Windows.Forms.TextBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbdecompress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbprimary)).BeginInit();
            this.SuspendLayout();
            // 
            // btncompress
            // 
            this.btncompress.Location = new System.Drawing.Point(679, 107);
            this.btncompress.Name = "btncompress";
            this.btncompress.Size = new System.Drawing.Size(75, 23);
            this.btncompress.TabIndex = 1;
            this.btncompress.Text = "Compress";
            this.btncompress.UseVisualStyleBackColor = true;
            this.btncompress.Click += new System.EventHandler(this.btncompress_Click);
            // 
            // btndecompress
            // 
            this.btndecompress.Location = new System.Drawing.Point(679, 146);
            this.btndecompress.Name = "btndecompress";
            this.btndecompress.Size = new System.Drawing.Size(75, 23);
            this.btndecompress.TabIndex = 2;
            this.btndecompress.Text = "Decompress";
            this.btndecompress.UseVisualStyleBackColor = true;
            this.btndecompress.Click += new System.EventHandler(this.btndecompress_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // tbinfo
            // 
            this.tbinfo.Location = new System.Drawing.Point(23, 343);
            this.tbinfo.Multiline = true;
            this.tbinfo.Name = "tbinfo";
            this.tbinfo.Size = new System.Drawing.Size(1271, 49);
            this.tbinfo.TabIndex = 3;
            // 
            // pbdecompress
            // 
            this.pbdecompress.Location = new System.Drawing.Point(359, 22);
            this.pbdecompress.Name = "pbdecompress";
            this.pbdecompress.Size = new System.Drawing.Size(268, 301);
            this.pbdecompress.TabIndex = 5;
            this.pbdecompress.TabStop = false;
            // 
            // pbprimary
            // 
            this.pbprimary.Location = new System.Drawing.Point(23, 22);
            this.pbprimary.Name = "pbprimary";
            this.pbprimary.Size = new System.Drawing.Size(277, 301);
            this.pbprimary.TabIndex = 6;
            this.pbprimary.TabStop = false;
            // 
            // btnchoose
            // 
            this.btnchoose.Location = new System.Drawing.Point(679, 66);
            this.btnchoose.Name = "btnchoose";
            this.btnchoose.Size = new System.Drawing.Size(75, 23);
            this.btnchoose.TabIndex = 8;
            this.btnchoose.Text = "Choose";
            this.btnchoose.UseVisualStyleBackColor = true;
            this.btnchoose.Click += new System.EventHandler(this.btnchoose_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "RLE",
            "RLE(2)",
            "LZW",
            "HUFFMAN"});
            this.comboBox1.Location = new System.Drawing.Point(648, 22);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 9;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(696, 43);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(58, 17);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "bmp24";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // tbinput
            // 
            this.tbinput.Location = new System.Drawing.Point(775, 23);
            this.tbinput.Multiline = true;
            this.tbinput.Name = "tbinput";
            this.tbinput.Size = new System.Drawing.Size(252, 301);
            this.tbinput.TabIndex = 11;
            // 
            // tboutput
            // 
            this.tboutput.Location = new System.Drawing.Point(1033, 23);
            this.tboutput.Multiline = true;
            this.tboutput.Name = "tboutput";
            this.tboutput.Size = new System.Drawing.Size(252, 301);
            this.tboutput.TabIndex = 12;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(696, 191);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(47, 17);
            this.checkBox2.TabIndex = 13;
            this.checkBox2.Text = "Text";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1311, 404);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.tboutput);
            this.Controls.Add(this.tbinput);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnchoose);
            this.Controls.Add(this.pbprimary);
            this.Controls.Add(this.pbdecompress);
            this.Controls.Add(this.tbinfo);
            this.Controls.Add(this.btndecompress);
            this.Controls.Add(this.btncompress);
            this.Name = "Form1";
            this.Text = "Compress-Decompress";
            ((System.ComponentModel.ISupportInitialize)(this.pbdecompress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbprimary)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btncompress;
        private System.Windows.Forms.Button btndecompress;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.PictureBox pbdecompress;
        private System.Windows.Forms.PictureBox pbprimary;
        private System.Windows.Forms.Button btnchoose;
        public System.Windows.Forms.TextBox tbinfo;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox tbinput;
        private System.Windows.Forms.TextBox tboutput;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}



