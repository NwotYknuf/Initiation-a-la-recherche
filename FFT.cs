using System.Numerics;
using System;

namespace musique{

    public static class FFT{

        /*
         *source : https://code.msdn.microsoft.com/windowsapps/FFTLibrary-e1942867
         */

        public static Complex[] RecursiveFFT(Complex[] a){ 
            int n = a.Length; 
            int n2 = n / 2; 
 
            if (n == 1) 
                return a; 
 
            Complex z = new Complex(0.0, 2.0 * Math.PI / n); 
            Complex omegaN = Complex.Exp(z); 
            Complex omega = new Complex(1.0, 0.0); 
            Complex[] a0 = new Complex[n2]; 
            Complex[] a1 = new Complex[n2]; 
            Complex[] y0 = new Complex[n2]; 
            Complex[] y1 = new Complex[n2]; 
            Complex[] y = new Complex[n]; 
 
            for (int i = 0; i < n2; i++) 
            { 
                a0[i] = new Complex(0.0, 0.0); 
                a0[i] = a[2 * i]; 
                a1[i] = new Complex(0.0, 0.0); 
                a1[i] = a[2 * i + 1]; 
            } 
 
            y0 = RecursiveFFT(a0); 
            y1 = RecursiveFFT(a1); 
 
            for (int k = 0; k < n2; k++) 
            { 
                y[k] = new Complex(0.0, 0.0); 
                y[k] = y0[k] + (y1[k] * omega); 
                y[k + n2] = new Complex(0.0, 0.0); 
                y[k + n2] = y0[k] - (y1[k] * omega); 
                omega = omega * omegaN; 
            } 
 
            return y; 
        } 
 
        public static Complex[] DFT(double[] x) { 
            double pi2oN = 2.0 * Math.PI / x.Length; 
            int k, n; 
            Complex[] X = new Complex[x.Length]; 
 
            for (k = 0; k < x.Length; k++) 
            { 
                X[k] = new Complex(0.0, 0.0); 
 
                for (n = 0; n < x.Length; n++) 
                { 
                    X[k] += new Complex(x[n] * Math.Cos(pi2oN * k * n), 0); 
                    X[k] -= new Complex(0.0, x[n] * Math.Sin(pi2oN * k * n));
                } 
 
                X[k] /= x.Length;
            } 
 
            return X; 
        } 
 
        public static double[] InverseDFT(Complex[] X){ 
            double[] x = new double[X.Length]; 
            double imag, pi2oN = 2.0 * Math.PI / X.Length; 
 
            for (int n = 0; n < X.Length; n++) 
            { 
                imag = x[n] = 0.0; 
 
                for (int k = 0; k < X.Length; k++) 
                { 
                    x[n] += X[k].Real * Math.Cos(pi2oN * k * n) 
                          - X[k].Imaginary * Math.Sin(pi2oN * k * n); 
                    imag += X[k].Real * Math.Sin(pi2oN * k * n) 
                          + X[k].Imaginary * Math.Cos(pi2oN * k * n); 
                } 
            } 
 
            return x; 
        } 
 
        // This computes an in-place complex-to-complex FFT  
        // x and y are the real and imaginary arrays of 2^m points. 
        // dir =  1 gives forward transform 
        // dir = -1 gives reverse transform  
        // see http://astronomy.swin.edu.au/~pbourke/analysis/dft/ 
 
        public static void fft(short dir, int m, double[] x, double[] y){ 
            int n, i, i1, j, k, i2, l, l1, l2; 
            double c1, c2, tx, ty, t1, t2, u1, u2, z; 
 
            // Calculate the number of points 
 
            n = 1; 
 
            for (i = 0; i < m; i++) 
                n *= 2; 
 
            // Do the bit reversal 
              
            i2 = n >> 1; 
            j = 0; 
            for (i = 0; i < n - 1; i++) 
            { 
                if (i < j) 
                { 
                    tx = x[i]; 
                    ty = y[i]; 
                    x[i] = x[j]; 
                    y[i] = y[j]; 
                    x[j] = tx; 
                    y[j] = ty; 
                } 
                k = i2; 
                 
                while (k <= j) 
                { 
                    j -= k; 
                    k >>= 1; 
                } 
 
                j += k; 
            } 
 
            // Compute the FFT 
 
            c1 = -1.0; 
            c2 = 0.0; 
            l2 = 1; 
 
            for (l = 0; l < m; l++) 
            { 
                l1 = l2; 
                l2 <<= 1; 
                u1 = 1.0; 
                u2 = 0.0; 
 
                for (j = 0; j < l1; j++) 
                { 
                    for (i = j; i < n; i += l2) 
                    { 
                        i1 = i + l1; 
                        t1 = u1 * x[i1] - u2 * y[i1]; 
                        t2 = u1 * y[i1] + u2 * x[i1]; 
                        x[i1] = x[i] - t1; 
                        y[i1] = y[i] - t2; 
                        x[i] += t1; 
                        y[i] += t2; 
                    } 
 
                    z = u1 * c1 - u2 * c2; 
                    u2 = u1 * c2 + u2 * c1; 
                    u1 = z; 
                } 
 
                c2 = Math.Sqrt((1.0 - c1) / 2.0); 
 
                if (dir == 1) 
                    c2 = -c2; 
 
                c1 = Math.Sqrt((1.0 + c1) / 2.0); 
            } 
 
            // Scaling for forward transform 
             
            if (dir == 1) 
            { 
                for (i = 0; i < n; i++) 
                { 
                    x[i] /= n; 
                    y[i] /= n; 
                } 
            } 
        } 

    }
}