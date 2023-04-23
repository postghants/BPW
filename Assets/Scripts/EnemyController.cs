using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float playerDistance;
    public float stillDistance = 5;
    public float maxDistance = 7;
    public float minDistance = 2;

    public float speed = 3;
    public float bulletSpeed = 2;
    public float whackedSpeed = 3;
    public float fireDistance = 0.5f;
    public float fireDelay = 2;
    public float deviationMoving = 10;
    public float deviationStill = 4;

    public GameObject powerUp;
    public float dropChance = 10;

    public float xSpawnDistance;
    public float ySpawnDistance;

    public float fireTimer;
    public SpriteRenderer sprite;
    public SpriteRenderer gunSprite;
    public Transform shadow;
    public Color spawnColor;
    public BoxCollider2D boxCollider;
    public Transform player;
    public EnemyWhacked enemyWhacked;
    public GameObject projectile;

    private Vector2 whackedDirection;
    private float spawnTimer;
    private bool LookRight;
    private bool LookLeft;
    public enum StateEnum {Moving, Retreating, Still, Spawning}
    private StateEnum state;

    private void Start()
    {
        state = StateEnum.Spawning;
        transform.Translate(0, 3, 0);
        shadow.Translate(0, -3, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "ReflectBullet":
                if(other.gameObject.name == "EnemyMelee(Clone)")
                {
                }
                else
                {
                    Destroy(other.gameObject);
                }
                Destroy(gameObject); break;
            case "Swing":
                if (Random.Range(0, 100) < dropChance)
                {
                    GameObject pu = Instantiate(powerUp, transform.position, Quaternion.identity, transform.parent);
                    pu.transform.localScale = new Vector3(1/transform.parent.localScale.x, 1/transform.parent.localScale.y, 1);
                }
                gameObject.tag = "ReflectBullet"; 
                enemyWhacked.enabled = true;
                whackedDirection = other.transform.position - player.position;
                enemyWhacked.direction = whackedDirection;
                enemyWhacked.speed = whackedSpeed;

                this.enabled = false; break;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            transform.Translate((other.transform.position - transform.position).normalized * -0.05f);
        }
    }

    private void Update()
    {
        if (player.position.x > transform.position.x + 0.2 && !LookRight)
        {
            sprite.flipX = false;
            gunSprite.transform.parent.transform.localScale = Vector3.one;
            gunSprite.sortingOrder = 2;
            LookRight = true;
            LookLeft = false;
        }
        if (player.position.x < transform.position.x - 0.2 && !LookLeft)
        {
            sprite.flipX = true;
            gunSprite.transform.parent.transform.localScale = new Vector3(-1f, 1f, 1f);
            gunSprite.sortingOrder = 0;
            LookLeft = true;
            LookRight = false;
        }
    }

    private void FixedUpdate()
    {
        playerDistance = Vector2.Distance(transform.position, player.position);
        fireTimer -= Time.deltaTime;
        if(fireTimer <= 0)
        {
            Fire();
        }

        switch(state)
        {
            case StateEnum.Moving: MovingBehaviour(); break;
            case StateEnum.Retreating: RetreatingBehaviour(); break;
            case StateEnum.Still: StillBehaviour(); break;
            case StateEnum.Spawning: SpawningBehaviour(); break;
        }
    }

    public void MovingBehaviour()
    {
        Vector2 move = player.position - transform.position;
        move.Normalize();
        move *= speed;
        move *= Time.deltaTime;
        transform.Translate(move);

        if(playerDistance <= stillDistance)
        {
            state = StateEnum.Still;
        }
    }

    public void RetreatingBehaviour()
    {
        if(transform.position.x >= xSpawnDistance || transform.position.y >= ySpawnDistance || transform.position.x <= -xSpawnDistance || transform.position.y <= -ySpawnDistance)
        {
            state = StateEnum.Still;
            return;
        }
        
        Vector2 move = transform.position - player.position;
        move.Normalize();
        move *= speed;
        move *= Time.deltaTime;
        transform.Translate(move);

        if(playerDistance >= stillDistance)
        {
            state = StateEnum.Still;
        }
        
    }
    public void StillBehaviour()
    {

        if(playerDistance > maxDistance)
        {
            state = StateEnum.Moving;
        }
        
        if(playerDistance < minDistance)
        {
            state = StateEnum.Retreating;
        }
    }

    public void SpawningBehaviour()
    {
        spawnColor.a += 0.05f;
        sprite.color = spawnColor;
        gunSprite.color = spawnColor;
        transform.Translate(0, -0.15f, 0);
        shadow.Translate(0, 0.15f, 0);
        spawnTimer += 0.15f;
        if (spawnTimer >= 3)
        {
            boxCollider.enabled = true;
            state = StateEnum.Still;
        }


    }

    public void Fire()
    {
        fireTimer = fireDelay;
        Vector2 lookRelative = player.position - transform.position;
        GameObject bullet = Instantiate(projectile);
        BulletController bulletController = bullet.GetComponent<BulletController>();

        bulletController.bulletDirection = lookRelative.normalized;
        bulletController.bulletSpeed = bulletSpeed;

        bullet.transform.position = lookRelative.normalized * fireDistance + new Vector2(transform.position.x, transform.position.y);
        bullet.transform.parent = transform.parent;
    }

}
