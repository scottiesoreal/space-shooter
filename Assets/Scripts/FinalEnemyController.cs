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
    [SerializeField]
    private Transform _laserFirePos;
    
    
    //[SerializeField]
    //private bool _isFiringRapid = false;
    [SerializeField]
    private bool _isRapidFireRunning = false;
    [SerializeField]
    private int _numberOfShotsPerBurst = 30;


    //Phase 3
    [SerializeField]
    private bool _isShieldActive = false;
    //private int _shieldStrength = 300;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _shieldRenderer;



    // State for the boss behavior
    public enum BossState
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

    

    [SerializeField]
    private bool _isInvincible = false;
    

    private BossState _currentState = BossState.Descending;

    // Laser firing variables
    [SerializeField]
    private GameObject _laserPrefab; // Normal laser
    [SerializeField]
    private GameObject _doubleLaserPrefab; // Double laser
    [SerializeField]
    private float _fireRate = 1f;
    private float _phase2FireRate = .1f; // Higher rate of fire for Phase 2
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
        switch (_currentState)
        {
            case BossState.Descending:
                // Descend logic inside the Descend coroutine.
                break;
            case BossState.Waiting:
                // Logic for waiting state
                break;
            case BossState.MovingLeft:
            case BossState.MovingRight:
            case BossState.ReturningCenter:
                CalculateMovement();
                FireLaser();
                break;
            case BossState.PhaseTransition:
                // Logic for phase transition
                break;
            case BossState.Phase2:
                FacePlayer();//face player logic
                if (!_isRapidFireRunning)
                {
                    StartCoroutine(RapidFireRoutine());
                }
                break;
            case BossState.Phase3:
                // Logic for phase 3
                FacePlayer();//face player logic
               
                // Add cases for other states as needed
                break;
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
        _isInvincible = true; //Boss is vulnerable after descending and begins MovingLeft
        yield return new WaitForSeconds(delay);
        _currentState = BossState.MovingLeft;
    }


    private void StartPhaseTwoTransition()
    {
        StopAllCoroutines(); //halts all phase 1 activity
        _currentState = BossState.PhaseTransition;
        _isInvincible = true;
        //_hitCount = 0;
        StartCoroutine(PhaseTwoTransitionDelay(5f));
    }

    private void StartPhaseThreeTransition()
    {
        StopAllCoroutines(); //halts activity
        _currentState = BossState.PhaseTransition;
        _isInvincible = true;
        //_hitCount = 0;
        StartCoroutine(PhaseThreeTransitionDelay(5f));
    }

    private IEnumerator PhaseTwoTransitionDelay(float delay) //
    {
        Debug.Log("Phase One transition started");
        _isInvincible = true;
        yield return new WaitForSeconds(delay);
        StartCoroutine(MoveToPhase2StartPosition());
    }

    private IEnumerator PhaseThreeTransitionDelay(float delay) //
    {
        Debug.Log("Phase Two transition started");
        _isInvincible = true;
        yield return new WaitForSeconds(delay);
        StartCoroutine(StartPhase3());
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
        Debug.Log("Phase 2 Started");
        _currentState = BossState.Phase2; 
        _isInvincible = false;
        
    }

    public BossState GetCurrentBossState()
    {
        return _currentState;
    }

    private IEnumerator StartPhase3()
    {
        
        _currentState = BossState.Phase3;
        

        if (_currentState == BossState.Phase3)
        {
            //debug log that says, "Phase 3 started"
            Debug.Log("Phase 3 started");
            _isInvincible = false;
            BossShieldActive(); 
            
            yield return new WaitForSeconds(5f);           
            

        }

    }

    public void BossShieldActive()
    {
        if (_isShieldActive == false)
        {
            _isShieldActive = true;
            _shieldVisualizer.SetActive(true);
            _shieldRenderer.SetActive(true);
            //_shieldStrength;
        }
        else if (_hitCount >= 170)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            _shieldRenderer.SetActive(false);
        }



    }


    private void CalculateMovement()
    {
        if (_currentState == BossState.MovingLeft)
        {
            if (transform.position.x > _leftLimit)
            {
                transform.position += Vector3.left * _speed * Time.deltaTime;
                _isInvincible = false;
                FireLaser();
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
                FireLaser();

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
                FireLaser();
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
            Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;
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

    //Double-cannon fire routine
    

    private IEnumerator RapidFireRoutine()
    {
        _isRapidFireRunning = true;
        while (_currentState == BossState.Phase2)
        {
            //burst of rapid fire
            for (int i = 0; i < _numberOfShotsPerBurst; i++)
            {
                // Fire the laser at the rate determined by _phase2FireRate
                FireLaserPhase2();
                Debug.Log("Fired laser shot number: " + (i + 1));
                yield return new WaitForSeconds(_phase2FireRate);
            }

            // After firing 30 shots, wait for a random time between 2 to 5 seconds
            float pauseDuration = Random.Range(2f, 5f);
            Debug.Log("Pausing for " + pauseDuration + " seconds"); // Logs the pause duration
            yield return new WaitForSeconds(pauseDuration);

        }
        _isRapidFireRunning = false;
    }

    // New method specifically for Phase 2 rapid fire
    private void FireLaserPhase2()
    {
        // Instantiate the laser at the specified firing position
        GameObject enemyLaser = Instantiate(_laserPrefab, _laserFirePos.position, Quaternion.Euler(0, 0, transform.eulerAngles.z));

        // Assign it as enemy laser and set the direction based on the boss's current rotation for Phase 2
        Laser laserScript = enemyLaser.GetComponent<Laser>();
        if (laserScript != null)
        {
            laserScript.AssignEnemyLaser();
        }

        // No need to check Time.time > _canFire because the coroutine handles the firing rate
        Debug.Log("Phase 2 Laser fired!");
    }



    private void FireLaser()
    {
        if (Time.time > _canFire)
        {
            // Determine the position and rotation of the laser based on the boss's orientation
            Vector3 laserPos = transform.position + new Vector3(0, 1, 0); // Default position is the boss's position
            // Adjust if the laser should come from a specific point on the boss
            Quaternion laserRot = (_currentState == BossState.Phase2)
                ? Quaternion.Euler(0f, 0f, transform.eulerAngles.z) // Phase 2: Use boss's current rotation
                : Quaternion.identity; // Other Phases: Default downward direction

            GameObject enemyLaser = Instantiate(_laserPrefab, _laserFirePos.position, laserRot);
            _fireRate = Random.Range(.10f, 1.0f);
            _canFire = Time.time + _fireRate;

            // Assign it as enemy laser and set the direction based on the boss's current rotation for Phase 2
            Laser laserScript = enemyLaser.GetComponent<Laser>();
            if (laserScript != null)
            {
                laserScript.AssignEnemyLaser();
            }
        }
    }

    private void DoubleLaser()
    {

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
                StartPhaseTwoTransition();
            }
            //check if time to transition to phase 3
            if (_hitCount == 70 && _currentState != BossState.PhaseTransition && _currentState ==BossState.Phase2 && _currentState != BossState.Phase3)
            {

                StartPhaseThreeTransition();
                //StartCoroutine(StartPhase3());
            }
            //turn off shield after 170 hits
            if (_hitCount == 250 && _currentState == BossState.Phase3)
            {
                Destroy(this.gameObject);
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
