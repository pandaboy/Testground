using UnityEngine;
using UnityEngine.UI;
using MonoGraph;
using RelationshipGraph;
using RelationshipGraph.Relationships;
using System.Collections.Generic;

public class Demo3GameController : MonoBehaviour
{
    public GameObject castMemberPrefab;
    public GameObject castLead;

    public InputField messageFied;

    // guard attributes
    protected GuardScript gs;

    // our relationship graph
    protected Graph graph = Graph.Instance;

    // relationships used in this demo: FRIEND, RIVAL, ENEMY, MEMBER
    protected Relationship friend = new Relationship(RelationshipType.FRIEND);
    protected Relationship enemy = new Relationship(RelationshipType.ENEMY);
    protected Relationship rival = new Relationship(RelationshipType.RIVAL);
    protected Relationship member = new Relationship(RelationshipType.MEMBER);

    protected List<GameObject> cast;

	// Use this for initialization
	void Start ()
    {
        gs = castLead.GetComponent<GuardScript>();
        // instantiate the cast members!
        cast = new List<GameObject>();

        GameObject alice = Instantiate(castMemberPrefab);
        alice.name = "Alice";
        graph.AddDirectConnection(new Connection(castLead, alice, friend));
        cast.Add(alice);

        // alice is a friend of 

        GameObject charlie = Instantiate(castMemberPrefab);
        charlie.name = "Charlie";
        graph.AddDirectConnection(new Connection(castLead, charlie, enemy));
        cast.Add(charlie);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            // get the connection from our lead to Alice
            Connection conn = graph.GetConnection(castLead.GetComponent<Demo3Character>(), cast[0].GetComponent<Demo3Character>());

            if(conn.Relationship.RelationshipType == RelationshipType.FRIEND)
            {
                gs.SetResponse(Responses.AGREE, "YEAH THAT'S MY FRIEND");
            }
            else
            {
                gs.SetResponse(Responses.DISAGREE, "Don't know who that is");
            }
        }
	}



    public void SendStringMessage()
    {
        // get the text in the input field
        StringMessage strMessage = new StringMessage(messageFied.text);
        castLead.GetComponent<Demo3Character>().HandleMessage(strMessage);
        messageFied.text = "";
    }

    public void PositiveReaction()
    {
        gs.SetResponse(Responses.AGREE, "Yeah, yeah what about it");
    }

    public void NegativeReaction()
    {
        gs.SetResponse(Responses.DISAGREE, "Nope, Get outta here!");
    }
}
