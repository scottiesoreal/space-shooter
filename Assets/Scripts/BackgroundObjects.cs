using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObjects : MonoBehaviour
{
    
    [SerializeField]
    private float _movementSpeed = 1f;
    [SerializeField]
    private float _speedMultiplier = 2;
    [SerializeField]

    //public SpriteRenderer planetSpriteRenderer;
    //public Color[] spriteColors;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }


    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _movementSpeed * Time.deltaTime);

        // Wrap position vertically
        if (transform.position.y <= -7.55f)
        {
            float randomX = Random.Range(-9.50f, 9.50f);
            float randomZ = Random.Range(-5.60f, 5f);
            transform.position = new Vector3(randomX, 7.60f, randomZ);
        }
    }


}
