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
    public string[] castNames = new string[4];
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
            graph.AddDirectConnection(new Connection(castLead, castMember, friend));

            int ypos = 125 - (i * 20);
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
	
	// Update is called once per frame
	void Update ()
    {
        /*
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
        */
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

        //RelationshipType type = RelationshipType.FRIEND; // relationship type
        RelationshipType type = activeRelationshipType;
        Relationship rel = new Relationship(type, weight);

        //GameObject to = cast[0]; // cast member
        GameObject to = activeCastMember;

        // our "from" attribute is always the castLead for this demo
        Connection connection = new Connection(castLead, to, rel);

        // then we send it
        ConnectionMessage connMessage = new ConnectionMessage(connection);
        castLead.GetComponent<Demo3Character>().HandleMessage(connMessage);
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

    public void PositiveReaction()
    {
        gs.SetResponse(Responses.AGREE, "Yeah, yeah what about it");
    }

    public void NegativeReaction()
    {
        gs.SetResponse(Responses.DISAGREE, "Nope, Get outta here!");
    }
}
