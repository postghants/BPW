using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PointerController : MonoBehaviour
{
    public InputMaster actions;
    public new Camera camera;
    public float sens = 0.05f;

    private InputAction Look;

    private Vector2 look = Vector2.zero;
    void Awake()
    {
        actions = new InputMaster();
        actions.Enable();
        Look = actions.Player.Look;
        Look.Enable();
    }

    private void OnEnable()
    {
        Look.Enable();
    }

    private void OnDisable()
    {
        Look.Disable();
    }

    void Update()
    {
        look = Look.ReadValue<Vector2>() * sens;
        transform.Translate(look);

        if(transform.position.y > camera.orthographicSize)
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x, camera.orthographicSize, transform.position.z), transform.rotation);
        }
        if(transform.position.y < -camera.orthographicSize)
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x, -camera.orthographicSize, transform.position.z), transform.rotation);
        }
        if(transform.position.x > camera.orthographicSize * camera.aspect)
        {
            transform.SetPositionAndRotation(new Vector3(camera.orthographicSize * camera.aspect, transform.position.y, transform.position.z), transform.rotation);
        }
        if (transform.position.x < -camera.orthographicSize * camera.aspect)
        {
            transform.SetPositionAndRotation(new Vector3(-camera.orthographicSize * camera.aspect, transform.position.y, transform.position.z), transform.rotation);
        }
    }
}
