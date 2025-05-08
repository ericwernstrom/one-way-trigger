using UnityEngine;
using UnityEngine.InputSystem.HID;

public class XPOrb : MonoBehaviour
{
    [SerializeField]
    private int xpAmount = 10;

    [SerializeField]
    private float moveSpeed = 2.0f;
    [SerializeField]
    private float fallSpeed = 2.0f;
    [SerializeField]
    private float maxSpeed = 10.0f;
    [SerializeField]
    private float acceleration = 3.0f;
    [SerializeField]
    private float fallAcceleration = 3.0f;
    [SerializeField]
    private LayerMask groundLayer;
    private bool onGround = false;

    private Transform target;
    private bool isAttracted = false;

    // AUDIO
    public AudioClip pickup_sound;

    public void StartAttraction(Transform playerTransform)
    {
        target = playerTransform;
        isAttracted = true;
    }

    void Update()
    {
        if (isAttracted && target != null) 
        {
            moveSpeed = Mathf.Lerp(moveSpeed, maxSpeed, acceleration * Time.deltaTime);

            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            Fall();
        }
    }

    private void Fall()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.5f, Vector3.down, out hit, 1f, groundLayer))
        {
            onGround = true;
            fallSpeed = 0f;
            // Optionally snap to ground
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.5f, transform.position.z);
            return;
        }
        else if(!onGround)
        {
            fallSpeed = Mathf.Lerp(fallSpeed, 40f, fallAcceleration * Time.deltaTime);
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    public void Collect()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            PlayerStats stats = playerObj.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.AddXP(xpAmount);
            }
        }
        // AUDIO
        AudioSource.PlayClipAtPoint(pickup_sound, transform.position);
        Destroy(gameObject);
    }

    public ref int getXPAmount()
    {
        return ref xpAmount;
    }
}

