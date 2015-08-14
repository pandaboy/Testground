using UnityEngine;
using System.Collections;

public class SteeringBehaviours
{
    // reference to the vehicle to be moved
    private Vehicle vehicle;
    private Vector3 steeringForce;
    private Vector3 target;
    private Rigidbody rigidbody;

    public SteeringBehaviours(Vehicle vehicle)
    {
        this.vehicle = vehicle;
        rigidbody = this.vehicle.GetComponent<Rigidbody>();
        // retrieve the target from the vehicle settings
        target = this.vehicle.target.position;
    }

    public Vector3 Calculate()
    {
        steeringForce = Vector3.zero;

        Vector3 force = Seek();

        if(!AccumulateForce(force))
            return steeringForce;

        return steeringForce;
    }

    public Vector3 Seek()
    {
        Vector3 desiredV = (target - rigidbody.position) * vehicle.maxSpeed;

        return (desiredV - rigidbody.velocity);
    }

    public bool AccumulateForce(Vector3 forceToAdd)
    {
        float magnitudeSoFar = steeringForce.magnitude;
        float magnitudeRemaining = vehicle.maxForce - magnitudeSoFar;

        if(magnitudeRemaining <= 0.0)
            return false;

        float magnitudeToAdd = forceToAdd.magnitude;

        if(magnitudeToAdd < magnitudeRemaining)
            steeringForce += forceToAdd;
        else
            steeringForce += forceToAdd.normalized * magnitudeRemaining;

        return true;
    }
}
