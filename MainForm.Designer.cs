namespace VTT2TXT
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Btn_SetPath = new Button();
            Lbl_Path = new Label();
            Lbx_Files = new ListBox();
            Tbx_Filter = new TextBox();
            Btn_Filter = new Button();
            Btn_ConvSelected = new Button();
            Btn_ConvAll = new Button();
            orderByName = new RadioButton();
            orderByCreateTm = new RadioButton();
            label1 = new Label();
            SuspendLayout();
            // 
            // Btn_SetPath
            // 
            Btn_SetPath.Font = new Font("Cascadia Mono", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            Btn_SetPath.Location = new Point(12, 12);
            Btn_SetPath.Name = "Btn_SetPath";
            Btn_SetPath.Size = new Size(85, 44);
            Btn_SetPath.TabIndex = 0;
            Btn_SetPath.Text = "Path:";
            Btn_SetPath.UseVisualStyleBackColor = true;
            Btn_SetPath.Click += Btn_SetPath_Click;
            // 
            // Lbl_Path
            // 
            Lbl_Path.Font = new Font("Cascadia Mono", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Lbl_Path.Location = new Point(103, 12);
            Lbl_Path.Name = "Lbl_Path";
            Lbl_Path.Size = new Size(1242, 44);
            Lbl_Path.TabIndex = 3;
            Lbl_Path.Text = "C:\\Default_Path\\WhereExeExist";
            Lbl_Path.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Lbx_Files
            // 
            Lbx_Files.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Lbx_Files.FormattingEnabled = true;
            Lbx_Files.ItemHeight = 17;
            Lbx_Files.Items.AddRange(new object[] { "xxx1.vtt", "xxx2.srt" });
            Lbx_Files.Location = new Point(103, 111);
            Lbx_Files.Name = "Lbx_Files";
            Lbx_Files.SelectionMode = SelectionMode.MultiExtended;
            Lbx_Files.Size = new Size(1242, 140);
            Lbx_Files.TabIndex = 4;
            // 
            // Tbx_Filter
            // 
            Tbx_Filter.Font = new Font("Cascadia Mono", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Tbx_Filter.Location = new Point(103, 52);
            Tbx_Filter.Name = "Tbx_Filter";
            Tbx_Filter.Size = new Size(593, 26);
            Tbx_Filter.TabIndex = 6;
            Tbx_Filter.Text = "*.vtt *.srt";
            // 
            // Btn_Filter
            // 
            Btn_Filter.Font = new Font("Cascadia Mono", 13F, FontStyle.Regular, GraphicsUnit.Point);
            Btn_Filter.Location = new Point(12, 68);
            Btn_Filter.Name = "Btn_Filter";
            Btn_Filter.Size = new Size(85, 44);
            Btn_Filter.TabIndex = 7;
            Btn_Filter.Text = "Filter";
            Btn_Filter.UseVisualStyleBackColor = true;
            Btn_Filter.Click += Btn_Filter_Click;
            // 
            // Btn_ConvSelected
            // 
            Btn_ConvSelected.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Btn_ConvSelected.Location = new Point(12, 128);
            Btn_ConvSelected.Name = "Btn_ConvSelected";
            Btn_ConvSelected.Size = new Size(85, 60);
            Btn_ConvSelected.TabIndex = 8;
            Btn_ConvSelected.Text = "Convert selected";
            Btn_ConvSelected.UseVisualStyleBackColor = true;
            Btn_ConvSelected.Click += Btn_ConvSelected_Click;
            // 
            // Btn_ConvAll
            // 
            Btn_ConvAll.Font = new Font("Cascadia Mono", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Btn_ConvAll.Location = new Point(12, 203);
            Btn_ConvAll.Name = "Btn_ConvAll";
            Btn_ConvAll.Size = new Size(85, 44);
            Btn_ConvAll.TabIndex = 9;
            Btn_ConvAll.Text = "Cvt. All";
            Btn_ConvAll.UseVisualStyleBackColor = true;
            Btn_ConvAll.Click += Btn_ConvAll_Click;
            // 
            // orderByName
            // 
            orderByName.AutoSize = true;
            orderByName.Location = new Point(288, 86);
            orderByName.Name = "orderByName";
            orderByName.Size = new Size(71, 19);
            orderByName.TabIndex = 10;
            orderByName.Text = "檔名 Asc";
            orderByName.UseVisualStyleBackColor = true;
            orderByName.CheckedChanged += Btn_Filter_Click;
            // 
            // orderByCreateTm
            // 
            orderByCreateTm.AutoSize = true;
            orderByCreateTm.Checked = true;
            orderByCreateTm.Location = new Point(182, 86);
            orderByCreateTm.Name = "orderByCreateTm";
            orderByCreateTm.Size = new Size(103, 19);
            orderByCreateTm.TabIndex = 11;
            orderByCreateTm.TabStop = true;
            orderByCreateTm.Text = "建立時間 Desc";
            orderByCreateTm.UseVisualStyleBackColor = true;
            orderByCreateTm.CheckedChanged += Btn_Filter_Click;
            // 
            // label1
            // 
            label1.Font = new Font("Cascadia Mono", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(105, 76);
            label1.Name = "label1";
            label1.Size = new Size(82, 37);
            label1.TabIndex = 12;
            label1.Text = "Orderby";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PaleGreen;
            ClientSize = new Size(1357, 257);
            Controls.Add(orderByCreateTm);
            Controls.Add(orderByName);
            Controls.Add(Tbx_Filter);
            Controls.Add(Lbx_Files);
            Controls.Add(label1);
            Controls.Add(Btn_ConvAll);
            Controls.Add(Btn_ConvSelected);
            Controls.Add(Btn_Filter);
            Controls.Add(Lbl_Path);
            Controls.Add(Btn_SetPath);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "VTT2TXT By CHL";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Btn_SetPath;
        private Label Lbl_Path;
        private ListBox Lbx_Files;
        private TextBox Tbx_Filter;
        private Button Btn_Filter;
        private Button Btn_ConvSelected;
        private Button Btn_ConvAll;
        private RadioButton orderByName;
        private RadioButton orderByCreateTm;
        private Label label1;
    }
}
