using UnityEngine;
using System.Collections;

[System.Flags]
public enum BehaviourType
{
    None = 0,
    SEEK = 1,
    FLEE = 2,
    ARRIVE = 4,
    PURSUIT = 8,
    EVADE = 16,
    WANDER = 32
}

public enum Deceleration
{
    FAST = 1,
    MEDIUM,
    SLOW
}

public class SteeringBehaviours
{
    #region Private Members
    // reference to the vehicle to be moved
    private Vehicle vehicle;
    private Vector3 steeringForce;
    private Vector3 target;
    private Rigidbody rigidbody;
    private BehaviourType behaviourType = BehaviourType.ARRIVE;
    private Vehicle otherVehicle = null;

    // wander settings
    private float wanderRadius = 1.2f;
    private float wanderDistance = 2.0f;
    private float wanderJitter = 80.0f;
    private Vector3 wanderTarget;
    #endregion Private Members

    public SteeringBehaviours(Vehicle vehicle)
    {
        this.vehicle = vehicle;
        rigidbody = this.vehicle.GetComponent<Rigidbody>();
        // retrieve the target from the vehicle settings
        target = this.vehicle.target.position;
        BehaviourType = this.vehicle.behaviourType;
        
        if(this.vehicle.other != null)
            otherVehicle = this.vehicle.other.GetComponent<Vehicle>();

        wanderTarget = Random.onUnitSphere * wanderRadius;
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
            if (!AccumulateForce(Seek(target)))
                return steeringForce;
        }

        if (On(BehaviourType.FLEE))
        {
            if (!AccumulateForce(Flee(target)))
                return steeringForce;
        }

        if (On(BehaviourType.ARRIVE))
        {
            if (!AccumulateForce(Arrive(target, Deceleration.MEDIUM)))
                return steeringForce;
        }

        if(On(BehaviourType.PURSUIT))
        {
            if (!AccumulateForce(Pursuit(otherVehicle)))
                return steeringForce;
        }

        if(On(BehaviourType.EVADE))
        {
            if (!AccumulateForce(Evade(otherVehicle)))
                return steeringForce;
        }

        if(On(BehaviourType.WANDER))
        {
            if (!AccumulateForce(Wander()))
                return steeringForce;
        }

        return steeringForce;
    }

    public Vector3 Seek(Vector3 targetPosition)
    {
        Vector3 desiredV = (targetPosition - vehicle.Position).normalized * vehicle.maxSpeed;

        return (desiredV - vehicle.Velocity);
    }

    public Vector3 Flee(Vector3 targetPosition, float panicDistance = 10.0f)
    {
        // only flee if enemy within radius
        panicDistance *= panicDistance;

        // if we're far enough away, no reason to panic
        if ((vehicle.Position - targetPosition).sqrMagnitude > panicDistance)
            return Vector3.zero;

        Vector3 desiredV = (vehicle.Position - targetPosition).normalized * vehicle.maxSpeed;

        return (desiredV - vehicle.Velocity);
    }

    public Vector3 Arrive(Vector3 targetPosition, Deceleration deceleration)
    {
        Vector3 toTarget = targetPosition - vehicle.Position;

        float distToTarget = toTarget.magnitude;

        if(distToTarget > 0)
        {
            const float decelerationTweaker = 0.3f;

            float speed = distToTarget / ((float)deceleration * decelerationTweaker);

            Mathf.Min(speed, vehicle.maxSpeed);

            Vector3 desiredV = toTarget * (speed / distToTarget);

            return desiredV - vehicle.Velocity;
        }

        return Vector3.zero;
    }

    public Vector3 Pursuit(Vehicle evader)
    {
        // if evader is ahead and facing the agent, just seek to the evaders
        // current position
        Vector3 toEvader = evader.Position - vehicle.Position;

        float relativeHeading = Vector3.Dot(vehicle.Forward, evader.Forward);

        if (Vector3.Dot(toEvader, vehicle.Forward) > 0 && (relativeHeading < -0.95f))
            return Seek(evader.Position);

        // try to predict where the evader will be
        float lookAheadTime = toEvader.magnitude / (vehicle.maxSpeed + evader.Speed);

        lookAheadTime += TurnaroundTime(this.vehicle, evader.Position);

        // seek to the predicted future position
        return Seek(evader.Position + evader.Velocity * lookAheadTime);
    }

    public Vector3 Evade(Vehicle pursuer)
    {
        Vector3 toPursuer = pursuer.Position - vehicle.Position;

        // look ahead time is proportional to the distance between
        // the pursuer and the evader, and inversely proportional to
        // the sum of the two vehicle's velocities
        float lookAheadTime = toPursuer.magnitude / (vehicle.maxSpeed + pursuer.Speed);

        // flee from predicted future position of pursuer
        return Flee(pursuer.Position + pursuer.Velocity * lookAheadTime);
    }

    public Vector3 Wander()
    {
        float jitterSlice = wanderJitter * Time.deltaTime;

        wanderTarget += new Vector3(
            Random.Range(-1.0f, 1.0f) * jitterSlice,
            0.0f,
            Random.Range(-1.0f, 1.0f) * jitterSlice
        );

        // reproject back onto a unit circle
        wanderTarget.Normalize();

        // increase length of vector to the same as the radius of the wander circle
        wanderTarget *= wanderRadius;

        // move the target wanderDistance ahead of the vehicle
        Vector3 targetLocal = wanderTarget + new Vector3(wanderDistance, 0, wanderDistance);
        //Vector3 targetLocal = wanderTarget + (vehicle.Forward * wanderDistance);

        // project target to world space
        Vector3 targetWorld = vehicle.transform.TransformPoint(targetLocal);

        // steer towards it
        return targetWorld - vehicle.Position;
    }

    float TurnaroundTime(Vehicle agent, Vector3 target)
    {
        // normalised vector to the target
        Vector3 toTarget = (target - agent.Position).normalized;

        // dot product will be 1 if the target is directly ahead
        // and -1 if the target is directly behind.
        float dot = Vector3.Dot(agent.Heading, toTarget);

        // higher value for higher max turn rate.
        // 0.5f means the vehicle will take 1 second to
        // turnaround to its target from facing the opposite way.
        float coefficient = 0.5f;

        // subtracting by 1 and multiplying by the negative of the
        // coefficient will give a positive value proportional to 
        // the rotational displacement of the vehicle and target
        return (dot - 1.0f) * -coefficient;
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
