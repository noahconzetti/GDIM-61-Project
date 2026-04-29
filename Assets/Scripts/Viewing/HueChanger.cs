using System;
using UnityEngine;

namespace Viewing {
    public class HueChanger : MonoBehaviour {
        [SerializeField] private float hueChangeSpeed = 1f;
        private float _hue = 0;

        private SpriteRenderer _sprite;

        private void Awake() {
            TryGetComponent(out _sprite);
        }

        private void Update() {
            _hue += hueChangeSpeed * Time.deltaTime;
            if (_hue > 1) {
                _hue -= 1;
            }
            
            _sprite.color = Color.HSVToRGB(_hue, 1f, 1f);
        }
    }
}
