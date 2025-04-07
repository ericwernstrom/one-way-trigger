using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private float dashTime;
    private float dashCooldownTime;
    private bool isDashing;
    private Vector2 dashDirection;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle input
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (CanDash() && Input.GetKeyDown(KeyCode.K))
            StartDash();

        if (isDashing)
        {
            if (Time.time >= dashTime)
                EndDash();
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = dashDirection * dashSpeed;
        }
        else
        {
            rb.velocity = moveInput * moveSpeed;
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTime = Time.time + dashDuration;
        dashCooldownTime = Time.time + dashDuration + dashCooldown;
        dashDirection = moveInput.normalized;
    }

    void EndDash()
    {
        isDashing = false;
    }
    bool CanDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= dashCooldownTime)
            return true;
        else
            return false;
    }
}
