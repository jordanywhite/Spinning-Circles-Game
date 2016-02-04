﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public const int LEFT = 0;
    public const int RIGHT = 1;
    public const int UP = 2;
    public const int DOWN = 3;

    private bool[] facingDirs;

    Stack<GameObject> onCircle;

	float moveSpeed;
	GameObject player;
	GameObject circleOne;
    GameObject circleTwo;
    GameObject circleThree;
	GameObject circleFour;
	GameObject circleFive;
    GameObject centerCircle;
	GameObject keyOne;
	GameObject keyTwo;
	GameObject keyThree;
	GameObject door;
	GameObject Fire;
	Animator animator;
    Quaternion rotation;
	bool key1 = false;
	bool key2 = false;
	bool key3 = false;

    public float restartLevelDelay;

    GameObject[] spawnPoints;

    GameObject currentSpawn;

    bool canMove = true;

    //tags
    string fireTag = "Fire";
    string webTag = "Web";
    string pitfallTag = "Pitfall";
    string spikesTag = "Spikes";
    string fruitTag = "Fruit";
	string key1Tag = "key 1";
	string key2Tag = "key 2";
	string key3Tag = "key 3";
	string doorTag = "door";




    void Start()
	{
		player = GameObject.Find("Player");
		circleOne = GameObject.Find("Circle1");
        circleTwo = GameObject.Find("Circle2");
        circleThree = GameObject.Find("Circle3");
		circleFour = GameObject.Find("Circle4");
		circleFive = GameObject.Find("Circle5");
		keyOne = GameObject.Find ("key 1");
		keyTwo = GameObject.Find ("key 2");
		keyThree = GameObject.Find ("key 3");
		door = GameObject.Find ("door");

        restartLevelDelay = 1f;

        moveSpeed = 3F;

        // Start facing right
        facingDirs = new bool[] { false, true, false, false };

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
		// Lock rotation
        player.transform.rotation = rotation;

        moveUpdate();

    }

    private void moveUpdate()
    {
        bool isMoving = false;

        if (canMove)
        {
            if ((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)))
            {
                transform.position += new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
                isMoving = true;
                facingDirs[RIGHT] = true;
                facingDirs[LEFT] = false;
            }

            if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)))
            {

                transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0.0f, 0.0f);
                isMoving = true;
                facingDirs[LEFT] = true;
                facingDirs[RIGHT] = false;
            }
            if ((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W)))
            {

                transform.position += new Vector3(0.0f, moveSpeed * Time.deltaTime, 0.0f);
                isMoving = true;
                facingDirs[UP] = true;
                facingDirs[DOWN] = false;
            }
            if ((Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.S)))
            {

                transform.position -= new Vector3(0.0f, moveSpeed * Time.deltaTime, 0.0f);
                isMoving = true;
                facingDirs[DOWN] = true;
                facingDirs[UP] = false;
            }

            faceDirection();
            setMoving(isMoving);
        }
    }

    private void faceDirection()
    {
        if (facingDirs[LEFT])
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (facingDirs[RIGHT])
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
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

        yield return new WaitForSeconds(1);

        respawn();
    }

    private void levelCompleted()
    {
        GameManager.instance.victoryText.enabled = true;
        GameManager.instance.victoryText.text += GameManager.instance.timerLabel.text;
        GameManager.instance.stopTimer();
    }

    private void respawn()
    {
		GameManager.instance.resetTimer ();
		canMove = true;
        animator.ResetTrigger("CatWalk");
        animator.ResetTrigger("CatDead");

        player.transform.position = currentSpawn.transform.position;

        GameManager.instance.resetTimer();
        GameManager.instance.startTimer();

        player.transform.parent = null;
        onCircle.Clear();
        onCircle.Push(null);
    }


    // Player enters a trigger
	private void OnTriggerEnter2D (Collider2D other)
	{
        // Attach player to a circle when entering a circle
		if (other.tag == circleOne.tag || other.tag == circleTwo.tag || other.tag == circleThree.tag || 
            other.tag == circleFour.tag || other.tag == circleFive.tag)
        {
			onCircle.Push (other.gameObject);
			player.transform.parent = other.gameObject.transform;
		}

        // Fatal obstacle encountered
        else if (other.tag == fireTag || other.tag == spikesTag)
        {
            StartCoroutine((catDied()));
		} 

        // Slowed down from web
        else if (other.tag == webTag)
        {
            moveSpeed = 1F;
        }

        // Fruit collection successful
        else if (other.tag == fruitTag)
        {
            levelCompleted();
        }
    }

    // Player leaves a trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        // Left a Circle to enter an outer circle or no circle
		if (other.tag == circleOne.tag || other.tag == circleTwo.tag || other.tag == circleThree.tag|| 
            other.tag == circleFour.tag|| other.tag == circleFive.tag)
        {
            // Forget the last circle the player was on
            GameObject circle = onCircle.Pop();

            // Check if the player is entering a previously entered circle
            if (onCircle.Peek() != null)
            {

                // Attach to the circle the player entered previous to circle the player just left
                player.transform.parent = onCircle.Peek().transform;
            }

            // The player is no longer on a circle
            else
            {
                // Destroy the parent
                player.transform.parent = null;
            }
        }

        // Restore player movement speed
        else if (other.tag == webTag)
        {
            moveSpeed = 3F;
        }
		else if (other.tag == key1Tag)
		{
			key1 = true;
		}
		else if (other.tag == key2Tag)
		{
			key2 = true;
		}
		else if (other.tag == key3Tag)
		{
			key3 = true;
		}
		else if (other.tag == doorTag)
		{
			if (key1 == true && key2 == true && key3 == true) {
				door.SetActive (false);
			}
		}
    }


    private void Restart()
    {
        SceneManager.LoadScene("Level " + GameManager.instance.getLevelNumber());
    }

}