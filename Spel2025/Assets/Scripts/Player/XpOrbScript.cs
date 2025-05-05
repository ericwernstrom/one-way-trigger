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

    private Transform target;
    private bool isAttracted = false;

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
        if (Physics.SphereCast(transform.position, 0.5f, -Vector3.up, out hit, 0f, groundLayer))
        {
            fallSpeed = 0f;
            return;
        }
        else
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

        Destroy(gameObject);
    }

    public ref int getXPAmount()
    {
        return ref xpAmount;
    }
}

