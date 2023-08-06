using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject EnemyBulletGO;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("FireEnemyBullet", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FireEnemyBullet()
    {
        GameObject playership = GameObject.Find("PlayerGO");

        if(playership != null)
        {
            GameObject bullet = (GameObject)Instantiate(EnemyBulletGO);
            bullet.transform.position = transform.position;

            Vector2 dir = playership.transform.position - bullet.transform.position;

            bullet.GetComponent<EnemyBullet>().SetDirection(dir);
        }
    }
}
