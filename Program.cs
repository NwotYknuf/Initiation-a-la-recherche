using System;
using CSCore;
using CSCore.Codecs;
using System.IO;
using System.Numerics;

namespace musique{

    class Program{

        static void Main(string[] args){

            Analyzer ana = new Analyzer(1.0);

            ana.loadSong("test.mp3");
            ana.computeFFT();
            
            double[] res = ana.computeSpectralCentroid();

            StreamWriter s = new StreamWriter("test.txt");

            for(int i = 0; i < res.Length; i++){
                s.WriteLine(res[i]);
            }

            s.Close();
            
        }
    }
}
