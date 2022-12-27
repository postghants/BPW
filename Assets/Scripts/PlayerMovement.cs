using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerMovement : MonoBehaviour
{
    
    public float speed = 5;
    public float rollDist = 20;
    public float rollDelay = 10;
    public float rollTime = 0.7f;
    public float fireDelay = 0.4f;
    public float fireLifetime = 0.2f;
    public float fireDistance = 1f;

    public float xFallDistance;
    public float yFallDistance;

    public bool isRolling = false;
    public bool isFiring = false;
    public bool disableMove = false;

    public Transform look;
    public Object projectile;
    public InputMaster actions;
    public GameManager gameManager;

    public SpriteRenderer spriteRenderer;
    public SpriteRenderer bat;
    public Transform batTransform;

    private InputAction Move;
    private InputAction Roll;
    private InputAction Fire;

    Vector2 move = new Vector2();
    private float fireRotation = 0f;
    private Vector2 initialBatLocation;
    public float initialBatRotation;
    public Vector2 swungBatLocation;
    public float swungBatRotation;

    private Vector2 rollStart = new Vector2();
    private Vector2 rollGoal = new Vector2();
    private Vector2 rollMove = new Vector2();
    private bool LookLeft = false;
    private bool LookRight = true;

    

    private void Awake()
    {
        actions = new InputMaster();
        actions.Enable();
        Move = actions.Player.Move;
        Roll = actions.Player.Roll;
        Fire = actions.Player.Fire;
        Move.Enable();
        Roll.Enable();
        Fire.Enable();

        Fire.performed += OnFire;
        Roll.performed += OnRoll;

        initialBatLocation = bat.gameObject.transform.localPosition;
    }

    private void OnEnable()
    {
        Move.Enable();
        Roll.Enable();
        Fire.Enable();
    }

    private void OnDisable()
    {
        Move.Disable();
        Roll.Disable();
        Fire.Disable();
    }

    private void Update()
    {
        move = Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!disableMove)
        {
            move *= speed;
            move *= Time.deltaTime;
            transform.Translate(move);

            if (transform.position.x >= xFallDistance || transform.position.y >= yFallDistance || transform.position.x <= -xFallDistance || transform.position.y <= -yFallDistance)
            {
                gameManager.EndGame();
                return;
            }
        }
        
        if (look.position.x > transform.position.x + 0.2 && !LookRight)
        {
            spriteRenderer.flipX = false;
            batTransform.localScale = Vector3.one;
            LookRight = true;
            LookLeft = false;
        }
        if (look.position.x < transform.position.x - 0.2 && !LookLeft)
        {
            spriteRenderer.flipX = true;
            batTransform.localScale = new Vector3(-1f, 1f, 1f);
            LookLeft = true;
            LookRight = false;
        }
        

        if (isRolling)
        {
            rollMove = (rollGoal - rollStart) / rollDelay;
            transform.Translate(rollMove);
            rollStart += rollMove;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Enemy")) && !disableMove)
        {
            gameManager.EndGame();
            Destroy(other.gameObject);
        }
    }

    private void OnRoll(InputAction.CallbackContext context)
    {
        if (!isRolling && move != Vector2.zero)
        {
            isRolling = true;
            disableMove = true;
            StartCoroutine(Rolling());

        }
        else return;
    }

    private IEnumerator Rolling()
    {
        rollStart = new Vector2(transform.position.x, transform.position.y);
        rollGoal = move.normalized * rollDist + rollStart;
        yield return new WaitForSeconds(rollTime);
        isRolling = false;
        disableMove = false;
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(Firing());
        }

    }

    private IEnumerator Firing()
    {
        Vector2 lookRelative = look.position - transform.position;
        GameObject swing = (GameObject)PrefabUtility.InstantiatePrefab(projectile);
        Transform swingTrans = swing.transform;
        bat.enabled = false;


        swingTrans.position = lookRelative.normalized * fireDistance + new Vector2(transform.position.x, transform.position.y);
        if(swingTrans.position.y < transform.position.y)
        {
            fireRotation = -Vector2.Angle(new Vector2(1, 0), lookRelative);
        }
        else
        {
            fireRotation = Vector2.Angle(new Vector2(1, 0), lookRelative);
        }
        swingTrans.Rotate(new Vector3(0, 0, fireRotation));
        swingTrans.parent = transform;

        yield return new WaitForSeconds(fireLifetime);
        Destroy(swing);
        bat.enabled = true;
        bat.gameObject.transform.SetPositionAndRotation(bat.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        bat.gameObject.transform.localPosition = swungBatLocation;
        bat.gameObject.transform.SetPositionAndRotation(bat.gameObject.transform.position, Quaternion.Euler(0, 0, swungBatRotation * batTransform.localScale.x));
        yield return new WaitForSeconds(fireDelay);
        isFiring = false;
        bat.gameObject.transform.SetPositionAndRotation(bat.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        bat.gameObject.transform.localPosition = initialBatLocation;
        bat.gameObject.transform.SetPositionAndRotation(bat.gameObject.transform.position, Quaternion.Euler(0, 0, initialBatRotation * batTransform.localScale.x));
    }
}
