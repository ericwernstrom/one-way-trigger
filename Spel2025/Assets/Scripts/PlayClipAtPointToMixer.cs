using UnityEngine;
using UnityEngine.Audio;

public class AudioUtils : MonoBehaviour
{
    public static void PlayClipAtPointToMixer(AudioClip clip, Vector3 position, AudioMixerGroup mixerGroup, float volume = 1f)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = position;

        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip;
        aSource.outputAudioMixerGroup = mixerGroup; // assign the mixer group
        aSource.volume = volume;
        aSource.spatialBlend = 1f; // make it 3D
        aSource.Play();

        Object.Destroy(tempGO, clip.length); // destroy after clip is done playing
    }
}
