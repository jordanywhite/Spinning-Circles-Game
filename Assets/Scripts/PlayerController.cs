using UnityEngine;
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
	GameObject circleOne;
    GameObject circleTwo;
    GameObject circleThree;
	GameObject circleFour;
	GameObject circleFive;
    GameObject centerCircle;
	GameObject keyOne;
	GameObject keyTwo;
	GameObject keyThree;
	GameObject Fire;
	Animator animator;
    Quaternion rotation;

    public float restartLevelDelay;

    GameObject[] spawnPoints;

    private GameObject currentSpawn;

    private bool canMove = true;
    private bool level_finished = false;

    private int[] keyCount = new int[] { 0, 0, 0 };

    //tags
    string fireTag = "Fire";
    string webTag = "Web";
    string pitfallTag = "Pitfall";
    string spikesTag = "Spikes";
    string fruitTag = "Fruit";
	string door1Tag = "door 1";
    string door2Tag = "door 2";
    string door3Tag = "door 3";
    string key1Tag = "key 1";
    string key2Tag = "key 2";
    string key3Tag = "key 3";

    
    void Start()
	{
		circleOne = GameObject.Find("Circle1");
        circleTwo = GameObject.Find("Circle2");
        circleThree = GameObject.Find("Circle3");
		circleFour = GameObject.Find("Circle4");
		circleFive = GameObject.Find("Circle5");

        restartLevelDelay = 1f;

        moveSpeed = 3F;

        if(GameManager.instance.isNux())
        {
            moveSpeed = 6F;
            keyCount = new int[] { 999, 999, 999};
        }

        // Start facing right
        facingDirs = new bool[] { false, true, false, false };

        GameObject spawnOne = GameObject.Find("Spawn1");

        currentSpawn = spawnOne;

        spawnPoints = new GameObject[] {spawnOne};

        rotation = gameObject.transform.rotation;

		animator = GetComponent<Animator> ();
        onCircle = new Stack<GameObject>();
        onCircle.Push(null);
    }

	void Update()
	{
		// Lock rotation
        gameObject.transform.rotation = rotation;
        
        moveUpdate();

    }

    private void moveUpdate()
    {
        bool isMoving = false;
        

        if (canMove && GameManager.instance.canMove)
        {
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene("level" + GameManager.instance.level);
            }
            if(Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("Intro");
            }
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
        GameManager.instance.canMove = false;

        yield return new WaitForSeconds((float)1.5);

        respawn();
    }

    private void respawn()
    {
		GameManager.instance.resetTimer();
		canMove = true;
        GameManager.instance.canMove = true;
        animator.ResetTrigger("CatWalk");
        animator.ResetTrigger("CatDead");

        gameObject.transform.position = currentSpawn.transform.position;

        GameManager.instance.resetTimer();
        GameManager.instance.startTimer();

        gameObject.transform.parent = null;
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
			gameObject.transform.parent = other.gameObject.transform;
		}

        // Fruit collection successful
        else if (other.tag == fruitTag && !level_finished)
        {
            level_finished = true;
            StartCoroutine(GameManager.instance.levelCompleted());
            Destroy(other.gameObject);
        }

        
		else if (other.tag == key1Tag)
		{
            other.gameObject.SetActive(false);
            keyCount[0] += 1;
		}
        else if (other.tag == key2Tag)
        {
            other.gameObject.SetActive(false);
            keyCount[1] += 1;
        }
        else if (other.tag == key3Tag)
        {
            other.gameObject.SetActive(false);
            keyCount[2] += 1;
        }
        else if (other.tag == door1Tag)
		{
			if (keyCount[0] > 0) {
                keyCount[0] -= 1;
                other.gameObject.SetActive(false);
                other.gameObject.transform.parent.gameObject.SetActive (false);
			}
		}
        else if (other.tag == door2Tag)
        {
            if (keyCount[1] > 0)
            {
                keyCount[1] -= 1;
                other.gameObject.SetActive(false);
                other.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
        else if (other.tag == door3Tag)
        {
            if (keyCount[2] > 0)
            {
                keyCount[2] -= 1;
                other.gameObject.SetActive(false);
                other.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }




        else if (GameManager.instance.isNux())
        {
            GameObject.Destroy(other.gameObject);
            return;
        }



        // Fatal obstacle encountered
        else if (other.tag == fireTag || other.tag == spikesTag)
        {
            StartCoroutine((catDied()));
        }


        // Slowed down from web
        else if (other.tag == webTag)
        {
            moveSpeed /= 3;
        }
    }

    // Player leaves a trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        // Left a Circle to enter an outer circle or no circle
		if (other.tag == circleOne.tag || other.tag == circleTwo.tag || other.tag == circleThree.tag|| 
            other.tag == circleFour.tag|| other.tag == circleFive.tag)
        {

            // Check if the player is entering a previously entered circle
            if (onCircle.Count > 0 && onCircle.Peek() != null)
            {
                // Forget the last circle the player was on
                GameObject circle = onCircle.Pop();

                // Attach to the circle the player entered previous to circle the player just left
                if (onCircle.Peek() != null)
                {
                    gameObject.transform.parent = onCircle.Peek().transform;
                }
                else
                {
                    // Destroy the parent
                    gameObject.transform.parent = null;
                }
            }

            // The player is no longer on a circle
            else
            {
                // Destroy the parent
                gameObject.transform.parent = null;
            }
        }

        // Restore player movement speed
        else if (other.tag == webTag)
        {
            moveSpeed = 3F;
        }

    }


}