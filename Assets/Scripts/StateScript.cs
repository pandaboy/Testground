using UnityEngine;
using System.Collections;

public enum States
{
    NEUTRAL = 0,
    AGGRESSIVE,
    SUPPORTIVE
}

public class StateScript : MonoBehaviour {

    public States state = States.NEUTRAL;

	// Use this for initialization
	void Start () {
        ChangeColor();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void ChangeState(States newState)
    {
        state = newState;
        ChangeColor();
    }

    void ChangeColor()
    {
        Color newColor;
        switch(this.state)
        {
            case States.AGGRESSIVE: newColor = new Color(0.8f, 0.1f, 0.1f); break;
            case States.SUPPORTIVE: newColor = new Color(0.1f, 0.1f, 0.8f); break;
            case States.NEUTRAL:
            default:
                newColor = new Color(0.8f, 0.8f, 0.8f);
                break;
        }

        GetComponent<Renderer>().material.color = newColor;
    }
}
