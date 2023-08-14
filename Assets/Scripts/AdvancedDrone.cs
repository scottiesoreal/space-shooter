using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AdvancedDrone : MonoBehaviour
{
    //drone movement variables
    //drone will move downward at speed of 2.5f
    //drone will have line of sight
    //will turn towards player and fire if within 5f of player

    //movement and firing variables
    //[SerializeField]
    //private float _speed = 1.5f;//movement speed
    

    [SerializeField]
    private float _turnSpeed = 2.0f;//how fast drone will face the player
    [SerializeField]
    private bool _enemyInRange = false;
    [SerializeField]
    private float _fireRate = 2.5f;
    [SerializeField]
    private float _canFire = -1f;//time stamp for next fire
    [SerializeField]
    private float _detectionDistance = 5f;//distance from player to fire
    [SerializeField]
    private float _fireAngle = 5f;//angle from player to fire

    [SerializeField]
    private GameObject _laserPrefab;

    //player variables
    private Player _player; //Declares player variable
    private Transform _playerTransform; // Reference to the player's Transform component
    [SerializeField]
    private bool _isEnemyLaser = false;
    

   






    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");


        if (playerObject != null)
        {
            // Get the Transform component of the player GameObject.
            _player = playerObject.GetComponent<Player>();
            _playerTransform = playerObject.transform;
        }

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }
    }

    private void Update()
    {
        CalculateMovement();
        FireLaser();
    }

    private void CalculateMovement()
    {
                
        if (_playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
            if (distanceToPlayer <= _detectionDistance)
            {

                _enemyInRange = true;
                Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;

                //Calculate the angle  with the X-axis
                float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

                
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angleToPlayer + 90f); 

                //rotate towards this target rotation (only rotating in the Z-axis)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);
               
            }
            else
            {
                _enemyInRange = false;
            }
            
        }
    }

    public void HandleLaserDetected()
    {
        if (!_isEnemyLaser)
        {
            //laser dodge behavior
        }
    }

    private void FireLaser()
    {
        
        
        if (_enemyInRange && Time.time > _canFire)
        {
            // Get the angle of the drone
            float angle = transform.eulerAngles.z + 180f;


            // Instantiate laser with rotation
            GameObject laser = Instantiate(_laserPrefab, transform.position, Quaternion.Euler(0f, 0f, angle));


            // Set the direction of the laser to be its local up (you can set this in the Laser script)
            laser.GetComponent<Laser>().SetDirection(Vector3.up);

            //Set the firing drone reference in laser
            laser.GetComponent<Laser>().SetFiringDrone(GetComponent<AdvancedDrone>());
            laser.tag = "AdvancedDroneLaser";

            _canFire = Time.time + _fireRate; // Update the canFire timestamp
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

            Destroy(GetComponent<Collider2D>());
            //_anim.SetTrigger("OnEnemyDeath");//trigger anim
            
            //_audioSource.Play();

            //Destroy parent object

            
            //DestroyChildrenObjects();
            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Player player = GameObject.Find("Player").GetComponent<Player>();
            if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(GetComponent<Collider2D>());
            //_anim.SetTrigger("OnEnemyDeath");
            //_speed = 0;
            //_audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            //DestroyChildrenObjects();
            Destroy(this.gameObject);
        }

    }
}
