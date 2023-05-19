using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private float _willFire = 0.4f;
    [SerializeField]
    private float _fireRate = 3f;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private Player _player;
    private Animator _anim;

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
    void Update()
    {
        EnemyMovement();        
    }

    void EnemyFire()
    {
        _willFire = Time.time + _fireRate;
        GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position + Vector3.down, Quaternion.identity);
        

        //if (Time.time > _willFire)
        //{
            //_fireRate = Random.Range(3f, 7f);
            //_willFire = Time.time + _fireRate;
            //GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            //Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            //for (int i = 0; i < lasers.Length; i++)
            //{
              //  lasers[i].AssignEnemyLaser();
            //}
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
         
        
        if (other.tag == "Player")
        {
            //damage player
            Player player = other.transform.GetComponent<Player>();
            

            //null check
            if (player != null)
            {
                player.Damage();
            }

             _anim.SetTrigger("OnEnemyDeath");//trigger anim
            _speed = 0;
            _audioSource.Play();
             Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            //Player player = GameObject.Find("Player").GetComponent<Player>();
            if (_player != null)
            {
                _player.AddScore(10);
            }

             _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -10.55f)
        {            
            float randomX = Random.Range(-9.50f, 9.50f);
            transform.position = new Vector3(Random.Range(randomX, 9.50f), 7.60f, 0);
        }

    }
}
