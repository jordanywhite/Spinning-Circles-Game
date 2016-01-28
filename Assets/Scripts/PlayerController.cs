using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{


	float moveSpeed =3f;
	GameObject object1;
	GameObject object2;


	void Start()
	{
		object1 = GameObject.Find("Player");
		object2 = GameObject.Find("Circle1");
			
		
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

	private void OnTriggerEnter2D (Collider2D other)
	{
		
		if(other.tag == "Circle 1")
		{

			object1.transform.parent = object2.transform;
		
	    }

}



}