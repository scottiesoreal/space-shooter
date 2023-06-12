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
    [SerializeField]
    private GameObject _bigPlanetPrefab;
    [SerializeField]
    private GameObject _smallPlanetPrefab;


    private bool _stopSpawning = false;
    
  
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        SpawnBackgroundObjectsRoutine();
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
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnBackgroundObjectsRoutine()
    {
      while (_stopSpawning == false)
       {
            Vector3 posToSpawn = new Vector3(Random.Range(-10f, 10f), 4f, Random.Range(-7f, 30f)); 
            GameObject newBigPlanet = Instantiate(_bigPlanetPrefab, posToSpawn, Quaternion.identity);
            GameObject newLittlePlanet = Instantiate(_smallPlanetPrefab, posToSpawn, Quaternion.identity);
            
            SpriteRenderer bigPlanetRenderer = newBigPlanet.GetComponent<SpriteRenderer>();
            if (bigPlanetRenderer != null)
            {
                bigPlanetRenderer.color = Random.ColorHSV();
            }


            SpriteRenderer smallPlanetRenderer = newLittlePlanet.GetComponent<SpriteRenderer>();
            if (bigPlanetRenderer != null)
            {
                bigPlanetRenderer.color = Random.ColorHSV();
            }

            yield return new WaitForSeconds(Random.Range(7, 16));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
