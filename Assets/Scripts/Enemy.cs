using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private float _rotationSpeed = 60.0f;
    [SerializeField]
    private float _radius = 3.0f;
    private float _angle = 0f;
    [SerializeField]
    private GameObject _laserPrefab;
    private Player _player;
    private Animator _anim;
    private float _fireRate = 3.0f;
    private float _canFire = -1;


    //audio
    [SerializeField]
    private AudioSource _audioSource;
    

    private void Start()
    {

        
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        
        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        _anim = GetComponent<Animator>();


        if (_anim == null)
        {
            Debug.LogError("animator is null");//assign component to Anim
        }

        

        if (_audioSource == null)
        {
            Debug.LogError("Audio for Enemy is Null.");
        }
        

    }
    
    // Update is called once per frame
    private void Update()
    {
        CalculateMovement();
        
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for (int i = 0; i < lasers.Length; i ++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void CalculateMovement()
    {
        // Apply downward movement
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // Apply circular movement
        float x = Mathf.Cos(_angle * Mathf.Deg2Rad) * _radius;
        float y = Mathf.Sin(_angle * Mathf.Deg2Rad) * _radius;

        Vector3 circularMotion = new Vector3(x, y, 0);
        transform.position += circularMotion * Time.deltaTime;

        // Update angle within 360 degrees
        _angle += _rotationSpeed * Time.deltaTime;
        if (_angle >= 360.0f)
        {
            _angle -= 360.0f;
        }

        // Wrap position vertically
        if (transform.position.y <= -10.55f)
        {
            float randomX = Random.Range(-9.50f, 9.50f);
            transform.position = new Vector3(randomX, 7.60f, 0);
        }    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
         
        
        if (other.tag == "Player")
        {
            //damage player
            Player player = other.transform.GetComponent<Player>();         
            if (player != null)
            {
                player.Damage();
            }

            Destroy(GetComponent<Collider2D>());
            _anim.SetTrigger("OnEnemyDeath");//trigger anim
            _speed = 0;
            _audioSource.Play();

            DestroyChildrenObjects();
            Destroy(this.gameObject, 2.8f);
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
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            DestroyChildrenObjects();
            Destroy(this.gameObject, 2.8f);
        }

    }
    
    private void DestroyChildrenObjects()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }



}
