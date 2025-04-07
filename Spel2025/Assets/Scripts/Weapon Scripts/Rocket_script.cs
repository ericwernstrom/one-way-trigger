using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket_script : MonoBehaviour
{
    public float damage = 40f;
    public GameObject explosion_prefab;
    public GameObject explosion_hitbox;
    public GameObject smoke_trail;
    public GameObject aftermath;
    public float explosionScale;

    public float rotation_speed;
    


    private void Start()
    {

    }

    private void Update()
    {
        // Makes the rocket rotate
        gameObject.transform.Rotate(new Vector3(0f, rotation_speed, 0f) * Time.deltaTime, Space.Self); 

    }

    
     void OnTriggerEnter(Collider collision)
     {
        // Spawns a hitbox upon collision
        GameObject hitbox = (GameObject)Instantiate(explosion_hitbox, transform.position, explosion_prefab.transform.rotation);

        // Set the scale of the hitbox
        hitbox.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);

        GameObject explosion = (GameObject)Instantiate(explosion_prefab, transform.position, explosion_prefab.transform.rotation);
        // GameObject aftermath_obj = (GameObject)Instantiate(aftermath, transform.position, aftermath.transform.rotation);
        Destroy(gameObject);

     }
    
    
}
