using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 50f;
    public float radius = 15f;

    [Header("Difficulty Ramp")]
    public float rampEverySeconds = 50f;     // how often difficulty increases
    public int enemiesPerSpawn = 1;          // start spawning 1 per tick
    public int enemiesPerRamp = 1;           // increase batch size by this amount
    public float intervalDecrease = 0.1f;    // spawn faster each ramp
    public float minSpawnInterval = 0.5f;    // don't go faster than this
    public int maxEnemiesAlive = 40;         // cap so it doesn't explode

    float rampTimer = 0f;



    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);

        
    }
    void Update()
            {
                rampTimer += Time.deltaTime;

                if (rampTimer >= rampEverySeconds)
                {
                    rampTimer = 0f;

                    // Spawn more per tick
                    enemiesPerSpawn += enemiesPerRamp;

                    // Spawn faster over time
                    spawnInterval = Mathf.Max(minSpawnInterval, spawnInterval - intervalDecrease);

                    // Restart InvokeRepeating with new interval
                    CancelInvoke(nameof(SpawnEnemy));
                    InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
                    Debug.Log($"[SPAWNER RAMP] enemiesPerSpawn = {enemiesPerSpawn}, spawnInterval = {spawnInterval}");

                }
            }
    void SpawnEnemy()
    {
        // Cap the number alive (requires enemies tagged "Enemy")
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxEnemiesAlive)
            return;

        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * radius;
            pos.y = 1.3f;

            GameObject enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemy, pos, Quaternion.identity);
        }
    }

}
