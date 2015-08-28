using UnityEngine;
using UnityEngine.UI;
using MonoGraph;
using RelationshipGraph;
using RelationshipGraph.Relationships;
using System.Collections.Generic;

public class Demo3GameController : MonoBehaviour
{
    public GameObject castMemberPrefab;
    public GameObject castMemberCheckPrefab;
    public GameObject castLead;
    public string[] castNames = new string[10];
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

    protected RelationshipType activeRelationshipType;
    protected GameObject activeCastMember;

    protected Dictionary<string, RelationshipType> string_enums;
    protected Dictionary<string, GameObject> cast;

	// Use this for initialization
	void Start ()
    {
        // get the canvas element
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");

        // Lookups from string to enums!
        string_enums = new Dictionary<string, RelationshipType>();
        string_enums.Add("FRIEND", RelationshipType.FRIEND);
        string_enums.Add("ENEMY", RelationshipType.ENEMY);
        string_enums.Add("RIVAL", RelationshipType.RIVAL);
        string_enums.Add("MEMBER", RelationshipType.MEMBER);

        // instantiate the cast members!
        gs = castLead.GetComponent<GuardScript>();
        cast = new Dictionary<string, GameObject>();

        for (int i = 0; i < castNames.Length; i++)
        {
            GameObject castMember = Instantiate(castMemberPrefab);
            castMember.name = castNames[i];

            // group connection
            if (i == 4)
            {
                castMember.GetComponent<Demo3Character>().EntityType = EntityType.GROUP;
                graph.AddDirectConnection(new Connection(castLead, castMember, member));
            }

            // direct connections
            if (i < 4)
            {
                graph.AddDirectConnection(new Connection(castLead, castMember, friend));
            }

            // indirect connection
            if (i > 5)
            {
                graph.AddConnection(castLead.GetComponent<Demo3Character>(), new Connection(cast[castNames[3]], cast[castNames[5]], rival));
            }

            // connection known through group i.e. group knows so I know
            if (i == 10)
            {
                graph.AddDirectConnection(new Connection(cast[castNames[4]], cast[castNames[8]], enemy));
            }

            // otherwise, the entities are unknown

            int ypos = 250 - (i * 20);
            GameObject castCheckbox = Instantiate(castMemberCheckPrefab, new Vector3(90, ypos, 0), Quaternion.identity) as GameObject;
            castCheckbox.transform.SetParent(canvas.transform);
            castCheckbox.GetComponentInChildren<Text>().text = castNames[i];
            castCheckbox.name = castNames[i];
            castCheckbox.GetComponent<Toggle>().group = GameObject.FindGameObjectWithTag("CastToggleGroup").GetComponent<ToggleGroup>();
            if (i == 0)
                castCheckbox.GetComponent<Toggle>().isOn = true;

            // track the cast members
            cast.Add(castNames[i], castMember);
        }

        activeRelationshipType = RelationshipType.FRIEND;
        activeCastMember = cast[castNames[0]];
	}

    // this will construct a connection using the given source,
    // and destination entities, relationship type and value
    // and send that to the message handler
    public void SendConnectionMessage()
    {
        InputField weightInput = GameObject.FindGameObjectWithTag("Weight").GetComponent<InputField>();
        SetActiveCastMember();

        // first we build the connection to send by fetching the values we set in the
        // UI

        // get the weight and store it as an int
        string weightString = weightInput.text;
        int weight = int.Parse(weightString);

        RelationshipType type = activeRelationshipType;
        Relationship rel = new Relationship(type, weight);

        GameObject to = activeCastMember;

        // our "from" attribute is always the castLead for this demo
        Connection connection = new Connection(castLead, to, rel);

        // then we send it
        ConnectionMessage connMessage = new ConnectionMessage(connection);
        castLead.GetComponent<Demo3Character>().HandleMessage(connMessage);
    }

    // check the graph to see if we "know" the current active cast member
    public void KnowCastMember()
    {
        SetActiveCastMember();

        Demo3Character leadCharacter = castLead.GetComponent<Demo3Character>();
        Demo3Character activeCharacter = activeCastMember.GetComponent<Demo3Character>();

        // check if the active member is in either indirect or direct relationships
        bool knowsOf = graph.KnowsConnectionsOf(leadCharacter, activeCharacter);
        bool connectedTo = graph.HasConnection(leadCharacter, activeCharacter);

        if (knowsOf)
        {
            gs.SetResponse(Responses.AGREE, "YES I KNOW OF " + activeCastMember.name);
        }
        else if(connectedTo)
        {
            gs.SetResponse(Responses.AGREE, "YES, I KNOW " + activeCastMember.name + " PERSONALLY");
        }
        else
        {
            gs.SetResponse(Responses.DISAGREE, "I DON'T KNOW WHO THAT IS");
        }

    }

    public void SetActiveRelationshipType()
    {
        // this is an EXTREMELY poor hack but will work for now
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("RELATIONSHIP"))
        {
            if(go.GetComponent<Toggle>().isOn)
            {
                activeRelationshipType = string_enums[go.name];
            }
        }
    }

    public void SetActiveCastMember()
    {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("CASTMEMBER"))
        {
            if(go.GetComponent<Toggle>().isOn)
            {
                activeCastMember = cast[go.name];
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

    public void PositiveReaction(string message = "Yeah, yeah what about it")
    {
        gs.SetResponse(Responses.AGREE, message);
    }

    public void NegativeReaction(string message = "Nope, Get outta here!")
    {
        gs.SetResponse(Responses.DISAGREE, message);
    }

    public void AngryReaction(string message = "ARGH, DON'T GET ME STARTED!")
    {
        gs.SetResponse(Responses.ANGRY, message);
    }

    public void HappyReaction(string message = "Hahahaha, yeah I know!")
    {
        gs.SetResponse(Responses.HAPPY, message);
    }

    public void DismissiveReaction(string message = "I don't care about that!")
    {
        gs.SetResponse(Responses.DISMISSIVE, message);
    }
}
