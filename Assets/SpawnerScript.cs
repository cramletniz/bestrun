using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform obstacleParent;

    public float obstacleSpawnTime = 2f;
    private float timeUntilObstacleSpawn;
    public float obstacleSpeed = 4f;

    [Range(0, 1)] public float obstacleSpawnTimeFactor = 0.1f;
    [Range(0, 1)] public float obstacleSpeedFactor = 0.2f;

    private float _obstacleSpawnTime;
    private float _obstacleSpeed;

    private float timeAlive; 

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.onGameOver.AddListener(ClearObstacles);
        GameManager.Instance.onPlay.AddListener(ResetFactors);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isPlaying) {
            timeAlive += Time.deltaTime;

            CalculateFactors();

            SpawnLoop();
        }
    }

    private void SpawnLoop(){
        timeUntilObstacleSpawn += Time.deltaTime;

        if(timeUntilObstacleSpawn >= obstacleSpawnTime) {
            Spawn();
            timeUntilObstacleSpawn = 0f;
        }
    }

    private void ClearObstacles(){
        foreach(Transform child in obstacleParent) {
            Destroy(child.gameObject);
        }
    }

    private void CalculateFactors(){
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnTimeFactor);
        _obstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);
    }

    private void ResetFactors(){
        timeAlive = 1f;
        _obstacleSpawnTime = obstacleSpawnTime;
        _obstacleSpeed = obstacleSpeed;
    }

    private void Spawn(){
        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
        
        spawnedObstacle.transform.parent = obstacleParent;

        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.velocity = Vector2.left * _obstacleSpeed;
    }
}
