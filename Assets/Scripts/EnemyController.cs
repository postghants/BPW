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

    public float xSpawnDistance;
    public float ySpawnDistance;

    public float fireTimer;
    public Transform player;
    public EnemyWhacked enemyWhacked;
    public Object projectile;

    private Vector2 whackedDirection;
    public enum StateEnum {Moving, Retreating, Still}
    private StateEnum state;

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "ReflectBullet":
                if(other.gameObject.name == "EnemyMelee")
                {
                }
                else
                {
                    Destroy(other.gameObject);
                }
                Destroy(gameObject); break;
            case "Swing": 
                gameObject.tag = "ReflectBullet"; 
                enemyWhacked.enabled = true;
                whackedDirection = other.transform.position - player.position;
                enemyWhacked.direction = whackedDirection;
                enemyWhacked.speed = whackedSpeed;

                this.enabled = false; break;
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
        }
    }

    void MovingBehaviour()
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

    void RetreatingBehaviour()
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
    void StillBehaviour()
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

    void Fire()
    {
        fireTimer = fireDelay;
        Vector2 lookRelative = player.position - transform.position;
        GameObject bullet = (GameObject)PrefabUtility.InstantiatePrefab(projectile);
        BulletController bulletController = bullet.GetComponent<BulletController>();

        bulletController.bulletDirection = lookRelative.normalized;
        bulletController.bulletSpeed = bulletSpeed;

        bullet.transform.position = lookRelative.normalized * fireDistance + new Vector2(transform.position.x, transform.position.y);
        bullet.transform.parent = transform.parent;
    }
}
