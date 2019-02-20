using System;
using System.Numerics;

namespace musique{

    public static class Traitements{
        
        /*
         * Calcule la transformée de fourrier d'un signal réel
         * Retourne la transformée de fourrier pour les fréquences de 0 Hz à Fe/2 (La partie -Fe/2 à 0 est ignorée)
         * Ou Fe est la fréquence d'échantillonage du signal 
         */
        public static double[] FFT(double[] signal, int debut, int tailleFenetre){

            int m = (int)Math.Ceiling(Math.Log(tailleFenetre)/Math.Log(2.0));
            int size = (int)Math.Pow(2.0, m);

            double [] reel = new double[size];
            double[] imaginaire = new double[size];

            for(int i = 0; i < tailleFenetre; i++){
                reel[i] = signal[debut+i];
            }

            musique.FFT.fft(1, m, reel, imaginaire);

            double[] res = new double[size/2];

            for(int i = 0; i < size/2; i++){
                res[i] = Math.Sqrt(reel[i] * reel[i] + imaginaire[i] * imaginaire[i]);
            }

            return res;

        }

        /*
         * Calcule l'énergie moyenne du signal sur une période donnée
         */
        public static double RMS(double [] signal, int debut, int tailleFenetre){

            double somme = 0;
            for(int i = 0; i < tailleFenetre; i++){
                somme += signal[debut + i] * signal[debut + i];
            }

            return Math.Sqrt(somme / tailleFenetre);

        }

        /*
         * Calcule le nombre de fois que ou le signal est passé par zéro sur une période donnée
         */
        public static double ZRC(double [] signal, int debut, int tailleFenetre){
                
            double passages = 0;
            
            for(int i = 0; i < tailleFenetre - 1; i++){
                if(signal[debut+i] * signal[debut+i+1] < 0){
                    passages += 1;
                }
            }

            return passages / (tailleFenetre-1);
        }

        /*
         * Calcule la "fréquence moyenne" pour une transformée de fourrier donnée
         */
        public static double Centroid(double[] fft, int Fe){

            double somme = 0.0;
            double sommePonderee = 0.0;

            for(int i = 0; i < fft.Length; i++){

                double m = (double)Fe/(2*fft.Length) * i;
                somme += fft[i];
                sommePonderee += m * fft[i];
            }

            return sommePonderee / somme;
            
        }

        /*
         * Calcule "l'écart type de fréquence" pour une transformée de fourrier donnée
         * Il est possible de donner la Centroid si elle a déjà été calculée
         */
        public static double Spread(double[] fft, int Fe, double centroid = double.NegativeInfinity){
            
            if(centroid == double.NegativeInfinity){
                centroid = Traitements.Centroid(fft,Fe);
            }
    
            double somme = 0.0;
            double sommePonderee = 0.0;

            for(int i = 0; i < fft.Length; i++){
                double m = (double)Fe/(2*fft.Length) * i;
                somme += fft[i];
                sommePonderee += fft[i] * (m - centroid) * (m - centroid);
            }

            return Math.Sqrt(sommePonderee / somme);
        }

        /*
         * Calcule la fréquence à partir de laquelle Energie_cumulée > gama * Energie_totale
         */
        public static double RollOff(double[] fft, double gama, int Fe){

            double somme = 0.0;

             for(int i = 0; i < fft.Length; i++){
                somme += fft[i];
            }

            int indice = 0;
            double sommePartielle = 0;

            while(sommePartielle < gama * somme){
                sommePartielle += fft[indice];
                indice ++;
            }

            return (double)indice * Fe / (2 * fft.Length);

        }

    }
}
