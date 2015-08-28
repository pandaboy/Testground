﻿using UnityEngine;
using UnityEngine.UI;
using MonoGraph;
using RelationshipGraph;
using RelationshipGraph.Relationships;
using System.Collections.Generic;

public class Demo3GameController : MonoBehaviour
{
    public GameObject castMemberPrefab;
    public GameObject castLead;

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
                gs.SetResponse("YEAH THAT'S MY FRIEND", true);
            }
            else
            {
                gs.SetResponse("Don't know who that is", false);
            }
        }
	}
}