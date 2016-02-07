using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
    public float turnDelay = 0.1f;                          //Delay between each Player turn.
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.


    private Text levelText;                                 //Text to display current level number.
    private GameObject levelImage;                          //Image to block out level as levels are being set up, background for levelText.]
    private int level = 1;                                  //Current level number, expressed in game as "Day 1".
    private bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.


    public Text timerLabel;
    public Text victoryText;
    private float time;
    private bool isTiming;

    //Awake is always called before any Start functions
    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //This is called each time a scene is loaded.
    private void OnLevelWasLoaded(int index)
    {
        //Add one to our level number.
        level++;
        //Call InitGame to initialize our level.
        InitGame();
    }

    //Initializes the game for each level.
    private void InitGame()
    {
        //While doingSetup is true the player can't move, prevent player from moving while title card is up.
        doingSetup = true;

        victoryText.enabled = false;
        isTiming = true;

        doingSetup = false;
    }


    //Hides black image used between levels
    private void HideLevelImage()
    {
        //Disable the levelImage gameObject.
        levelImage.SetActive(false);

        //Set doingSetup to false allowing player to move again.
        doingSetup = false;
    }

    //Update is called every frame.
    private void Update()
    {
        if (isTiming)
        {
            time += Time.deltaTime;
            var minutes = time / 60; //Divide the guiTime by sixty to get the minutes.
            var seconds = time % 60;//Use the euclidean division for the seconds.
            var fraction = (time * 100) % 100;

            //update the label value
            timerLabel.text = string.Format("Time: {0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
        }

        return;
    }
    
    public void resetTimer()
    {
        time = 0;
    }

    public void startTimer()
    {
        isTiming = true;
    }

    public void stopTimer()
    {
        isTiming = false;
    }

    public void GameOver()
    {
        //Set levelText to display number of levels passed and game over message
        levelText.text = "Game Over";

        //Enable black background image gameObject.
        levelImage.SetActive(true);

        //Disable this GameManager.
        enabled = false;
    }

    public int getLevelNumber()
    {
        return level;
    }
}