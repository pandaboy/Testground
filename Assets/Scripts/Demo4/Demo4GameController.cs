using UnityEngine;
using System.Collections.Generic;
using MonoGraph;
using RelationshipGraph.Relationships;

public class Demo4GameController : MonoBehaviour
{
    public Entity entityPrefab;
    private Graph graph = Graph.Instance;
    private List<Entity> characters;
    private Relationship friendRelationship = new Relationship(RelationshipGraph.RelationshipType.FRIEND);

	void Start ()
    {
        // stores the list of characters
        characters = new List<Entity>();

        // create the characters
        for (int i = 0; i < 10000; i++)
        {
            Entity ch = Instantiate(entityPrefab) as Entity;

            characters.Add(ch);
        }

        // set up relationships
        foreach (Entity character in characters)
        {
            int j = 0;

            // set up a relationship from this character to every other character
            foreach (Entity other in characters)
            {
                if (j == 100) break;
                j++;
                Connection conn = new Connection(character, other, friendRelationship);

                for (int l = 0; l < 9; l++)
                {
                    conn.Relationship = new Relationship((RelationshipGraph.RelationshipType)l);
                }

                graph.AddDirectConnection(conn);
            }
        }

        //graph.PrintConnections();
	}

    void Update()
    {
        // trigger an action
        if(Input.GetKeyDown(KeyCode.A))
        {
            //IList<Entity> matches = graph.WithRelationshipHistoryTo(characters[0], friendRelationship);
            // Debug.Log("Matches found: " + matches.Count);
        }
    }
}
