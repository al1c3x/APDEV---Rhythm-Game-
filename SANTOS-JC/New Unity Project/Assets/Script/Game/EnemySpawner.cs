using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyGO;
    public float spawnTimer = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        GameObject anEnemy = (GameObject)Instantiate(EnemyGO);
        anEnemy.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        SpawnEnemyRelease();
    }

    void SpawnEnemyRelease()
    {
        float timer;

        if (spawnTimer > 1.0f)
        {
            timer = Random.Range(1.0f, spawnTimer);
        }
        else
        {
            timer = 1.0f;
        }

        Invoke("SpawnEnemy", timer);
    }

    void LevelDiff()
    {
        if (spawnTimer > 1.0f)
        {
            spawnTimer--;
        }
        if (spawnTimer == 1)
        {
            CancelInvoke("LevelDiff");
        }
    }
    public void StartSpawner()
    {
         spawnTimer = 5.0f;
        Invoke("SpawnEnemy", spawnTimer);

        InvokeRepeating("LevelDiff", 0, 30);

    }

    public void StopSpawner()
    {
        CancelInvoke("SpawnEnemy");
        CancelInvoke("LevelDiff");
    }
}
