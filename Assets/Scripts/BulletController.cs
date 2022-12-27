using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    public Vector2 bulletDirection;
    private Vector2 bulletMove;
    private float reflectStopper = 1;
    private bool reflectDone = false;

    public float xExit = 20;
    public float yExit = 15;
    public float reflectBoost = 1.5f;
    public float stunnedTime = 0.1f;

    private void FixedUpdate()
    {
        bulletMove = bulletDirection * bulletSpeed;
        bulletMove *= Time.deltaTime;
        bulletMove *= reflectStopper;
        transform.Translate(bulletMove);
    }

    private void Update()
    {
        if(gameObject.CompareTag("ReflectBullet") && !reflectDone)
        {
            StartCoroutine(Reflect());
        }

        if(transform.position.x > xExit || transform.position.y > yExit || transform.position.x < -xExit || transform.position.y < -yExit)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Reflect()
    {
        reflectStopper = 0;
        reflectDone = true;
        yield return new WaitForSeconds(stunnedTime);
        bulletSpeed *= -reflectBoost;
        reflectStopper = 1;
    }
}