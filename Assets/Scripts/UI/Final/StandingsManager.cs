using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Gameplay.RaceManagement;
using TMPro;
using UnityEngine;

namespace Final {
    public class StandingsManager : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI wintext;
        [SerializeField] private float showStandingsWaitTime = 1.5f;

        private void OnEnable() {
            FinishLineTrigger.OnStandingsFinalized += HandleStandingsFinalized;
        }
        private void OnDisable() {
            FinishLineTrigger.OnStandingsFinalized -= HandleStandingsFinalized;
        }

        private void Start() {
            wintext.gameObject.SetActive(false);
        }

        private void HandleStandingsFinalized(List<Coconut> standings) {
            StartCoroutine(ShowStandings(standings));
        }

        private IEnumerator ShowStandings(List<Coconut> standings) {
            yield return new WaitForSeconds(showStandingsWaitTime);

            wintext.gameObject.SetActive(true);
            wintext.text = "Player " + standings[0].PlayerID + "won yay";
        }
    }
}