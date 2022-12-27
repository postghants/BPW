using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : MonoBehaviour
{
    public int health;

    public FloorManager floorManager;
    public BoxCollider2D boxCollider;
    public SpriteRenderer spriteRenderer;
    public Sprite broken;

    private void OnEnable()
    {
        floorManager = GameObject.FindGameObjectWithTag("FloorManager").GetComponent<FloorManager>();
        health = (int)floorManager.currentFloorStats[11];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Swing") || other.gameObject.CompareTag("ReflectBullet")){
            health--; 
            if (health <= 0)
            {
                floorManager.BeamDestroyed();
                spriteRenderer.sprite = broken;
                boxCollider.enabled = false;
                this.enabled = false;
            }
        }
        
    }
}
