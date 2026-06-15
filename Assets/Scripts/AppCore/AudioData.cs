using UnityEngine;

namespace AppCore {
	[System.Serializable]
    public class AudioData {
        [SerializeField] public AudioClip clip;
        [SerializeField] public float volume = 1;

        public void Play() {
            AudioManager.PlayAudio(this);
        }
    }
}