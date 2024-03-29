﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _droneEnemyPrefab;
    [SerializeField] 
    private GameObject _droneContainer;
    [SerializeField]
    private GameObject _advDroneEnemyPrefab;
    [SerializeField] 
    private GameObject _advDroneEnemyContainer;
    [SerializeField]
    private GameObject _bossPrefab;
    [SerializeField] 
    private GameObject _bossContainer;
    
    [SerializeField]
    private GameObject[] _powerups;//array of powerups
    
    private Player _player;//reference to player script

    //[SerializeField]
    //private GameManager _gameManager;

    //time keeping variable
    [SerializeField]
    private float _totalTime = 0.0f;
    [SerializeField]
    private float _betweenWaveTime = 0.0f;//time between waves
    [SerializeField]
    private bool _isWaitingBetweenWaves = false;

    //wave tracking variable
    [SerializeField]
    private int _waveNumber = 0;//current wave

    //Enemies on screen
    //[SerializeField]
    //private int _enemyCount = 0;
    //[SerializeField]
    //private int _droneEnemyCount = 0;
    

    //Enemy Spawns
    [SerializeField]
    private bool _stopEnemySpawning = false;//declared at the class level for full availablity
    [SerializeField]
    private bool _stopDroneEnemySpawning = true;
    [SerializeField]
    private bool _stopAdvDroneEnemySpawning = true;



    

    public void StartSpawning()
    {
        StartCoroutine(SpawnWaveRoutine());
        StartCoroutine(SpawnPowerupRoutine());

    }


    IEnumerator SpawnWaveRoutine()
    {
        _waveNumber += 1;

        while (true)  // Keeps the routine going indefinitely.
        {
            switch (_waveNumber)
            {
                case 1:
                    // Spawn regular enemies
                    if (_totalTime >= 15f)
                    {
                        _stopEnemySpawning = true;
                    }

                    if (!_stopEnemySpawning)
                    {
                        SpawnEnemy(_enemyPrefab, _enemyContainer);
                        yield return new WaitForSeconds(5.0f);
                        _totalTime += 5.0f;
                    }
                    else
                    {
                        yield return StartCoroutine(WaitBetweenWaves());
                        _waveNumber++;
                    }
                    break;

                case 2:
                    // Spawn drone enemies
                    if (_totalTime >= 35f)
                    {
                        _stopDroneEnemySpawning = true;
                    }

                    if (!_stopDroneEnemySpawning)
                    {
                        SpawnEnemy(_droneEnemyPrefab, _enemyContainer);
                        yield return new WaitForSeconds(5.0f);
                        _totalTime += 5.0f;
                    }
                    else
                    {
                        yield return StartCoroutine(WaitBetweenWaves());
                        _waveNumber++;
                    }
                    break;

                case 3:
                    // Spawn advanced drone enemies
                    if (_totalTime >= 65f)
                    {
                        _stopAdvDroneEnemySpawning = true;
                    }

                    if (!_stopAdvDroneEnemySpawning)
                    {
                        SpawnEnemy(_advDroneEnemyPrefab, _enemyContainer);
                        yield return new WaitForSeconds(5.0f);
                        _totalTime += 5.0f;
                    }
                    else
                    {
                        yield return StartCoroutine(WaitBetweenWaves());
                        _waveNumber++;  // Or reset to 1 if you want to loop back to the first wave after the last wave.
                    }
                    break;
                case 4:
                    SpawnBoss();
                    // Once spawned, break out of spawn loop to prevent further waves from spawing
                    _stopEnemySpawning = true;
                    _stopDroneEnemySpawning = true;
                    _stopAdvDroneEnemySpawning = true;
                    yield break;



            }

            yield return null;  // Ensures the loop doesn't run too fast and hog CPU.
        }
    }

    void SpawnEnemy(GameObject enemyPrefab, GameObject container)
    {
        Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.5f, 0f);
        GameObject newEnemy = Instantiate(enemyPrefab, posToSpawn, Quaternion.identity);
        newEnemy.transform.parent = container.transform;
    }

    void SpawnBoss()
    {
        Vector3 posToSpawn = new Vector3(0f, 12, 0f);
        GameObject newBoss = Instantiate(_bossPrefab, posToSpawn, Quaternion.identity);
        newBoss.transform.parent = _bossContainer.transform;
    }

    IEnumerator WaitBetweenWaves()
    {
        _isWaitingBetweenWaves = true;
        while (_isWaitingBetweenWaves)
        {
            _betweenWaveTime += Time.deltaTime;
            if (_betweenWaveTime >= 12f)
            {
                _isWaitingBetweenWaves = false;
                ResetWaveSettings();
            }
            yield return null;
        }
    }

    void ResetWaveSettings()
    {
        _betweenWaveTime = 0.0f;
        _stopEnemySpawning = false;
        _stopDroneEnemySpawning = false;
        _stopAdvDroneEnemySpawning = false;
    }



    IEnumerator SpawnPowerupRoutine()
{
    _stopEnemySpawning = false; // Reset stop spawning at the beginning of each wave
    bool shieldSpawnOnce = false; // Flag to track first shield spawn

    while (!_stopEnemySpawning)
    {
        Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.6f, 0f);

        // Define the probabilities of spawning each powerup
        // Ensure the sum of all probabilities is equal to 1.0

        float ammoPickupProbability = 0.45f;         // 55% chance of ammo pickup
        float shieldPickupProbability = shieldSpawnOnce ? 0.9f : 0.2f;  // 90% chance for the first spawn, 20% for subsequent spawns
        float antiAmmoPickupProbability = 0.04f;     // 4% chance of anti ammo pickup
        float homingShotProbability = 0.15f;         // 15% chance of homing shot

        // Generate a random number between 0.0 and 1.0
        float randomPickup = Random.value;

        if (randomPickup < ammoPickupProbability)
        {
            int randomAmmoPickup = Random.Range(3, _powerups.Length);
            Instantiate(_powerups[randomAmmoPickup], posToSpawn, Quaternion.identity);
        }
        else if (randomPickup < (ammoPickupProbability + antiAmmoPickupProbability))
        {
            int randomAntiAmmoPickup = Random.Range(6, _powerups.Length);
            Instantiate(_powerups[randomAntiAmmoPickup], posToSpawn, Quaternion.identity);
        }
        else if (randomPickup < (ammoPickupProbability + shieldPickupProbability + antiAmmoPickupProbability))
        {
            int randomShieldPickup = Random.Range(2, _powerups.Length);
            Instantiate(_powerups[randomShieldPickup], posToSpawn, Quaternion.identity);
        }
        else if (randomPickup < (ammoPickupProbability + shieldPickupProbability + antiAmmoPickupProbability + homingShotProbability))
        {
            // Add the logic for spawning the homing shot powerup.
           
            int randomHomingPickup = Random.Range(7, _powerups.Length);
            Instantiate(_powerups[randomHomingPickup], posToSpawn, Quaternion.identity);
        }
        else
        {
            // Spawn health pickup... There's only one for now
            Instantiate(_powerups[2], posToSpawn, Quaternion.identity);
        }

        // Wait between 3 and 8 seconds before spawning next powerup
        yield return new WaitForSeconds(Random.Range(3, 8));
    }
}




    public void OnPlayerDeath()
    {
        _stopEnemySpawning = true;
    }
}
