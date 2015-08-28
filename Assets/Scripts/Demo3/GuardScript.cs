using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Responses
{
    IDLERESPONSE = 0,
    AGREE,
    DISAGREE,
    MESSAGE
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
    public void SetResponse(Responses responseType = Responses.IDLERESPONSE, string message = "")
    {
        if(responseType == Responses.IDLERESPONSE)
        {
            // on first load, just say hi.
            if (firstLoad)
            {
                text.text = "I'm " + this.name + "! What do you want?";
                firstLoad = false;
            }
            else
            {
                int choice = Random.Range(0, idleResponses.Length + 3);
                // otherwise pick one from a random set of messages
                if (choice < idleResponses.Length)
                    text.text = idleResponses[choice];
                else
                    text.text = message;
            }
        }
        else {
            text.text = message;
            
            // call the relevant animation
            switch(responseType)
            {
                case Responses.AGREE:    anim.SetTrigger("agree"); break;
                case Responses.DISAGREE: anim.SetTrigger("disagree"); break;
                case Responses.MESSAGE:  anim.SetTrigger("message"); break;
            }
        }
    }
}
