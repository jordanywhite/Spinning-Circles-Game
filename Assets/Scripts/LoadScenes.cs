using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScenes : MonoBehaviour {
    
    public void LoadScene(int level)
    {
        GameManager.instance.level = level;
        SceneManager.LoadScene("Level" + level);
    }

    public void LevelSelection()
    {
        SceneManager.LoadScene("Level_Selection");
    }

    public void LoadIntro()
    {
        SceneManager.LoadScene("Intro");
    }

    public void LoadScoreScreen()
    {
        SceneManager.LoadScene("ScoreScreen");
    }



    public void Quit()
    {
        Application.Quit();
    }
}
