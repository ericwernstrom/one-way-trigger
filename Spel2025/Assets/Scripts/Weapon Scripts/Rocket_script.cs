using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Rocket_script : MonoBehaviour
{
    [SerializeField]
    private float damage = 40f;
    
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

    
    private void OnCollisionEnter(Collision collision)
    {
        // Spawns a hitbox upon collision
        GameObject hitbox = (GameObject)Instantiate(explosion_hitbox, transform.position, explosion_prefab.transform.rotation);

        // Set the scale of the hitbox
        hitbox.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);

        GameObject explosion = (GameObject)Instantiate(explosion_prefab, transform.position, explosion_prefab.transform.rotation);
        // GameObject aftermath_obj = (GameObject)Instantiate(aftermath, transform.position, aftermath.transform.rotation);

        AudioUtils.PlayClipAtPointToMixer(audioClip, transform.position, mixerGroup);
        Destroy(gameObject);

    }
    
    
}
