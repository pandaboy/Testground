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

public enum Deceleration
{
    FAST = 1,
    MEDIUM,
    SLOW
}

public class SteeringBehaviours
{
    // reference to the vehicle to be moved
    private Vehicle vehicle;
    private Vector3 steeringForce;
    private Vector3 target;
    private Rigidbody rigidbody;
    private BehaviourType behaviourType = BehaviourType.ARRIVE;
    private Vehicle pursuitVehicle = null;

    public SteeringBehaviours(Vehicle vehicle)
    {
        this.vehicle = vehicle;
        rigidbody = this.vehicle.GetComponent<Rigidbody>();
        // retrieve the target from the vehicle settings
        target = this.vehicle.target.position;
        BehaviourType = this.vehicle.behaviourType;
        
        if(this.vehicle.pursuit != null)
            pursuitVehicle = this.vehicle.pursuit.GetComponent<Vehicle>();
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
            Vector3 force = Seek(target);

            if (!AccumulateForce(force))
                return steeringForce;
        }

        if (On(BehaviourType.FLEE))
        {
            Vector3 force = Flee(target);

            if (!AccumulateForce(force))
                return steeringForce;
        }

        if (On(BehaviourType.ARRIVE))
        {
            Vector3 force = Arrive(target, Deceleration.MEDIUM);

            if (!AccumulateForce(force))
                return steeringForce;
        }

        if(On(BehaviourType.PURSUIT))
        {
            Vector3 force = Pursuit(pursuitVehicle);

            if (!AccumulateForce(force))
                return steeringForce;
        }

        return steeringForce;
    }

    public Vector3 Seek(Vector3 targetPosition)
    {
        Vector3 desiredV = (targetPosition - rigidbody.position).normalized * vehicle.maxSpeed;

        return (desiredV - rigidbody.velocity);
    }

    public Vector3 Flee(Vector3 targetPosition, float panicDistance = 10.0f)
    {
        // only flee if enemy within radius
        panicDistance *= panicDistance;

        // if we're far enough away, no reason to panic
        if ((rigidbody.position - targetPosition).sqrMagnitude > panicDistance)
            return Vector3.zero;

        Vector3 desiredV = (rigidbody.position - targetPosition).normalized * vehicle.maxSpeed;

        return (desiredV - rigidbody.velocity);
    }

    public Vector3 Arrive(Vector3 targetPosition, Deceleration deceleration)
    {
        Vector3 toTarget = targetPosition - rigidbody.position;

        float distToTarget = toTarget.magnitude;

        if(distToTarget > 0)
        {
            const float decelerationTweaker = 0.3f;

            float speed = distToTarget / ((float)deceleration * decelerationTweaker);

            Mathf.Min(speed, vehicle.maxSpeed);

            Vector3 desiredV = toTarget * (speed / distToTarget);

            return desiredV - rigidbody.velocity;
        }

        return Vector3.zero;
    }

    public Vector3 Pursuit(Vehicle evader)
    {
        // if evader is ahead and facing the agent, just seek to the evaders
        // current position
        Vector3 toEvader = evader.Position - rigidbody.position;

        float relativeHeading = Vector3.Dot(vehicle.Heading, evader.Heading);

        if (Vector3.Dot(toEvader, vehicle.Heading) > 0 && (relativeHeading < -0.95f))
            return Seek(evader.Position);

        // try to predict where the evader will be
        float lookAheadTime = toEvader.magnitude / (vehicle.maxSpeed + evader.Speed);

        // seek to the predicted future position
        return Seek(evader.Position + evader.Velocity * lookAheadTime);
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
