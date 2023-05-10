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
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score:" + 0; //assign text component to handle
    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    {        
        _scoreText.text = "Score: " + playerScore.ToString();
    }
    public void UpdateLives(int currentLives)
    {
        //access display img sprite
        //give it  new one based on the current lives index
        _LivesImage.sprite = _livesSprites[currentLives];
    }
}
