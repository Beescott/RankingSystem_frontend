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
            AddListeners();
        }

        private void OnDisable() {
            ascendingButton.onClick.RemoveAllListeners();
            descendingButton.onClick.RemoveAllListeners();
        }

        private void ToggleButtons()
        {
            bool isAscending = RankingSystemController.Instance.systemStyle == RankingSystemStyle.Ascending;
            ascendingButton.interactable = !isAscending;
            descendingButton.interactable = isAscending;
        }

        private void AddListeners()
        {
            ascendingButton.onClick.AddListener(() => {
                RankingSystemController.Instance.ToggleAscendingList(true);
            });

            descendingButton.onClick.AddListener(() => {
                RankingSystemController.Instance.ToggleAscendingList(false);
            });
        }
    }

}