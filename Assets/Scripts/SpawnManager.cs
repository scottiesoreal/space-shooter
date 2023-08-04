using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Enemies
    private List<GameObject> _enemyPrefabs;//list of enemy prefabs
    [SerializeField]
    private GameObject _droneEnemyPrefab;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    
    //Powerups
    [SerializeField]
    private GameObject[] powerups;//array of powerups


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

    //Enemy Aggression probability
    [SerializeField]
    private float _aggressionProbability = .25f; // 25% chance of enemy being aggressive
    
    



    private bool _stopSpawning = false;


    public void StartSpawning()
    {
        _enemyPrefabs = new List<GameObject>();
        _enemyPrefabs.Add(_enemyPrefab);


        //Initialize other routines
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnWavesRoutine());

    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.5f, 0f);//spawn at top of screen
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);//spawn enemy

            //Enemy Aggression based on probability
            float randomAggression = Random.value;
            if (randomAggression < _aggressionProbability)
            {
                newEnemy.GetComponent<Enemy>().SetAggressive(true);
            }

            newEnemy.transform.parent = _enemyContainer.transform;//set parent to enemy container

            _enemySpawnedCount++;//increment number of enemies spawned

            //check if desired number of enemies have spawned
            if (_enemySpawnedCount >= _enemiesPerSpawn)
            {
                _stopSpawning = true;
            }

            yield return new WaitForSeconds(10.0f);
        }
    }

    //Enemy aggression behavior


    IEnumerator SpawnWavesRoutine()
    {
        while (_currentWave <= _totalEnemyWaveCount)
        {
            yield return new WaitForSeconds(20.5f); // Wave delay

            _stopSpawning = false;
            _enemySpawnedCount = 0; // Reset spawn count for each wave

            // Increase enemies per wave
            _enemiesPerSpawn += _enemiesPerWaveIncrement;

            // Randomly choose between original enemy and drone enemy
            for (int i = 0; i < _enemiesPerSpawn; i++)
            {
                GameObject enemyToSpawn;
                float randomEnemy = Random.value;

                if (randomEnemy < 0.5f) // 50% chance to spawn original enemy
                    enemyToSpawn = _enemyPrefabs[0];
                else // 50% chance to spawn drone enemy
                    enemyToSpawn = _droneEnemyPrefab;

                Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.5f, 0f);
                GameObject newEnemy = Instantiate(enemyToSpawn, posToSpawn, Quaternion.identity);

                // ... (Other settings and scripts for enemy spawning)

                newEnemy.transform.parent = _enemyContainer.transform;
            }

            // Increment the wave # for next iteration
            _currentWave++;
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        _stopSpawning = false; //Reset stop spawning at begining of each wave
        bool shieldSpawnOnce = false; //Flag to track first shield spawn

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 7.6f, 0f);

            //Define the probabiliies of spawning each powerup
            //Ensure the sum of all probabilities is equal to 1.0


            //float healthPickupProbability = 0.1f; //10% chance of health pickup (if more than 1 powerup)
            float ammoPickupProbability = 0.6f; //60% chance of ammo pickup
            float shieldPickupProbability = shieldSpawnOnce ? 0.9f : 0.25f; // 90% chance for the first spawn, 25% for subsequent spawns
            float antiAmmoPickupProbability = 0.05f; //5% chance of anti ammo pickup

            //Generate a random number between 0.0 and 1.0
            float randomPickup = Random.value;

            //Determine which powerup to spawn based on the probability
            if (randomPickup < ammoPickupProbability)
            {
                int randomAmmoPickup = Random.Range(3, powerups.Length); //element 3 is ammo pickup
                Instantiate(powerups[randomAmmoPickup], posToSpawn, Quaternion.identity);
            }
            else if (randomPickup < (ammoPickupProbability + antiAmmoPickupProbability))
            {
                //Spawn the anti ammo pickup
                int randomAntiAmmoPickup = Random.Range(6, powerups.Length); //element 6 is anti ammo pickup
                Instantiate(powerups[randomAntiAmmoPickup], posToSpawn, Quaternion.identity);
            }

            else if (randomPickup < (ammoPickupProbability + shieldPickupProbability))
            {
                //Spawn the shield pickup
                int randomShieldPickup = Random.Range(2, powerups.Length); //element 2 is shield pickup
            }
            else
            {
                //Spawn health pickup... There's only one for now
                Instantiate(powerups[2], posToSpawn, Quaternion.identity);
            }

            //Wait between 3 and 8 seconds before spawning next powerup
            yield return new WaitForSeconds(Random.Range(3, 8));

            

        }

    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
