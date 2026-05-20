using UnityEngine;

public class BoostParticles : MonoBehaviour
{
    public void DestroyParticles() {
        Destroy(transform.parent.gameObject);
    }
}
