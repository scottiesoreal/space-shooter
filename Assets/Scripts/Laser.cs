using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    
    //speed variable of 8
    [SerializeField]
    private float _speed = 8.0f;  
   
    // Update is called once per frame
    void Update()
    {
        //translate = laser move up
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

    
        if (transform.position.y > 7.30f)
        {
            //check if this object has a parent
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            //destroy parent
            Destroy(this.gameObject);
        }
    }
    //Testing contact
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        
    }

}
