using UnityEngine;
using System.Collections;

public class CameraFollowGameObject : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    void Awake()
    {
        player = FindObjectOfType<DemoGameController>().leadCharacter;
    }

	void Start ()
    {
        // offset the camera from the player position
        offset = transform.position - player.transform.position;
	}
	
	void LateUpdate ()
    {
        // update the camera position by tracking the player position
        transform.position = player.transform.position + offset;
	}
}
