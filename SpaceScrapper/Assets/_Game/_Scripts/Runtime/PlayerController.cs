using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputConfig configA;
    [SerializeField] private InputConfig configB;

    private InputConfig config => configSwapped ? configB : configA;
    private bool configSwapped = false;

    private Camera cam;
    private Rigidbody2D rb;
    private Vector2 velocity;

    private Vector2 inputDirection;
    private bool isBraking;

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

        if (inputDirection.y < 0 && config.Brake == InputConfig.BrakeKey.S)
            inputDirection.y = 0;

        inputDirection = config.WASDMultipliers.ApplyTo(inputDirection);

        if (config.LocalSpaceDirections)
        {
            // I have no idea why right and up have to be flipped
            // And why x has to be flipped?
            inputDirection = transform.right * inputDirection.y + transform.up * -inputDirection.x;
        }


        inputDirection = Vector2.ClampMagnitude(inputDirection, 1);

        isBraking = config.Brake switch
        {
            InputConfig.BrakeKey.S => Input.GetKey(KeyCode.S),
            InputConfig.BrakeKey.Space => Input.GetKey(KeyCode.Space),
            _ => false,
        };
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;
        velocity += inputDirection * config.Acceleration;

        if (isBraking)
        {
            velocity *= 1f - config.BrakeDrag;
        }
        else if (inputDirection.magnitude < 0.1 || config.ApplyConstantDragAlways)
        {
            velocity *= 1f - config.ConstantDrag;
        }

        velocity = Vector2.ClampMagnitude(velocity, config.MaxSpeed);
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
