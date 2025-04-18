using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunBomb : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 1.6f; // Time delay before explosion
    public GameObject stun_hitbox;
    [SerializeField] private float explosionScale = 1.0f; //Scale of the explosion area


    private bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        // Start countdown for explosion
        Invoke("Explode", explosionDelay);
    }

    void Explode()
    {
        if (!hasExploded)
        {
            hasExploded = true;

            GameObject hitbox = (GameObject)Instantiate(stun_hitbox, transform.position, stun_hitbox.transform.rotation);

            // Set the scale of the hitbox
            hitbox.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);

            // Destroy the grenade object
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
