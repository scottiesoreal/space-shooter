﻿using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speedBase = 3.5f;    
    [SerializeField]
    private float _speedMultiplier = 2f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.4f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _shieldStrength = 3;
    private SpawnManager _spawnManager;

    // variable: Is[powerup]Active
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;

    //damage variable: isVariableActive
    [SerializeField]
    private bool _isRightDamagedActive = false;
    [SerializeField]
    private bool _isLeftDamagedActive = false;
    //[SerializeField]
    //private bool _isThrusterSpeedActive = false;

    //visualizers
    [SerializeField]
    private GameObject _shieldVisualizer; //variable reference to the shield visualizer
    [SerializeField]
    private GameObject _rightDamageVisualizer, _leftDamageVisualizer;
    [SerializeField]
    private SpriteRenderer _shieldRenderer;
    //thruster visualizer

    //audio
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private AudioSource _audioSource;


    [SerializeField]
    private int _score;
    
    private UIManager _uiManager;
    

    

    // Start is called before the first frame update
    void Start()
    {
        // take the current position = new position (0x, 0y, 0z)
        transform.position = new Vector3(0, 1, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); // find the object.
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on player is Null!");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }


    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        ShiftBoost();


    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontalInput, verticalInput, 0);

        if (_isSpeedBoostActive == false)
        {
            transform.Translate(inputDirection * _speedBase * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            _isSpeedBoostActive = true;
            transform.Translate(inputDirection * _speedBase * _speedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(inputDirection * (_speedBase * _speedMultiplier) * Time.deltaTime);
        }

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 9.25f)
        {
            transform.position = new Vector3(9.25f, transform.position.y, 0);
        }
        else if (transform.position.x <= -9.25f)
        {
            transform.position = new Vector3(-9.25f, transform.position.y, 0);
        }



    }

    

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + Vector3.up * .6f, Quaternion.identity);
        }

        _audioSource.Play();//play laser audio clip

    }

    public void Damage()
    {
        if (_isShieldActive && _shieldStrength > 0)
        {
            _shieldStrength--;

            switch (_shieldStrength)
            {
                case 3:
                    _shieldRenderer.color = Color.white;
                    break;
                case 2:
                    _shieldRenderer.color = Color.yellow;
                    break;
                case 1:
                    _shieldRenderer.color = Color.red;
                    break;
                case 0:
                    _isShieldActive = false;
                    _shieldVisualizer.SetActive(false);
                    break;
            }

            return;
            //switch (_shieldStrength)
            //{
              //  case 0:
                    

                //    break;
                //case 1:
                  //  _shieldRenderer.color = Color.yellow;

                    //break;
                //case 2:
                  //  _shieldRenderer.color = Color.white;


                    //break;

            //}         
                       
        }

        
        

        _lives--;

        if (_lives == 2)
        {            
            _leftDamageVisualizer.SetActive(true);//enables visual
            _isLeftDamagedActive = true;
        }   
        
        if (_lives == 1)
        {            
            _rightDamageVisualizer.SetActive(true);
            _isRightDamagedActive = true;
        }

         
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _audioSource.Play();
            _spawnManager.OnPlayerDeath();            
            Destroy(this.gameObject);
        }

        
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {

        _isSpeedBoostActive = true;
        _speedBase *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }

   
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        if (_isShieldActive == false)
        {
            _isShieldActive = true;
            _shieldVisualizer.SetActive(true);
            _shieldRenderer.color = Color.white;
            _shieldStrength = 3;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    
    public void ShiftBoost()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isSpeedBoostActive = true; //flame visualizer (true);
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }
        else
        {
            _isSpeedBoostActive = false; //normal flame visualiser;
        }
    }


        //communicate with UI to update speed boost

}