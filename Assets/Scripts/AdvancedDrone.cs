using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedDrone : MonoBehaviour
{
    //movement variables
    [SerializeField]
    private float _speed = 0f;
    

    //laser variables
    private GameObject _laserPrefab;

    //firing variables
    private float _fireRate = 1.5f;
    private float _canFire = -1;

    //enemy line of sight variables
    [SerializeField]
    private float _lineOfSight = 5.0f;

    //enemy damage/lives variables
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private bool _isDead = false;

    //player variables
    private Player _player;
        
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateAdDroneMovement();
    }

    private void CalculateAdDroneMovement()
    {
        //apply downward movement
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //if player is within 5f of enemy's y position, enemy faces direction of player and fires
        if (_player != null)
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < _lineOfSight)
            {
                AdvancedDroneFiring();
            }
        }

    }



    public void AdvancedDroneFiring()
    {
        // Check if the player is within the line of sight distance
        if (_player != null && Vector3.Distance(transform.position, _player.transform.position) < _lineOfSight)
        {
            // Calculate direction to the player
            Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;

            // Calculate the rotation angle towards the player
            float targetRotationAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;

            // Smoothly rotate the drone towards the player
            float rotationStep = _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, targetRotationAngle), rotationStep);

            // Check if it's time to fire
            if (Time.time > _canFire)
            {
                _fireRate = 2.5f;
                _canFire = Time.time + _fireRate;

                // Instantiate the laser at the front of the drone
                Instantiate(_laserPrefab, transform.position + transform.up * 0.5f, transform.rotation);
            }
        }
    }


}
