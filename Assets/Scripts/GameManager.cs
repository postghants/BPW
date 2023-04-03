using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform deathUI;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void BeamDestroyed()
    {

    }    

    public void EndGame()
    {
        FindObjectOfType<PlayerMovement>().enabled = false;
        FindObjectOfType<EnemySpawner>().enabled = false;
        FindObjectOfType<PointerController>().enabled = false;
        EnemyController[] enemyList = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemyList)
        {
            enemy.enabled = false;
        }
        EnemyMeleeController[] meleeList = FindObjectsOfType<EnemyMeleeController>();
        foreach (EnemyMeleeController melee in meleeList)
        {
            melee.enabled = false;
        }
        BulletController[] bulletList = FindObjectsOfType<BulletController>();
        foreach (BulletController bullet in bulletList)
        {
            bullet.enabled = false;
        }
        deathUI.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void FinishGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
