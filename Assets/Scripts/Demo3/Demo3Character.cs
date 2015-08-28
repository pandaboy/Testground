using UnityEngine;
using MonoGraph;
using RelationshipGraph;
using RelationshipGraph.Interfaces;
using System.Collections;

public class Demo3Character : Entity
{
    protected GuardScript gs;

    override public void Awake()
    {
        base.Awake();

        gs = GetComponent<GuardScript>();
    }

    public override bool HandleMessage(IMessage message)
    {
        // pass on message to Entity handler
        //base.HandleMessage(message);

        if(message.GetType() == typeof(ConnectionMessage))
        {
            ConnectionMessage msg = message as ConnectionMessage;
            string text = "Connection to " + msg.Connection.To.name + " R[" + msg.Connection.Relationship + "]";
            gs.SetResponse(Responses.MESSAGE, text);
        }

        if (message.GetType() == typeof(StringMessage))
        {
            StringMessage msg = message as StringMessage;
            gs.SetResponse(Responses.MESSAGE, "\"" + msg.Text + "\"");
        }

        return true;
    }
}
