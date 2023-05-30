using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shields : MonoBehaviour
{
    [SerializeField]
    private int _shieldLives = 3;
    [SerializeField]
    private GameObject _shield;

    // Active variables
    [SerializeField]
    private bool _isShieldActive = false;

    // Public property to access the shield's active state
    public bool IsShieldActive
    {
        get { return _isShieldActive; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShieldDamage()
    {
        _shieldLives--;

        if (_shieldLives == 2)
        {
            // Change shield appearance or perform an effect for medium damage
        }

        if (_shieldLives == 1)
        {
            // Change shield appearance or perform an effect for low damage
        }

        if (_shieldLives < 1)
        {
            DisableShield();
        }
    }

    public void ActivateShield()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    public void DisableShield()
    {
        _isShieldActive = false;
        _shield.SetActive(false);
    }
}
