using UnityEngine;
using System.Collections;

public class CoroutineExample : MonoBehaviour {

    IEnumerator Fade()
    {
        for(float i = 1f; i >= 0.0f; i -= 0.1f)
        {
            Color c = GetComponent<Renderer>().material.color;
            c.r = i;
            GetComponent<Renderer>().material.color = c;
            yield return null;
        }
    }

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = new Color(1.0f, 0.1f, 0.1f);
	}
	
	// Update is called once per frame
    void Update()
    {
        StartCoroutine("Fade");
	}
}
