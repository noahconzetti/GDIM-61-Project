using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Gameplay.RaceManagement;
using PlayerSelection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Final {
    public class StandingsManager : MonoBehaviour {
        [SerializeField] private GameObject winScreen;
        [SerializeField] private CustomizationSelectionPreview[] placePreviews;
        [SerializeField] private float showStandingsWaitTime = 1.5f;

        private void OnEnable() {
            FinishLineTrigger.OnStandingsFinalized += HandleStandingsFinalized;
        }
        private void OnDisable() {
            FinishLineTrigger.OnStandingsFinalized -= HandleStandingsFinalized;
        }

        private void Start() {
            winScreen.gameObject.SetActive(false);
        }

        private void HandleStandingsFinalized(List<Coconut> standings) {
            StartCoroutine(ShowStandings(standings));
        }

        private IEnumerator ShowStandings(List<Coconut> standings) {
            yield return new WaitForSeconds(showStandingsWaitTime);
            
            winScreen.gameObject.SetActive(true);

            for (int i = 0; i < placePreviews.Length; i++) {
                placePreviews[i].gameObject.SetActive(true);
                placePreviews[i].SetData(standings[i]);
            }
            
            placePreviews[0].Winner();
        }

        public void Restart() {
            SceneManager.LoadScene(0);
        }
    }
}