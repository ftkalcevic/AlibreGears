using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GearsLib
{
    public class SpurGearInfo
    {
        public int teeth;
        public double pitchCircle;
        public double addendumCircle;
        public double dedendumCircle;

        public double boreDiameter;
        public double thickness;

        public List<Shape> shapes;
    };

    public class SpurGearSetInfo
    {
        public double module;
        public double DP;
        public double pressureAngle;
        public double addendum;
        public double dedendum;

        public double centerDistance;

        public SpurGearInfo gear1;
        public SpurGearInfo gear2;

        public SpurGearSetInfo()
        {
            gear1 = new SpurGearInfo();
            gear2 = new SpurGearInfo();
        }

        public void CalculateParameters()
        {
            if (module == 0)
                module = Math.PI / DP;
            else
                DP = Math.PI / module;

            addendum = 1.0 * module;
            dedendum = 1.25 * module;


            gear1.pitchCircle = gear1.teeth * module;
            gear1.addendumCircle = gear1.pitchCircle + 2 * addendum;
            gear1.dedendumCircle = gear1.pitchCircle - 2 * dedendum;

            gear2.pitchCircle = gear2.teeth * module;
            gear2.addendumCircle = gear2.pitchCircle + 2 * addendum;
            gear2.dedendumCircle = gear2.pitchCircle - 2 * dedendum;

            centerDistance = (gear1.pitchCircle + gear2.pitchCircle) / 2.0;
        }
    };

}
