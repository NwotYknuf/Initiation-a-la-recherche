using System;
using System.Numerics;

namespace musique{

    public static class Processing{
        
        /*
         * Calcul la transformée de fourrier d'un signal réel
         * Retourne la transformée de fourrier pour les fréquences de 0 Hz à Fe/2 (La partie -Fe/2 à 0 est ignorée)
         * Ou Fe est la fréquence d'échantillonage du signal 
         */
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

        /*
         * Calcul l'énergie moyenne du signal sur une période donnée
         */
        public static double RootMeanSquare(double [] samples, int start, int windowSize){

            double val = 0;
            for(int i = 0; i < windowSize; i++){
                val += samples[start + i] * samples[start + i];
            }

            return Math.Sqrt(val / windowSize);

        }

        /*
         * Calcul le nombre de fois que ou le signal est passé par zéro sur une période donnée
         */
        public static double ZeroCrossingRate(double [] samples, int start, int windowSize){
                
            double crossings = 0;
            
            for(int i = 0; i < windowSize - 1; i++){
                if(samples[start+i] * samples[start+i+1] < 0){
                    crossings += 1;
                }
            }

            return crossings / (windowSize-1);
        }

        /*
         * Calcul la "fréquence moyenne" pour une transformée de fourrier donnée
         */
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

        /*
         * Calcul la fréquence à partir de laquelle Energie_cumulée > gama * Energie_totale
         */
        public static double SpectralRollOff(double[] fft, double gama, int sampleRate){

            double sum = 0.0;

             for(int i = 0; i < fft.Length; i++){
                sum += fft[i];
            }

            int indice = 0;
            double part_sum = 0;

            while(part_sum < gama * sum){
                part_sum += fft[indice];
                indice ++;
            }

            return (double)indice * sampleRate / (2 * fft.Length);

        }

    }
}
