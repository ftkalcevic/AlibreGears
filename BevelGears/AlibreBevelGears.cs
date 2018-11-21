#define KNOTS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlibreX;
using System.Runtime.InteropServices;
using GearsLib;

namespace BevelGears
{
    class AlibreBevelGears
    {
        public void Create(string name, BevelGearSetInfo gears, bool createGear1, bool createGear2, bool createMount)
        {
            IAutomationHook hook = null;
            try
            {
                hook = (IAutomationHook)Marshal.GetActiveObject("AlibreX.AutomationHook");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to connect to Alibre Design.  Is it running?", e);
            }

            IADRoot root = (IADRoot)hook.Root;
            IADAssemblySession assembly = root.CreateEmptyAssembly(name);

            IADOccurrence gear1 = null, gear2 = null, mount = null;
            if (createGear1) gear1 = CreateGear(assembly, "Gear1 M" + gears.module.ToString() + " T", gears, gears.gear1);
            //if (createGear2) gear2 = CreateGear(assembly, "Gear2 M" + gears.module.ToString() + " T", gears, gears.gear2);
            //if (createMount)
            //{
            //    mount = CreateMount(assembly, "Mounting Plate", gears.centerDistance, gears.gear1.boreDiameter, gears.gear2.boreDiameter, Math.Max(gears.gear1.thickness, gears.gear2.thickness));
            //    mount.IsAnchored = true;
            //    if (createGear1)
            //    {
            //        assembly.AssemblyConstraints.AddConstraint(assembly.NewTargetProxy(gear1, gear1.DesignSession.DesignPlanes.Item("XY-Plane")),
            //                                                    assembly.NewTargetProxy(mount, mount.DesignSession.DesignPlanes.Item("XY-Plane")),
            //                                                    ADAssemblyConstraintType.AD_ALIGN_TYPE);
            //        assembly.AssemblyConstraints.AddConstraint(assembly.NewTargetProxy(gear1, gear1.DesignSession.DesignAxes.Item("Z-Axis")),
            //                                                    assembly.NewTargetProxy(mount, mount.DesignSession.DesignAxes.Item("Gear1")),
            //                                                    ADAssemblyConstraintType.AD_ALIGN_TYPE);
            //    }
            //    if (createGear2)
            //    {
            //        assembly.AssemblyConstraints.AddConstraint(assembly.NewTargetProxy(gear2, gear2.DesignSession.DesignPlanes.Item("XY-Plane")),
            //                                                    assembly.NewTargetProxy(mount, mount.DesignSession.DesignPlanes.Item("XY-Plane")),
            //                                                    ADAssemblyConstraintType.AD_ALIGN_TYPE);
            //        assembly.AssemblyConstraints.AddConstraint(assembly.NewTargetProxy(gear2, gear2.DesignSession.DesignAxes.Item("Z-Axis")),
            //                                                    assembly.NewTargetProxy(mount, mount.DesignSession.DesignAxes.Item("Gear2")),
            //                                                    ADAssemblyConstraintType.AD_ALIGN_TYPE);
            //    }
            //}
        }

        private Point Rotate(Point pt, double rads)  // rotate pt by rads radians about origin
        {
            var sinA = Math.Sin(rads);
            var cosA = Math.Cos(rads);

            return new Point(pt.x * cosA - pt.y * sinA, pt.x * sinA + pt.y * cosA);
        }

        private Point Rotate(double x, double y, double rads)  // rotate pt by rads radians about origin
        {
            return Rotate(new Point(x, y), rads);
        }

        private IADOccurrence CreateGear(IADAssemblySession assembly, string name, BevelGearSetInfo gears, BevelGearInfo gear)
        {
            IADTransformation trans = assembly.GeometryFactory.CreateIdentityTransform();
            IADOccurrence occurrence = assembly.RootOccurrence.Occurrences.AddEmptyPart(name, isSheetMetal: false, pTransform: trans);
            IADPartSession part = (IADPartSession)occurrence.DesignSession;


            IADDesignPlane xyPlane = part.DesignPlanes.Item("XY-Plane");
            IADSketch sketch = part.Sketches.AddSketch(null, part.DesignPlanes.Item("XY-Plane"), "Gear Blank");

            sketch.BeginChange();
            sketch.Figures.AddLine(MMToCM(gear.hubEnd.X), MMToCM(0), MMToCM(gear.hubEnd.X), MMToCM(gear.hubEnd.Y));
            sketch.Figures.AddLine(MMToCM(gear.hubEnd.X), MMToCM(gear.hubEnd.Y), MMToCM(gear.hubBase.X), MMToCM(gear.hubBase.Y));
            sketch.Figures.AddLine(MMToCM(gear.hubBase.X), MMToCM(gear.hubBase.Y), MMToCM(gear.apexCorner.X), MMToCM(gear.apexCorner.Y));
            sketch.Figures.AddLine(MMToCM(gear.apexCorner.X), MMToCM(gear.apexCorner.Y), MMToCM(gear.apex.X), MMToCM(gear.apex.Y));
            sketch.Figures.AddLine(MMToCM(gear.apex.X), MMToCM(gear.apex.Y), MMToCM(gear.tip.X), MMToCM(gear.tip.Y));
            sketch.Figures.AddLine(MMToCM(gear.tip.X), MMToCM(gear.tip.Y), MMToCM(gear.inner.X), MMToCM(gear.inner.Y));
            sketch.Figures.AddLine(MMToCM(gear.inner.X), MMToCM(gear.inner.Y), MMToCM(gear.inner.X), MMToCM(0));
            sketch.Figures.AddLine(MMToCM(gear.inner.X), MMToCM(0), MMToCM(gear.hubEnd.X), MMToCM(0));
            sketch.EndChange();

            IADDesignAxis xAxis = part.DesignAxes.Item("X-Axis");
            part.Features.AddRevolvedBoss(sketch, null, xAxis, 2 * Math.PI, "Revolve Gear Blank");

            //sketch.BeginChange();
            //for (int t = 0; t < teeth; t++)
            //{
            //    double x = 0, y = 0;
            //    double rotate = 2.0 * Math.PI * (double)t / (double)teeth;
            //    foreach (Shape s in shapes)
            //    {
            //        switch (s.GetType().Name)
            //        {
            //            case "MoveTo":
            //                {
            //                    MoveTo mt = (MoveTo)s;
            //                    x = mt.x;
            //                    y = mt.y;
            //                    break;
            //                }
            //            case "LineTo":
            //                {
            //                    LineTo lt = (LineTo)s;
            //                    Point from = Rotate(x, y, rotate);
            //                    Point to = Rotate(lt.x, lt.y, rotate);
            //                    sketch.Figures.AddLine(MMToCM(from.x), MMToCM(from.y), MMToCM(to.x), MMToCM(to.y));
            //                    x = lt.x;
            //                    y = lt.y;
            //                    break;
            //                }
            //            case "Arc":
            //                {
            //                    // Assuming circular arc
            //                    Arc a = (Arc)s;
            //                    System.Diagnostics.Debug.Assert(a.rx == a.ry);

            //                    // arc center -  https://stackoverflow.com/questions/36211171/finding-arc-circle-center-given-2-points-and-radius
            //                    double q = Math.Sqrt(Math.Pow((a.x - x), 2) + Math.Pow((a.y - y), 2));
            //                    double y3 = (y + a.y) / 2;
            //                    double x3 = (x + a.x) / 2;
            //                    double basex = Math.Sqrt(Math.Pow(a.rx, 2) - Math.Pow((q / 2), 2)) * (y - a.y) / q; //calculate once
            //                    double basey = Math.Sqrt(Math.Pow(a.rx, 2) - Math.Pow((q / 2), 2)) * (a.x - x) / q; //calculate once
            //                    double cx1 = x3 + basex; //center x of circle 1
            //                    double cy1 = y3 + basey; //center y of circle 1
            //                    double cx2 = x3 - basex; //center x of circle 2
            //                    double cy2 = y3 - basey; //center y of circle 2

            //                    void CalculateArcAngles(double centerX, double centerY, double x1, double y1, double x2, double y2, double rx, out double startAng, out double sweepAng)
            //                    {
            //                        //if (x1 != x2)
            //                        //{
            //                        //    startAng = Math.Acos((x1 - centerX) / rx);
            //                        //    sweepAng = Math.Acos((x2 - centerX) / rx);
            //                        //}
            //                        //else
            //                        //{
            //                        //    startAng = Math.Asin((y1 - centerY) / rx);
            //                        //    sweepAng = Math.Asin((y2 - centerY) / rx);
            //                        //}
            //                        startAng = Math.Atan2(y1 - centerY, x1 - centerX);
            //                        sweepAng = Math.Atan2(y2 - centerY, x2 - centerX);
            //                    }

            //                    double startAngle, sweepAngle;
            //                    double cx, cy;
            //                    CalculateArcAngles(cx1, cy1, x, y, a.x, a.y, a.rx, out startAngle, out sweepAngle);
            //                    if (startAngle < sweepAngle)  // CCW
            //                    {
            //                        if (a.sweep)
            //                        {
            //                            cx = cx1; cy = cy1;
            //                        }
            //                        else
            //                        {
            //                            CalculateArcAngles(cx2, cy2, x, y, a.x, a.y, a.rx, out startAngle, out sweepAngle);
            //                            cx = cx2; cy = cy2;
            //                        }
            //                    }
            //                    else // CW
            //                    {
            //                        if (!a.sweep)
            //                        {
            //                            cx = cx1; cy = cy1;
            //                        }
            //                        else
            //                        {
            //                            CalculateArcAngles(cx2, cy2, x, y, a.x, a.y, a.rx, out startAngle, out sweepAngle);
            //                            cx = cx2; cy = cy2;
            //                        }
            //                    }

            //                    //float n = 5.0f / zoom;
            //                    //g.DrawLine(penCenter, (float)cx - n, (float)cy - n, (float)cx + n, (float)cy + n);
            //                    //g.DrawLine(penCenter, (float)cx - n, (float)cy + n, (float)cx + n, (float)cy - n);
            //                    // angles

            //                    //startAngle = startAngle / Math.PI * 180;
            //                    //sweepAngle = sweepAngle / Math.PI * 180;
            //                    sweepAngle = sweepAngle - startAngle;
            //                    if (sweepAngle > Math.PI && !a.largeArc)
            //                        sweepAngle = sweepAngle - 2 * Math.PI;
            //                    else if (sweepAngle < 180 && a.largeArc)
            //                        sweepAngle = 2 * Math.PI - sweepAngle;

            //                    // Find the bounding rectangle
            //                    //RectangleF r = new RectangleF();
            //                    //r.X = (float)(cx - a.rx);
            //                    //r.Y = (float)(cy - a.rx);
            //                    //r.Width = (float)a.rx * 2.0f;
            //                    //r.Height = (float)a.ry * 2.0f;

            //                    //gr.DrawArc(penArc, r, (float)startAngle, (float)sweepAngle);

            //                    //g.DrawRectangle(penRect, r.X, r.Y, r.Width,r.Height);
            //                    //g.DrawLine(penArcLine, new PointF((float)x, (float)y), new PointF((float)a.x, (float)a.y));
            //                    Point from, to;
            //                    if (sweepAngle > 0)
            //                    {
            //                        from = Rotate(x, y, rotate);
            //                        to = Rotate(a.x, a.y, rotate);
            //                    }
            //                    else
            //                    {
            //                        to = Rotate(x, y, rotate);
            //                        from = Rotate(a.x, a.y, rotate);
            //                    }
            //                    Point center = Rotate(cx, cy, rotate);
            //                    sketch.Figures.AddCircularArcByCenterStartEnd(MMToCM(center.x), MMToCM(center.y), MMToCM(from.x), MMToCM(from.y), MMToCM(to.x), MMToCM(to.y));

            //                    x = a.x;
            //                    y = a.y;
            //                    break;
            //                }
            //            case "Bezier":
            //                {
            //                    Bezier b = (Bezier)s;

            //                    // Convert Bezier curve to points then bspline.
            //                    List<Point> curvePoints = new List<Point>();
            //                    {
            //                        List<Point> bezierPoints = new List<Point>();
            //                        bezierPoints.Add(new Point(x, y));    // start
            //                        bezierPoints.AddRange(b.points);
            //                        for (int p = 0; p < bezierPoints.Count-2; p += 3)
            //                        {
            //                            const int POINTS = 10;
            //                            for (int n = 0; n < POINTS; n++)
            //                            {
            //                                double u = (double)n / (double)POINTS;
            //                                // https://www.geeksforgeeks.org/cubic-bezier-curve-implementation-in-c/
            //                                double xu = Math.Pow(1 - u, 3) * bezierPoints[p].x + 3 * u * Math.Pow(1 - u, 2) * bezierPoints[p + 1].x + 3 * Math.Pow(u, 2) * (1 - u) * bezierPoints[p + 2].x + Math.Pow(u, 3) * bezierPoints[p + 3].x;
            //                                double yu = Math.Pow(1 - u, 3) * bezierPoints[p].y + 3 * u * Math.Pow(1 - u, 2) * bezierPoints[p + 1].y + 3 * Math.Pow(u, 2) * (1 - u) * bezierPoints[p + 2].y + Math.Pow(u, 3) * bezierPoints[p + 3].y;
            //                                curvePoints.Add(new Point(xu, yu));
            //                            }
            //                        }
            //                        curvePoints.Add(bezierPoints.Last());
            //                    }


            //                    // Create the spline
            //                    int order = 3;
            //                    int splinePoints = curvePoints.Count;
            //                    Array points = new double[splinePoints * 2];
            //                    Array knots = new double[splinePoints + order];
            //                    Array weights = new double[splinePoints];

            //                    int idx = 0;
            //                    for (int i = 0; i < curvePoints.Count; i++)
            //                    {
            //                        Point pt = Rotate(curvePoints[i], rotate);
            //                        points.SetValue(MMToCM(pt.x), idx * 2);
            //                        points.SetValue(MMToCM(pt.y), idx * 2 + 1);
            //                        weights.SetValue(0, idx);
            //                        idx++;
            //                    }

            //                    idx = 0;
            //                    for (int i = 0; i < splinePoints - order + 1; i++)
            //                    {
            //                        knots.SetValue((float)i / (float)(splinePoints - order + 1), i + order - 1);
            //                    }
            //                    for (int i = 0; i < order; i++)
            //                    {
            //                        knots.SetValue(0, i);
            //                        knots.SetValue(1, splinePoints + i);
            //                    }


            //                    sketch.Figures.AddBspline(order, splinePoints, ref points, ref knots, ref weights);

            //                    x = b.points.Last().x;
            //                    y = b.points.Last().y;
            //                    break;
            //                }
            //        }
            //    }
            //}
            //sketch.EndChange();

            //part.Features.AddExtrudedBoss(pSketch: sketch,
            //                              depth: MMToCM(thickness),
            //                              endCondition: ADPartFeatureEndCondition.AD_TO_DEPTH,
            //                              toGeometryOcc: null,
            //                              toGeometryObject: null,
            //                              toGeometryOffset: 0,
            //                              direction: ADDirectionType.AD_ALONG_NORMAL,
            //                              pDirectionOcc: null,
            //                              pDirectionObject: null,
            //                              isDirectionReversed: false,
            //                              draftAngle: null,
            //                              IsOutwardDraft: false,
            //                              name: "Involute Profile");

            // Create the bore for the motor
            sketch = part.Sketches.AddSketch(null, part.DesignPlanes.Item("YZ-Plane"), "Bore");
            sketch.BeginChange();
            sketch.Figures.AddCircle(0, 0, MMToCM(gear.boreDiameter / 2.0));
            sketch.EndChange();
            part.Features.AddExtrudedCutout(pSketch: sketch,
                                             depth: 0,
                                             endCondition: ADPartFeatureEndCondition.AD_THROUGH_ALL,
                                             toGeometryOcc: null,
                                             toGeometryObject: null,
                                             toGeometryOffset: 0,
                                             direction: ADDirectionType.AD_ALONG_NORMAL,
                                             pDirectionOcc: null,
                                             pDirectionObject: null,
                                             isDirectionReversed: false,
                                             draftAngle: null,
                                             IsOutwardDraft: false,
                                             name: "Bore");

            // Create planes to extrude cut tooth profile
            IADDesignPlane outerPlane = part.DesignPlanes.CreateBy3Points(null, part.DesignPoints.CreatePoint(MMToCM(gear.apex.X), MMToCM(gear.apex.Y), MMToCM(0), "op1"),
                                                                          null, part.DesignPoints.CreatePoint(MMToCM(gear.apex.X), MMToCM(gear.apex.Y), MMToCM(1), "op2"),
                                                                          null, part.DesignPoints.CreatePoint(MMToCM(gear.apexCorner.X), MMToCM(gear.apexCorner.Y), MMToCM(0), "op3"),
                                                                          "outer");
            IADDesignPlane innerPlane = part.DesignPlanes.CreateBy3Points(null, part.DesignPoints.CreatePoint(MMToCM(gear.tip.X), MMToCM(gear.tip.Y), MMToCM(0), "ip1"),
                                                                          null, part.DesignPoints.CreatePoint(MMToCM(gear.tip.X), MMToCM(gear.tip.Y), MMToCM(1), "ip2"),
                                                                          null, part.DesignPoints.CreatePoint(MMToCM(gear.inner.X), MMToCM(gear.inner.Y), MMToCM(0), "ip3"),
                                                                          "inner");

            //object filename = @"c:\temp\ad";
            //part.SaveAs(ref filename, "name");
            return occurrence;
        }

        private IADOccurrence CreateMount(IADAssemblySession assembly, string name, double centerDistance, double boreDiameter1, double boreDiameter2, double shaftLength)
        {
            IADTransformation trans = assembly.GeometryFactory.CreateIdentityTransform();
            IADOccurrence occurrence = assembly.RootOccurrence.Occurrences.AddEmptyPart(name, isSheetMetal: false, pTransform: trans);
            IADPartSession part = (IADPartSession)occurrence.DesignSession;

            IADSketch sketch = part.Sketches.AddSketch(null, part.DesignPlanes.Item("XY-Plane"), "Plate");
            sketch.BeginChange();
            sketch.Figures.AddRectangle(MMToCM(-centerDistance), MMToCM(-centerDistance), MMToCM(centerDistance), MMToCM(centerDistance));
            sketch.EndChange();

            part.Features.AddExtrudedBoss(pSketch: sketch,
                                          depth: MMToCM(5),
                                          endCondition: ADPartFeatureEndCondition.AD_TO_DEPTH,
                                          toGeometryOcc: null,
                                          toGeometryObject: null,
                                          toGeometryOffset: 0,
                                          direction: ADDirectionType.AD_ALONG_NORMAL,
                                          pDirectionOcc: null,
                                          pDirectionObject: null,
                                          isDirectionReversed: true,
                                          draftAngle: null,
                                          IsOutwardDraft: false,
                                          name: "Plate");

            // Create shafts
            sketch = part.Sketches.AddSketch(null, part.DesignPlanes.Item("XY-Plane"), "Shafts");
            sketch.BeginChange();
            sketch.Figures.AddCircle(MMToCM(-centerDistance / 2), 0, MMToCM(boreDiameter1 / 2.0));
            sketch.Figures.AddCircle(MMToCM(centerDistance/2), 0, MMToCM(boreDiameter2 / 2.0));
            sketch.EndChange();
            part.Features.AddExtrudedBoss(pSketch: sketch,
                                          depth: MMToCM(shaftLength),
                                          endCondition: ADPartFeatureEndCondition.AD_TO_DEPTH,
                                          toGeometryOcc: null,
                                          toGeometryObject: null,
                                          toGeometryOffset: 0,
                                          direction: ADDirectionType.AD_ALONG_NORMAL,
                                          pDirectionOcc: null,
                                          pDirectionObject: null,
                                          isDirectionReversed: false,
                                          draftAngle: null,
                                          IsOutwardDraft: false,
                                          name: "Shafts");

            part.DesignAxes.CreateBy2Points(occurrence, part.DesignPoints.CreatePoint(MMToCM(-centerDistance/2), 0, 0, "G1P1"),
                                             occurrence, part.DesignPoints.CreatePoint(MMToCM(-centerDistance / 2), 0, 1, "G1P2"), "Gear1");
            part.DesignAxes.CreateBy2Points(occurrence, part.DesignPoints.CreatePoint(MMToCM(centerDistance / 2), 0, 0, "G2P1"),
                                             occurrence, part.DesignPoints.CreatePoint(MMToCM(centerDistance / 2), 0, 1, "G2P2"), "Gear2");

            return occurrence;
        }


        private double MMToCM(double mm)
        {
            return mm / 10.0;
        }
    }
}
