using UnityEngine;
using System.Collections;

public class MovingEntity : MonoBehaviour
{
    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }

    protected Vector3 velocity;
    public Vector3 Velocity
    {
        get
        {
            return velocity;
        }

        set
        {
            velocity = value;
        }
    }

    public float Speed
    {
        get
        {
            return velocity.magnitude;
        }
    }

    public float SpeedSquared
    {
        get
        {
            return velocity.sqrMagnitude;
        }
    }

    protected Vector3 heading;
    public Vector3 Heading
    {
        get
        {
            return heading;
        }

        set
        {
            heading = value;
        }
    }

    protected Vector3 side;
    public Vector3 Side
    {
        get
        {
            return side;
        }

        set
        {
            side = value;
        }
    }

    // available from the inspector
    public float maxSpeed;
    public float maxForce;
    public float turnRate;
}
