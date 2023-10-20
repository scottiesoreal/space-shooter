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
    private GameObject[] _powerups;

    //time keeping variable
    [SerializeField]
    private float _totalTime = 0.0f;

    //Enemy Spawns
    [SerializeField]
    private bool _stopEnemySpawning = false;//declared at the class level for full availablity
    [SerializeField]
    private bool _stopDroneEnemySpawning = true;


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        
    }


    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopEnemySpawning == false)
        {
            if (_totalTime >= 20f)
            {
                _stopEnemySpawning = true;
            }
            
            Vector3 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.5f, 0f);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;//set parent of enemy to enemy container
            yield return new WaitForSeconds(5.0f);//wait 5 seconds before spawning next enemy

            _totalTime += 5.0f;//increment time by 5 seconds
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        _stopEnemySpawning = false; //Reset stop spawning at begining of each wave
        bool shieldSpawnOnce = false; //Flag to track first shield spawn

        while (_stopEnemySpawning == false)
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
                int randomAmmoPickup = Random.Range(3, _powerups.Length); //element 3 is ammo pickup
                Instantiate(_powerups[randomAmmoPickup], posToSpawn, Quaternion.identity);
            }
            else if (randomPickup < (ammoPickupProbability + antiAmmoPickupProbability))
            {
                //Spawn the anti ammo pickup
                int randomAntiAmmoPickup = Random.Range(6, _powerups.Length); //element 6 is anti ammo pickup
                Instantiate(_powerups[randomAntiAmmoPickup], posToSpawn, Quaternion.identity);
            }

            else if (randomPickup < (ammoPickupProbability + shieldPickupProbability))
            {
                //Spawn the shield pickup
                int randomShieldPickup = Random.Range(2, _powerups.Length); //element 2 is shield pickup
            }
            else
            {
                //Spawn health pickup... There's only one for now
                Instantiate(_powerups[2], posToSpawn, Quaternion.identity);
            }

            //Wait between 3 and 8 seconds before spawning next powerup
            yield return new WaitForSeconds(Random.Range(3, 8));



        }

    }



    public void OnPlayerDeath()
    {
        _stopEnemySpawning = true;
    }
}
