using System;
using Gameplay;
using TMPro;
using UnityEngine;

public class StartSequenceManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI numberText;

    private Animator _animator;
    private RaceInfo _raceInfo;
    
    private void OnEnable() {
        GameManager.OnGameStart += HandleGameStart;
    }
    
    private void OnDisable() {
        GameManager.OnGameStart -= HandleGameStart;
    }

    private void Awake() {
        TryGetComponent(out _animator);
    }

    private void HandleGameStart(RaceInfo raceInfo) {
        // _animator.SetTrigger("Start");
        _raceInfo = raceInfo;
    }
    
    public void AnimEnd() {
        _raceInfo.Players.ForEach(p => p.ActiveRB(true));
    }

    public void ChangeNumber(int number) {
        numberText.text = number.ToString();
    }
}