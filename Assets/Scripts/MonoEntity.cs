using UnityEngine;
using System.Collections;
using RelationshipGraph.Interfaces;

public class MonoEntity : MonoBehaviour, INode<MonoEntity>
{
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

    public int Count
    {
        get
        {
            return count;
        }
    }

    void Awake()
    {
        EntityId = ++count;
    }

	// Use this for initialization
	void Start ()
    {
        Debug.Log("I am " + this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Required implementations
    public virtual bool HandleMessage(IMessage message)
    {
        return false;
    }

    public virtual bool Equals(MonoEntity other)
    {
        if (other == null)
            return false;

        if (this.EntityId == other.EntityId)
            return true;

        return false;
    }
    #endregion

    public override string ToString()
    {
        return "Entity: " + EntityId;
    }
}
