using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Rocket_script : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion_prefab;
    [SerializeField]
    private GameObject explosion_hitbox;
    [SerializeField]
    private GameObject smoke_trail;
    [SerializeField]
    private GameObject aftermath;
    [SerializeField]
    public static float explosionScale = 5f;
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

    private void OnCollisionEnter(Collision collision)
    {
        // Spawns a hitbox upon collision
        GameObject hitbox = (GameObject)Instantiate(explosion_hitbox, transform.position, explosion_prefab.transform.rotation);

        // Set the scale of the hitbox
        hitbox.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);

        //Set custom damage values for the hitbox
        Explosion_hitbox_script hitbox_script = hitbox.GetComponent<Explosion_hitbox_script>();

        GameObject explosion = (GameObject)Instantiate(explosion_prefab, transform.position, explosion_prefab.transform.rotation);
        explosion.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);

        AudioUtils.PlayClipAtPointToMixer(audioClip, transform.position, mixerGroup);
        Destroy(gameObject);

    }
    
    
}
