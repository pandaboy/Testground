using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Responses
{
    IDLERESPONSE = 0,
    AGREE,
    DISAGREE,
    MESSAGE,
    HAPPY,
    DISMISSIVE,
    ANGRY
}

public class GuardScript : MonoBehaviour
{
    Animator anim;
    public Text text;
    public string[] idleResponses = new string[5];
    bool firstLoad = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // calls the relevant animation and displays a response during the animation
    public void SetResponse(Responses responseType = Responses.IDLERESPONSE, string msg = "")
    {
        if(responseType == Responses.IDLERESPONSE)
        {
            // on first load, just say hi.
            if (firstLoad)
            {
                firstLoad = false;
                text.text = "I'm " + this.name + "! What do you want?";
            }
            else
            {
                int choice = Random.Range(0, idleResponses.Length + 3);
                // otherwise pick one from a random set of messages
                if (choice < idleResponses.Length)
                {
                    text.text = idleResponses[choice];
                }
                else
                {
                    text.text = msg;
                }
            }
        }
        else {
            text.text = msg;
            
            // call the relevant animation
            switch(responseType)
            {
                case Responses.AGREE:      anim.SetTrigger("agree");      break;
                case Responses.DISAGREE:   anim.SetTrigger("disagree");   break;
                case Responses.MESSAGE:    anim.SetTrigger("message");    break;
                case Responses.ANGRY:      anim.SetTrigger("angry");      break;
                case Responses.HAPPY:      anim.SetTrigger("happy");      break;
                case Responses.DISMISSIVE: anim.SetTrigger("dismissive"); break;
            }
        }
    }
}
