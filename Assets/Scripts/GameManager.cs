using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const int LEVEL_COUNT = 3;

    public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
    public float turnDelay = 0.1f;                          //Delay between each Player turn.
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.


    private Text levelText;                                 //Text to display current level number.
    private GameObject levelImage;                          //Image to block out level as levels are being set up, background for levelText.]
    private int level = 1;                                  //Current level number, expressed in game as "Day 1".


    private Text timerLabel;
    private Text victoryText;
    private float time;
    private bool isTiming;

    private bool show_menu = false;
    private bool show_timer = false;

    private Scene[] scenes = new Scene[LEVEL_COUNT];

    private string[] level_texts = new string[LEVEL_COUNT];
    private Dictionary<int, string> level_to_level_texts = new Dictionary<int, string>();

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

        level_texts[0] = "Pick up the fruit as fast as you can! \nAvoid walls and obstacles!";
        level_texts[1] = "Watch out! \nSome obstacles are fatal, while other can have other interesting effects.";
        level_texts[2] = "Some keys open some doors but not others.";
        level_texts[3] = "";
        level_texts[4] = "";
        level_texts[5] = "";
        level_texts[6] = "";
        level_texts[7] = "";
        level_texts[8] = "";
        level_texts[9] = "Don't fight the spin. \nSpin to win!";

        for (int i = 0; i < LEVEL_COUNT; i++)
        {
            scenes[i] = SceneManager.GetSceneByName("level" + (i + 1));
        }

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //This is called each time a scene is loaded.
    private void OnLevelWasLoaded(int index)
    {
        timerLabel = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        victoryText = GameObject.FindGameObjectWithTag("WinText").GetComponent<Text>();
    }

    //Initializes the game for each level.
    private void InitGame()
    {
        timerLabel = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        victoryText = GameObject.FindGameObjectWithTag("WinText").GetComponent<Text>();

        victoryText.enabled = false;
        isTiming = true;
    }


    //Hides black image used between levels
    private void HideLevelImage()
    {
        //Disable the levelImage gameObject.
        levelImage.SetActive(false);
    }

    //Update is called every frame.
    private void Update()
    {
        if (isTiming && timerLabel != null)
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
        levelText.text = "Thank You for Playing!";

        //Enable black background image gameObject.
        levelImage.SetActive(true);

        //Disable this GameManager.
        enabled = false;
    }

    public IEnumerator levelCompleted()
    {
        victoryText.enabled = true;
        victoryText.text += timerLabel.text;

        stopTimer();
        resetTimer();

        yield return new WaitForSeconds((float) 5);

        level++;
        nextLevel();
    }

    private void nextLevel()
    {

        if (level <= scenes.Length)
        {
            SceneManager.LoadScene("Level" + level);
        }
        else
        {
            GameOver();
        }



        //Call InitGame to initialize our level.
        InitGame();
    }

    public int getLevelNumber()
    {
        return level;
    }
}