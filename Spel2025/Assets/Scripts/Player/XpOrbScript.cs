using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpAmount = 10;

    public float moveSpeed = 2.0f;
    public float maxSpeed = 10.0f;
    public float acceleration = 3.0f;

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
}

