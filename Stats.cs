using System;

namespace musique{

    public static class Stats{

        public static void normalise(ref double[] donnes){
            double min = Double.PositiveInfinity;
            double max = Double.NegativeInfinity;

            for(int i = 0; i < donnes.Length; i++){
                min = Math.Min(min, donnes[i]);
                max = Math.Max(max, donnes[i]);
            }

            for(int i = 0; i < donnes.Length; i++){
                donnes[i] = (donnes[i]-min) / (max - min);
            }
        }

        public static double moyenne(double[] donnes){

            double sum = 0.0;
            for(int i =0; i < donnes.Length; i++){
                sum+=donnes[i];
            }

            return sum / donnes.Length;
        }

        public static double mediane(double[] donnes){
            double sum = 0.0;

            for(int i = 0; i < donnes.Length ;i++){
                sum += donnes[i];
            }

            int ind = 0;
            double part_sum = 0.0;

            do{
                part_sum += donnes[ind];
                ind++;
            }while(part_sum <= sum/2);

            return donnes[ind];
        }

        public static double ecart_type(double[] donnes){
            double sum = 0.0;

            double moyenne = Stats.moyenne(donnes);

            for(int i = 0; i < donnes.Length;i++){
                sum += (donnes[i] - moyenne)*(donnes[i] - moyenne);
            }

            return Math.Sqrt(sum / donnes.Length);
        }


    }
}