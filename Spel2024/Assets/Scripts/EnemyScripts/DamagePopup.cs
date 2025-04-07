using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    GameObject player;
    private Vector3 dif;
    private Quaternion rotation;
    private Vector3 speed = new Vector3(0, 1.3f, 0);
    private Color32 alpha_change = new Color32(0, 0, 0, 4);
    private float starttime;
    private TMP_Text m_TextComponent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        starttime = Time.time;
        m_TextComponent = GetComponent<TMP_Text>();
        m_TextComponent.color = new Color32(255, 0, 0, 255);

    }

    // Update is called once per frame
    void Update()
    {
        // make it so that the text faces the camera 
        dif = transform.position - player.transform.position;
        rotation = Quaternion.LookRotation(dif);
        transform.rotation = rotation;

        // make the text float upwards
        transform.position += speed * Time.deltaTime;
        // make the text fade out and then destroy
        if((Time.time - starttime) > 1f)
            m_TextComponent.color -= alpha_change;
        if ((Time.time - starttime) > 2f)
            Destroy(gameObject);
    }
}
