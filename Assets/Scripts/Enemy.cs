using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private Animator _anim;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }

        _anim = GetComponent<Animator>();


        if (_anim == null)
        {
            Debug.LogError("animator is null");//assign component to Anim
        }

    }
    // Update is called once per frame
    void Update()
    {
        EnemyMovement();        
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
