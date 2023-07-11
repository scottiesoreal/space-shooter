using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    

    //wave
    [SerializeField]
    private int _enemySpawnedCount = 0;//current number of enemies spawned (in wave)
    [SerializeField]
    private int _enemiesPerSpawn = 5; //number of enemy to spawn per wave
    [SerializeField]
    private int _currentWave = 1; // Current Wave number
    [SerializeField]
    private int _initialEnemiesPerSpawm = 5;
    [SerializeField]
    private int _enemiesPerWaveIncrement = 5;
    [SerializeField]
    private int _totalEnemyWaveCount = 3;//number of waves to spawn
    

    private bool _stopSpawning = false;
    
  
    public void StartSpawning()
    {

        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnWavesRoutine());

    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.5f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            _enemySpawnedCount++;

            //check if desired number of enemies have spawned
            if (_enemySpawnedCount >= _enemiesPerSpawn)
            {
                _stopSpawning = true;
               
            }

            yield return new WaitForSeconds(10.0f);
        }
    }

    IEnumerator SpawnWavesRoutine()
    {
        while (_currentWave <= _totalEnemyWaveCount)
        {
            yield return new WaitForSeconds(20.5f); // Wave delay

            _stopSpawning = false;
            _enemySpawnedCount = 0; //reset spawn count for each wave

            // Increase enemies per wave
            _enemiesPerSpawn += _enemiesPerWaveIncrement;

            // start spawning enemies for the current wave
            StartCoroutine(SpawnEnemyRoutine());

            // Increment the wave # for next iteration
            _currentWave++;
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.6f, 0f);
            int randomPowerUp = Random.Range(0, 5);
            if (randomPowerUp >= 4)
            {
                randomPowerUp = 5; //set random power up to 5 (6th power up) if the random number is 4 or higher
            }
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));

        }

    }


    //public void EnemyWaveReset()
   // {
    //    _enemySpawnedCount = 0;
   // }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
