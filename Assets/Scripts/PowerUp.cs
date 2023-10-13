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
    private Player _player;

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
                    case 5:
                        player.OmniShotActive();
                        break;
                    case 6://negative ammo power up
                        player.AntiAmmo();
                        break;
                    case 7://Homing missle power up
                       player.HomingMissleActive();
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
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Player player = GameObject.Find("Player").GetComponent<Player>();
            if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(GetComponent<Collider2D>());
            //_anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            //_audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            //DestroyChildrenObjects();
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

    


