using System.Collections;
using System.Collections.Generic;
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
    private int _enemySpawnedCount = 0;//number of enemies spawned (in wave)
    [SerializeField]
    private int _enemiesPerpawn = 5;
    [SerializeField]
    private int _enemyWaveCount = 3;//number of waves to spawn
   
    [SerializeField]
    private int _totalWaveSpawn = 3;// total number of waves


    private bool _stopSpawning = false;
    
  
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
       
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

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.5f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            _enemySpawnedCount++;

            //check if desired number of enemies have spawned
            if (_enemySpawnedCount >= 5)
            {
                _stopSpawning = true;
                //EnemyWaveReset();

                //reset counter to 0 for next wave
            }

            yield return new WaitForSeconds(5.0f);
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
