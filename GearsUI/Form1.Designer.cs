namespace GearsUI
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCenterDistance = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkMountingPlate = new System.Windows.Forms.CheckBox();
            this.chkGear2 = new System.Windows.Forms.CheckBox();
            this.chkGear1 = new System.Windows.Forms.CheckBox();
            this.btnAlibre = new System.Windows.Forms.Button();
            this.txtPressureAngle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtModule = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbGear2 = new System.Windows.Forms.GroupBox();
            this.txtGear2Thickness = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtGear2Bore = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtGear2Teeth = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gbGear1 = new System.Windows.Forms.GroupBox();
            this.txtGear1Bore = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtGear1Thickness = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGear1Teeth = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.gbGear2.SuspendLayout();
            this.gbGear1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(812, 426);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtCenterDistance);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.chkMountingPlate);
            this.groupBox1.Controls.Add(this.chkGear2);
            this.groupBox1.Controls.Add(this.chkGear1);
            this.groupBox1.Controls.Add(this.btnAlibre);
            this.groupBox1.Controls.Add(this.txtPressureAngle);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtDP);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtModule);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.gbGear2);
            this.groupBox1.Controls.Add(this.gbGear1);
            this.groupBox1.Location = new System.Drawing.Point(12, 445);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(812, 119);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gear Info";
            // 
            // txtCenterDistance
            // 
            this.txtCenterDistance.Location = new System.Drawing.Point(706, 43);
            this.txtCenterDistance.Name = "txtCenterDistance";
            this.txtCenterDistance.ReadOnly = true;
            this.txtCenterDistance.Size = new System.Drawing.Size(100, 20);
            this.txtCenterDistance.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(641, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Center Dist";
            // 
            // chkMountingPlate
            // 
            this.chkMountingPlate.AutoSize = true;
            this.chkMountingPlate.Location = new System.Drawing.Point(503, 85);
            this.chkMountingPlate.Name = "chkMountingPlate";
            this.chkMountingPlate.Size = new System.Drawing.Size(97, 17);
            this.chkMountingPlate.TabIndex = 14;
            this.chkMountingPlate.Text = "Mounting Plate";
            this.chkMountingPlate.UseVisualStyleBackColor = true;
            this.chkMountingPlate.Click += new System.EventHandler(this.GearParametersChanged);
            // 
            // chkGear2
            // 
            this.chkGear2.AutoSize = true;
            this.chkGear2.Location = new System.Drawing.Point(503, 65);
            this.chkGear2.Name = "chkGear2";
            this.chkGear2.Size = new System.Drawing.Size(58, 17);
            this.chkGear2.TabIndex = 13;
            this.chkGear2.Text = "Gear 2";
            this.chkGear2.UseVisualStyleBackColor = true;
            this.chkGear2.Click += new System.EventHandler(this.GearParametersChanged);
            // 
            // chkGear1
            // 
            this.chkGear1.AutoSize = true;
            this.chkGear1.Location = new System.Drawing.Point(503, 45);
            this.chkGear1.Name = "chkGear1";
            this.chkGear1.Size = new System.Drawing.Size(58, 17);
            this.chkGear1.TabIndex = 12;
            this.chkGear1.Text = "Gear 1";
            this.chkGear1.UseVisualStyleBackColor = true;
            this.chkGear1.Click += new System.EventHandler(this.GearParametersChanged);
            // 
            // btnAlibre
            // 
            this.btnAlibre.Location = new System.Drawing.Point(503, 15);
            this.btnAlibre.Name = "btnAlibre";
            this.btnAlibre.Size = new System.Drawing.Size(91, 23);
            this.btnAlibre.TabIndex = 11;
            this.btnAlibre.Text = "Alibre Design";
            this.btnAlibre.UseVisualStyleBackColor = true;
            this.btnAlibre.Click += new System.EventHandler(this.btnAlibre_Click);
            // 
            // txtPressureAngle
            // 
            this.txtPressureAngle.Location = new System.Drawing.Point(380, 69);
            this.txtPressureAngle.Name = "txtPressureAngle";
            this.txtPressureAngle.Size = new System.Drawing.Size(100, 20);
            this.txtPressureAngle.TabIndex = 10;
            this.txtPressureAngle.TextChanged += new System.EventHandler(this.GearParametersChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(315, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Press. Angle";
            // 
            // txtDP
            // 
            this.txtDP.Location = new System.Drawing.Point(380, 43);
            this.txtDP.Name = "txtDP";
            this.txtDP.Size = new System.Drawing.Size(100, 20);
            this.txtDP.TabIndex = 8;
            this.txtDP.TextChanged += new System.EventHandler(this.GearParametersChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(315, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "DP";
            // 
            // txtModule
            // 
            this.txtModule.Location = new System.Drawing.Point(380, 17);
            this.txtModule.Name = "txtModule";
            this.txtModule.Size = new System.Drawing.Size(100, 20);
            this.txtModule.TabIndex = 6;
            this.txtModule.TextChanged += new System.EventHandler(this.GearParametersChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(315, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Module";
            // 
            // gbGear2
            // 
            this.gbGear2.Controls.Add(this.txtGear2Thickness);
            this.gbGear2.Controls.Add(this.label9);
            this.gbGear2.Controls.Add(this.txtGear2Bore);
            this.gbGear2.Controls.Add(this.label8);
            this.gbGear2.Controls.Add(this.txtGear2Teeth);
            this.gbGear2.Controls.Add(this.label5);
            this.gbGear2.Location = new System.Drawing.Point(163, 19);
            this.gbGear2.Name = "gbGear2";
            this.gbGear2.Size = new System.Drawing.Size(151, 98);
            this.gbGear2.TabIndex = 4;
            this.gbGear2.TabStop = false;
            this.gbGear2.Text = "Gear 2";
            // 
            // txtGear2Thickness
            // 
            this.txtGear2Thickness.Location = new System.Drawing.Point(63, 43);
            this.txtGear2Thickness.Name = "txtGear2Thickness";
            this.txtGear2Thickness.Size = new System.Drawing.Size(83, 20);
            this.txtGear2Thickness.TabIndex = 1;
            this.txtGear2Thickness.TextChanged += new System.EventHandler(this.GearParametersChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Thickness";
            // 
            // txtGear2Bore
            // 
            this.txtGear2Bore.Location = new System.Drawing.Point(63, 67);
            this.txtGear2Bore.Name = "txtGear2Bore";
            this.txtGear2Bore.Size = new System.Drawing.Size(83, 20);
            this.txtGear2Bore.TabIndex = 2;
            this.txtGear2Bore.TextChanged += new System.EventHandler(this.GearParametersChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Bore";
            // 
            // txtGear2Teeth
            // 
            this.txtGear2Teeth.Location = new System.Drawing.Point(63, 19);
            this.txtGear2Teeth.Name = "txtGear2Teeth";
            this.txtGear2Teeth.Size = new System.Drawing.Size(83, 20);
            this.txtGear2Teeth.TabIndex = 0;
            this.txtGear2Teeth.TextChanged += new System.EventHandler(this.GearParametersChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Teeth";
            // 
            // gbGear1
            // 
            this.gbGear1.Controls.Add(this.txtGear1Bore);
            this.gbGear1.Controls.Add(this.label7);
            this.gbGear1.Controls.Add(this.txtGear1Thickness);
            this.gbGear1.Controls.Add(this.label6);
            this.gbGear1.Controls.Add(this.txtGear1Teeth);
            this.gbGear1.Controls.Add(this.label4);
            this.gbGear1.Location = new System.Drawing.Point(6, 19);
            this.gbGear1.Name = "gbGear1";
            this.gbGear1.Size = new System.Drawing.Size(151, 98);
            this.gbGear1.TabIndex = 3;
            this.gbGear1.TabStop = false;
            this.gbGear1.Text = "Gear 1";
            // 
            // txtGear1Bore
            // 
            this.txtGear1Bore.Location = new System.Drawing.Point(63, 67);
            this.txtGear1Bore.Name = "txtGear1Bore";
            this.txtGear1Bore.Size = new System.Drawing.Size(82, 20);
            this.txtGear1Bore.TabIndex = 12;
            this.txtGear1Bore.TextChanged += new System.EventHandler(this.GearParametersChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Bore";
            // 
            // txtGear1Thickness
            // 
            this.txtGear1Thickness.Location = new System.Drawing.Point(63, 42);
            this.txtGear1Thickness.Name = "txtGear1Thickness";
            this.txtGear1Thickness.Size = new System.Drawing.Size(82, 20);
            this.txtGear1Thickness.TabIndex = 10;
            this.txtGear1Thickness.TextChanged += new System.EventHandler(this.GearParametersChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Thickness";
            // 
            // txtGear1Teeth
            // 
            this.txtGear1Teeth.Location = new System.Drawing.Point(63, 17);
            this.txtGear1Teeth.Name = "txtGear1Teeth";
            this.txtGear1Teeth.Size = new System.Drawing.Size(82, 20);
            this.txtGear1Teeth.TabIndex = 8;
            this.txtGear1Teeth.TextChanged += new System.EventHandler(this.GearParametersChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Teeth";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 576);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Spur Gears";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbGear2.ResumeLayout(false);
            this.gbGear2.PerformLayout();
            this.gbGear1.ResumeLayout(false);
            this.gbGear1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPressureAngle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtModule;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbGear2;
        private System.Windows.Forms.TextBox txtGear2Teeth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox gbGear1;
        private System.Windows.Forms.TextBox txtGear1Teeth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAlibre;
        private System.Windows.Forms.CheckBox chkMountingPlate;
        private System.Windows.Forms.CheckBox chkGear2;
        private System.Windows.Forms.CheckBox chkGear1;
        private System.Windows.Forms.TextBox txtGear2Thickness;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtGear2Bore;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtGear1Bore;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtGear1Thickness;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCenterDistance;
        private System.Windows.Forms.Label label10;
    }
}

