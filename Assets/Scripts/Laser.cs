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

    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
            
        }
        else
        {
            MoveDown();            
        }
    }


    void MoveUp()
    {
        //translate = laser move up
        transform.Translate(Vector3.up * _speed * Time.deltaTime);


        if (transform.position.y > 8.0f)
        {
            //check if this object has a parent
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            //destroy parent
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        //translate = laser move up
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

    
        if (transform.position.y < -8.0f)
        {
            //check if this object has a parent
            if(transform.parent != null)
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
               
        if (other.tag == "Player" && _isEnemyLaser && !_hasDamagedPlayer == true)
        {
            
            Player player = other.GetComponent<Player>();
                        
            if (player != null)
            {
                player.Damage();
               
            }
        }

    }

}
