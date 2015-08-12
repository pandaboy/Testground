using UnityEngine;
using System.Collections.Generic;
using RelationshipGraph.Relationships;

public class GameController : MonoBehaviour {

    private MonoEntityGraph graph = new MonoEntityGraph();

    private GameObject primary;
    private MonoEntity primaryEntity;

    public GameObject character;

    private IList<GameObject> characters = new List<GameObject>();

    private Relationship enemyRelationship = new Relationship(RelationshipGraph.RelationshipType.ENEMY);
    private Relationship friendlyRelationship = new Relationship(RelationshipGraph.RelationshipType.FRIEND);

	// Use this for initialization
	void Start () {
        primary = Instantiate(character, new Vector3(8.8f, .5f, 0), Quaternion.identity) as GameObject;
        primaryEntity = primary.GetComponent<MonoEntity>();

	    // make two characters
        for(int i = 0; i < 2; i++)
        {
            GameObject c = Instantiate(character, new Vector3(i - 2.5f, .5f, i - 2.5f), Quaternion.identity) as GameObject;
            characters.Add(c);

            MonoEntity ci = c.GetComponent<MonoEntity>();

            // add connection to primary
            graph.AddDirectEdge(new MonoConnection(primaryEntity, ci, friendlyRelationship));
        }

        for(int i = 0; i < 5; i++)
        {
            GameObject c = Instantiate(character, new Vector3(i + 2.5f, .5f, i + 2.5f), Quaternion.identity) as GameObject;
            characters.Add(c);

            MonoEntity ce = c.GetComponent<MonoEntity>();

            graph.AddDirectEdge(new MonoConnection(primaryEntity, ce, enemyRelationship));
        }

	}
	
	// Update is called once per frame
	void Update () {
        // use this to highlight relationships
        if(Input.GetKeyDown(KeyCode.H))
        {
            // display friends of primary
            foreach(MonoConnection connection in graph.GetDirectEdges(primaryEntity))
            {
                if(connection.Relationship == friendlyRelationship)
                {
                    connection.To.GetComponent<StateScript>().ChangeState(States.SUPPORTIVE);
                }

                if (connection.Relationship == enemyRelationship)
                {
                    connection.To.GetComponent<StateScript>().ChangeState(States.AGGRESSIVE);
                }
            }
        }
	}
}
