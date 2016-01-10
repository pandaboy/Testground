using RelationshipGraph.Messages;

namespace MonoGraph
{
    public class ConnectionMessage : EdgeMessage<Connection>
    {
        public ConnectionMessage() : base() { }
        public ConnectionMessage(Connection connection) : base(connection) { }

        public Connection Connection
        {
            get
            {
                return Edge;
            }

            set
            {
                Edge = value;
            }
        }
    }
}