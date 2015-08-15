using UnityEngine;
using System.Collections;

public class Vehicle : MovingEntity {

    private SteeringBehaviours steering;
    private Rigidbody rb;

    public Transform target;
    public BehaviourType behaviourType = BehaviourType.ARRIVE;

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
}
