using UnityEngine;
using UnityEngine.UI;

namespace PlayerSelection {
    [CreateAssetMenu(menuName = "Customization Option Data")]
    public class CustomizationOptionData : ScriptableObject {
        [SerializeField] public Color[] colors;
        [SerializeField] public Sprite[] hats;
    }
}