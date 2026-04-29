using System;
using System.Collections;
using Gameplay.Abilities.Abilities;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Camera {
    public class CameraShaker : MonoBehaviour {
        [SerializeField] private float cutoff = 0.01f;
        [SerializeField] private CameraShakeData defaults;
        [FormerlySerializedAs("baseIntensity")] [SerializeField] private float baseAmplitude = .1f;
        [SerializeField] private float baseFrequency = .1f;
        [SerializeField] private LayerMask groundLayer;
        
        private CinemachineBasicMultiChannelPerlin _cinemachineNoise;

        private void Awake() {
            TryGetComponent(out _cinemachineNoise);
        }

        private void Start() {
            _cinemachineNoise.AmplitudeGain = baseAmplitude;
            _cinemachineNoise.FrequencyGain = baseFrequency;
        }

        private void OnDestroy() {
            _cinemachineNoise.AmplitudeGain = 0;
            _cinemachineNoise.FrequencyGain = 0;
        }

        private void OnEnable() {
            Coconut.OnCollision += CoconutCollision;
        }

        private void OnDisable() {
            Coconut.OnCollision -= CoconutCollision;
        }

        private void CoconutCollision(Coconut coconut, Collision2D collision) {
            if (coconut.IsUsingAbility(typeof(EnlargeAbility))) {
                InternalShake(defaults);
            }

        }

        private void InternalShake(CameraShakeData data) {
            StartCoroutine(ShakeCoroutine(data));
        }
        
        private IEnumerator ShakeCoroutine(CameraShakeData data) {
            _cinemachineNoise.AmplitudeGain += data.intensity;
            _cinemachineNoise.FrequencyGain += data.frequency;
            if (data.decay) {
                float timer = data.time;
                while (timer > 0) {
                    _cinemachineNoise.AmplitudeGain -= data.intensity * Time.deltaTime / data.time;
                    _cinemachineNoise.FrequencyGain -= data.frequency * Time.deltaTime / data.time;
                    
                    timer -= Time.deltaTime;
                    yield return null;
                }
            } else {
                yield return new WaitForSeconds(data.time);
                _cinemachineNoise.AmplitudeGain -= data.intensity;
                _cinemachineNoise.FrequencyGain -= data.frequency;
            }
            
            if (_cinemachineNoise.AmplitudeGain < baseAmplitude + cutoff) _cinemachineNoise.AmplitudeGain = baseAmplitude;
            if (_cinemachineNoise.FrequencyGain < baseFrequency + cutoff) _cinemachineNoise.FrequencyGain = baseFrequency;
        }
    }
}