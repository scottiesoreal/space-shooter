using System.Collections;
using UnityEngine;

public class FinalEnemyController : MonoBehaviour
{
    //Phase1
    //Movement and positioning variables

    
    private float _speed = 2.0f;//phase 1: movement speed
    private Vector3 _centerPosition = new Vector3(0f, 3f, 0f);
    private float _leftLimit = -8.5f;
    private float _rightLimit = 8.5f;


    //Phase2
    [SerializeField]
    private float _turnSpeed = 1.5f; //how fast the boss will face the player
    //Detection distance
    [SerializeField]
    private float _detectionDistance = 5f; //distance from player to fire
    [SerializeField]
    private float _fireAngle = 5f; //angle from player to fire
    [SerializeField]
    private bool _enemyInRange = false;
    
    


    // State for the boss behavior
    private enum BossState
    {
        Descending,
        Waiting,
        MovingLeft,
        MovingRight,
        ReturningCenter,
        PhaseTransition,
        Phase2,
        Phase3
    }

    

    private bool _isInvincible = false;

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
        _player = FindObjectOfType<Player>();
    }


    private void Update()
    {
        if (_currentState != BossState.Descending && _hitCount < 25)
        {
            CalculateMovement();
            FireLaser();
        }
        else if (_hitCount >= 25 && _currentState != BossState.PhaseTransition)
        {
            StartPhaseTransition();
        }

        if (_currentState == BossState.Phase2 && _hitCount < 70)
        {
            // Phase 2 behaviors
            FacePlayer();
            //FireLaser();
        }

    }

    private IEnumerator Descend()
    {
        _isInvincible = true; //Boss is invincible while descending
        
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
        _isInvincible = false; //Boss is vulnerable after descending
        yield return new WaitForSeconds(delay);
        _currentState = BossState.MovingLeft;
    }


    private void StartPhaseTransition()
    {
        StopAllCoroutines(); //halts all phase 1 activity
        _currentState = BossState.PhaseTransition;
        _isInvincible = true;
        _hitCount = 0;
        StartCoroutine(PhaseTransitionDelay(5f));
    }
       
    private IEnumerator PhaseTransitionDelay(float delay) //
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(MoveToPhase2StartPosition());
    }

    private IEnumerator MoveToPhase2StartPosition()
    {
        Vector3 startPhase2Position = Vector3.zero; // The target position for phase 2

        while (transform.position != startPhase2Position)
        {
            // Move towards (0,0,0) using MoveTowards at the boss's speed
            transform.position = Vector3.MoveTowards(transform.position, startPhase2Position, _speed * Time.deltaTime);
            yield return null; // Wait until next frame before continuing the loop
        }

        // Once the boss is in position, update the state to Phase2
        yield return new WaitForSeconds(5f);
        _currentState = BossState.Phase2;
        _isInvincible = false;
        // Here you can start phase 2 behaviors or set a flag to trigger them in Update
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

    private void FacePlayer()
    {
        if (_player != null)
        {
            Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;//get direction to player
            //Calculate the angle to player
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;//convert to degrees
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90f));// Add 90 degrees to the angle to face the player;may need adjustments
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnSpeed * Time.deltaTime);//rotate towards player
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
        if (_isInvincible) return; // Ignore collisions while invincible
        
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
            if (_player != null)
            {
               _player.AddScore(50);
            }
                        
            //check if time to transition to phase 2
            if (_hitCount >= 25 && _currentState != BossState.PhaseTransition && _currentState != BossState.Phase2)
            {
                StartPhaseTransition();
            }

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
