using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWhacked : MonoBehaviour
{
    public float speed;
    public float stunnedTime;
    public Color stunnedColor;
    public Color whackedColor;
    public Vector2 direction;
    public SpriteRenderer spriteRenderer;

    private Vector2 move;
    private bool stun;

    void OnEnable()
    {
        stun = true;
        StartCoroutine(Stunned());
    }

    // Update is called once per frame
    void Update()
    {
        if (!stun)
        {
            move = direction * speed;
            move *= Time.deltaTime;
            transform.Translate(move);
        }
    }
    
    private IEnumerator Stunned()
    {
        spriteRenderer.color = stunnedColor;
        yield return new WaitForSeconds(stunnedTime);
        spriteRenderer.color = whackedColor;
        stun = false;
    }
}
