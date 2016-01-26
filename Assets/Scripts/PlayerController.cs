using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{


	private float moveSpeed = 3f;

	void Start()
	{
		
	}

	void Update()
	{

 

		if((Input.GetKey(KeyCode.RightArrow))||(Input.GetKey(KeyCode.D)))
		{
			transform.position += new Vector3(moveSpeed * Time.deltaTime, 0.0f,0.0f);
		}
		if((Input.GetKey(KeyCode.LeftArrow))||(Input.GetKey(KeyCode.A)))
		{
			transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
		}
		if((Input.GetKey(KeyCode.UpArrow))||(Input.GetKey(KeyCode.W)))
		{
			transform.position += new Vector3(0.0f, moveSpeed * Time.deltaTime, 0.0f);
		}
		if((Input.GetKey(KeyCode.DownArrow))||(Input.GetKey(KeyCode.S)))
		{
			transform.position -= new Vector3(0.0f, moveSpeed * Time.deltaTime, 0.0f);
		}
	}

//	private void OnTriggerEnter2D (Collider2D other)
//	{
//		//Check if the tag of the trigger collided with is Exit.
//		if(other.tag == "Circle 1")
//		{
//			transform.Rotate(Vector3.forward * 7);
//		}
//
//		if(other.tag == "Circle 2")
//		{
//			transform.Rotate (Vector3.forward * -2);
//		}
//	}



}