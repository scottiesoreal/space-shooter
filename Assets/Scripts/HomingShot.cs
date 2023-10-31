using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingShot : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f; // Speed of the projectile.
    private GameObject _currentTarget = null; // Enemy the projectile is currently targeting.
    [SerializeField]
    private float _homingDetectionDistance = 2.5f; // How far the projectile looks for enemies.
    [SerializeField]
    private bool _isHomingInRange = false; // Flag to determine if the projectile should start homing.

    // On start, the projectile tries to find the closest enemy.
    private void Start()
    {
        FindClosestEnemy();
    }



    // Find the closest enemy with the "Enemy" tag.
    private void FindClosestEnemy()
    {
        // 1. Get all enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // If there are no enemies in scene, just return
        if (enemies.Length == 0)
        {
            _currentTarget = null;
            _isHomingInRange = false;
            return;
        }

        // Initialize the closest enemy and distance
        GameObject closestEnemy = enemies[0];
        float shortestDistance = Vector3.Distance(transform.position, enemies[0].transform.position);

        // 2 & 3. Loop through all enemies to determine the closest
        for (int i = 1; i < enemies.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, enemies[i].transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemies[i];
            }
        }

        // 4. Set the closest enemy as the target
        _currentTarget = closestEnemy;

        // 5. Check if the projectile should start homing
        if (shortestDistance <= _homingDetectionDistance)
        {
            _isHomingInRange = true;
        }
        else
        {
            _isHomingInRange = false;
        }
    }

    private void Update()
    {
        // New Addition: Logic to update _isHomingInRange based on proximity to _currentTarget
        if (_currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _currentTarget.transform.position);
            if (distanceToTarget <= _homingDetectionDistance)
            {
                _isHomingInRange = true;
            }
            else
            {
                _isHomingInRange = false;
            }
        }
        else
        {
            _isHomingInRange = false;
        }
        
        // If we're in homing range and have a target, move towards it.
        if (_isHomingInRange && _currentTarget != null)
        {
            MoveTowardsTarget();
        }
        else
        {
            RegularMovement();
        }

        // Logic to destroy the projectile if it goes off-screen can be added here.
        OffScreenCheck();
    }

    // Method to move the projectile towards the current target.
    private void MoveTowardsTarget()
    {
        // Determine the direction to the target.
        Vector3 directionToTarget = (_currentTarget.transform.position - transform.position).normalized;

        // Move in that direction.
        transform.position += directionToTarget * _speed * Time.deltaTime;
    }

    // Method for regular movement of the projectile (typically upwards).
    private void RegularMovement()
    {
        transform.position += Vector3.up * _speed * Time.deltaTime;
    }

    // Method to check if the projectile has gone off-screen and destroy it if so.
    private void OffScreenCheck()
    {
        // This assumes the top of the screen is y = 8 for simplicity, adjust as needed.
        if (transform.position.y > 8f)
        {
            Destroy(gameObject);
        }
    }
}
