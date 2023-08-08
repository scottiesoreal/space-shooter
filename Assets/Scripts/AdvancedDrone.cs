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
    // Forward direction of the drone
    private Vector3 _forwardDirection = Vector3.down; // Initialize the forward direction to be downward

    [SerializeField]
    private bool _enemyInRange = false;






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

    private void FireLaser()
    {
        
        Debug.Log("FireLaser() called");
        if (_enemyInRange && Time.time > _canFire)
        {
            Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;//direction from drone to player
            float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;//angle from drone to player

            //Check if the angle to the player is within the fire angle
            if (Mathf.Abs(angleToPlayer) <= _fireAngle)
            {
                _canFire = Time.time + _fireRate;
                _fireRate = Random.Range(0.3f, 3.5f);
                GameObject laser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                Laser[] lasers = laser.GetComponentsInChildren<Laser>();//get laser components
                foreach (Laser l in lasers)
                {
                    l.AssignEnemyLaser();//assign laser as enemy laser
                }
            }

        }

    }

    //test
    //_fireRate = Random.Range(1f, 3f);
            //_canFire = Time.time + _fireRate;
           // GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

    //Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            //for (int i = 0; i<lasers.Length; i++)
            //{
                //lasers[i].AssignEnemyLaser();

        
}
