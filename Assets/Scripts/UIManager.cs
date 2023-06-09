using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private Text _ammoTxt;
    [SerializeField]
    private Slider _thrusterSlider;

    private Coroutine _pauseFlickerCoroutine;

    private GameManager _gameManager;

    void Start()
    {
        _scoreTxt.text = "Score: " + 0;
        _ammoTxt.text = 15.ToString();
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

    public void UpdateThrusterScale(float elapsedTime, float thrusterScale)
    {
        _thrusterSlider.value = thrusterScale;

        thrusterScale = thrusterScale - elapsedTime;
        if (thrusterScale < 0)
        {
            thrusterScale = 0;
        }
        _thrusterSlider.value = thrusterScale;

        if (thrusterScale > 9.0f && thrusterScale <= 10.0f)
        {
            _thrusterSlider.gameObject.transform.Find("FillArea").Find("Fill").GetComponent<Image>().color = Color.green;
        }
        else if (thrusterScale > 7.0 && thrusterScale <= 9.0f)
        {
            _thrusterSlider.gameObject.transform.Find("FillArea").Find("Fill").GetComponent<Image>().color = Color.yellow;
        }
        else if (thrusterScale > 5.0 && thrusterScale <= 7.0f)
        {
            _thrusterSlider.gameObject.transform.Find("FillArea").Find("Fill").GetComponent<Image>().color = Color.cyan;
        }
        else if (thrusterScale > 3.0 && thrusterScale <= 5.0f)
        {
            _thrusterSlider.gameObject.transform.Find("FillArea").Find("Fill").GetComponent<Image>().color = Color.blue;
        }
        else if (thrusterScale <= 3.0f)
        {
            _thrusterSlider.gameObject.transform.Find("FillArea").Find("Fill").GetComponent<Image>().color = Color.red;
        }
    }

    public Text colorChangeFont;

    public void UpdateAmmoCount(int playerAmmo)
    {
        _ammoTxt.text = playerAmmo.ToString();
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
            yield return new WaitForSecondsRealtime(1.9f);
            _pauseTxt.text = "";
            yield return new WaitForSecondsRealtime(1.5f);
        }
    }

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
