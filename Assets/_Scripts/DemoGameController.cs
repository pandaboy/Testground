using UnityEngine;
using System.Collections.Generic;
using MonoGraph;
using RelationshipGraph.Relationships;

public class DemoGameController : MonoBehaviour
{
    // GameObjects we will use as our characters
    public GameObject otherCharacter;
    public GameObject leadCharacter;

    private Graph graph = Graph.Instance;
    private GameObject mainCharacter;
    private List<GameObject> otherCharacters;

    // some relationships to use
    // basically, we will evade enemies, and arrive at friends
    private Relationship friendlyR = new Relationship(RelationshipGraph.RelationshipType.FRIEND);
    private Relationship enemyR = new Relationship(RelationshipGraph.RelationshipType.ENEMY);

    // Use this for initialization
	void Start ()
    {
        // instantiate the main character
        mainCharacter = Instantiate(leadCharacter, new Vector3(50, 0.5f, -25), Quaternion.identity) as GameObject;
        //mainCharacter = leadCharacter;

        // create some random characters, make them enemies of the main characters
        otherCharacters = new List<GameObject>();

        for(int i = 0; i < 150; i++)
        {
            // instantiate a new character at a random location far from the mainCharacter
            GameObject ch = Instantiate(
                otherCharacter,
                new Vector3((float)i, 0.5f, Random.Range(-25.0f, 25.0f)),
                Quaternion.identity
            ) as GameObject;

            // Add it to the list of other characters
            otherCharacters.Add(ch);

            // set half as friendly, half as enemies
            if (i % 2 == 0)
            {
                graph.AddDirectConnection(new Connection(ch, mainCharacter, enemyR));
            }
            else
            {
                graph.AddDirectConnection(new Connection(ch, mainCharacter, friendlyR));
            }
        }

	}
}
