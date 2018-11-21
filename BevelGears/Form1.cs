using GearsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BevelGears
{
    public partial class Form1 : Form
    {
        BevelGearSetInfo g = new BevelGearSetInfo();
        double zoom;
        bool gear1, gear2, mount;

        public Form1()
        {
            InitializeComponent();

            zoom = 1.0;
            panel1.MouseWheel += Panel1_MouseWheel;

            txtGear1Teeth.Text = "20";
            txtGear1Bore.Text = "5";
            txtGear1HubDia.Text = "10";
            txtGear1HubLen.Text = "10";
            txtGear1Thickness.Text = "5";
            txtGear2Teeth.Text = "40";
            txtGear2Bore.Text = "5";
            txtGear2HubDia.Text = "10";
            txtGear2HubLen.Text = "10";
            txtGear2Thickness.Text = "5";
            txtModule.Text = "3";
            txtPressureAngle.Text = "20";
            txtShaftAngle.Text = "90";
            chkGear1.Checked = true;
            chkGear2.Checked = true;
            chkMountingPlate.Checked = true;

            CalculateParameters();
        }

        private void Panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            zoom += zoom * 0.02 * (float)e.Delta / (float)SystemInformation.MouseWheelScrollDelta;
            CalculateDisplayValues();
            panel1.Refresh();
        }

        private void CalculateParameters()
        {
            Int32 i = 0;
            double d = 0;
            g.gear1.teeth = i.Parse(txtGear1Teeth.Text, 10);
            g.gear1.boreDiameter = d.Parse(txtGear1Bore.Text, 5);
            g.gear1.hubDiameter = d.Parse(txtGear1HubDia.Text, 10);
            g.gear1.hubLen = d.Parse(txtGear1HubLen.Text, 10);
            g.gear1.thickness = d.Parse(txtGear1Thickness.Text, 5);

            g.gear2.teeth = i.Parse(txtGear2Teeth.Text, 10);
            g.gear2.boreDiameter = d.Parse(txtGear2Bore.Text, 5);
            g.gear2.hubDiameter = d.Parse(txtGear2HubDia.Text, 10);
            g.gear2.hubLen = d.Parse(txtGear2HubLen.Text, 10);
            g.gear2.thickness = d.Parse(txtGear2Thickness.Text, 5);

            g.module = d.Parse(txtModule.Text, 3.0);
            g.DP = d.Parse(txtDP.Text, defaultValue: 0.0);
            g.pressureAngle = d.Parse(txtPressureAngle.Text, 20.0);
            g.shaftAngle = d.Parse(txtShaftAngle.Text, 90.0);

            g.CalculateParameters();

            gear1 = chkGear1.Checked;
            gear2 = chkGear2.Checked;
            mount = chkMountingPlate.Checked;

            CalculateDisplayValues();
        }

        private void CalculateDisplayValues()
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            GearsLib.Point pt;

            float scale = 3;
            scale *= (float)zoom;

            gr.Transform = new System.Drawing.Drawing2D.Matrix();
            gr.ScaleTransform(scale, -scale);
            gr.TranslateTransform(100, -100);

            Pen penAxis = new Pen(Color.Red, 1.0f / scale);
            Pen penShaftAxis = new Pen(Color.Black, 1.0f / scale);
            Pen penConeAxis = new Pen(Color.Red, 1.0f / scale);
            penConeAxis.DashPattern = new float[] { 1f, 1f };
            Pen penCone = new Pen(Color.LightGreen, 1.0f / scale);
            Pen penGear = new Pen(Color.Black, 2.0f / scale);
            Pen penGearHidden = new Pen(Color.Black, 2.0f / scale);
            penGearHidden.DashPattern = new float[] { 1f, 2f };
            Pen penPink = new Pen(Color.Pink, 1.0f / scale);
            Pen penGreen = new Pen(Color.Green, 1.0f / scale);

            //// axis
            //gr.DrawLine(penAxis, 0.0f, 0.0f, 100f, 0f);
            //pt = new GearsLib.Point(100, 0);
            //pt = pt.Rotate(g.shaftAngle * Math.PI / 180.0);
            //gr.DrawLine(penAxis, new PointF(0.0f, 0.0f), (PointF)pt);

            // shaft axis
            pt = new GearsLib.Point(0, -100);
            gr.DrawLine(penShaftAxis, new PointF(0.0f, 0.0f), (PointF)pt);
            pt = new GearsLib.Point(0, -100);
            pt = pt.Rotate((g.gear1.coneAngle + g.gear2.coneAngle) * Math.PI / 180.0);
            gr.DrawLine(penShaftAxis, new PointF(0.0f, 0.0f), (PointF)pt);

            // cone axis
            pt = new GearsLib.Point(0,-100);
            pt = pt.Rotate(g.gear2.coneAngle * Math.PI / 180.0);
            gr.DrawLine(penConeAxis, new PointF(0.0f, 0.0f), (PointF)pt);

            // Gears
            for (int gear = 0; gear < 2; gear++)
            {
                BevelGearInfo gi, gi2;

                gr.Transform = new System.Drawing.Drawing2D.Matrix();
                gr.ScaleTransform(scale, -scale);
                gr.TranslateTransform(100, -100);
                if (gear == 0)
                {
                    gi = g.gear1;
                    gi2 = g.gear2;
                }
                else
                {
                    gi = g.gear2;
                    gi2 = g.gear1;
                    gr.RotateTransform(-90);
                }

                gr.DrawLine(penGear, gi.hubEnd.MirrorY(), gi.hubEnd);
                gr.DrawLine(penGear, gi.hubEnd, gi.hubBase);
                gr.DrawLine(penGear, gi.hubBase, gi.apexCorner);
                gr.DrawLine(penGear, gi.apexCorner, gi.apex);
                gr.DrawLine(penGear, gi.apex, gi.tip);
                gr.DrawLine(penGear, gi.tip, gi.inner);
                gr.DrawLine(penGear, gi.inner, gi.inner.MirrorY());
                gr.DrawLine(penGear, gi.tip.MirrorY(), gi.inner.MirrorY());
                gr.DrawLine(penGear, gi.apex.MirrorY(), gi.tip.MirrorY());
                gr.DrawLine(penGear, gi.apexCorner.MirrorY(), gi.apex.MirrorY());
                gr.DrawLine(penGear, gi.hubBase.MirrorY(), gi.apexCorner.MirrorY());
                gr.DrawLine(penGear, gi.hubEnd.MirrorY(), gi.hubBase.MirrorY());

                gr.DrawLine(penPink, (float)(g.module * gi2.teeth / 2), (float)(-g.module * gi.teeth / 2), (float)(g.module * gi2.teeth / 2), (float)(g.module * gi.teeth / 2));
                gr.DrawLine(penGreen, (float)(gi.pitchApexToCrown),(float)(gi.pitchApexToCrownY), (float)(gi.pitchApexToCrown), (float)(-gi.pitchApexToCrownY));

                gr.DrawRectangle(penGearHidden, gi.inner.X, -(float)(gi.boreDiameter / 2), gi.hubEnd.X - gi.inner.X, (float)gi.boreDiameter);
                //break;
            }
        }

        private void GearParametersChanged(object sender, EventArgs e)
        {
            CalculateParameters();
            panel1.Refresh();
        }

        private void btnAlibre_Click(object sender, EventArgs e)
        {
            AlibreBevelGears abg = new AlibreBevelGears();
            abg.Create("gear", g, gear1, gear2, mount);
        }
    }
}


// https://khkgears.net/new/gear_knowledge/gear_technical_reference/calculation_gear_dimensions.html