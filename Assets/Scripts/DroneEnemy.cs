using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemy : MonoBehaviour
{
    //movement
    [SerializeField]
    private float _speed = 4.0f;

    //create zigzag movement
    [SerializeField]
    private float _zigzagSpeed = 3.0f;
    [SerializeField]
    private float _zigzagRange = 3.0f;


    //fire variable
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;//3 seconds between fire
    private float _canFire = -1;//time stamp

    //Damage
    [SerializeField]
    private bool _isEnShieldActive = true;
    
    [SerializeField]
    private int _enemyLives = 2;//Enemy "lives"/"health

    //Player
    private Player _player; //Declares player variable

    //visualizer
    [SerializeField]
    private GameObject _enShieldVisualizer;
    [SerializeField]
    private SpriteRenderer _enShieldRenderer;




    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        
        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        //animation
        //_anim = GetComponent<Animator>();

        //audio source(?)
    }   //

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        FireLaser();
    }

    private void CalculateMovement()
    {
        // Move down at the given speed
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // Calculate horizontal movement (zigzag) based on time and speed
        float zigzagDirection = Mathf.Sin(Time.time * _zigzagSpeed);
        float moveDirection = zigzagDirection > 0 ? 1f : -1f;

        // Calculate horizontal offset based on zigzagDirection and zigzagRange
        float xOffset = moveDirection * _zigzagRange;

        // Move "Drone" enemy along x-axis with updated xOffset
        transform.Translate(new Vector3(xOffset, 0f, 0f) * Time.deltaTime);
        
    }

    public void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1f, 3f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //damage player
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                EnemyDamage();
            }

            _player = player;
        } 

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            EnemyDamage();
            if (_enemyLives < 1 && _player != null)
            {
                _player.AddScore(20);
            }            
            
        }
    }

    public void EnemyDamage()
    {
        {
            _enemyLives--;

            if (_enemyLives < 2)
            {
                EnemyShieldActive();
            }

            if (_enemyLives < 1)
            {
                Destroy(GetComponent<Collider2D>());
                DestroyChildrenObjects();
                Destroy(this.gameObject);
            
            }

        }
    }

    public void EnemyShieldActive()
    {
        if (_enemyLives == 2 )
        {
            _isEnShieldActive = true;
            _enShieldVisualizer.SetActive(true);
            _enShieldRenderer.color = Color.white;
        }
        if (_enemyLives < 2)
        {
            _isEnShieldActive = false;
            _enShieldVisualizer.SetActive(false);
        }
    }

    private void DestroyChildrenObjects()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}