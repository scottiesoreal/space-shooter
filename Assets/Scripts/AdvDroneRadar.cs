using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvDroneRadar : MonoBehaviour
{
    [SerializeField]
    private float _laserDetectionDistance = 2.0f;

    [SerializeField]
    private bool _laserInRange = false;

    // Properties to access the variables
    public float LaserDetectionDistance => _laserDetectionDistance;
    public bool LaserInRange => _laserInRange;
    public Vector3 DetectedLaserPosition { get; private set; }


    private Player _player;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        LaserDetection();
    }


    //handle laser detection
    private void LaserDetection()
    {
        if (_player == null)
        {
            Debug.LogError("Player is NULL.");
            return;
        }

        //check if laser is in range
        if (Vector3.Distance(transform.position, _player.transform.position) < _laserDetectionDistance)
        {
            _laserInRange = true;
            Debug.Log("Laser detected");
            //Report laser detection to parent of drone script
            transform.parent.GetComponent<AdvancedDrone>().HandleLaserDetected();
        }
        else
        {
            _laserInRange = false;
            Debug.Log("Laser not detected");
        }

    }

    public void SetLaserInRange(bool value)
    {
        _laserInRange = value;
    }

}
