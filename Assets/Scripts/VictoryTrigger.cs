using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Finish");
        if (other.CompareTag("Player"))
            gameManager.FinishGame();
    }
}
