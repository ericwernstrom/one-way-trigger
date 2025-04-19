using UnityEngine;

public class XPDropper : MonoBehaviour
{
    [SerializeField]
    private GameObject XPOrbPrefab;
    [SerializeField]
    private int xpAmount = 10;
    [SerializeField]
    private int numberOfOrbs = 1;

    public void DropXP()
    {
        if (!Application.isPlaying || XPOrbPrefab == null) return;

        for (int i = 0; i < numberOfOrbs; i++)
        {
            GameObject orb = Instantiate(XPOrbPrefab, transform.position, Quaternion.identity);

            Rigidbody rb = orb.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 scatter = Random.insideUnitSphere * 1.2f;
                rb.AddForce(scatter, ForceMode.Impulse);
            }

            XPOrb xp = orb.GetComponent<XPOrb>();
            if (xp != null)
            {
                
                xp.getXPAmount() = xpAmount / numberOfOrbs;
            }
        }
    }
}

