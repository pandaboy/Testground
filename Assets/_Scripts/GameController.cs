using UnityEngine;
using System.Collections.Generic;
using RelationshipGraph.Relationships;
using MonoGraph;

public class GameController : MonoBehaviour
{
    #region Private members
    private Graph graph = Graph.Instance;
    private GameObject primary;
    private Entity mob;
    private IList<GameObject> characters = new List<GameObject>();
    #endregion

    #region Relationships
    private Relationship enemyRelationship = new Relationship(RelationshipGraph.RelationshipType.ENEMY);
    private Relationship friendlyRelationship = new Relationship(RelationshipGraph.RelationshipType.FRIEND);
    private Relationship memberRelationship = new Relationship(RelationshipGraph.RelationshipType.MEMBER);
    private Relationship followerRelationship = new Relationship(RelationshipGraph.RelationshipType.FOLLOWER);
    #endregion

    public GameObject character;
    public GameObject group;

	// Use this for initialization
	void Start () {
        primary = Instantiate(character, new Vector3(8.8f, .5f, 0), Quaternion.identity) as GameObject;
        if(group != null)
        {
            group = Instantiate(group, Vector3.zero, Quaternion.identity) as GameObject;
            graph.AddDirectConnection(new Connection(group, primary, followerRelationship));
        }

        // make two characters
        for (int i = 0; i < 2; i++)
        {
            GameObject c = Instantiate(character, new Vector3(i - 2.5f, .5f, i - 2.5f), Quaternion.identity) as GameObject;
            characters.Add(c);

            // add connection to primary
            graph.AddDirectConnection(new Connection(c, primary, friendlyRelationship));
            if (group != null)
            {
                graph.AddDirectConnection(new Connection(c, group, memberRelationship));
            }
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject c = Instantiate(character, new Vector3(i + 2.5f, .5f, i + 2.5f), Quaternion.identity) as GameObject;
            characters.Add(c);

            graph.AddDirectConnection(new Connection(c, primary, enemyRelationship));
            if (group != null)
            {
                graph.AddDirectConnection(new Connection(c, group, memberRelationship));
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        // use this to highlight relationships
        if(Input.GetKeyDown(KeyCode.H))
        {
            ColorCharacters();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("MESSAGE TO BECOME CHUMS");
            // send a message to followers of primary to be friendly.
            friendlyRelationship.Weight += 10;
            SendMessage(primary, friendlyRelationship);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("MESSAGE TO BECOME ENEMIES");
            // send a message to followers of primary to be friendly.
            enemyRelationship.Weight += 10;
            SendMessage(primary, enemyRelationship);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            graph.PrintConnections();
        }
	}

    private void ColorCharacters()
    {
        Debug.Log("COLORING TIME");
        Entity pEntity = primary.GetComponent<Entity>();
        IList<Entity> enemies = graph.WithRelationshipTo(pEntity, enemyRelationship);
        if (enemies.Count > 0)
        {
            foreach (Entity e in enemies)
            {
                if (e.GetComponent<StateScript>())
                {
                    e.GetComponent<StateScript>().ChangeState(States.AGGRESSIVE);
                }
            }
        }
        
        IList<Entity> friends = graph.WithRelationshipTo(pEntity, friendlyRelationship);
        if (friends.Count > 0)
        {
            foreach (Entity e in friends)
            {
                if (e.GetComponent<StateScript>())
                {
                    e.GetComponent<StateScript>().ChangeState(States.SUPPORTIVE);
                }
            }
        }
    }

    private void SendMessage(GameObject go, Relationship rel)
    {
        Connection newConnection = new Connection(group, primary, rel);
        ConnectionMessage msg = new ConnectionMessage(newConnection);
        graph.SendMessageTo(go.GetComponent<Entity>(), followerRelationship, msg);
    }
}
