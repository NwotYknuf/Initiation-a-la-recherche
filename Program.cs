using System;
using CSCore;
using CSCore.Codecs;
using System.IO;
using System.Numerics;

namespace musique{

    class Program{        

        static void Main(string[] args){

            string dossier = "./songs/";
            string sortie = "./output/";
            
            Annaliseur ana = new Annaliseur(1.0);

            string[] fichiers = null;
            fichiers = Directory.GetFiles(dossier);
            
            foreach(string fichier in fichiers){
                 ana.chargeChanson(fichier);
                 ana.calculFFT();

                double[] rms = ana.computeRMS();
                double[] zrc = ana.calculZCR();
                double[] rolloff = ana.computeRollOff(0.85);
                double[] centroid = ana.calculCentroid();

                Path.GetFileNameWithoutExtension(fichier);

                StreamWriter s = new StreamWriter(sortie + Path.GetFileNameWithoutExtension(fichier));

                s.WriteLine("rms zrc rollof centroid");

                for(int i = 0; i < rms.Length; i++){
                    s.WriteLine("{0} {1} {2} {3}", rms[i],zrc[i],rolloff[i],centroid[i]);
                }

                s.Close();
            }
        }
    }
}
