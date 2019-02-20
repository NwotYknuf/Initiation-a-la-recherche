using System;
using CSCore;
using CSCore.Codecs;
using System.IO;
using System.Numerics;

namespace musique{

    class Program{        

        
        static void Main(string[] args){

            string dossier = "D:/Musique/";
            string sortie = "./output/";

            string[] fichiers = null;
            fichiers = Directory.GetFiles(dossier);
            
            foreach(string fichier in fichiers){

                double[] signal;
                double Fe;

                SongLoadder.chargeChanson(fichier, out Fe, out signal);

                Annaliseur ana = new Annaliseur(1.0, Fe, signal);
                ana.calculeFFT();

                double[] rms = ana.calculeRMS();
                double[] zrc = ana.calculeZCR();
                double[] rolloff = ana.calculeRollOff(0.85);
                double[] centroid = ana.calculeCentroid();

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
