using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvDroneMover : MonoBehaviour
{
    //movement
    [SerializeField]
    private float _speed = 2.0f;
    
    

    //laser dodge
    [SerializeField]
    private float _laserEscapeSpeed = 1.0f;

    private Player _player;
    private AdvDroneRadar _radar;

    private bool _avoidingLaser = false;
    private Vector3 _avoidanceDirection = Vector3.zero;
    private Vector3 _detectedLaserPosition = Vector3.zero; // Added line to store detected laser position




    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _radar = GetComponentInChildren<AdvDroneRadar>();//Assign the AdvDroneRadar reference


        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }


    }

    // Update is called once per frame
    void Update()
    {
        LaserDetection();
        if (!_avoidingLaser)
        {
            Movement();
        }
        else
        {
            LaserDodge();
        }
        
    }

    //movement downward
    private void Movement()
    {
        if (!_radar.LaserInRange) // Access the property from AdvDroneRadar
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else
        {
            LaserDodge();
        }
    }

    private void LaserDetection()
    {
        Laser[] lasers = FindObjectsOfType<Laser>();
        _radar.SetLaserInRange(false); // Access the property from AdvDroneRadar


        foreach (Laser laser in lasers)
        {
            if (laser.tag == "AdvancedDroneLaser")
            {
                continue;
            }

            float distanceToLaser = Vector3.Distance(transform.position, laser.transform.position);

            if (distanceToLaser <= _radar.LaserDetectionDistance) // Access the property from AdvDroneRadar
            {
                _radar.SetLaserInRange(true); // Access the property from AdvDroneRadar
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //damage player
        if (other.tag == "Player")
        {
            //damage player
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            DestroyChildrenObjects();
            //Destroy(this.gameObject, 2.8f);

        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Player player = GameObject.Find("Player").GetComponent<Player>();
            if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(GetComponent<Collider2D>());
            //_anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
           // _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            DestroyChildrenObjects();
            Destroy(this.gameObject, 2.8f);
        }


    }

    

    private void LaserDodge()
    {
        
        // Calculate a direction away from the laser's position
        _avoidanceDirection = transform.position - _detectedLaserPosition; // Access the property from AdvDroneRadar

        //normallize the direction to maintain constant speed
        _avoidanceDirection.Normalize();

        //move in avoidance direction
        transform.Translate(_avoidanceDirection * _speed * _laserEscapeSpeed * Time.deltaTime);
    }

    private void DestroyChildrenObjects()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}
