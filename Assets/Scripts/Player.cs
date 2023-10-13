using System.Collections;
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
    private GameObject _omniShotPrefab;
    [SerializeField]
    private GameObject _playerHomingPrefab;
    [SerializeField] 
    private float _fireRate = 0.4f;
    private float _canFire = -1f;
    [SerializeField] 
    private int _lives = 3;
    [SerializeField] 
    private int _shieldStrength = 3;

    [SerializeField]
    public int _maxAmmo = 30;
    [SerializeField]
    private int _ammoCount = 0;


    [SerializeField] 
    private float _thrusterScaleMax = 6f;
    [SerializeField] 
    private float _thrusterScale;
    [SerializeField] 
    private bool _isThrusterRecharging = false;

    private SpawnManager _spawnManager;

    //Powerups
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField] 
    private bool _isShieldActive = false;
    [SerializeField] 
    private bool _isSpeedBoostActive = false;
    [SerializeField] 
    private bool _isOmniShotActive = false;
    [SerializeField] 
    private bool _isAntiAmmoActive = false;
    [SerializeField]
    private bool _isPlayerHomingActive = false;

    [SerializeField] 
    private bool _isRightDamagedActive = false;
    [SerializeField] 
    private bool _isLeftDamagedActive = false;

    [SerializeField] 
    private GameObject _shieldVisualizer;
    [SerializeField] 
    private GameObject _rightDamageVisualizer, _leftDamageVisualizer;
    [SerializeField] 
    private SpriteRenderer _shieldRenderer;

    [SerializeField] 
    private AudioSource _audioSource;
    [SerializeField] 
    private AudioClip _laserSoundClip;
    [SerializeField] 
    private AudioClip _explosionSoundClip;
    [SerializeField] 
    private AudioClip _noAmmoClip;

    [SerializeField] 
    private int _score;
    private UIManager _uiManager;
    private ShakeCamera _camera;

    void Start()
    {
        // take the current position = new position (0x, 0y, 0z)
        transform.position = new Vector3(0, 1, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>(); // find the object.
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _thrusterScale = _thrusterScaleMax;
        _audioSource = GetComponent<AudioSource>();
        _camera = Camera.main.GetComponent<ShakeCamera>();

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

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_ammoCount == 0)
            {
                AudioSource.PlayClipAtPoint(_noAmmoClip, transform.position); // play empty clip sound
                return;
            }

            FireLaser();
        }

        ShiftBoost();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontalInput, verticalInput, 0);

        _isSpeedBoostActive = Input.GetKey(KeyCode.LeftShift);//checks if left shift is pressed

        if (_isSpeedBoostActive)
        {
            transform.Translate(inputDirection * _speedBase * _speedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(inputDirection * _speedBase * Time.deltaTime);
        }

        if (transform.position.y >= 6)
        {
            transform.position = new Vector3(transform.position.x, 6, 0);//locks player to no > 6 on y axis
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);//locks player to no < -3.8 on y axis
        }

        if (transform.position.x >= 9.25f)
        {
            transform.position = new Vector3(9.25f, transform.position.y, 0);//locks player to no > 9.25 on x axis
        }
        else if (transform.position.x <= -9.25f)
        {
            transform.position = new Vector3(-9.25f, transform.position.y, 0);//locks player to no < -9.25 on x axis
        }
    }

    void FireLaser()
    {
        if (_ammoCount <= 0)
        {
            return;
        }

        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else if (_isOmniShotActive)
        {
            Instantiate(_omniShotPrefab, transform.position, Quaternion.identity);
        }
        else if (_isPlayerHomingActive)
        {
            Instantiate(_playerHomingPrefab, transform.position, Quaternion.identity);
        }
        
        else
        {
            Instantiate(_laserPrefab, transform.position + Vector3.up * .6f, Quaternion.identity);
        }

        
        AmmoCount(-1);
        _audioSource.Play(); // play laser audio clip
    }



    public void Damage()
    {
        if (_isShieldActive && _shieldStrength > 0)
        {
            _shieldStrength--;
            _camera.StartShaking();

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
        }

        _lives--;
        _camera.StartShaking();

        if (_lives == 2)
        {
            _isLeftDamagedActive = true;
            _leftDamageVisualizer.SetActive(true); // enables visual
        }

        if (_lives == 1)
        {
            _isRightDamagedActive = true;
            _rightDamageVisualizer.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _audioSource.Play();
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void RestoreLives()
    {
        _lives = 3;
        _uiManager.UpdateLives(_lives);
        _rightDamageVisualizer.SetActive(false);
        _leftDamageVisualizer.SetActive(false);
    }

    public void AmmoCount(int lasers)
    {
        if (_ammoCount + lasers > _maxAmmo)
        {
            _ammoCount = _maxAmmo;
        }
        else
        {
            _ammoCount += lasers;
        }

        _uiManager.UpdateAmmoCount(_ammoCount, _maxAmmo);
    }

    public void AntiAmmo()
    {
        _isAntiAmmoActive = true;
        _ammoCount = 0;
        _uiManager.UpdateAmmoCount(_ammoCount, _maxAmmo);
    }

    public void OmniShotActive()
    {
        _isOmniShotActive = true;
        _isTripleShotActive = false;
        StartCoroutine(OmniShotPowerDownRoutine());
    }

    IEnumerator OmniShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isOmniShotActive = false;
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        _isOmniShotActive = false;
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
        _speedBase += _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        if (!_isShieldActive)
        {
            _isShieldActive = true;
            _shieldVisualizer.SetActive(true);
            _shieldRenderer.color = Color.white;
            _shieldStrength = 3;
        }
        else
        {
            _isShieldActive = true;
            _shieldVisualizer.SetActive(true);
            _shieldRenderer.color = Color.white;
            _shieldStrength = 3;
        }
    }

    public void PlayerHomingActive()
    {
        if (!_isPlayerHomingActive)
        {
            _isPlayerHomingActive = true;
            StartCoroutine(PlayerHomingPowerDownRoutine());
        }
        

    }

    IEnumerator PlayerHomingPowerDownRoutine()
        {
        yield return new WaitForSeconds(7.5f);
            _isPlayerHomingActive = false;
        }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void ShiftBoost()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _thrusterScale > 0f && !_isThrusterRecharging)
        {
            _thrusterScale -= Time.deltaTime;

            if (_thrusterScale <= 0f)
            {
                _thrusterScale = 0f;
                _isThrusterRecharging = true;
                StartCoroutine(ThrusterRechargeRoutine());
            }
        }
        else
        {
            _isSpeedBoostActive = false;
            _thrusterScale = Mathf.Min(_thrusterScale + Time.deltaTime, _thrusterScaleMax);
        }

        _uiManager.UpdateThrusterScale(Time.deltaTime, _thrusterScale);
    }

    private IEnumerator ThrusterRechargeRoutine()
    {
        yield return new WaitForSeconds(5f); // Adjust the recharge delay as needed
        _isThrusterRecharging = false;
    }
}
