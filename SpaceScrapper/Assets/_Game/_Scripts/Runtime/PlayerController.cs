using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputConfig configA;
    [SerializeField] private InputConfig configB;

    [SerializeField] private bool useBAsBoost = false;
    [SerializeField] private KeyCode boostKey = KeyCode.LeftShift;

    private InputConfig Config
    {
        get
        {
            if (useBAsBoost)
            {
                return isBoosting ? configB : configA;
            }
            return configSwapped ? configB : configA;
        }
    }

    private bool configSwapped = false;

    private Camera cam;
    private Rigidbody2D rb;
    private Vector2 velocity;

    private Vector2 inputDirection;
    private bool isBraking;
    private bool isBoosting;

    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            configSwapped = !configSwapped;

        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.y = Input.GetAxisRaw("Vertical");

        isBoosting = Input.GetKey(boostKey);

        if (isBoosting && useBAsBoost)
            inputDirection = Vector2.up;

        if (inputDirection.y < 0 && Config.Brake == InputConfig.BrakeKey.S)
            inputDirection.y = 0;

        inputDirection = Config.WASDMultipliers.ApplyTo(inputDirection);

        if (Config.LocalSpaceDirections)
        {
            // I have no idea why right and up have to be flipped
            // And why x has to be flipped?
            inputDirection = transform.right * inputDirection.y + transform.up * -inputDirection.x;
        }


        inputDirection = Vector2.ClampMagnitude(inputDirection, 1);

        isBraking = Config.Brake switch
        {
            InputConfig.BrakeKey.S => Input.GetKey(KeyCode.S),
            InputConfig.BrakeKey.Space => Input.GetKey(KeyCode.Space),
            _ => false,
        };
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;
        velocity += inputDirection * Config.Acceleration;

        if (isBraking)
        {
            velocity *= 1f - Config.BrakeDrag;
        }
        else if (Config.ApplyConstantDragAlways)
        {
            velocity *= 1f - Config.ConstantDrag;
        }
        else if (!Config.ApplyDragIndpendently && inputDirection.magnitude < 0.1)
        {
            velocity *= 1f - Config.ConstantDrag;
        }
        else if (Config.ApplyDragIndpendently)
        {
            if (Mathf.Abs(inputDirection.x) < 0.1f)
                velocity.x *= 1f - Config.ConstantDrag;
            if (Mathf.Abs(inputDirection.y) < 0.1f)
                velocity.y *= 1f - Config.ConstantDrag;
        }

        velocity = Vector2.ClampMagnitude(velocity, Config.MaxSpeed);
        rb.velocity = velocity;

        LookAtMouse();
    }

    private void LookAtMouse()
    {
        var dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, transform.forward);
    }
}
