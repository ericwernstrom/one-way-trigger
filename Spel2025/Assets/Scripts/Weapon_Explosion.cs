using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Explosion : MonoBehaviour
{
    private float timer = 0.0f;
  
    private float instantiationInterval = 0.02f; // Set this to the desired interval between instantiations
  
    public static float max_scale = 2.5f;

    private float scale_modifier = 0.1f;
  
    private float linger_time = 0.4f;

    [SerializeField] private float growDuration = 0.1f; // time to reach max scale
    [SerializeField] private float initialScale = 0.7f;


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * initialScale;
        scale_modifier = (max_scale - initialScale) / (growDuration / instantiationInterval);
    }


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // Increases the size of the explosion if the size is less than the max size
        if (timer > instantiationInterval)
        {
            if (gameObject.transform.localScale.x >= max_scale)
            {
                Destroy(gameObject, linger_time);
            }
            else
            {
                gameObject.transform.localScale += new Vector3(scale_modifier, scale_modifier, scale_modifier);
            }
            timer = 0.0f;

        }

    }
}
