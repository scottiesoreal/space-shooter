using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private bool _isGamePaused = false;

    private void Update()
    {
        //if the r key is pressed
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        SceneManager.LoadScene(1);//current Game Scene

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P) && _isGamePaused == false)
        {
            PauseGame();
            
        }
        else if (Input.GetKeyDown(KeyCode.P) && _isGamePaused == true) 
        {
            ResumeGame();
        }
        

    }

    

    public void GameOver()
    {
        _isGameOver = true;
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        _isGamePaused = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        _isGamePaused = false;
    }

}
