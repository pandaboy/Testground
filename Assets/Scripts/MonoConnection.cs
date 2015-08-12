using UnityEngine;
using System;
using System.Collections.Generic;
using RelationshipGraph.Edges;
using RelationshipGraph.Relationships;

public class MonoConnection : HistoryEdge<MonoEntity, Relationship>
{
    public MonoConnection() : base() { }
    public MonoConnection(MonoEntity from, MonoEntity to, Relationship relationship)
        : base(from, to, relationship) { }

    public override string ToString()
    {
        string history = From + " [";
        foreach(Relationship relationship in History)
        {
            history += relationship + " ";
        }
        history += "] " + To;

        return history;
    }
}