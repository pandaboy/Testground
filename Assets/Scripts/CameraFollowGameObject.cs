using UnityEngine;
using System.Collections;

public class CameraFollowGameObject : MonoBehaviour {

    public GameObject player;

    private Vector3 offset;

    void Awake()
    {
        player = FindObjectOfType<DemoGameController>().leadCharacter;
    }

	// Use this for initialization
	void Start () {
        offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.transform.position + offset;
	}
}
