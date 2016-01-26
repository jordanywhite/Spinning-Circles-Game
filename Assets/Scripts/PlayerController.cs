using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{


	private float moveSpeed = 5f;



	void Start()
	{
		
	}

	void Update()
	{



		if(Input.GetKeyDown (KeyCode.D))
		{

			transform.position += new Vector3(moveSpeed * Time.deltaTime, 0.0f,0.0f);
		}


		if(Input.GetKeyDown (KeyCode.A))
		{

			transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
		}
		if(Input.GetKeyDown (KeyCode.W))
		{

			transform.position += new Vector3(0.0f, moveSpeed * Time.deltaTime, 0.0f);
		}
		if(Input.GetKeyDown (KeyCode.S))
		{

			transform.position -= new Vector3(0.0f, moveSpeed * Time.deltaTime, 0.0f);
		}
	}
}