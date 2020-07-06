using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RankingSystem
{
    public class OrderButtonsController : MonoBehaviour
    {
        [SerializeField] private Button _ascendingButton = null;
        [SerializeField] private Button _descendingButton = null;

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
            _ascendingButton.onClick.RemoveAllListeners();
            _descendingButton.onClick.RemoveAllListeners();
        }

        private void ToggleButtons()
        {
            bool isAscending = RankingSystemController.Instance.systemStyle == RankingSystemStyle.Ascending;
            _ascendingButton.interactable = !isAscending;
            _descendingButton.interactable = isAscending;
        }

        private void AddListeners()
        {
            _ascendingButton.onClick.AddListener(() => {
                RankingSystemController.Instance.ToggleAscendingList(true);
            });

            _descendingButton.onClick.AddListener(() => {
                RankingSystemController.Instance.ToggleAscendingList(false);
            });
        }
    }

}