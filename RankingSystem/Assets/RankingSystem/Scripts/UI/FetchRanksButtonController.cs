using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RankingSystem
{
    public class FetchRanksButtonController : MonoBehaviour
    {
        private Button _fetchRankButton;

        private void Start()
        {
            _fetchRankButton = GetComponent<Button>();
            _fetchRankButton.onClick.AddListener(() =>
            {
                NetworkController.Instance.RequestScores();
            });
        }
    }
}
