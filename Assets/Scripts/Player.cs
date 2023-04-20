using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
     //Time.delta = Real time
     // public or private reference
     //data type (Int, float, bool, string)
     //Every variable has a name
     //optional: Value assigned
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    //New Vector3 variable created to add to player
    [SerializeField]
    private float _fireRate = 0.8f;
    private float _canFire = -1f;
    
    
    
    // Start is called before the first frame update
    void Start()
    {   
        //take the current position = new position (0x, 0y, 0z)
        transform.position = new Vector3(0, 1, 0);
        
    }

    // Update is called once per frame
    void Update()
    {                  
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

       

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //new Vector3 (1, 0, 0) * -1 * 3.5 * real time
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        //More optimal method to wrtie the movement code below comment
        //transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        //Vecotr3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //if player position on the y is greater than 0
        //y position = 0
        //else if position on the y is less than -3.8f

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }
        
        //if plwyer on the x >11.25f
        //x pos = -11.25f
        //if player on the x  < -9.25
        //x pos = 9.25

        if (transform.position.x >= 12.75f)
        {
            transform.position = new Vector3(12.75f, transform.position.y, 0);
        }
        else if (transform.position.x <= -12.75f)
        {
            transform.position = new Vector3(-12.75f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
         
        {   
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
        }
    }

}
