using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const int LEVEL_COUNT = 10;

    public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
    public float turnDelay = 0.1f;                          //Delay between each Player turn.
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.


    private Text levelText;                                 //Text to display current level number.
    private GameObject levelImage;                          //Image to block out level as levels are being set up, background for levelText.]
    public int level = 1;                                  //Current level number, expressed in game as "Day 1".

    public bool nux_mode = false;

    private Text timerLabel;
    private Text victoryText;
    private float time;
    private bool isTiming;

    private bool show_menu = false;
    private bool show_timer = false;

    private Scene[] scenes = new Scene[LEVEL_COUNT];

    private string[] level_texts = new string[LEVEL_COUNT];
    private Dictionary<int, string> level_to_level_texts = new Dictionary<int, string>();

    private float[] scores = new float[LEVEL_COUNT];

    public bool canMove = false;

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

        level_texts[0] = "Use wasd or the arrow keys to move. Press escape to return to the main menu or 'R' to restart a level. Aquire the fruit and don't die!";
        level_texts[1] = "Watch out! \nSome obstacles are fatal, while others are just there to ruin your score. Find the quickest way around them.";
        level_texts[2] = "Some keys open some doors but not others.";
        level_texts[3] = "Remeber: Dying doesn't hurt your score. Time does! Don't be afraid to restart if you had a slow start.";
        level_texts[4] = "Staying still doesn't mean your safe. Many obstacles move independently of the circle they appear to be on.";
        level_texts[5] = "Pay attention to the obstacles. Some move. Others don't.";
        level_texts[6] = "You need keys to get keys.";
        level_texts[7] = "";
        level_texts[8] = "";
        level_texts[9] = "Don't fight the spin. \nSpin to win!";

        for (int i = 0; i < LEVEL_COUNT; i++)
        {
            scenes[i] = SceneManager.GetSceneByName("level" + (i + 1));
            level_to_level_texts.Add(i + 1, level_texts[i]);
        }

    }

    //This is called each time a scene is loaded.
    private void OnLevelWasLoaded(int index)
    {
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            GameObject.Find("Toggle").GetComponent<Toggle>().isOn = nux_mode;
        }
            if (SceneManager.GetActiveScene().name == "ScoreScreen")
        {
            for (int i = 0; i < GameManager.LEVEL_COUNT; i++)
            {
                GameObject.Find("level" + (i + 1)).GetComponent<Text>().text += convertFloatToTime(scores[i]);
            }
        }

        //Call the InitGame function to initialize the first level 
        if (index > 2)
        {
            canMove = false;
            InitGame();
        }

    }

    //Initializes the game for each level.
    private void InitGame()
    {
        timerLabel = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        victoryText = GameObject.FindGameObjectWithTag("WinText").GetComponent<Text>();
        canMove = false;


        StartCoroutine(showLevelImage(4));

        victoryText.enabled = false;
    }

    private IEnumerator showLevelImage(float time)
    {

        isTiming = false;
        GameObject image = GameObject.FindGameObjectWithTag("LevelImage");
        Text text = GameObject.FindGameObjectWithTag("LevelText").GetComponent<Text>();
        
        text.text = level_to_level_texts[level];

        yield return new WaitForSeconds(time);

        image.SetActive(false);
        canMove = true;
        isTiming = true;
    }

    //Update is called every frame.
    private void Update()
    {
        if (isTiming && timerLabel != null)
        {
            time += Time.deltaTime;

            //update the label value
            timerLabel.text = "Time: " + convertFloatToTime(time);
        }

        return;
    }

    public string convertFloatToTime(float number)
    {
        var minutes = number / 60; //Divide the guiTime by sixty to get the minutes.
        var seconds = number % 60;//Use the euclidean division for the seconds.
        var fraction = (number * 100) % 100;
        
        return string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
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

        //Disable this GameManager.
        enabled = false;
    }

    public IEnumerator levelCompleted()
    {
        victoryText.enabled = true;
        victoryText.text += timerLabel.text;

        stopTimer();
        
        if(checkBetterTime(time, level))
        {
            victoryText.text += "\nNew Best Time!";
        }
        
        resetTimer();

        yield return new WaitForSeconds((float) 5);

        level++;
        nextLevel();
    }

    public bool checkBetterTime(float time, int level)
    {
        if(scores[level-1] > time || scores[level - 1] == 0)
        {
            scores[level - 1] = time;
            return true;
        }

        return false;
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

    public void NuxMode(Toggle toggle)
    {
        nux_mode = toggle.isOn;
    }

    public bool isNux()
    {
        return nux_mode;
    }
}