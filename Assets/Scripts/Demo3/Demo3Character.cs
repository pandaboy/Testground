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
            gs.SetResponse(Responses.AGREE, "I GOT A CONNECTION MESSAGE!");

        if (message.GetType() == typeof(StringMessage))
        {
            StringMessage msg = message as StringMessage;
            gs.SetResponse(Responses.MESSAGE, "MESSAGE: \"" + msg.Text + "\"");
        }

        return true;
    }
}
