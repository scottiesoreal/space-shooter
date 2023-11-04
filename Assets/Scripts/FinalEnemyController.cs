using System.Collections;
using UnityEngine;

public class FinalEnemyController : MonoBehaviour
{
    // Movement and positioning variables
    [SerializeField]
    private float _speed = 2.0f;
    private Vector3 _centerPosition = new Vector3(0f, 3f, 0f);
    //private Vector3 _phase1CenterPosition = new Vector3(0f, 3f, 0f);
    private float _leftLimit = -8.5f;
    private float _rightLimit = 8.5f;

    // State for the boss behavior
    private enum BossState
    {
        Descending,
        Waiting,
        MovingLeft,
        MovingRight,
        ReturningCenter
    }

    private BossState _currentState = BossState.Descending;

    // Laser firing variables
    [SerializeField]
    private GameObject _laserPrefab; // Normal laser
    [SerializeField]
    private float _fireRate = 2.5f;
    private float _canFire = -1f; // Timestamp for next fire

    // Game object variables
    [SerializeField]
    private int _hitCount = 0; // tracking boss hits


    //Player
    private Player _player;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(Descend());
    }

    private IEnumerator Descend()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(startPos.x, 3f, startPos.z);

        while (transform.position.y > endPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, _speed * Time.deltaTime);
            yield return null;
        }

        _currentState = BossState.Waiting;
        StartCoroutine(StartMovingSequenceAfterDelay(5f)); // Wait for 5 seconds after descending.
    }

    private IEnumerator StartMovingSequenceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _currentState = BossState.MovingLeft;
    }

    private void Update()
    {
        if (_currentState != BossState.Descending && _hitCount < 25)
        {
            CalculateMovement();
            FireLaser();
        }
    }

    private void CalculateMovement()
    {
        if (_currentState == BossState.MovingLeft)
        {
            if (transform.position.x > _leftLimit)
            {
                transform.position += Vector3.left * _speed * Time.deltaTime;
            }
            else
            {
                StartCoroutine(WaitAndChangeState(BossState.MovingRight));
            }
        }
        else if (_currentState == BossState.MovingRight)
        {
            if (transform.position.x < _rightLimit)
            {
                transform.position += Vector3.right * _speed * Time.deltaTime;
            }
            else
            {
                StartCoroutine(WaitAndChangeState(BossState.ReturningCenter));
            }
        }
        else if (_currentState == BossState.ReturningCenter)
        {
            if (transform.position.x != _centerPosition.x)
            {
                Vector3 targetPosition = new Vector3(_centerPosition.x, 3f, _centerPosition.z);
                transform.position = Vector3.MoveTowards(transform.position, _centerPosition, _speed * Time.deltaTime);
            }
            else
            {
                StartCoroutine(WaitAndChangeState(BossState.MovingLeft));
            }
        }
    }

    private IEnumerator WaitAndChangeState(BossState newState)
    {
        _currentState = BossState.Waiting;
        yield return new WaitForSeconds(5f);
        if (_hitCount < 25) // Continue movement if phase 1 still active
        {
            _currentState = newState;
        }
        
    }

    private void FireLaser()
    {
        if (_currentState != BossState.Descending && _currentState != BossState.Waiting && Time.time > _canFire)
        {
            _fireRate = Random.Range(.3f, 1.7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0f, -1.5f, 0f), Quaternion.identity);

            //Assign it as enemy laser
            Laser laserScript = enemyLaser.GetComponent<Laser>();
            if (laserScript != null)
            {
                laserScript.AssignEnemyLaser();
            }
        }
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            _hitCount++;

        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _hitCount++;
            _player.AddScore(50);            
            
        }
   

    }

    // Assuming there's a method to handle boss damage, which needs to be implemented
    public void TakeDamage(int damageAmount)
    {
        _hitCount += damageAmount;

        //if (_hitCount >= 25)
        //{
        //    // Start phase 2
        //{
    }


}
