using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float acceleration = 2f;
    public float dragMult = 0.1f;
    public bool dragEnabled = true;

    Camera cam;
    Rigidbody2D rb;
    Vector2 inputAxes;
    Vector2 curVelocity;

    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        inputAxes.x = Input.GetAxis("Horizontal");
        inputAxes.y = Input.GetAxis("Vertical");
        inputAxes.Normalize();
    }

    private void FixedUpdate()
    {
        curVelocity = rb.velocity;
        float curSqrMag = curVelocity.sqrMagnitude;
        if(inputAxes.sqrMagnitude > 0.1)
        {
            curVelocity += inputAxes * acceleration;
        }
        if (dragEnabled && inputAxes.sqrMagnitude < 0.1)
        {
            curVelocity *= 1f - dragMult;
        }
        curVelocity = Vector2.ClampMagnitude(curVelocity, maxSpeed);
        rb.velocity = curVelocity;
        LookAtMouse();
    }

    private void LookAtMouse()
    {
        var dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, transform.forward);
    }
}
