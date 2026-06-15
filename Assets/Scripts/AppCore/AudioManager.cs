using System;
using UnityEngine;

namespace AppCore {
    public class AudioManager : MonoBehaviour {
        private static AudioManager Instance;

        private AudioSource _src;

        private void Awake() {
            if (Instance) {
                Destroy(gameObject);
            } else {
                transform.parent = null;
                Instance = this;
                DontDestroyOnLoad(gameObject);
                TryGetComponent(out _src);
            }
        }

        public static void PlayAudio(AudioData data) {
            Instance._src.PlayOneShot(data.clip, data.volume);
        }
    }
}