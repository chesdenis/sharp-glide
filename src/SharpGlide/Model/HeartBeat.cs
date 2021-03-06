using System.Xml.Serialization;

namespace SharpGlide.Model
{
    [XmlRoot]
    public class HeartBeat
    {
        [XmlElement]
        public int Level { get; set; }

        [XmlElement]
        public HeartBeat[] Children { get; set; }

        [XmlArray]
        public HeartBeatEntry[] Data { get; set; }

        [XmlElement]
        public string[] Exceptions { get; set; }
    }
}