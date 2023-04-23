using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyMeleeController : MonoBehaviour
{
    public Transform player;

    public float speed = 3;
    public float kbTime = 1.5f;
    public float kbActiveTime = 0.8f;
    public float kbDelay = 10;
    public float kbDistSwing = 6;
    public float kbDistProjectile = 3;

    public float xSpawnDistance;
    public float ySpawnDistance;

    private Vector2 kbStart = new Vector2();
    private Vector2 kbGoal = new Vector2();
    private Vector2 kbMove = new Vector2();
    public enum StateEnum { Moving, Knockback, Falling, Spawning }
    private StateEnum state;
    public SpriteRenderer sprite;
    public Transform shadow;
    public Color spawnColor;
    public BoxCollider2D boxCollider;
    private float spawnTimer;
    private bool LookRight;
    private bool LookLeft;

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
                gameObject.tag = "ReflectBullet";
                kbGoal = transform.position + (transform.position - other.transform.position).normalized * kbDistProjectile;
                OnKnockback();  break;
            case "Swing":
                gameObject.tag = "ReflectBullet";
                kbGoal = transform.position + (other.transform.position - player.position).normalized * kbDistSwing;
                OnKnockback(); break;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            transform.Translate((other.transform.position - transform.position).normalized * -0.05f);
        }
    }
    private void FixedUpdate()
    {
        switch (state)
        {
            case StateEnum.Moving: MovingBehaviour(); break;
            case StateEnum.Knockback: KnockbackBehaviour(); break;
            case StateEnum.Falling: FallingBehaviour(); break;
            case StateEnum.Spawning: SpawningBehaviour(); break;
        }
    }

    private void Update()
    {
        if (player.position.x > transform.position.x + 0.2 && !LookRight)
        {
            sprite.flipX = false;
            LookRight = true;
            LookLeft = false;
        }
        if (player.position.x < transform.position.x - 0.2 && !LookLeft)
        {
            sprite.flipX = true;
            LookLeft = true;
            LookRight = false;
        }
    }
    void SpawningBehaviour()
    {
        spawnColor.a += 0.05f;
        sprite.color = spawnColor;
        transform.Translate(0, -0.15f, 0);
        shadow.Translate(0, 0.15f, 0);
        spawnTimer += 0.15f;
        if (spawnTimer >= 3)
        {
            boxCollider.enabled = true;
            state = StateEnum.Moving;
        }


    }
    public void MovingBehaviour()
    {
        if (transform.position.x >= xSpawnDistance + 0.6 || transform.position.y >= ySpawnDistance + 0.7 || transform.position.x <= -xSpawnDistance - 0.6 || transform.position.y <= -ySpawnDistance - 0.7)
        {
            state = StateEnum.Falling;
            return;
        }
        Vector2 move = player.position - transform.position;
        move.Normalize();
        move *= speed;
        move *= Time.deltaTime;
        transform.Translate(move);

        
    }

    private void KnockbackBehaviour()
    {
        kbMove = (kbGoal - kbStart) / kbDelay;
        transform.Translate(kbMove);
        kbStart += kbMove;
    }

    private void OnKnockback()
    {
        kbStart = new Vector2(transform.position.x, transform.position.y);
        state = StateEnum.Knockback;
        StopCoroutine(Knockback());
        StartCoroutine(Knockback());
    }

    private IEnumerator Knockback()
    {
        yield return new WaitForSeconds(kbActiveTime);
        gameObject.tag = "Enemy";
        yield return new WaitForSeconds(kbTime - kbActiveTime);
        state = StateEnum.Moving;
    }

    private void FallingBehaviour()
    {
        transform.localScale = transform.localScale * 0.9f;
        if (transform.localScale.z <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
