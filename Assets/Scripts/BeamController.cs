using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    public int health;
    private float invulTimer;

    public float shakeDuration = 1;
    public float shakeMagnitude = 0.5f;
    public float shakeTimer = 0;
    private float currentShakeMagnitude;
    private Vector2 initialPosition;


    public FloorManager floorManager;
    public BoxCollider2D boxCollider;
    public SpriteRenderer spriteRenderer;
    public Sprite broken;

    private void OnEnable()
    {
        initialPosition = transform.localPosition;
        floorManager = GameObject.FindGameObjectWithTag("FloorManager").GetComponent<FloorManager>();
        health = 2;
    }

    private void FixedUpdate()
    {
        if (shakeTimer > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitCircle * currentShakeMagnitude;
            shakeTimer -= Time.deltaTime;
            currentShakeMagnitude -= shakeMagnitude / (shakeDuration / Time.deltaTime);
        }
        else
        {
            shakeTimer = 0;
            transform.localPosition = initialPosition;
        }

        if(invulTimer > 0)
        {
            invulTimer -= Time.deltaTime;
            if(invulTimer <= 0)
            {
                invulTimer = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (invulTimer != 0)
            return;

        if (other.gameObject.CompareTag("Swing") || (other.gameObject.CompareTag("ReflectBullet") && other.gameObject.name == "EnemyMelee(Clone)"))
        {
            health--;
            invulTimer = 0.1f;
            shakeTimer = shakeDuration;
            currentShakeMagnitude = shakeMagnitude;
            if (health <= 0)
            {
                floorManager.BeamDestroyed();
                spriteRenderer.sprite = broken;
                boxCollider.enabled = false;
            }
        }

    }
}
