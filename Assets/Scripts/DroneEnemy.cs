using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemy : MonoBehaviour
{
    //movement
    [SerializeField]
    private float _speed = 4.0f;

    //create zigzag movement
    [SerializeField]
    
    
    //fire variable
    
    //[SerializeField]//this is the time between shots
    private float _fireRate = 3.0f;
    private float _canFire = -1;//when to start shooting

    //shield
    [SerializeField]
    private bool _isShieldActive = true;
    
    //visualizer
    [SerializeField]
    private GameObject _shieldVisualizer;

    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
