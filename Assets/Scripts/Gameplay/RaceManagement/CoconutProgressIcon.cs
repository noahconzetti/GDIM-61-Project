using System;
using Gameplay.Abilities;
using Gameplay.Abilities.Abilities;
using PlayerSelection;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.RaceManagement {
    public class CoconutProgressIcon : MonoBehaviour {
        [SerializeField] private Image image;
        [SerializeField] private float enlargeScaleIncrease = 2f;
        [SerializeField] private float rotationSpeed = 0.4f;
        private int attachedID;

        private float _startX = 0;
        private float _width = 0;

        private float _rotation = 0f;
        
        public void Init(PlayerStartData player) {
            attachedID = player.PlayerID;
            image.color = player.PlayerColor;
            RectTransform parentRect =  transform.parent.GetComponent<RectTransform>();
            _startX = parentRect.rect.xMin;
            _width = parentRect.rect.width;
        }

        private void OnEnable() {
            RaceManager.OnRaceProgressUpdate += UpdateProgress;
            Coconut.OnUseAbilityStart += AbilityStarted;
            Coconut.OnUseAbilityEnd += AbilityEnded;
        }

        private void OnDisable() {
            RaceManager.OnRaceProgressUpdate -= UpdateProgress;
            Coconut.OnUseAbilityStart -= AbilityStarted;
            Coconut.OnUseAbilityEnd -= AbilityEnded;
        }

        private void Update() {
            _rotation += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, _rotation);
        }

        private void AbilityStarted(Coconut coconut, AbilityData ability) {
            if (coconut.PlayerID != attachedID) return;
            if (ability.GetType() == typeof(EnlargeAbility)) {
                transform.localScale = new Vector3(enlargeScaleIncrease, enlargeScaleIncrease, 1f);
            }
        }

        private void AbilityEnded(Coconut coconut, AbilityData ability) {
            if (coconut.PlayerID != attachedID) return;
            if (ability.GetType() == typeof(EnlargeAbility)) {
                transform.localScale = new Vector3(1, 1, 1f);
            }
        }

        private void UpdateProgress(int id, float progress) {
            if (attachedID != id) return;
            // float newX = _startX;
            float newX = _startX + (progress * _width);
            transform.localPosition = new Vector3(newX, transform.localPosition.y, 0);
        }
    }
}