using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public enum StateEnum { Moving, Knockback, Falling }
    private StateEnum state;

    private void Start()
    {
        state = StateEnum.Moving;
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
    
    private void FixedUpdate()
    {
        switch (state)
        {
            case StateEnum.Moving: MovingBehaviour(); break;
            case StateEnum.Knockback: KnockbackBehaviour(); break;
            case StateEnum.Falling: FallingBehaviour(); break;
        }
    }

    void MovingBehaviour()
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
        Destroy(gameObject);
    }
}
