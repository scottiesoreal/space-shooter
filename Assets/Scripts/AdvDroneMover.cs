using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvDroneMover : MonoBehaviour
{
    //movement
    [SerializeField]
    private float _speed = 2.0f;

    private Player _player;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        //audio source(?)

        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
        }


    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    //movement downward
    private void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
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

    private void DestroyChildrenObjects()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}
