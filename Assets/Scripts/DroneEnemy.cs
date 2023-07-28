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
    //[SerializeField]
    //private int _damage = 2;//Enemy "lives"/"health

    //shield
    //[SerializeField]
    //private bool _isShieldActive = true;
    
    //visualizer
    //[SerializeField]
    //private GameObject _shieldVisualizer;

    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

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

        // If the bottom of the screen, respawn at the top with a new random x position
        if (transform.position.y < -5.5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }

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
            _fireRate = Random.Range(3f, 7f);
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
            }
        }
    }



}

 


