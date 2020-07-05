using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RankingSystem
{
    public class SearchPlayerController : MonoBehaviour
    {
        [SerializeField] private InputField _playerInputField;
        [SerializeField] private Button _searchButton;
        [SerializeField] private Text _playerNameText;
        [SerializeField] private Text _playerScoreText;

        private void Start()
        {
            _searchButton.onClick.AddListener(() =>
            {
                NetworkController.Instance.RequestPlayerScore(_playerInputField.text);
            });

            NetworkController.Instance.OnServerPlayerRequest += DisplayPlayer;
            NetworkController.Instance.OnNullServerPlayerRequest += DisplayError;
        }

        private void DisplayPlayer(PlayerScore ps)
        {
            _playerNameText.text = ps.name.Trim('"');
            _playerScoreText.text = ps.score.ToString("F" + RankingSystemController.Instance.floatPrecision.ToString());
        }

        private void DisplayError()
        {
            _playerNameText.text = "Player is not in database";
            _playerScoreText.text = "null";
        }
    }

}