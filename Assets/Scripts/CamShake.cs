using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    public float shakeMagnitude = 0.5f;
    public float shakeTimer = 0;
    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.localPosition;
    }

    public void ShakeScreen(float duration)
    {
        shakeTimer = duration;
    }

    public void FinalShake()
    {
        shakeTimer = Mathf.Infinity;
    }

    private void FixedUpdate()
    {
        if (shakeTimer > 0)
        {
            Vector2 shake = Random.insideUnitCircle * shakeMagnitude;
            transform.localPosition = new Vector3(initialPosition.x + shake.x, initialPosition.y + shake.y, initialPosition.z);
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeTimer = 0;
            transform.localPosition = initialPosition;
        }
    }
}
