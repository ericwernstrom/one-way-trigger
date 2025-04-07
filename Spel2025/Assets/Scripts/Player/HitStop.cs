using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    // bool to check if the function is already active
    bool waiting = false;

    // function to freeze the time
    public void Freeze(float duration)
    {
        if(waiting)
        {
            return;
        }
        Time.timeScale = 0;
        StartCoroutine(UnFreeze(duration));
    }
    
    // unfreezes the time after the duration
    IEnumerator UnFreeze(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        waiting = false;
    }
}
