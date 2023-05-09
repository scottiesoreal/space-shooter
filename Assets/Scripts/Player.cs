using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
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
    private Spawn_Manager _spawnManager;

    // variable: Is[powerup]Active
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;//variable reference to the shield visualizer

    // Start is called before the first frame update
    void Start()
    {
        // take the current position = new position (0x, 0y, 0z)
        transform.position = new Vector3(0, 1, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>(); // find the object.

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
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
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_isSpeedBoostActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
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
    }

    public void Damage()
    {
        //if shields is active
        //do nothing 
        //deactivate shields
        //return;

        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            //disable visualizer
            return;
        }

        _lives--;

        if (_lives < 1)
        {
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
        _speed *= _speedMultiplier;
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
            _shieldVisualizer.SetActive(true);//enabled the visualaizer
        }
    }
}