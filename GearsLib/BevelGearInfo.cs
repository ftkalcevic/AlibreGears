using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GearsLib
{
    public class BevelGearInfo
    {
        public int teeth;
        public double outsidePitchDiameter;
        public double addendumCircle;
        public double dedendumCircle;
        public double addendum;
        public double dedendum;
        public double coneAngle;    // 1/2 angle
        public double diameter;
        public double addendumAngle;
        public double dedendumAngle;
        public double tipAngle;
        public double rootAngle;
        public double tipDiameter;
        public double pitchApexToCrown;
        public double pitchApexToCrownY;
        public double axialFaceWidth;
        public double innerTipDiameter;

        public double boreDiameter;
        public double hubDiameter;
        public double hubLen;

        public double thickness;

        public PointF hubEnd, hubBase, apexCorner, apex, tip,inner;
    };

    public class BevelGearSetInfo
    {
        public double module;
        public double DP;
        public double pressureAngle;
        public double shaftAngle;
        public double addendum;
        public double dedendum;
        public double coneDistance;
        public double faceWidth;

        public BevelGearInfo gear1;
        public BevelGearInfo gear2;

        public BevelGearSetInfo()
        {
            gear1 = new BevelGearInfo();
            gear2 = new BevelGearInfo();
        }

        public void CalculateParameters()
        {
            if (module == 0)
                module = Math.PI / DP;
            else
                DP = Math.PI / module;

            addendum = 1.0 * module;
            dedendum = 1.25 * module;


            gear1.outsidePitchDiameter = gear1.teeth * module;
            gear1.addendumCircle = gear1.outsidePitchDiameter + 2 * addendum;
            gear1.dedendumCircle = gear1.outsidePitchDiameter - 2 * dedendum;
            gear1.diameter = gear1.teeth * module;
            gear1.coneAngle = Math.Atan2(Math.Sin(shaftAngle * Math.PI / 180.0), (double)gear2.teeth / (double)gear1.teeth + Math.Cos(shaftAngle * Math.PI / 180.0)) * 180.0 / Math.PI;
            
            gear2.outsidePitchDiameter = gear2.teeth * module;
            gear2.addendumCircle = gear2.outsidePitchDiameter + 2 * addendum;
            gear2.dedendumCircle = gear2.outsidePitchDiameter - 2 * dedendum;
            gear2.diameter = gear2.teeth * module;
            gear2.coneAngle = Math.Atan2(Math.Sin(shaftAngle * Math.PI / 180.0), (double)gear1.teeth / (double)gear2.teeth + Math.Cos(shaftAngle * Math.PI / 180.0)) * 180.0 / Math.PI;

            coneDistance = gear2.diameter / (2 * Math.Sin(gear2.coneAngle / 180.0 * Math.PI));
            faceWidth = coneDistance / 3;

            gear2.addendum = 0.540 * module + 0.460 * module / ((gear2.teeth * Math.Cos(gear1.coneAngle / 180 * Math.PI)) / (gear1.teeth * Math.Cos(gear2.coneAngle / 180 * Math.PI)));
            gear1.addendum = 2 * module - gear2.addendum;
            gear1.dedendum = 2.188 * module - gear1.addendum;
            gear2.dedendum = 2.188 * module - gear2.addendum;
            gear1.dedendumAngle = Math.Atan2(gear1.dedendum, coneDistance) / Math.PI * 180;
            gear2.dedendumAngle = Math.Atan2(gear2.dedendum, coneDistance) / Math.PI * 180;
            gear1.addendumAngle = gear2.dedendumAngle;
            gear2.addendumAngle = gear1.dedendumAngle;
            gear1.tipAngle = gear1.coneAngle + gear1.addendumAngle;
            gear2.tipAngle = gear2.coneAngle + gear2.addendumAngle;
            gear1.rootAngle = gear1.coneAngle - gear1.dedendumAngle;
            gear2.rootAngle = gear2.coneAngle - gear2.dedendumAngle;
            gear1.tipDiameter = module * gear1.teeth + 2 * gear1.addendum * Math.Cos(gear1.coneAngle / 180 * Math.PI);
            gear2.tipDiameter = module * gear2.teeth + 2 * gear2.addendum * Math.Cos(gear2.coneAngle / 180 * Math.PI);
            gear1.pitchApexToCrown = coneDistance * Math.Cos(gear1.coneAngle / 180 * Math.PI) - gear1.addendum * Math.Sin(gear1.coneAngle / 180 * Math.PI);
            gear2.pitchApexToCrown = coneDistance * Math.Cos(gear2.coneAngle / 180 * Math.PI) - gear2.addendum * Math.Sin(gear2.coneAngle / 180 * Math.PI);
            gear1.pitchApexToCrownY = gear1.pitchApexToCrown * Math.Tan((gear1.coneAngle + gear1.addendumAngle) / 180 * Math.PI);
            gear2.pitchApexToCrownY = gear2.pitchApexToCrown * Math.Tan((gear2.coneAngle + gear2.addendumAngle) / 180 * Math.PI);
            gear1.axialFaceWidth = faceWidth * Math.Cos(gear1.tipAngle / 180 * Math.PI) / Math.Cos(gear1.addendumAngle / 180 * Math.PI);
            gear2.axialFaceWidth = faceWidth * Math.Cos(gear2.tipAngle / 180 * Math.PI) / Math.Cos(gear2.addendumAngle / 180 * Math.PI);
            gear1.innerTipDiameter = gear1.tipDiameter - 2 * faceWidth * Math.Sin(gear1.tipAngle / 180 * Math.PI) / Math.Cos(gear1.addendumAngle / 180 * Math.PI);
            gear2.innerTipDiameter = gear2.tipDiameter - 2 * faceWidth * Math.Sin(gear2.tipAngle / 180 * Math.PI) / Math.Cos(gear2.addendumAngle / 180 * Math.PI);

            double backSize = 1.2;

            gear1.hubEnd = new PointF((float)(gear1.pitchApexToCrown + gear1.hubLen), (float)(gear1.hubDiameter / 2));
            gear1.apexCorner = new PointF((float)(gear1.pitchApexToCrown + backSize * (gear1.addendum + gear2.addendum) * Math.Sin(gear1.coneAngle / 180 * Math.PI)),
                                          (float)(gear1.pitchApexToCrownY - backSize * (gear1.addendum + gear2.addendum) * Math.Cos(gear1.coneAngle / 180 * Math.PI)));
            if ( gear1.apexCorner.Y < gear1.hubDiameter / 2 )
            {
                gear1.hubBase = new PointF((float)(gear1.pitchApexToCrown + (gear1.pitchApexToCrownY - gear1.hubDiameter / 2) * Math.Tan(gear1.coneAngle / 180 * Math.PI)), (float)(gear1.hubDiameter / 2));
                gear1.apexCorner = gear1.hubBase;
            }
            else
            {
                gear1.hubBase = new PointF(gear1.apexCorner.X, (float)(gear1.hubDiameter / 2));
            }
            gear1.apex = new PointF((float)(gear1.pitchApexToCrown), (float)(gear1.pitchApexToCrownY));
            gear1.tip = new PointF((float)(gear1.apex.X - gear1.axialFaceWidth), (float)(gear1.apex.Y - gear1.axialFaceWidth * Math.Tan((gear1.coneAngle + gear1.addendumAngle) / 180 * Math.PI)));
            gear1.inner = new PointF((float)(gear1.tip.X + (gear1.addendum + gear2.addendum) * Math.Sin(gear1.coneAngle / 180 * Math.PI)), (float)(gear1.tip.Y - (gear1.addendum + gear2.addendum) * Math.Cos(gear1.coneAngle / 180 * Math.PI)));

            gear2.hubEnd = new PointF((float)(gear2.pitchApexToCrown + gear2.hubLen), (float)(gear2.hubDiameter / 2));
            gear2.apexCorner = new PointF((float)(gear2.pitchApexToCrown + backSize * (gear1.addendum + gear2.addendum) * Math.Sin(gear2.coneAngle / 180 * Math.PI)),
                                          (float)(gear2.pitchApexToCrownY - backSize * (gear1.addendum + gear2.addendum) * Math.Cos(gear2.coneAngle / 180 * Math.PI)));
            if (gear2.apexCorner.Y < gear2.hubDiameter / 2)
            {
                gear2.hubBase = new PointF((float)(gear2.pitchApexToCrown + (gear2.pitchApexToCrownY - gear2.hubDiameter / 2) * Math.Tan(gear2.coneAngle / 180 * Math.PI)), (float)(gear2.hubDiameter / 2));
                gear2.apexCorner = gear2.hubBase;
            }
            else
            {
                gear2.hubBase = new PointF(gear2.apexCorner.X, (float)(gear2.hubDiameter / 2));
            }
            gear2.apex = new PointF((float)(gear2.pitchApexToCrown), (float)(gear2.pitchApexToCrownY));
            gear2.tip = new PointF((float)(gear2.apex.X - gear2.axialFaceWidth), (float)(gear2.apex.Y - gear2.axialFaceWidth * Math.Tan((gear2.coneAngle + gear2.addendumAngle) / 180 * Math.PI)));
            gear2.inner = new PointF((float)(gear2.tip.X + (gear1.addendum + gear2.addendum) * Math.Sin(gear2.coneAngle / 180 * Math.PI)), (float)(gear2.tip.Y - (gear1.addendum + gear2.addendum) * Math.Cos(gear2.coneAngle / 180 * Math.PI)));
        }
    };

}
