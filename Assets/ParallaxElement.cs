using UnityEngine;
using Viewing;

public class ParallaxElement : MonoBehaviour {
    [Tooltip("0 = Foreground (no parallax). 1 = Sky (moves perfectly with camera).")]
    [SerializeField, Range(0f, 1f)] private float xParallaxStrength = 0.2f;

    private Camera _cam;
    private Vector3 _startObjPos;
    private Vector3 _startCamPos;
    private float _startCamSize;
    private Vector3 _startLocalScale;

    private void Start() {
        _cam = Camera.main;
        if (_cam != null) {
            _startObjPos = transform.position;
            _startCamPos = _cam.transform.position;
            _startCamSize = _cam.orthographicSize;
            _startLocalScale = transform.localScale;
        }
    }

    private void OnEnable() {
        CameraManager.OnCameraMovement += UpdateParallax;
    }
    
    private void OnDisable() {
        CameraManager.OnCameraMovement -= UpdateParallax;
    }

    private void UpdateParallax(Vector3 movement, float size) {
        if (_cam == null) return;

        float zoomRatio = size / _startCamSize;

        transform.localScale = _startLocalScale * Mathf.Lerp(1f, zoomRatio, xParallaxStrength);

        float screenLockedX = _cam.transform.position.x + (_startObjPos.x - _startCamPos.x) * zoomRatio;
        float screenLockedY = _cam.transform.position.y + (_startObjPos.y - _startCamPos.y) * zoomRatio;

        float targetX = Mathf.Lerp(_startObjPos.x, screenLockedX, xParallaxStrength);

        float targetY = screenLockedY;

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}