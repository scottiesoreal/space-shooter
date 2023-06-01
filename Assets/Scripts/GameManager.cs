using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private bool _isGamePaused = false;
    [SerializeField]
    private UIManager _uiManager;
        

    private void Update()
    {
        //if the r key is pressed
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);//current Game Scene
        }        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P) && _isGamePaused == false)
        {
            PauseGame();
            _uiManager.PauseSequence();            
        }
        else if (Input.GetKeyDown(KeyCode.P) && _isGamePaused == true) 
        {
            ResumeGame();
            _uiManager.StopPauseFlicker();
        }
        

    }

    

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void PauseGame()
    {
        _isGamePaused = true;
        Time.timeScale = 0;        
    }

    public void ResumeGame()
    {
        _isGamePaused = false;
        Time.timeScale = 1;        
    }

}
