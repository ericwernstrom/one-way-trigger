using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleTranslator : MonoBehaviour
{
    public float duration = 1;
    public AnimationCurve accelCurve;

    float time = 0f;
    float position = 0f;
    float direction = 1f;

    public new Rigidbody rigidbody;
    public Vector3 start = -Vector3.up;
    public Vector3 end = Vector3.up;

    public bool activate;

    
    public void FixedUpdate()
    {
        time = time + (direction * Time.deltaTime / duration);
        // Makes the platform go back and forth
        position = Mathf.PingPong(time, 1f);

        if (activate)
            PerformTransform(position);
    }

    public void PerformTransform(float position)
    {
        // Acceleration dependent on accelCurve, so it's just not linear
        var curvePosition = accelCurve.Evaluate(position);
        // Calculate
        var pos = transform.TransformPoint(Vector3.Lerp(start, end, curvePosition));
        // Move
        rigidbody.MovePosition(pos);
        
    }
}
