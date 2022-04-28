namespace SharpGlide.Flow
{
    public class FlowModel
    {
        public Settings Settings { get; set; } = new Settings();
        public Tunnels Tunnels { get; set; } = new Tunnels();
        public Parts Parts { get; set; } = new Parts();
        public Bindings Bindings { get; set; } = new Bindings();
    }
}