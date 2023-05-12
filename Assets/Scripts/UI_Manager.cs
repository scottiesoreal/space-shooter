using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    //handle to Text
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

    private GameManager _gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0; //assign text component to handle
        _gameOverTxt.gameObject.SetActive(false);
        _restartTxt.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        
        if (_gameManager == null )
        {
            Debug.LogError("Game Manager Null!");
        }

    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    {        
        _scoreText.text = "Score: " + playerScore.ToString();
    }
    public void UpdateLives(int currentLives)
    {

        _LivesImage.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();            
        }

     void GameOverSequence()
        {
            _gameManager.GameOver();
            _gameOverTxt.gameObject.SetActive(true);
            StartCoroutine(FlashText());//game over            
            _restartTxt.gameObject.SetActive(true);
            StartCoroutine(RestartFlicker());
        }

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

}
