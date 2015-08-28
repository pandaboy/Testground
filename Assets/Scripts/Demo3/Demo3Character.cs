using UnityEngine;
using MonoGraph;
using RelationshipGraph;
using RelationshipGraph.Interfaces;
using System.Collections;

public class Demo3Character : Entity
{
    override public void Awake()
    {
        base.Awake();
    }

    public override bool HandleMessage(IMessage message)
    {
        // pass on message to Entity handler
        //base.HandleMessage(message);

        return true;
    }
}
