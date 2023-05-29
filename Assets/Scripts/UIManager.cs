using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImage;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _gameOverTxt;
    [SerializeField]
    private Text _restartTxt;
    [SerializeField]
    private Text _escText;

    private GameManager _gameManager;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverTxt.gameObject.SetActive(false);
        _restartTxt.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager Null!");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImage.sprite = _livesSprites[currentLives];

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

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverTxt.gameObject.SetActive(true);
        StartCoroutine(FlashText());
        _restartTxt.gameObject.SetActive(true);
        StartCoroutine(RestartFlicker());
        _escText.gameObject.SetActive(true);
        StartCoroutine(EscFlicker());
    }

    IEnumerator FlashText()
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
            _escText.text = "Press 'Esc' to Quit";
            yield return new WaitForSeconds(0.5f);
            _escText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
