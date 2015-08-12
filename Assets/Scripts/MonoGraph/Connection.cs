using UnityEngine;
using RelationshipGraph.Edges;
using RelationshipGraph.Relationships;

namespace MonoGraph
{
    public class Connection : HistoryEdge<Entity, Relationship>
    {
        #region Constructors
        public Connection() : base() { }

        public Connection(Entity from, Entity to, Relationship relationship)
            : base(from, to, relationship) { }

        // accepts GameObjects for faster initialization
        public Connection(GameObject from, GameObject to, Relationship relationship)
            : base(from.GetComponent<Entity>(), to.GetComponent<Entity>(), relationship)
        {
        }
        #endregion Constructors

        public override string ToString()
        {
            string history = From + " [";
            foreach (Relationship relationship in History)
            {
                history += relationship + " ";
            }
            history += "] " + To;

            return history;
        }
    }
}