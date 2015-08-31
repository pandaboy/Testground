using UnityEngine;
using System.Collections;

public class MoveCharacter : MonoBehaviour
{

    public float speed;

    private Rigidbody rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
        Vector3 movement = new Vector3(
            Input.GetAxis("Horizontal"),
            0.0f,
            Input.GetAxis("Vertical")
        );

        rb.AddForce(movement * speed);
	}
}
