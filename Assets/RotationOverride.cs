using UnityEngine;

public class RotationOverride : MonoBehaviour
{
    void LateUpdate() {
        transform.rotation = Quaternion.identity; 
    }

}
