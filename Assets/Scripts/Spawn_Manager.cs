using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _TripleShotPU_Prefab;

    private bool _stopSpawning = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    //spawn game objects every five seconds
    //create a coroutine of type IEnumerator -- Yield Events
    //while loop (infinite game loop) -- will run as long as condition remains true
    // "while (true) will crash the computer because it won't stop running, and will take up memory
    // use a Yield Event

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 9f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }      
        
        //Then this line is called
        //while loop (infinite)
            //Instantiate enemy prefab
            //yield wait for 5 seconds.

        
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.6f, 0f);
            Instantiate(_TripleShotPU_Prefab, posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));

        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    


}
