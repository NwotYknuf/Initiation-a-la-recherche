using System;
using CSCore;
using CSCore.Codecs;
using System.IO;
using System.Numerics;

namespace musique{

    class Program{

        

        static void Main(string[] args){

            string path = "./songs/";
            string output = "./output/";
            
            Analyzer ana = new Analyzer(1.0);

            string[] files = null;
            files = Directory.GetFiles(path);
            
            foreach(string file in files){
                 ana.loadSong(file);
                 ana.computeFFT();

                double[] rms = ana.computeRMS();
                double[] zrc = ana.computeZCR();
                double[] rolloff = ana.computeRollOff(0.85);
                double[] centroid = ana.computeSpectralCentroid();

                Path.GetFileNameWithoutExtension(file);

                StreamWriter s = new StreamWriter(output + Path.GetFileNameWithoutExtension(file));

                s.WriteLine("rms zrc rollof centroid");

                for(int i = 0; i < rms.Length; i++){
                    s.WriteLine("{0} {1} {2} {3}", rms[i],zrc[i],rolloff[i],centroid[i]);
                }

                s.Close();
            }
        }
    }
}
