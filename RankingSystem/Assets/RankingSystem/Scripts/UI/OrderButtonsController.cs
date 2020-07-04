using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RankingSystem
{
    public class OrderButtonsController : MonoBehaviour
    {
        public Button ascendingButton;
        public Button descendingButton;

        private void Start()
        {
            RankingSystemController.Instance.OnRankingStyleChange += ToggleButtons;
        }

        private void OnEnable()
        {
            ToggleButtons();
        }

        private void ToggleButtons()
        {
            bool isAscending = RankingSystemController.Instance.systemStyle == RankingSystemStyle.Ascending;
            ascendingButton.interactable = !isAscending;
            descendingButton.interactable = isAscending;
        }
    }

}