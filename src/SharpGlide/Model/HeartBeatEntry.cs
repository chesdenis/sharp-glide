using System.Xml.Serialization;

namespace SharpGlide.Model
{
    [XmlRoot]
    public class HeartBeatEntry
    {
        [XmlElement]
        public string Name { get; set; } 
        
        [XmlElement]
        public string Value { get; set; }
    }
}