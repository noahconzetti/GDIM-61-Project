using System;
using UnityEngine;
using Viewing;

public class ParallaxElement : MonoBehaviour {
    [SerializeField] private float parallaxStrength = .2f;
    
    private void OnEnable() {
        CameraManager.OnCameraMovement += UpdateParallax;
    }
    
    private void OnDisable() {
        CameraManager.OnCameraMovement -= UpdateParallax;
    }

    private void UpdateParallax(Vector3 movement) {
        transform.position += movement * parallaxStrength;
    }
}
