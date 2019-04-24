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
            
            StreamWriter s = new StreamWriter(sortie + "stats");
            //s.WriteLine("Song rms(medianne) rms(ecart type) zrc(mediane) zrc(ecart type) centroid(medianne) centroid(ecart type) spread(mediane) spread(ecart type)");

            foreach(string fichier in fichiers){

                Console.WriteLine("Analyse de : {0}", Path.GetFileNameWithoutExtension(fichier));

                double[] signal;
                double Fe;

                SongLoadder.chargeChanson(fichier, out Fe, out signal);

                Annaliseur ana = new Annaliseur(1.0, Fe, signal);
                ana.calculeFFT();

                double[] rms = ana.calculeRMS();
                double[] zrc = ana.calculeZCR();
                double[] rolloff = ana.calculeRollOff(0.85);
                double[] centroid = ana.calculeCentroid();
                double[] spread = ana.calculeSpread(centroid);

                s.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                    Path.GetFileNameWithoutExtension(fichier),
                    Stats.mediane(rms), Stats.ecart_type(rms),
                    Stats.mediane(zrc), Stats.ecart_type(zrc),
                    Stats.mediane(centroid), Stats.ecart_type(centroid),
                    Stats.mediane(spread), Stats.ecart_type(spread)
                    );
            }

            s.Close();
        }
    }
}
