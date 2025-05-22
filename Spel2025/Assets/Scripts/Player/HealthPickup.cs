using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HealthPickup : MonoBehaviour
{
    private int HealingAmount = 25;

    [SerializeField]
    private float fallSpeed = 2.0f;
    [SerializeField]
    private LayerMask groundLayer;
    private bool onGround = false;
    [SerializeField]
    private float fallAcceleration = 3.0f;

    // AUDIO
    [SerializeField]
    private AudioClip pickup_sound;
    [SerializeField]
    private AudioMixerGroup mixerGroup;

    void Update()
    {
        Fall();
    }

    public void Collect()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Debug.Log("Found player");
            PlayerHealth health = playerObj.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(HealingAmount);
            }
        }

        // AUDIO
        AudioUtils.PlayClipAtPointToMixer(pickup_sound, transform.position, mixerGroup);
        Destroy(gameObject);
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
        else if (!onGround)
        {
            fallSpeed = Mathf.Lerp(fallSpeed, 40f, fallAcceleration * Time.deltaTime);
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }
}
