using UnityEngine;
using System.Collections;
using MonoGraph;

[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MovingEntity
{
    #region private members
    private SteeringBehaviours steering;
    public SteeringBehaviours Steering
    {
        get
        {
            return steering;
        }
    }
    private Rigidbody rb;
    #endregion

    #region public members
    public Transform target;
    public BehaviourType behaviourType = BehaviourType.ARRIVE;
    // other vehicle (for evasion, pursuit, etc)
    public GameObject other = null;
    #endregion

    // Use this for initialization
	void Start ()
    {
        steering = new SteeringBehaviours(this);
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 steeringForce = steering.Calculate();
        Vector3 acceleration = steeringForce / rb.mass;

        rb.velocity += acceleration * Time.deltaTime;

        Vector3.ClampMagnitude(rb.velocity, maxForce);

        rb.position += velocity * Time.deltaTime;

        if(rb.velocity.sqrMagnitude > 0.00000001)
        {
            heading = rb.velocity.normalized;

            side = Vector3.Cross(heading, Vector3.up).normalized;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Vehicle otherVehicle = other.GetComponent<Vehicle>();
        if (other.tag == "NPC" && this.tag == "NPC" && otherVehicle.behaviourType != BehaviourType.PURSUIT)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            Entity player = playerObject.GetComponent<Entity>();

            // pass a message to become an enemy of the player
            Connection connection = new Connection(
                other.GetComponent<Entity>(),
                player,
                new RelationshipGraph.Relationships.Relationship(RelationshipGraph.RelationshipType.FRIEND, 5));

            Graph.Instance.AddDirectConnection(connection);
            other.GetComponent<Renderer>().material.color = new Color(.75f, .75f, 0.0f);
        }

        if(other.tag == "Player")
        {
            RelationshipGraph.Relationships.Relationship enemyR =
                new RelationshipGraph.Relationships.Relationship(RelationshipGraph.RelationshipType.ENEMY);
            RelationshipGraph.Relationships.Relationship friendlyR =
                new RelationshipGraph.Relationships.Relationship(RelationshipGraph.RelationshipType.FRIEND);
            Entity otherEntity = other.GetComponent<Entity>();
            Entity thisEntity = GetComponent<Entity>();
            
            // if it's an enemy, change state to evade!
            if(Graph.Instance.HaveRelationship(thisEntity, otherEntity, enemyR))
            {
                GetComponent<SteeringStateScript>().ChangeState(BehaviourType.EVADE, other.gameObject);
            }

            if (Graph.Instance.HaveRelationship(thisEntity, otherEntity, friendlyR))
            {
                GetComponent<SteeringStateScript>().ChangeState(BehaviourType.PURSUIT, other.gameObject);
            }
        }
    }
}
