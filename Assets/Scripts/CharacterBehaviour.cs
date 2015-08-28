using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using MonoGraph;
using RelationshipGraph;
using RelationshipGraph.Relationships;

public class CharacterBehaviour : MonoBehaviour
{
    private Graph graph;
    private Relationship enemyR;
    private Relationship friendlyR;
    private Entity thisEntity;

    public Material altMaterial;

    void Awake()
    {
        graph = Graph.Instance;
        enemyR = new Relationship(RelationshipType.ENEMY);
        friendlyR = new Relationship(RelationshipType.FRIEND);
        thisEntity = GetComponent<Entity>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Entity otherEntity = other.GetComponent<Entity>();

            // if it's an enemy, tell agent to chase!
            if (graph.HaveRelationship(thisEntity, otherEntity, enemyR))
            {
                GetComponent<AICharacterControl>().SetTarget(other.transform);
                GetComponentInChildren<Renderer>().material = altMaterial;
            }
        }
    }
}
