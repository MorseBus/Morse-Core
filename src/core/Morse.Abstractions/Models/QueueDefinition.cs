namespace Morse.Abstractions.Models
{
    public class QueueDefinition
    {
        public string Name { get; set; }
        public string RoutingKey { get; set; }
        public ConnectionDefinition Connection { get; set; }
    }
}
