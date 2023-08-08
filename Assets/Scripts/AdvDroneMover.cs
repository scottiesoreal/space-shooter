using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvDroneMover : MonoBehaviour
{
    //movement
    [SerializeField]
    private float _speed = 2.0f;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
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
}
