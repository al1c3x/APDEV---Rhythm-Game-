using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    float speed;
    Vector2 dir;
    bool IsReady;

    private void Awake()
    {
        speed = 5.0f;
        IsReady = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetDirection(Vector2 dirr)
    {
        dir = dirr.normalized;
        IsReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsReady)
        {
            Vector2 pos = transform.position;
            pos += dir * speed * Time.deltaTime;

            transform.position = pos;

            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            if(transform.position.x < min.x || transform.position.x > max.x|| transform.position.y < min.y || transform.position.y > max.y)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerShipTag")
        {
            Destroy(gameObject);
        }
    }


}
