using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerMovement : MonoBehaviour
{
    
    public float speed = 5;
    public float rollDist = 20;
    public float rollDelay = 10;
    public float rollTime = 0.7f;
    public bool isRolling = false;
    public bool disableMove = false;
    public InputMaster actions;

    private InputAction Move;
    private InputAction Roll;
    Vector2 move = new Vector2();

    private Vector2 rollStart = new Vector2();
    private Vector2 rollGoal = new Vector2();
    private Vector2 rollMove = new Vector2();

    private void Awake()
    {
        actions = new InputMaster();
        actions.Enable();
        Move = actions.Player.Move;
        Roll = actions.Player.Roll;
        Move.Enable();
        Roll.Enable();

        Roll.performed += OnRoll;
    }

    private void OnEnable()
    {
        Move.Enable();
        Roll.Enable();
    }

    private void OnDisable()
    {
        Move.Disable();
        Roll.Disable();
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
        }

        if (isRolling)
        {
            rollMove = (rollGoal - rollStart) / rollDelay;
            transform.Translate(rollMove);
            rollStart += rollMove;
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
}
