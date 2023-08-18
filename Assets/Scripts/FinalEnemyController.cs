using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class FinalEnemyController : MonoBehaviour
{

    //movement
    [SerializeField]
    private float _speed = -0.15f;

    private Vector3 _targetPosition = new Vector3(0f, 10f, 0f); // Set this to the desired center position
    [SerializeField]
    private bool _isMovingToCenter = true; //Initial movement to center of screen
    [SerializeField]
    private bool _isMovingLeft = true; 
    //private bool _isMovingRight = true;
    private bool _isFiring = false; //firing state


    //firing variables
    [SerializeField]

    
    
    

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        if (_isMovingToCenter)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

            if (transform.position == _targetPosition)
            {
                _isMovingToCenter = false;
                // Start the left-right movement and firing coroutine here
                //StartCoroutine(MoveLeftRightAndFire());
            }
        }
        else
        {
            // Implement left-right movement logic here
        }
    }


    IEnumerator MoveLeftRightAndFire()
    {
        while (_isFiring)
        {
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }




}
