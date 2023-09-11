using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalEnemyController : MonoBehaviour
{
    //movement and positioning variables
    [SerializeField]
    private float _speed = 2.0f;
    private Vector3 _centerPosition = new Vector3(0f, 0f, 0f);
    private Vector3 _leftLimitPos = new Vector3(-8.5f, 0f, 0f);
    private Vector3 _rightLimitPos = new Vector3(8.5f, 0f, 0f);
    private float _leftLimit = -8.5f;
    private float _rightLimit = 8.5f;
    

    // movement check variables
    [SerializeField]
    private bool _isMovingLeft = false;
    [SerializeField]
    private bool _isMovingRight = false;
    [SerializeField]
    private bool _isReturningCenter = false;

    //laser firing variables
    [SerializeField]
    private GameObject _laserPrefab;//normal laser
    [SerializeField]
    private GameObject _doubleCannonsPrefab;//slower rate than normal laser
    [SerializeField]
    private GameObject _rapidLaserPrefab;//will be fired at much higher rate than normal laser

    [SerializeField]
    private float _fireRate = 2.5f;
    [SerializeField]
    private float _canFire = -1f;//time stamp for next fire

    //game object variables
    private FinalEnemyController _bossEnemy;

    // Enemy damage variables
    [SerializeField]
    private float _enemyDamage = 500f;
    [SerializeField]
    private float _enemyDamageRate = 0.5f;

    private void Start()
    {
        // Start the movement sequence after 5 seconds
        StartCoroutine(StartMovingSequence());
    }

    private IEnumerator StartMovingSequence()
    {
        yield return new WaitForSeconds(5f); // Initial delay before starting movement

        while (true)
        {
            // Move left until reaching the left limit
            _isMovingLeft = true;
            _isMovingRight = false;
            while (transform.position.x > _leftLimit)
            {
                yield return null;
            }
            _isMovingLeft = false;
            yield return new WaitForSeconds(5f); // Wait for 5 seconds at left limit

            // Move right until reaching the right limit
            _isMovingLeft = false;//Stop Moving Left: Imagine you were moving to the left, but now, just stop. No more left movement.
            _isMovingRight = true;//Start Moving Right: Imagine you were moving to the right, but now, just start. No more right movement.
            while (transform.position.x < _rightLimit)//While you are moving to the right, keep moving to the right until you reach the right limit.
            {
                yield return null;//Wait for the next frame to move to the right.
            }
            _isMovingRight = false;//Stop moving right
            yield return new WaitForSeconds(5f); // Wait for 5 seconds at right limit

            // Return to the center position after reaching the right limit
            _isReturningCenter = true;//Start moving to the center
            while (transform.position.x > _centerPosition.x)//While you are moving to the center, keep moving to the center until you reach the center.
            {
                yield return null;//Wait for the next frame to move to the center.
                _isReturningCenter = true;
            }
            _isReturningCenter = false;
            yield return new WaitForSeconds(5f); // Wait for 5 seconds at center position

            


        }
    }

    private void Update()
    {
        CalculateMovement();
    }

    private void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + new Vector3(0f, -1.5f, 0f), Quaternion.identity);
        }
    }



    private void CalculateMovement()
    {
        Vector3 targetPosition = _centerPosition;

        if (_isMovingLeft)
        {
            targetPosition = new Vector3(_leftLimit, transform.position.y, transform.position.z);
        }
        else if (_isMovingRight)
        {
            targetPosition = new Vector3(_rightLimit, transform.position.y, transform.position.z);
        }
        else if (_isReturningCenter)
        {
            targetPosition = _centerPosition;
        }

        // Only move towards the target position if either _isMovingLeft or _isMovingRight is true
        if (_isMovingLeft || _isMovingRight || _isReturningCenter)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
        }
    }


    //public void PauseMovementRoutine()
    // {
    // Pause coroutine
    //    if (_bossEnemy)

    //}
}
