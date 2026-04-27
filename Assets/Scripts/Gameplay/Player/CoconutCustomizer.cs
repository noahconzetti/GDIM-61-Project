using PlayerSelection;
using UnityEngine;

namespace Gameplay {
    public class CoconutCustomizer : MonoBehaviour{
        [SerializeField] public SpriteRenderer coconutBaseSprite;
        [SerializeField] public SpriteRenderer hatSprite;

        public PlayerStartData data;

        public void SetData(PlayerStartData playerStartData) {
            coconutBaseSprite.color = playerStartData.PlayerColor;
            hatSprite.sprite = playerStartData.PlayerHat;

            data = playerStartData;
        }
    }
}