using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingRocketAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource launchSource;
    [SerializeField]
    private AudioSource loopSource;

    
    void Start()
    {
        if (launchSource == null || loopSource == null)
        {
            Debug.LogError("AudioSources are not assigned in the inspector.");
            return;
        }
        
        
        // Start the loop sound
        loopSource.PlayDelayed(0.28f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
