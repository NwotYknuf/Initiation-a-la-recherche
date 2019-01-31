using System;
using System.Numerics;

namespace musique{

    public static class Processing{
        
        public static double[] computeFFT(double[] samples, int start, int windowSize){

            int m = (int)Math.Ceiling(Math.Log(windowSize)/Math.Log(2.0));
            int size = (int)Math.Pow(2.0, m);

            double [] real = new double[size];
            double[] imaginary = new double[size];

            for(int i = 0; i < windowSize; i++){
                real[i] = samples[start+i];
            }

            FFT.fft(1,m,real,imaginary);

            double[] res = new double[size/2];

            for(int i = 0; i < size/2; i++){
                res[i] = Math.Sqrt(real[i] * real[i] + imaginary[i] * imaginary[i]);
            }

            return res;

        }

        public static double RootMeanSquare(double [] samples, int start, int windowSize){

            double val = 0;
            for(int i = 0; i < windowSize; i++){
                val += samples[start + i] * samples[start + i];
            }

            return Math.Sqrt(val / windowSize);

        }

        public static double ZeroCrossingRate(double [] samples, int start, int windowSize){
                
            double crossings = 0;
            
            for(int i = 0; i < windowSize - 1; i++){
                if(samples[start+i] * samples[start+i+1] < 0){
                    crossings += 1;
                }
            }

            return crossings / (windowSize-1);
        }

        public static double SpectralCentroid(double[] fft, int sampleRate){

            double sum = 0.0;
            double weightedSum = 0.0;

            for(int i = 0; i < fft.Length; i++){

                double m = (double)sampleRate/(2*fft.Length) * i;
                sum += fft[i];
                weightedSum += m * fft[i];
            }

            return weightedSum / sum;
            
        }

    }
}
