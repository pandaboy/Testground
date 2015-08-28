using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
	
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            SetResponse("Yeah, Yeah what about it");
            anim.SetTrigger("agree");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            SetResponse("Nope, Get outta here!");
            anim.SetTrigger("disagree");
        }
	}

    public void SetResponse(string response = "", bool responseType = true)
    {
        if(response == "")
        {
            // on first load, just say hi.
            if (firstLoad)
            {
                text.text = "I'm " + this.name + "! What do you want?";
                firstLoad = false;
            }
            else
            {
                // otherwise pick one from a random set of messages
                string responseText = idleResponses[Random.Range(0, idleResponses.Length)];
                text.text = responseText;
            }
        }
        else {
            text.text = response;
            if (responseType)
                anim.SetTrigger("agree");
            else
                anim.SetTrigger("disagree");
        }
    }
}
