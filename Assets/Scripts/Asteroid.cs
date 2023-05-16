using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 3.0f;    
    [SerializeField]
    private GameObject _explosionPrefab;


   
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    //check for LASER collision (Trigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

    }

    //isnantiate explosion at the position of the asteroid
    //destroy the explosion after 3 seconds

}
