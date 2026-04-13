using PlayerSelection;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.RaceManagement {
    public class CoconutProgressIcon : MonoBehaviour {
        [SerializeField] private Image image;
        private int attachedID;

        private float _startX = 0;
        private float _width = 0;
        
        public void Init(PlayerStartData player) {
            attachedID = player.PlayerID;
            image.color = player.PlayerColor;
            RectTransform parentRect =  transform.parent.GetComponent<RectTransform>();
            _startX = parentRect.rect.xMin;
            _width = parentRect.rect.width;
        }

        private void OnEnable() {
            RaceManager.OnRaceProgressUpdate += UpdateProgress;
        }

        private void OnDisable() {
            RaceManager.OnRaceProgressUpdate -= UpdateProgress;
        }

        private void UpdateProgress(int id, float progress) {
            if (attachedID != id) return;
            // float newX = _startX;
            float newX = _startX + (progress * _width);
            transform.localPosition = new Vector3(newX, 0, 0);
        }
    }
}