using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    //ID for Powerups
    [SerializeField] // 0 = TripSh, 1 = _speed, 2 = Shields
    private int powerupID;
    [SerializeField]
    private GameObject _PowerUpPrefab;


    void Update()
    {
        PowerUpMovement();                
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Collider");
        if (other.tag == "Player")
        {
            //Debug.Log("Player");
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                //if powerUP is 0
                //if (powerupID == 0)
                //player.TripleShotActive();
                //else if powerUP is 1
                //else if (powerupID == 1)
                //{
                //    Debug.Log("collected Speed");
                //}
                //else if (powerupID == 2)
                //{
                //    Debug.Log("Shield collected");
                //}
                
                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        Debug.Log("Speed");
                        break;
                    case 2:
                        player.ShieldActive();
                        Debug.Log("Got it");
                        break;
                    default:
                        Debug.Log("Default Value:)");
                        break;
                }
                //speed PU activate
                
                //else if PU is 2
                //shields powerup
            }
            Destroy(this.gameObject);
        }
        
    }

    void PowerUpMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -6.0f)
        {
            Destroy(this.gameObject);
            float randomX = Random.Range(-9.50f, 9.50f);
            transform.position = new Vector3(Random.Range(randomX, 9.50f), 7.60f, 0);
        }

    }
}
