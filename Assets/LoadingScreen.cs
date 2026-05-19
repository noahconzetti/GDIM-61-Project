using System;
using System.Collections;
using PlayerSelection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingScreen : MonoBehaviour {
    [SerializeField] private bool isLoadingScreen = true;
    [SerializeField] private Image faceImage;
    [SerializeField] private Image hatImage;
    [SerializeField] private Image coconutImage;
    [SerializeField] private Animator coconutAnimator;
    [SerializeField] private Sprite[] coconutSpriteOptions;
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private StartSequenceManager startSequence;

    private AsyncOperation _operation;

    private void Start() {
        fadeAnimator.SetTrigger("From Black");
        if (!isLoadingScreen) return;
        
        
        int randomIndex = Random.Range(0, CustomizationPersistantData.Instance.Players.Count);
        PlayerStartData data = CustomizationPersistantData.Instance.Players[randomIndex];
        int randomSprite = Random.Range(0, coconutSpriteOptions.Length);
        coconutAnimator.SetTrigger("Win");
        coconutImage.color = data.PlayerColor;
        hatImage.sprite = data.PlayerHat;
        faceImage.sprite = coconutSpriteOptions[randomSprite];
        Debug.Log("Player: " + randomIndex + " Face: " + randomSprite + " Face name: " + coconutSpriteOptions[randomSprite].name);
    }

    private IEnumerator LoadGameScene() {
        yield return null;
        _operation = SceneManager.LoadSceneAsync(2);
        _operation.allowSceneActivation = false;
        while (_operation.progress < .9f) {
            yield return null;
        }
        fadeAnimator.SetTrigger("To Black");
    }

    private void StartLoadingAnimation() {
        if (!isLoadingScreen) {
            startSequence.StartTrigger();
            return;
        }
        StartCoroutine(LoadGameScene());
    }

    private void DoneLoadingAnimation() {
        if (!isLoadingScreen) {
            return;
        }
        _operation.allowSceneActivation = true;
    }
}
