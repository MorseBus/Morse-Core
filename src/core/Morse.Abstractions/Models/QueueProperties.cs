namespace Morse.Abstractions.Models
{
    public class QueueProperties
    {
        public string Name { get; set; }
        public string RoutingKey { get; set; }
        public ConnectionProperties Connection { get; set; }
    }
}
