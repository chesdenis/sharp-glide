using System;
using System.IO;
using System.Runtime.Serialization;

namespace XDataFlow.Tunnels.FileSystem
{
    public class FileSystemPublishTunnel<T> : PublishTunnel<T>
    {
        private const string DefaultTunnelPlace = "C:\\samplePublishTunnel";
         
        public override Action<T, string, string> PublishPointer()
        {
            return (data, topicName, routingKey) =>
            {
                var path = Path.Combine(DefaultTunnelPlace, $"{Guid.NewGuid():N}.data");
                
                var writer = new FileStream(path, FileMode.Create);
                var ser = new DataContractSerializer(typeof(T));
                ser.WriteObject(writer, data);
                writer.Close();
            };
        }

        public override void SetupInfrastructure(string topicName, string routingKey)
        {
            Directory.CreateDirectory(DefaultTunnelPlace);
        }
    }
}