// Involute calculations from ...

/*==========================================================================*
  Filename: gearUtils-05.js
  By: Dr A.R.Collins

  JavaScript involute gear drawing utilities.
  Requires:
  'involuteBezCoeffs' can stand alone,
  'createGearTooth' and 'createIntGearTooth' generate draw commands for use
  in Cango graphics library but may be simply converted for use in SVG.

  Kindly give credit to Dr A.R.Collins <http://www.arc.id.au/>
  Report bugs to tony at arc.id.au

  Date   |Description                                                   |By
  --------------------------------------------------------------------------
  20Feb13 First public release                                           ARC
  21Feb13 Clarified variable names of start and end parameters           ARC
  06Mar13 Fixed Rf and filletAngle calculations                          ARC
  25Jun13 Code tidy for JSLint, use strict                               ARC
  16Mar15 Convert sweep direction of "A" commands for RHC coordinates    ARC
 *==========================================================================*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GearsLib
{
    delegate double chebyExpnCoeffsDelegate(double theta);

    public class SpurGears
    {
        ///* ----------------------------------------------------------
        // * involuteBezCoeffs
        // *
        // * JavaScript calculation of Bezier coefficients for
        // * Higuchi et al. approximation to an involute.
        // * ref: YNU Digital Eng Lab Memorandum 05-1
        // *
        // * Parameters:
        // * module - sets the size of teeth (see gear design texts)
        // * numTeeth - number of teeth on the gear
        // * pressure angle - angle in degrees, usually 14.5 or 20
        // * order - the order of the Bezier curve to be fitted [3, 4, 5, ..]
        // * fstart - fraction of distance along tooth profile to start
        // * fstop - fraction of distance along profile to stop
        // *-----------------------------------------------------------*/
        private List<Point> involuteBezCoeffs(double module, int numTeeth, double pressureAngle, int order, double fstart, double fstop)
        {
            double Rpitch = module * numTeeth / 2;       // pitch circle radius
            double phi = pressureAngle;        // pressure angle
            double Rb = Rpitch * Math.Cos(phi * Math.PI / 180.0); // base circle radius
            double Ra = Rpitch + module;               // addendum radius (outer radius)
            int p = order;                   // order of Bezier approximation
            double ta = Math.Sqrt(Ra * Ra - Rb * Rb) / Rb;   // involute angle at addendum
            double stop = fstop;
            double start = 0.01;
            double te, ts;
            List<Point> bzCoeffs = new List<Point>();
            int i;
            Point bcoeff;

            double chebyExpnCoeffs(int j, chebyExpnCoeffsDelegate func)
            {
                int N = 50;      // a suitably large number  N>>p
                int k;
                double c = 0.0;

                for (k = 1; k <= N; k++)
                {
                    c += func(Math.Cos(Math.PI * ((double)k - 0.5) / (double)N)) * Math.Cos(Math.PI * (double)j * ((double)k - 0.5) / (double)N);
                }
                return 2 * c / N;
            }
    
            double [] chebyPolyCoeffs(int pp, chebyExpnCoeffsDelegate func)
            {
                double [] coeffs = new double[pp+1];
                double [] fnCoeff = new double[pp+1];
                double[,] T = new double[pp+1,pp + 1];
                int j, k, pwr;

                // populate 1st 2 rows of T
                T[0,0] = 1;
                T[1,1] = 1;

                /* now generate the Chebyshev polynomial coefficient using
                    formula T(k+1) = 2xT(k) - T(k-1) which yields
                T = [ [ 1,  0,  0,  0,  0,  0],    // T0(x) =  +1
                        [ 0,  1,  0,  0,  0,  0],    // T1(x) =   0  +x
                        [-1,  0,  2,  0,  0,  0],    // T2(x) =  -1  0  +2xx
                        [ 0, -3,  0,  4,  0,  0],    // T3(x) =   0 -3x    0   +4xxx
                        [ 1,  0, -8,  0,  8,  0],    // T4(x) =  +1  0  -8xx       0  +8xxxx
                        [ 0,  5,  0,-20,  0, 16],    // T5(x) =   0  5x    0  -20xxx       0  +16xxxxx
                        ...                     ];
                */
                for (k = 1; k < pp; k++)
                {
                    for (j = 0; j < pp; j++)
                    {
                        T[k + 1,j + 1] = 2 * T[k,j];
                    }
                    for (j = 0; j < pp-1; j++)
                    {
                        T[k + 1,j] -= T[k - 1,j];
                    }
                }
                // convert the chebyshev function series into a simple polynomial
                // and collect like terms, out T polynomial coefficients
                for (k = 0; k <= pp; k++)
                {
                    fnCoeff[k] = chebyExpnCoeffs(k, func);
                    coeffs[k] = 0;
                }
                for (k = 0; k <= pp; k++)
                {
                    for (pwr = 0; pwr <= pp; pwr++)    // loop thru powers of x
                    {
                        coeffs[pwr] += fnCoeff[k] * T[k,pwr];
                    }
                }
                coeffs[0] -= chebyExpnCoeffs(0, func) / 2;  // fix the 0th coeff

                return coeffs;
            }


            // Equation of involute using the Bezier parameter t as variable
            double involuteXbez(double t)
            {
                // map t (0 <= t <= 1) onto x (where -1 <= x <= 1)
                var x = t * 2 - 1;
                //map theta (where ts <= theta <= te) from x (-1 <=x <= 1)
                var theta = x * (te - ts) / 2 + (ts + te) / 2;
                return Rb * (Math.Cos(theta) + theta * Math.Sin(theta));
            }


            double involuteYbez(double t)
            {
                // map t (0 <= t <= 1) onto x (where -1 <= x <= 1)
                var x = t * 2 - 1;
                    //map theta (where ts <= theta <= te) from x (-1 <=x <= 1)
                var theta = x * (te - ts) / 2 + (ts + te) / 2;

                return Rb * (Math.Sin(theta) - theta * Math.Cos(theta));
            }

            double binom(int n, int k)
            {
                double coeff = 1;
                int ii;

                for (ii = n - k + 1; ii <= n; ii++)
                {
                    coeff *= ii;
                }
                for (ii = 1; ii <= k; ii++)
                {
                    coeff /= ii;
                }

                return coeff;
            }



            double bezCoeff(int ii, chebyExpnCoeffsDelegate func)
            {
                // generate the polynomial coeffs in one go
                var polyCoeffs = chebyPolyCoeffs(p, func);
                double bc;
                int j;

                for (bc = 0, j = 0; j <= ii; j++)
                {
                    bc += binom(ii, j) * polyCoeffs[j] / binom(p, j);
                }
                return bc;
            }


            if (fstart < stop)
            {
                start = fstart;
            }
            te = Math.Sqrt(stop) * ta;          // involute angle, theta, at end of approx
            ts = Math.Sqrt(start) * ta;         // involute angle, theta, at start of approx
                                                // calc Bezier coeffs
            for (i = 0; i <= p; i++)
            {
                bcoeff = new Point( bezCoeff(i, involuteXbez), bezCoeff(i, involuteYbez) );
                bzCoeffs.Add( bcoeff );
            }

            return bzCoeffs;
        }

        private Point rotate(Point pt, double rads)  // rotate pt by rads radians about origin
        {
            var sinA = Math.Sin(rads);
            var cosA = Math.Cos(rads);

            return new Point( pt.x * cosA - pt.y * sinA, pt.x * sinA + pt.y * cosA );
        }

        private Point toCartesian(double radius, double angle)   // convert polar coords to cartesian
        {
            return new Point( radius* Math.Cos(angle), radius* Math.Sin(angle));
        }

        private double genInvolutePolar(double Rb, double R)  // Rb = base circle radius
        {
            // returns the involute angle as function of radius R.
            return (Math.Sqrt(R * R - Rb * Rb) / Rb) - Math.Acos(Rb / R);
        }

        ///*----------------------------------------------------------
        //  createGearTooth
        //  Create an array of drawing commands and their coordinates
        //  to draw a single spur gear tooth based on a circle
        //  involute using the metric gear standards.

        //  Requires Cango graphics library Rev 2.08 or later
        // ----------------------------------------------------------*/
        public List<Shape> createGearTooth(double module, int teeth, double pressureAngle)
        {
            // ****** external gear specifications
            double m = module;                                     // Module = mm of pitch diameter per tooth
            int Z = teeth;                                      // Number of teeth
            double phi = pressureAngle;                      // pressure angle (degrees)
            double addendum = m;                                   // distance from pitch circle to tip circle
            double dedendum = 1.25 * m;                              // pitch circle to root, sets clearance
            double clearance = dedendum - addendum;
            // Calculate radii
            double Rpitch = (double)Z * m / 2;                                 // pitch circle radius
            double Rb = Rpitch * Math.Cos(phi * Math.PI / 180.0);          // base circle radius
            double Ra = Rpitch + addendum;                         // tip (addendum) circle radius
            double Rroot = Rpitch - dedendum;                      // root circle radius
            double fRad = 1.5 * clearance;                           // fillet radius, max 1.5*clearance
            double Rf;                                             // radius at top of fillet
            // ****** calculate angles (all in radians)
            double pitchAngle = 2.0 * Math.PI / (double)Z;                       // angle subtended by whole tooth (rads)
            double baseToPitchAngle = genInvolutePolar(Rb, Rpitch);
            double pitchToFilletAngle = baseToPitchAngle;          // profile starts at base circle
            double filletAngle = Math.Atan(fRad / (fRad + Rroot));     // radians
            double fe, fs, fm;
            List<Point> dedBz, addBz, inv, invR;
            Point fillet, filletR, filletNext;
            Point rootR, rootNext;
            Point pt;
            int i;
            List<Shape> data;

            Rf = Math.Sqrt((Rroot + fRad) * (Rroot + fRad) - (fRad * fRad)); // radius at top of fillet
            if (Rb < Rf)
            {
                Rf = Rroot + clearance;
            }
            if (Rf > Rb)                   // start profile at top of fillet (if its greater)
            {
                pitchToFilletAngle -= genInvolutePolar(Rb, Rf);
            }
            // ****** generate Higuchi involute approximation
            fe = 1;                    // fraction of profile length at end of approx
            fs = 0.01;                 // fraction of length offset from base to avoid singularity
            if (Rf > Rb)
            {
                fs = (Rf * Rf - Rb * Rb) / (Ra * Ra - Rb * Rb);  // offset start to top of fillet
            }
            // approximate in 2 sections, split 25% along the involute
            fm = fs + (fe - fs) / 4;         // fraction of length at junction (25% along profile)
            dedBz = involuteBezCoeffs(m, Z, phi, 3, fs, fm);
            addBz = involuteBezCoeffs(m, Z, phi, 3, fm, fe);
            // join the 2 sets of coeffs (skip duplicate mid point)
            inv = dedBz;
            inv.AddRange(addBz.GetRange(1,addBz.Count-1));
            //create the back profile of tooth (mirror image)
            invR = new List<Point>();                // involute profile along back of tooth
            for (i = 0; i < inv.Count; i++)
            {
                // rotate all points to put pitch point at y = 0
                pt = rotate(inv[i], -baseToPitchAngle - pitchAngle / 4);
                inv[i] = pt;
                // generate the back of tooth profile nodes, mirror coords in X axis
                invR.Add(new Point( pt.x, -pt.y));
            }
            // ****** calculate section junction points R=back of tooth, Next=front of next tooth)
            fillet = toCartesian(Rf, -pitchAngle / 4 - pitchToFilletAngle); // top of fillet
            filletR = new Point( fillet.x, -fillet.y);   // flip to make same point on back of tooth
            rootR = toCartesian(Rroot, pitchAngle / 4 + pitchToFilletAngle + filletAngle);
            rootNext = toCartesian(Rroot, 3 * pitchAngle / 4 - pitchToFilletAngle - filletAngle);
            filletNext = rotate(fillet, pitchAngle);  // top of fillet, front of next tooth
                                                      // ****** create the drawing command data array for the tooth
            data = new List<Shape>();
            data.Add( new MoveTo(fillet.x, fillet.y));           // start at top of fillet
            if (Rf < Rb)
            {
                data.Add( new LineTo(inv[0].x, inv[0].y));         // line from fillet up to base circle
            }
            data.Add( new Bezier(inv[1].x, inv[1].y, inv[2].x, inv[2].y, inv[3].x, inv[3].y,
                           inv[4].x, inv[4].y, inv[5].x, inv[5].y, inv[6].x, inv[6].y));
            data.Add( new Arc(Ra, Ra, 0, false, true, invR[6].x, invR[6].y)); // arc across addendum circle, sweep 1 for RHC, 0 for SVG
            data.Add( new Bezier(invR[5].x, invR[5].y, invR[4].x, invR[4].y, invR[3].x, invR[3].y,
                           invR[2].x, invR[2].y, invR[1].x, invR[1].y, invR[0].x, invR[0].y));
            if (Rf < Rb)
            {
                data.Add( new LineTo(filletR.x, filletR.y));       // line down to top of fillet
            }
            if (rootNext.y > rootR.y)    // is there a section of root circle between fillets?
            {
                data.Add( new Arc(fRad, fRad, 0, false, false, rootR.x, rootR.y)); // back fillet, sweep 0 for RHC, 1 for SVG
                data.Add(new Arc(Rroot, Rroot, 0, false, true, rootNext.x, rootNext.y)); // root circle arc, sweep 1 for RHC, 0 for SVG
            }
            data.Add(new Arc(fRad, fRad, 0, false, false, filletNext.x, filletNext.y)); // sweep 0 for RHC, 1 for SVG

            return data;  // return an array of Cango (SVG) format draw commands
        }

        ///*----------------------------------------------------------
        //  createIntGearTooth
        //  Create an array of drawing commands and their coordinates
        //  to draw a single internal (ring)gear tooth based on a
        //  circle involute using the metric gear standards.

        //  Requires Cango graphics library Rev 2.08 or later
        // ----------------------------------------------------------*/
        //public void createIntGearTooth(module, teeth, pressureAngle)
        //{
        //    // ****** gear specifications
        //    var m = module,                               // Module = mm of pitch diameter per tooth
        //        Z = teeth,                                // Number of teeth
        //        phi = pressureAngle || 20,                // pressure angle (degrees)
        //        addendum = 0.6 * m,                         // pitch circle to tip circle (ref G.M.Maitra)
        //        dedendum = 1.25 * m,                        // pitch circle to root radius, sets clearance
        //                                                    // Calculate radii
        //        Rpitch = Z * m / 2,                           // pitch radius
        //        Rb = Rpitch * Math.cos(phi * Math.PI / 180),    // base radius
        //        Ra = Rpitch - addendum,                   // addendum radius
        //        Rroot = Rpitch + dedendum,                // root radius
        //        clearance = 0.25 * m,                       // gear dedendum - pinion addendum
        //        Rf = Rroot - clearance,                   // radius of top of fillet (end of profile)
        //        fRad = 1.5 * clearance,                     // fillet radius, 1 .. 1.5*clearance
        //        pitchAngle,                               // angle between teeth (rads)
        //        baseToPitchAngle,
        //        tipToPitchAngle,
        //        pitchToFilletAngle,
        //        filletAngle,
        //        fe, fs, fm,
        //        addBz, dedBz,
        //        inv, invR,
        //        pt, i, data,
        //        fillet, filletNext,
        //        tip, tipR,
        //        rootR, rootNext;

        //    // ****** calculate subtended angles
        //    pitchAngle = 2 * Math.PI / Z;                       // angle between teeth (rads)
        //    baseToPitchAngle = genInvolutePolar(Rb, Rpitch);
        //    tipToPitchAngle = baseToPitchAngle;             // profile starts from base circle
        //    if (Ra > Rb)
        //    {
        //        tipToPitchAngle -= genInvolutePolar(Rb, Ra);  // start profile from addendum
        //    }
        //    pitchToFilletAngle = genInvolutePolar(Rb, Rf) - baseToPitchAngle;
        //    filletAngle = 1.414 * clearance / Rf;               // to make fillet tangential to root
        //    // ****** generate Higuchi involute approximation
        //    fe = 1;                   // fraction of involute length at end of approx (fillet circle)
        //    fs = 0.01;                 // fraction of length offset from base to avoid singularity
        //    if (Ra > Rb)
        //    {
        //        fs = (Ra * Ra - Rb * Rb) / (Rf * Rf - Rb * Rb);    // start profile from addendum (tip circle)
        //    }
        //    // approximate in 2 sections, split 25% along the profile
        //    fm = fs + (fe - fs) / 4;        //
        //    addBz = involuteBezCoeffs(m, Z, phi, 3, fs, fm);
        //    dedBz = involuteBezCoeffs(m, Z, phi, 3, fm, fe);
        //    // join the 2 sets of coeffs (skip duplicate mid point)
        //    invR = addBz.concat(dedBz.slice(1));
        //    //create the front profile of tooth (mirror image)
        //    inv = [];         // back involute profile
        //    for (i = 0; i < invR.length; i++)
        //    {
        //        // rotate involute to put center of tooth at y = 0
        //        pt = rotate(invR[i], pitchAngle / 4 - baseToPitchAngle);
        //        invR[i] = pt;
        //        // generate the back of tooth profile, flip Y coords
        //        inv[i] = { x: pt.x, y: -pt.y};
        //    }
        //    // ****** calculate coords of section junctions
        //    fillet = { x: inv[6].x, y: inv[6].y};    // top of fillet, front of tooth
        //    tip = toCartesian(Ra, -pitchAngle / 4 + tipToPitchAngle);  // tip, front of tooth
        //    tipR = { x: tip.x, y: -tip.y};  // addendum, back of tooth
        //    rootR = toCartesian(Rroot, pitchAngle / 4 + pitchToFilletAngle + filletAngle);
        //    rootNext = toCartesian(Rroot, 3 * pitchAngle / 4 - pitchToFilletAngle - filletAngle);
        //    filletNext = rotate(fillet, pitchAngle);  // top of fillet, front of next tooth
        //    // ****** create the drawing command data array for the tooth
        //    data = [];
        //    data.push("M", inv[6].x, inv[6].y);  // start at top of front profile
        //    data.push("C", inv[5].x, inv[5].y, inv[4].x, inv[4].y, inv[3].x, inv[3].y,
        //                   inv[2].x, inv[2].y, inv[1].x, inv[1].y, inv[0].x, inv[0].y);
        //    if (Ra < Rb)
        //    {
        //        data.push("L", tip.x, tip.y);  // line from end of involute to addendum (tip)
        //    }
        //    data.push("A", Ra, Ra, 0, 0, 1, tipR.x, tipR.y); // arc across tip circle, sweep 1 for RHC, 0 for SVG
        //    if (Ra < Rb)
        //    {
        //        data.push("L", invR[0].x, invR[0].y);  // line from addendum to start of involute
        //    }
        //    data.push("C", invR[1].x, invR[1].y, invR[2].x, invR[2].y, invR[3].x, invR[3].y,
        //                   invR[4].x, invR[4].y, invR[5].x, invR[5].y, invR[6].x, invR[6].y);
        //    if (rootR.y < rootNext.y)    // there is a section of root circle between fillets
        //    {
        //        data.push("A", fRad, fRad, 0, 0, 1, rootR.x, rootR.y); // fillet on back of tooth, sweep 1 for RHC, 0 for SVG
        //        data.push("A", Rroot, Rroot, 0, 0, 1, rootNext.x, rootNext.y); // root circle arc, sweep 1 for RHC, 0 for SVG
        //    }
        //    data.push("A", fRad, fRad, 0, 0, 1, filletNext.x, filletNext.y); // fillet on next, sweep 1 for RHC, 0 for SVG 

        //    return data;  // return an array of Cango (SVG) format draw commands
        //}
    }
}
