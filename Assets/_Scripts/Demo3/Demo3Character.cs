using UnityEngine;
using MonoGraph;
using RelationshipGraph;
using RelationshipGraph.Interfaces;
using RelationshipGraph.Relationships;
using System.Collections;

public class Demo3Character : Entity
{
    protected GuardScript gs;

    public override void Awake()
    {
        base.Awake();

        gs = GetComponent<GuardScript>();
    }

    public override bool HandleMessage(IMessage message)
    {
        if(message.GetType() == typeof(ConnectionMessage))
        {
            // pass on message to Entity handler
            base.HandleMessage(message);
        }

        if (message.GetType() == typeof(StringMessage))
        {
            StringMessage msg = message as StringMessage;
            gs.SetResponse(Responses.MESSAGE, "\"" + msg.Text + "\"");
        }

        return true;
    }

    public override void MatchingConnection(Connection newConnection, Connection currentConnection)
    {
        string text = "Yeah I know " + newConnection.To.name + " we're " + newConnection.Relationship.RelationshipType + "'s";
        if (newConnection.To.EntityType == EntityType.GROUP) {
            text = "Yeah, I'm a " + newConnection.Relationship.RelationshipType + " of " + newConnection.To.name;
        }

        gs.SetResponse(Responses.HAPPY, text);

        base.MatchingConnection(newConnection, currentConnection);
    }

    public override void ConflictingConnection(Connection newConnection, Connection currentConnection = null)
    {
        string text;
        if (currentConnection == null) {
            text = "So, " + newConnection.To.name + " is a " + newConnection.Relationship.RelationshipType + "...";
            gs.SetResponse(Responses.MESSAGE, text);
        }
        else {
            Relationship newRelationship = newConnection.Relationship;
            Relationship currentRelationship = currentConnection.Relationship;

            if (currentRelationship.Weight <= newRelationship.Weight) {
                text = "I trust your judgement, " + newConnection.To.name + " is now my " + newRelationship.RelationshipType;
                gs.SetResponse(Responses.AGREE, text);
            }
            else {
                text = "You liar! " + currentConnection.To.name + " is my " + currentRelationship.RelationshipType + "!";
                gs.SetResponse(Responses.ANGRY, text);
            }
        }

        // pass on the logic
        base.ConflictingConnection(newConnection, currentConnection);
    }
}
