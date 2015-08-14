using UnityEngine;
using System.Collections;

public class MovingEntity : MonoBehaviour
{
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
