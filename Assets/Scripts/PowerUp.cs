using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int powerUpID;
    [SerializeField]
    private GameObject _powerUpPrefab;

    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _powerUpClip;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Audio for PowerUp is Null.");
        }
        else
        {
            Debug.Log("Audio source found for powerup");
        }
    }

    void Update()
    {
        PowerUpMovement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoCount(15);
                        break;
                    case 4:                        
                        player.RestoreLives();
                        break;
                    default:
                        Debug.LogError("Unknown powerup ID: " + powerUpID);
                        break;
                }

                AudioSource.PlayClipAtPoint(_powerUpClip, transform.position);
                Destroy(this.gameObject);
            }
            else
            {
                Debug.LogError("Player component not found on player object");
            }
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
