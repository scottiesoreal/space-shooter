using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    
    //speed variable of 8
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;
    private bool _hasDamagedPlayer = false;
    private Vector3 _initialDirection = Vector3.up;
    private AdvancedDrone _firingDrone;//Reference to the AdvDroneMover that fired this laser

    public bool IsEnemyLaser
    {
        get { return _isEnemyLaser; }
    }

    public void SetFiringDrone(AdvancedDrone drone)
    {
        _firingDrone = drone;
    }

    public AdvancedDrone GetFiringDrone()
    {
        return _firingDrone;
    }

    public void SetDirection(Vector3 direction)
    {
        _initialDirection = direction.normalized;
    }

    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveInDirection(_initialDirection); // Move the laser in the set initial direction
        }
        else
        {
            MoveInDirection(-_initialDirection); // Move the enemy laser in the opposite direction
        }
    }

    private void MoveInDirection(Vector3 direction)
    {
        transform.Translate(direction * _speed * Time.deltaTime);

        // check if laser is out of bounds
        if (transform.position.y > 8.0f || transform.position.y < -8.0f || transform.position.x > 11.3f || transform.position.x < -11.3f)
        {
            // check if object has a parent
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

   
    
    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUp") && _isEnemyLaser) // Check if it's a power-up and fired by an enemy
        {
            PowerUp powerUp = other.GetComponent<PowerUp>();
            if (powerUp != null)
            {
                Destroy(other.gameObject); // Destroy the power-up
                Destroy(this.gameObject); // Destroy the enemy's laser
                return; // Exit the method to avoid damaging the player
            }

        }

        if (other.tag == "Player" && _isEnemyLaser && _hasDamagedPlayer == true)
        {
            
            Player player = other.GetComponent<Player>();
                        
            if (player != null)
            {
                player.Damage();
               
            }
        }

    }   

}
