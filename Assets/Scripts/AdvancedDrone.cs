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
    private float _fireAngle = 10f;//angle from player to fire
    [SerializeField]
    private GameObject _laserPrefab;

    //player variables
    private Player _player; //Declares player variable
    private Transform _playerTransform; // Reference to the player's Transform component
    // Forward direction of the drone
    private Vector3 _forwardDirection = Vector3.down; // Initialize the forward direction to be downward








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
    }

    private void CalculateMovement()
    {
                
        if (_playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
            if (distanceToPlayer <= _detectionDistance)
            {
               
                Vector3 directionToPlayer = (_playerTransform.position - transform.position).normalized;

                //Calculate the angle  with the X-axis
                float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

                
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angleToPlayer + 90f); 

                //rotate towards this target rotation (only rotating in the Z-axis)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);

               
            }
        }
    }

    private void Laser()
    {

    }
}
