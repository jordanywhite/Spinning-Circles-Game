using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    Stack<GameObject> onCircle;

	float moveSpeed =3f;
	GameObject player;
	GameObject circleOne;
    GameObject circleTwo;
    GameObject circleThree;
	GameObject circleFour;
	GameObject circleFive;
    GameObject centerCircle;
	GameObject Fire;
	Animator animator;
    Quaternion rotation;

    GameObject[] spawnPoints;

    GameObject currentSpawn;

    bool canMove = true;


    void Start()
	{
		player = GameObject.Find("Player");
		circleOne = GameObject.Find("Circle1");
        circleTwo = GameObject.Find("Circle2");
        circleThree = GameObject.Find("Circle3");
		circleFour = GameObject.Find("Circle4");
		circleFive = GameObject.Find("Circle5");
		Fire = GameObject.Find("Fire");

        GameObject spawnOne = GameObject.Find("Spawn1");

        currentSpawn = spawnOne;

        spawnPoints = new GameObject[] {spawnOne};

        rotation = player.transform.rotation;

		animator = GetComponent<Animator> ();
        onCircle = new Stack<GameObject>();
        onCircle.Push(null);
    }

	void Update()
	{
        bool isMoving = false;

        // Lock rotation
        player.transform.rotation = rotation;

        if(!canMove)
        {
            return;
        }

        if ((Input.GetKey(KeyCode.RightArrow))||(Input.GetKey(KeyCode.D)))
		{
			transform.position += new Vector3(moveSpeed * Time.deltaTime, 0.0f,0.0f);
            isMoving = true;
            
		}
		if((Input.GetKey(KeyCode.LeftArrow))||(Input.GetKey(KeyCode.A)))
		{
			
			transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
            isMoving = true;
        }
	    if((Input.GetKey(KeyCode.UpArrow))||(Input.GetKey(KeyCode.W)))
		{
			
			transform.position += new Vector3(0.0f, moveSpeed * Time.deltaTime, 0.0f);
            isMoving = true;
        }
		if((Input.GetKey(KeyCode.DownArrow))||(Input.GetKey(KeyCode.S)))
		{
			
			transform.position -= new Vector3(0.0f, moveSpeed * Time.deltaTime, 0.0f);
            isMoving = true;
        }

        setMoving(isMoving);
    }

    private void setMoving(bool isMoving)
    {
        if(isMoving)
        {
            animator.ResetTrigger("CatIdle");
            animator.SetTrigger("CatWalk");
        }
        else
        {
            animator.SetTrigger("CatIdle");
            animator.ResetTrigger("CatWalk");
        }
    }

    private IEnumerator catDied()
    {
        animator.SetTrigger("CatDead");
        animator.ResetTrigger("CatIdle");
        animator.ResetTrigger("CatWalk");
        canMove = false;

        player.transform.parent = null;
        yield return new WaitForSeconds(1);

        respawn();
    }

    private void respawn()
    {
        canMove = true;
        animator.ResetTrigger("CatWalk");
        animator.ResetTrigger("CatDead");
        player.transform.position = currentSpawn.transform.position;
    }

	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == circleOne.tag || other.tag == circleTwo.tag || other.tag == circleThree.tag || other.tag == circleFour.tag || other.tag == circleFive.tag) {
			print ("in: " + other.tag);
			onCircle.Push (other.gameObject);
			player.transform.parent = other.gameObject.transform;
		} else if (other.tag == Fire.tag) {
            StartCoroutine((catDied()));
		}

       

    }

    private void OnTriggerExit2D(Collider2D other)
    {
		if (other.tag == circleOne.tag || other.tag == circleTwo.tag || other.tag == circleThree.tag|| other.tag == circleFour.tag|| other.tag == circleFive.tag)
        {
            print("out: " + other.tag);
            GameObject circle = onCircle.Pop();

            if (onCircle.Peek() != null)
            {
                player.transform.parent = onCircle.Peek().transform;
            }
            else
            {
                player.transform.parent = null;
            }
        }
    }




}