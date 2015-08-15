using UnityEngine;
using System.Collections;

[System.Flags]
public enum BehaviourType
{
    None = 0,
    SEEK = 1,
    FLEE = 2,
    ARRIVE = 4,
    PURSUIT = 8
}

public class SteeringBehaviours
{
    // reference to the vehicle to be moved
    private Vehicle vehicle;
    private Vector3 steeringForce;
    private Vector3 target;
    private Rigidbody rigidbody;
    private BehaviourType behaviourType = BehaviourType.FLEE;

    public SteeringBehaviours(Vehicle vehicle)
    {
        this.vehicle = vehicle;
        rigidbody = this.vehicle.GetComponent<Rigidbody>();
        // retrieve the target from the vehicle settings
        target = this.vehicle.target.position;
    }

    public bool On(BehaviourType type)
    {
        return (behaviourType & type) == type;
    }

    public BehaviourType BehaviourType
    {
        get
        {
            return behaviourType;
        }

        set
        {
            behaviourType = value;
        }
    }

    public Vector3 Calculate()
    {
        steeringForce = Vector3.zero;

        if(On(BehaviourType.SEEK))
        {
            Vector3 force = Seek();

            if (!AccumulateForce(force))
                return steeringForce;
        }

        if (On(BehaviourType.FLEE))
        {
            Vector3 force = Flee();

            if (!AccumulateForce(force))
                return steeringForce;
        }

        return steeringForce;
    }

    public Vector3 Seek()
    {
        Vector3 desiredV = (target - rigidbody.position).normalized * vehicle.maxSpeed;

        return (desiredV - rigidbody.velocity);
    }

    public Vector3 Flee()
    {
        Vector3 desiredV = (rigidbody.position - target).normalized * vehicle.maxSpeed;

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
