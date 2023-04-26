using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //spawn game objects every five seconds
    //create a coroutine of type IEnumerator -- Yield Events
    //while loop (infinite game loop) -- will run as long as condition remains true
    // "while (true) will crash the computer because it won't stop running, and will take up memory
    // use a Yield Event

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 0, Random.Range(-7.6f, 7.6f));
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }      
        
        //Then this line is called
        //while loop (infinite)
            //Instantiate enemy prefab
            //yield wait for 5 seconds.

        
    }
}
