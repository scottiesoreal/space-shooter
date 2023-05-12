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

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0; //assign text component to handle
        _gameOverTxt.gameObject.SetActive(false);
        StartCoroutine(FlashText());
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
            _gameOverTxt.gameObject.SetActive(true);
        }
    }

    IEnumerator FlashText()
    {
        while (true)
        {
            _gameOverTxt.text = "";
            yield return new WaitForSeconds(0.5f);
            _gameOverTxt.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
        }
    }
    
}
