using UnityEngine;
using MonoGraph;
using RelationshipGraph;
using RelationshipGraph.Interfaces;
using RelationshipGraph.Relationships;

namespace MonoGraph
{
    public class Entity : MonoBehaviour, INode<Entity>
    {
        /// <summary>
        /// Number of entities. Used to generate a unique ID
        /// </summary>
        protected static int count = 0;

        private int _EntityId;
        public int EntityId
        {
            get
            {
                return _EntityId;
            }

            set
            {
                _EntityId = value;
            }
        }

        public EntityType EntityType;

        public int Count
        {
            get
            {
                return count;
            }
        }
        
        public virtual void Awake()
        {
            EntityId = ++count;
        }

        public virtual bool HandleMessage(IMessage message)
        {
            if (message.GetType() == typeof(ConnectionMessage))
            {
                ConnectionMessage msg = message as ConnectionMessage;
                
                // process the message in the connection
                LearnConnection(msg.Connection);
            }

            return false;
        }

        public virtual bool Equals(Entity other)
        {
            if (other == null)
                return false;

            if (this.EntityId == other.EntityId)
                return true;

            return false;
        }

        public override bool Equals(object o)
        {
            if (o == null)
                return false;

            Entity entity = o as Entity;

            if (entity == null)
                return false;
            else
                return Equals(entity);
        }

        public override int GetHashCode()
        {
            return _EntityId;
        }

        public override string ToString()
        {
            return "[ID: " + EntityId + "]";
        }

        public virtual void LearnConnection(Connection newConnection)
        {
            Graph graph = Graph.Instance;
            Connection learnConnection = newConnection;

            // only learn if not a group.

            // check the "From" entity - if it's a group, and I'm a member of that
            // group, update it to point to this entity
            if (newConnection.From.EntityType == EntityType.GROUP)
            {
                // check if there's a connection from this entity to the "From" entity
                if (graph.EntityHasConnection(this, this, newConnection.From))
                {
                    Connection conn = graph.GetEntityConnection(this, this, newConnection.From);

                    // if we're a member of the "From" adopt it's place
                    if (conn.Relationship.RelationshipType == RelationshipType.MEMBER)
                    {
                        // basically replace the learnConnection with a new Connection where FROM is "this" entity
                        learnConnection = new Connection(this, newConnection.To, newConnection.Relationship);
                    }
                }
            }


            // if I'm a group, pass this on to my members
            // otherwise try to learn
            if (EntityType == EntityType.GROUP)
            {
                Relationship members = new Relationship(RelationshipType.MEMBER);
                ConnectionMessage message = new ConnectionMessage(newConnection);

                graph.SendMessageTo(this, members, message);
            }
            else
            {
                if (graph.EntityHasConnection(learnConnection.From, learnConnection))
                {
                    Connection currentConnection = graph.GetEntityConnection(learnConnection.From, learnConnection);

                    if (currentConnection.Relationship.Equals(learnConnection.Relationship))
                        MatchingConnection(learnConnection, currentConnection);
                    else
                        ConflictingConnection(learnConnection, currentConnection);
                }
                else
                {
                    ConflictingConnection(learnConnection);
                }
            }
        }

        public virtual void ConflictingConnection(Connection newConnection, Connection currentConnection = null)
        {
            Graph graph = Graph.Instance;
            if(currentConnection == null)
            {
                // learn newConnection
                graph.AddConnection(this, newConnection);
            }
            else
            {
                // compare connections by relationship to determine if the newConnection should be adopted
                Relationship newRelationship = newConnection.Relationship;
                Relationship currentRelationship = currentConnection.Relationship;

                if (currentRelationship.Weight <= newRelationship.Weight)
                {
                    // adopt the newConnection
                    graph.AddConnection(this, newConnection);
                }
            }
        }

        public virtual void MatchingConnection(Connection newConnection, Connection currentConnection)
        {
            Relationship newRelationship = newConnection.Relationship;
            Relationship currentRelationship = currentConnection.Relationship;

            if(currentRelationship.Weight < newRelationship.Weight)
            {
                currentRelationship.Weight = newRelationship.Weight;
            }
        }
    }
}