using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Rocket_script : MonoBehaviour
{
    [SerializeField]
    private static float minDamage = 40f;
    [SerializeField]
    private static float maxDamage = 60f;
    [SerializeField]
    private static float minKnockback = 10f;
    [SerializeField]
    private static float maxKnockback = 20f;
    [SerializeField]
    private static int maxLevel = 5;
    [SerializeField]
    private static int currentLevel = 0;

    [SerializeField]
    private GameObject explosion_prefab;
    [SerializeField]
    private GameObject explosion_hitbox;
    [SerializeField]
    private GameObject smoke_trail;
    [SerializeField]
    private GameObject aftermath;
    [SerializeField]
    private float explosionScale;
    [SerializeField]
    private float rotation_speed;

    // AUDIO
    [SerializeField]
    private AudioClip audioClip;
    [SerializeField]
    private AudioMixerGroup mixerGroup;

    private void Update()
    {
        // Makes the rocket rotate
        gameObject.transform.Rotate(new Vector3(0f, rotation_speed, 0f) * Time.deltaTime, Space.Self); 

    }

    public static void RocketLevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            minDamage += 100f;
            maxDamage += 100f;
            minKnockback += 5f;
            maxKnockback += 5f;
        }
    }

    public static void ResetUpgrade() 
    { 
        currentLevel = 0;
        minDamage = 40f;
        maxDamage = 60f;
        minKnockback = 10f;
        maxKnockback = 20f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Spawns a hitbox upon collision
        GameObject hitbox = (GameObject)Instantiate(explosion_hitbox, transform.position, explosion_prefab.transform.rotation);

        // Set the scale of the hitbox
        hitbox.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);

        //Set custom damage values for the hitbox
        Explosion_hitbox_script hitbox_script = hitbox.GetComponent<Explosion_hitbox_script>();
        if (hitbox_script != null) {
            hitbox_script.Setup(minDamage, maxDamage, minKnockback, maxKnockback);
        }

        GameObject explosion = (GameObject)Instantiate(explosion_prefab, transform.position, explosion_prefab.transform.rotation);
        // GameObject aftermath_obj = (GameObject)Instantiate(aftermath, transform.position, aftermath.transform.rotation);

        AudioUtils.PlayClipAtPointToMixer(audioClip, transform.position, mixerGroup);
        Destroy(gameObject);

    }
    
    
}
