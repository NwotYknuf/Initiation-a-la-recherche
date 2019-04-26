using System;
using System.Runtime.Serialization;

[DataContract]
public class DataSong
{
    internal string id;

    [DataMember]
    internal double median_rms;

    [DataMember]
    internal double ecarType_rms;

    [DataMember]
    internal double median_zrc;

    [DataMember]
    internal double ecarType_zrc;

    [DataMember]
    internal double median_centroid;

    [DataMember]
    internal double ecarType_centroid;

    [DataMember]
    internal double median_spread;

    [DataMember]
    internal double ecarType_spread;

    [DataMember]
    internal double songLenght;
}