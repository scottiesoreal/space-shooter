using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreTxt;
    [SerializeField]
    private Image _LivesImage;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _gameOverTxt;
    [SerializeField]
    private Text _restartTxt;
    [SerializeField]
    private Text _escTxt;
    [SerializeField]
    private Text _pauseTxt;

    private Coroutine _pauseFlickerCoroutine;

    private GameManager _gameManager;

    void Start()
    {
        
        _scoreTxt.text = "Score: " + 0;
        _pauseTxt.gameObject.SetActive(false);
        _gameOverTxt.gameObject.SetActive(false);
        _restartTxt.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager Null!");
            return;
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreTxt.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives >= 0 && currentLives < _livesSprites.Length)
        {
            _LivesImage.sprite = _livesSprites[currentLives];
        }
        else
        {
            Debug.LogError("Invalid Lives");
        }

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void PauseSequence()
    {
        _gameManager.PauseGame();
        _pauseTxt.gameObject.SetActive(true);
        if (_pauseFlickerCoroutine != null)
        {
            StopCoroutine(_pauseFlickerCoroutine);
        }
        _pauseFlickerCoroutine = StartCoroutine(PauseFlicker());
    }

    IEnumerator PauseFlicker()
    {
        while (true)
        {
            _pauseTxt.text = "GAME PAUSED";
            yield return new WaitForSeconds(0.5f);
            _pauseTxt.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    //public void StartPauseFlicker()
    //{
      //  _pauseFlickerCoroutine = StartCoroutine(PauseFlicker());
        //_pauseTxt.gameObject.SetActive(true);
    //}

    public void StopPauseFlicker()
    {
        if (_pauseFlickerCoroutine != null)
        {
            StopCoroutine(_pauseFlickerCoroutine);
            _pauseTxt.gameObject.SetActive(false);
        }
    }

    void GameOverSequence()
    {
        if (_gameManager != null)
        {
            _gameManager.GameOver();
            _gameOverTxt.gameObject.SetActive(true);
            StartCoroutine(GameOverFlicker());
            _restartTxt.gameObject.SetActive(true);
            StartCoroutine(RestartFlicker());
            _escTxt.gameObject.SetActive(true);
            StartCoroutine(EscFlicker());
        }
        else
        {
            Debug.LogError("Game Manager Null!");
        }
       
    }
    
    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverTxt.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverTxt.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator RestartFlicker()
    {
        while (true)
        {
            _restartTxt.text = "Press 'R' Key to restart the Level";
            yield return new WaitForSeconds(0.5f);
            _restartTxt.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator EscFlicker()
    {
        while (true)
        {
            _escTxt.text = "Press 'Esc' to Quit";
            yield return new WaitForSeconds(0.5f);
            _escTxt.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
