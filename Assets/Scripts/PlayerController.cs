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




    void Start()
	{
		player = GameObject.Find("Player");
		circleOne = GameObject.Find("Circle1");
        circleTwo = GameObject.Find("Circle2");
        circleThree = GameObject.Find("Circle3");
		circleFour = GameObject.Find("Circle4");
		circleFive = GameObject.Find("Circle5");

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

	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == circleOne.tag || other.tag == circleTwo.tag || other.tag == circleThree.tag || other.tag == circleFour.tag || other.tag == circleFive.tag)
        {
			print ("in: " + other.tag);
			onCircle.Push (other.gameObject);
			player.transform.parent = other.gameObject.transform;
		}
        else if (other.tag == fireTag || other.tag == spikesTag)
        {
            StartCoroutine((catDied()));
		} 
        else if (other.tag == webTag)
        {
            moveSpeed = 1F;
        }
        else if (other.tag == fruitTag)
        {
            levelCompleted();
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

        else if (other.tag == webTag)
        {
            moveSpeed = 3F;
        }
    }


    private void Restart()
    {
        SceneManager.LoadScene("Level " + GameManager.instance.getLevelNumber());
    }

}