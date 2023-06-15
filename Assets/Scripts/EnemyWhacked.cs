using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWhacked : MonoBehaviour
{
    public float speed;
    public float stunnedTime;
    public float shakeMagnitude;
    public Color stunnedColor;
    public Color whackedColor;
    public Vector2 direction;
    public SpriteRenderer spriteRenderer;

    private Vector2 initialPosition;
    private float shakeTimer;
    private float currentShakeMagnitude;

    private Vector2 move;

    void OnEnable()
    {
        initialPosition = transform.localPosition;
        StartCoroutine(Stunned());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shakeTimer > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitCircle * currentShakeMagnitude;
            shakeTimer -= Time.deltaTime;
            currentShakeMagnitude -= shakeMagnitude / (stunnedTime / Time.deltaTime);
        }
        else
        {
            move = direction * speed;
            move *= Time.deltaTime;
            transform.Translate(move);
        }
    }

    private IEnumerator Stunned()
    {
        spriteRenderer.color = stunnedColor;
        shakeTimer = stunnedTime;
        currentShakeMagnitude = shakeMagnitude;
        yield return new WaitForSeconds(stunnedTime);

        transform.localPosition = initialPosition;
        spriteRenderer.color = whackedColor;
    }
}
