using UnityEngine;
using UnityEngine.AI;


public class SpawnEnemy : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float counter;
    public int maxEnemies = 5;
    public float spawnInterval = 7f;
    public int currentEnemies = 0;
    public Transform Player;
    public NavMeshAgent agent;


    void Start()
    {
        
        if (Player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                Player = playerObject.transform;
            }
        }
    }


    void Update()
    {
        SpawnE();
        RandomStats();
        GetComponent<Transform>();


    }
    public void SpawnE()
    {
        counter += Time.deltaTime;
        if (counter >= spawnInterval)
        {

            Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
            currentEnemies += 1;
            counter = 0f;
        }
        for (int i = 0; i < currentEnemies; i++)
        {
            if (currentEnemies >= maxEnemies)
            {
                counter *= 0;
            }

        }
        if (currentEnemies <= maxEnemies)
        {
            counter *= 1;
        }

    }


    public void RandomStats()
    {
        agent.speed = (Random.Range(2f, 4f));
        agent.acceleration = (Random.Range(10f, 20f));
        agent.angularSpeed = (Random.Range(60f, 120f));
        agent.stoppingDistance = (Random.Range(1.5f, 2f));

    }
    
}
