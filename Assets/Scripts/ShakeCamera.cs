using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    [SerializeField]
    private bool _isShaking = false;
    [SerializeField]
    private float _duration = 0.5f;
    [SerializeField]
    private float _magnitude = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartShaking()
    {
        StartCoroutine(CameraShakeRoutine());
    }

    public IEnumerator CameraShakeRoutine()//returns camera to origional position
    {
        Vector3 defaultPosition = transform.position;
        float elapsed = 0.0f;
        
        while (elapsed < _duration)
        {
            float xPosition = Random.Range(-1f, 1f) * _magnitude;
            float yPosition = Random.Range(1f, 1f) * _magnitude;
            transform.position = new Vector3(xPosition, yPosition, -10f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = defaultPosition;
    }



}
