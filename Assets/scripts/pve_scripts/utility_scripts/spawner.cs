using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRadius = 25f;
    public float spawnInterval = 10f;
    public int maxEnemies = 5;

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    public void SpawnEnemy()
    {
        Vector2 randomPosition = GetRandomPositionOutsideCamera();
        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    }

    Vector2 GetRandomPositionOutsideCamera()
    {
        Camera mainCamera = Camera.main;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float randomX = Random.Range(-cameraWidth / 2f, cameraWidth / 2f);
        float randomY = Random.Range(-cameraHeight / 2f, cameraHeight / 2f);

        float spawnX = Mathf.Clamp(randomX, mainCamera.transform.position.x - cameraWidth / 2f, mainCamera.transform.position.x + cameraWidth / 2f);
        float spawnY = Mathf.Clamp(randomY, mainCamera.transform.position.y - cameraHeight / 2f, mainCamera.transform.position.y + cameraHeight / 2f);

        Vector2 spawnPosition = new Vector2(spawnX, spawnY);

        return spawnPosition;
    }
}