using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    #region unity_funtions
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region scene_transitions
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Level2Scene()
    {
        SceneManager.LoadScene("Level2");
    }

    public void WinGame()
    {
        SceneManager.LoadScene("WinScene");
    }
     
    public void LoseGame()
    {
        SceneManager.LoadScene("LoseScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    #endregion
}
