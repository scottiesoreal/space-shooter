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
    
    //[SerializeField]//this is the time between shots
    //private float _fireRate = 3.0f;
    //private float _canFire = -1;//when to start shooting

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

}
