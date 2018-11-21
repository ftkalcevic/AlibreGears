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

namespace GearsUI
{
    public partial class Form1 : Form
    {
        SpurGearSetInfo g = new SpurGearSetInfo();
        double zoom;
        bool gear1, gear2, mount;

        public Form1()
        {
            zoom = 1.0;

            InitializeComponent();

            panel1.MouseWheel += Panel1_MouseWheel;

            txtGear1Teeth.Text = "10";
            txtGear1Bore.Text = "5";
            txtGear1Thickness.Text = "5";
            txtGear2Teeth.Text = "20";
            txtGear2Bore.Text = "5";
            txtGear2Thickness.Text = "5";
            txtModule.Text = "1.5";
            txtPressureAngle.Text = "20";
            chkGear1.Checked = true;
            chkGear2.Checked = true;
            chkMountingPlate.Checked = true;

            CalculateParameters();
        }

        private void Panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            zoom += zoom * 0.02 * (float)e.Delta/(float)SystemInformation.MouseWheelScrollDelta;
            CalculateDisplayValues();
            panel1.Refresh();
        }

        private void CalculateParameters()
        {
            Int32 i=0;
            double d = 0;
            g.gear1.teeth = i.Parse(txtGear1Teeth.Text, 10);
            g.gear1.boreDiameter = d.Parse(txtGear1Bore.Text, 5);
            g.gear1.thickness = d.Parse(txtGear1Thickness.Text, 5);

            g.gear2.teeth = i.Parse(txtGear2Teeth.Text, 10);
            g.gear2.boreDiameter = d.Parse(txtGear2Bore.Text, 5);
            g.gear2.thickness = d.Parse(txtGear2Thickness.Text, 5);

            g.module = d.Parse(txtModule.Text,1.0);
            g.DP = d.Parse(txtDP.Text, defaultValue: 0.0);
            g.pressureAngle = d.Parse(txtPressureAngle.Text,20.0);
            g.CalculateParameters();

            gear1 = chkGear1.Checked;
            gear2 = chkGear2.Checked;
            mount = chkMountingPlate.Checked;

            SpurGears sg = new SpurGears();
            g.gear1.shapes = sg.createGearTooth(g.module, g.gear1.teeth, g.pressureAngle);
            g.gear2.shapes = sg.createGearTooth(g.module, g.gear2.teeth, g.pressureAngle);

            CalculateDisplayValues();

            txtCenterDistance.Text = g.centerDistance.ToString();
        }

        float scale, offsetX, offsetY;

        private void GearParametersChanged(object sender, EventArgs e)
        {
            CalculateParameters();
            panel1.Refresh();
        }

        private void btnAlibre_Click(object sender, EventArgs e)
        {
            AlibreSpurGears asg = new AlibreSpurGears();
            asg.Create("gear", g,gear1,gear2,mount);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            CalculateDisplayValues();
            panel1.Refresh();
        }

        private void CalculateDisplayValues()
        {
            double width = g.gear1.pitchCircle + g.gear2.pitchCircle + g.addendum * 2;
            double height = Math.Max(g.gear1.addendumCircle, g.gear2.addendumCircle);

            double displayWidth = panel1.Width * 0.9;
            double displayHeight = panel1.Height * 0.9;

            float scalex = (float)(displayWidth / width);
            float scaley = (float)(displayHeight / height);

            if (scalex > scaley)
                scale = scaley;
            else
                scale = scalex;
            scale *= (float)zoom;

            offsetX = (float)(width / 2);
            offsetY = (float)(height / 2);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            double x=0, y=0;

            Graphics gr = e.Graphics;

            //scale = 60;
            float zoom = scale;
            for (int gear = 0; gear < 2; gear++ )
            {
                List<Shape> shapes;
                int teeth;
                float xoffset, rotate, bore;
                if (gear == 0 )
                {
                    shapes = g.gear1.shapes;
                    teeth = g.gear1.teeth;
                    xoffset = -(float)(g.gear1.pitchCircle + g.gear2.pitchCircle )/4;
                    rotate = 360.0f / 2.0f / teeth;
                    bore = (float)g.gear1.boreDiameter;
                }
                else
                {
                    shapes = g.gear2.shapes;
                    teeth = g.gear2.teeth;
                    xoffset = (float)(g.gear1.pitchCircle + g.gear2.pitchCircle ) / 4;
                    rotate = 0;
                    bore = (float)g.gear2.boreDiameter;
                }
                gr.Transform = new System.Drawing.Drawing2D.Matrix();
                gr.ScaleTransform(scale, -scale);
                gr.TranslateTransform(offsetX+xoffset, -offsetY );
                gr.RotateTransform(rotate);
                

                Pen penLine = new Pen(Color.Black, (float)(1.0 / zoom));
                Pen penArc = new Pen(Color.Green, (float)(1.0 / zoom));
                Pen penArcLine = new Pen(Color.Green, (float)(1.0 / zoom));
                Pen penBez = new Pen(Color.Red, (float)(1.0 / zoom));
                Pen penRect = new Pen(Color.LightGreen, (float)(1.0 / zoom));
                Pen penCenter = new Pen(Color.Yellow, (float)(3.0 / zoom));

                gr.DrawEllipse(penArcLine, new RectangleF(-bore / 2, -bore / 2, bore, bore));

                for (int t = 0; t < teeth; t++)
                {
                    foreach (Shape s in shapes)
                    {
                        switch (s.GetType().Name)
                        {
                            case "MoveTo":
                                MoveTo mt = (MoveTo)s;
                                x = mt.x ;
                                y = mt.y;
                                break;
                            case "LineTo":
                                LineTo lt = (LineTo)s;
                                gr.DrawLine(penLine, new PointF((float)(x), (float)y), new PointF((float)(lt.x), (float)lt.y));
                                x = lt.x;
                                y = lt.y;
                                break;
                            case "Arc":
                                // Assuming circular arc
                                Arc a = (Arc)s;
                                System.Diagnostics.Debug.Assert(a.rx == a.ry);

                                // arc center -  https://stackoverflow.com/questions/36211171/finding-arc-circle-center-given-2-points-and-radius
                                double q = Math.Sqrt(Math.Pow((a.x - x), 2) + Math.Pow((a.y - y), 2));
                                double y3 = (y + a.y) / 2;
                                double x3 = (x + a.x) / 2;
                                double basex = Math.Sqrt(Math.Pow(a.rx, 2) - Math.Pow((q / 2), 2)) * (y - a.y) / q; //calculate once
                                double basey = Math.Sqrt(Math.Pow(a.rx, 2) - Math.Pow((q / 2), 2)) * (a.x - x) / q; //calculate once
                                double cx1 = x3 + basex; //center x of circle 1
                                double cy1 = y3 + basey; //center y of circle 1
                                double cx2 = x3 - basex; //center x of circle 2
                                double cy2 = y3 - basey; //center y of circle 2

                                void CalculateArcAngles(double centerX, double centerY, double x1, double y1, double x2, double y2, double rx, out double startAng, out double sweepAng)
                                {
                                    //if (x1 != x2)
                                    //{
                                    //    startAng = Math.Acos((x1 - centerX) / rx);
                                    //    sweepAng = Math.Acos((x2 - centerX) / rx);
                                    //}
                                    //else
                                    //{
                                    //    startAng = Math.Asin((y1 - centerY) / rx);
                                    //    sweepAng = Math.Asin((y2 - centerY) / rx);
                                    //}
                                    startAng = Math.Atan2(y1 - centerY, x1 - centerX);
                                    sweepAng = Math.Atan2(y2 - centerY, x2 - centerX);
                                }

                                double startAngle, sweepAngle;
                                double cx, cy;
                                CalculateArcAngles(cx1, cy1, x, y, a.x, a.y, a.rx, out startAngle, out sweepAngle);
                                if ( startAngle < sweepAngle )  // CCW
                                {
                                    if ( a.sweep )
                                    {
                                        cx = cx1; cy = cy1;
                                    }
                                    else
                                    {
                                        CalculateArcAngles(cx2, cy2, x, y, a.x, a.y, a.rx, out startAngle, out sweepAngle);
                                        cx = cx2; cy = cy2;
                                    }
                                }
                                else // CW
                                {
                                    if (!a.sweep)
                                    {
                                        cx = cx1; cy = cy1;
                                    }
                                    else
                                    {
                                        CalculateArcAngles(cx2, cy2, x, y, a.x, a.y, a.rx, out startAngle, out sweepAngle);
                                        cx = cx2; cy = cy2;
                                    }
                                }

                                float n = 5.0f / zoom;
                                //g.DrawLine(penCenter, (float)cx - n, (float)cy - n, (float)cx + n, (float)cy + n);
                                //g.DrawLine(penCenter, (float)cx - n, (float)cy + n, (float)cx + n, (float)cy - n);
                                // angles

                                startAngle = startAngle / Math.PI * 180;
                                sweepAngle = sweepAngle / Math.PI * 180;
                                sweepAngle = sweepAngle - startAngle;
                                if (sweepAngle > 180 && !a.largeArc)
                                    sweepAngle = sweepAngle - 360;
                                else if (sweepAngle < 180 && a.largeArc)
                                    sweepAngle = 360 - sweepAngle;
                                //startAngle -= 90.0;

                                // Find the bounding rectangle
                                RectangleF r = new RectangleF();
                                r.X = (float)(cx - a.rx);
                                r.Y = (float)(cy - a.rx);
                                r.Width = (float)a.rx * 2.0f;
                                r.Height = (float)a.ry * 2.0f;

                                gr.DrawArc(penArc, r, (float)startAngle, (float)sweepAngle);
                                //g.DrawRectangle(penRect, r.X, r.Y, r.Width,r.Height);
                                //g.DrawLine(penArcLine, new PointF((float)x, (float)y), new PointF((float)a.x, (float)a.y));
                                
                                x = a.x;
                                y = a.y;
                                break;
                            case "Bezier":
                                Bezier b = (Bezier)s;

                                PointF[] pts = new PointF[b.points.Count+1];
                                pts[0].X = (float)(x);
                                pts[0].Y = (float)y;
                                for (int i = 0; i < b.points.Count; i++)
                                {
                                    pts[i + 1].X = (float)(b.points[i].x);
                                    pts[i + 1].Y = (float)b.points[i].y;
                                }
                                gr.DrawBeziers(penBez, pts);

                                //foreach (var pt in b.points)
                                //{
                                //    gr.DrawLine(penLine, new PointF((float)x, (float)y), new PointF((float)pt.x, (float)pt.y));
                                //    x = pt.x;
                                //    y = pt.y;
                                //}
                                x = b.points.Last().x;
                                y = b.points.Last().y;
                                break;
                        }
                    }
                    gr.RotateTransform(360.0f / (float)teeth);
                }
            }
        }

    }
}
