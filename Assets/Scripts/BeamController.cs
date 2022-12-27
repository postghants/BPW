using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    public int health;
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
        health = (int)floorManager.currentFloorStats[11];
    }

    private void FixedUpdate()
    {
        if (shakeTimer > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitCircle * currentShakeMagnitude;
            shakeTimer -= Time.deltaTime;
            currentShakeMagnitude -= shakeMagnitude / (shakeDuration/Time.deltaTime);
        }
        else
        {
            shakeTimer = 0;
            transform.localPosition = initialPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Swing"))
        {
            health--;
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
