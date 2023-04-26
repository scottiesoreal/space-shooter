using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private GameObject _Enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 6f, 0);   
    }

    // Update is called once per frame
    void Update()
    {
        //move down 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        
        //if bottom of screen
        if (transform.position.y <= -10.55f)
        {
            //respawn at top with a new random x Position
            float randomX = Random.Range(-9.50f, 9.50f);
            transform.position = new Vector3(Random.Range(randomX, 9.50f), 7.60f, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
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

            Destroy(this.gameObject);
        }
       
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }


    }
}
