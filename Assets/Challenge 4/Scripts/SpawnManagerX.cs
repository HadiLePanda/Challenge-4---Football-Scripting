using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject powerupPrefab;
    [SerializeField] private GameObject player;

    [Header("Settings")]
    [SerializeField] private float spawnRangeX = 10;
    [SerializeField] private float spawnZMin = 15; // set min spawn Z
    [SerializeField] private float spawnZMax = 25; // set max spawn Z
    [SerializeField] private float speedBaseMultiplier = 1.0f;
    [SerializeField] private float speedIncreasePercentagePerWave = 0.2f;

    private int enemyCount;
    private int currentWave = 1;

    public static SpawnManagerX singleton;

    private void Awake()
    {
        singleton = this;
    }

    private void Update()
    {
        // keep track of how many enemies are there
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // when no enemies remain, spawn next wave
        if (enemyCount == 0)
        {
            SpawnEnemyWave(currentWave);
        }
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        // make powerups spawn at player end
        Vector3 powerupSpawnOffset = new(0, 0, -15);

        // no powerups are left on the map
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            // spawn a new powerup
            Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }

        // spawn enemy balls based on wave number
        for (int i = 0; i < currentWave; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }

        // increase wave number
        currentWave++;

        // put player back at start
        ResetPlayerPosition();
    }

    // generate random spawn position for powerups and enemy balls
    private Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }

    // move player back to their own goal
    private void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 1, -7);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public float GetCurrentWaveSpeedMultiplier()
    {
        // increase enemy speed by a flat % bonus for every wave
        // for level 1:
        // the speed should be at base multiplier
        if (currentWave == 1)
            return speedBaseMultiplier;

        // for waves after level 1:
        // flat increase of x% every wave
        return speedBaseMultiplier + (speedIncreasePercentagePerWave * (currentWave - 1));
    }
}
