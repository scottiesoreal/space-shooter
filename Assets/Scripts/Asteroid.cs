using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 3.0f;    
    [SerializeField]
    private GameObject _explosionPrefab;
    private Spawn_Manager _spawn_Manager;

    private void Start()
    {
        _spawn_Manager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();
        if (_spawn_Manager == null)
        {
            Debug.LogError("Spawn Manager is Null.");//good habits for GetComponent.
        }
    }


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
            _spawn_Manager.StartSpawning();
            Destroy(this.gameObject, .25f);
        }

    }

    //isnantiate explosion at the position of the asteroid
    //destroy the explosion after 3 seconds

}
