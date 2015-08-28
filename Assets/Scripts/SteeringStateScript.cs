using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Vehicle))]
public class SteeringStateScript : MonoBehaviour
{
    private BehaviourType behaviourType = BehaviourType.WANDER;

	// Use this for initialization
	void Start ()
    {
        //ChangeState(BehaviourType.None);
	}

    public void ChangeState(BehaviourType newType, GameObject other = null)
    {
        behaviourType = newType;
        SwitchSteering(newType, other);
        SwitchColor(newType);
    }

    void SwitchSteering(BehaviourType newType, GameObject other = null)
    {
        Vehicle v = GetComponent<Vehicle>();
        switch (newType)
        {
            case BehaviourType.PURSUIT:
                v.behaviourType = BehaviourType.PURSUIT;
                v.Steering.BehaviourType = BehaviourType.PURSUIT;
                v.Steering.OtherVehicle = other.GetComponent<Vehicle>();
                v.other = other;
                v.target = null;
                break;

            case BehaviourType.EVADE:
                v.behaviourType = BehaviourType.EVADE;
                v.Steering.BehaviourType = BehaviourType.EVADE;
                v.Steering.OtherVehicle = other.GetComponent<Vehicle>();
                v.other = other;
                v.target = null;
                break;

            case BehaviourType.ARRIVE:
                v.behaviourType = BehaviourType.ARRIVE;
                v.target = other.transform;
                v.other = null;
                break;

            case BehaviourType.WANDER:
                v.behaviourType = BehaviourType.WANDER;
                v.other = null;
                v.target = null;
                break;

            default:
                v.behaviourType = BehaviourType.None;
                v.other = null;
                v.target = null;
                break;
        }
    }

    void SwitchColor(BehaviourType newType)
    {
        Color newColor;
        switch (newType)
        {
            case BehaviourType.PURSUIT:
                newColor = new Color(0.8f, 0.1f, 0.1f);
                break;
            case BehaviourType.EVADE:
                newColor = new Color(0.1f, 0.1f, 0.8f);
                break;
            case BehaviourType.None:
                newColor = new Color(0.8f, 0.8f, 0.8f);
                break;
            default:
                newColor = new Color(0.8f, 0.8f, 0.8f);
                break;
        }

        GetComponent<Renderer>().material.color = newColor;
    }
}
