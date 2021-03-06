﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using UnityEngine.UI;

namespace RankingSystem
{
    public enum RankingSystemStyle
    {
        Ascending,
        Descending
    }

    [RequireComponent(typeof(SocketIOComponent))]
    [RequireComponent(typeof(NetworkController))]
    /// <summary>
    /// The ranking system controller class allows the ranking system handling.
    /// From the editor, you can specify the websocket URL, get informations about the last request made, -
    /// Choose the number of players, the players order and the float precision to display.
    /// You can finally change the ranking system style by choosing the font, the colors as well as the eventual sprites to display for each ranking
    /// </summary>
    public class RankingSystemController : MonoBehaviour
    {
        #region Singleton
        public static RankingSystemController Instance = null;
        #endregion

        #region Public variables
        public string websocketServerURL;
        public string lastEventStatus;

        public Transform targetCanvas;
        public Font fontToUse;

        public List<Image> primaryColorUsers;
        public List<Image> secondaryColorUsers;
        public Color primaryColor;
        public Color secondaryColor;

        public bool requestAllPlayers = true;
        public int wantedNumberOfPlayers;

        [Range(0, 10)]
        public int floatPrecision;

        public RankingSystemStyle systemStyle;

        public GameObject rankElementPrefab;

        public List<Sprite> rankingSprites;
        public bool displayAmountPlayersPerPage;
        public int numberOfPlayersPerPage;
        #endregion

        #region Private variables
        private NetworkController _networkController;
        private SocketIOComponent _socketComponent;
        private Text[] _allTextInTarget;
        private List<PlayerScore> _playerScores;
        #endregion

        #region Actions
        public Action OnRankingStyleChange = delegate { };
        public Action OnFloatPrecisionChange = delegate { };
        public Action OnUpdatedList = delegate { };
        #endregion

        private void Awake()
        {
            Instance = this;

            _networkController = GetComponent<NetworkController>();
            _socketComponent = GetComponent<SocketIOComponent>();

            _socketComponent.url = websocketServerURL;
            _socketComponent.autoConnect = false;

            _playerScores = new List<PlayerScore>();
        }

        private void Start()
        {
            NetworkController.Instance.OnServerPlayersUpdate += UpdatePlayerScoreList;
            ChangeFont();
        }

        /// <summary>
        /// For all text inside the target canvas, change the font to match the ranking system controller
        /// </summary>
        public void ChangeFont()
        {
            // For all gameobjects containing a text component from the specified targeted canvas, change its font
            _allTextInTarget = targetCanvas.GetComponentsInChildren<Text>(true);
            if (fontToUse != null)
            {
                foreach (Text text in _allTextInTarget)
                {
                    text.font = fontToUse;
                }
            }
        }

        /// <summary>
        /// Returns the list of player score in the right order following the Ranking system style
        /// </summary>
        /// <returns>List of PlayerScore</returns>
        public List<PlayerScore> GetPlayerScoreList()
        {
            if (systemStyle == RankingSystemStyle.Descending)
                return _playerScores;

            List<PlayerScore> tempList = new List<PlayerScore>(_playerScores);
            tempList.Reverse();

            return tempList;
        }

        /// <summary>
        /// Update the player score list
        /// </summary>
        /// <param name="ps"></param>
        private void UpdatePlayerScoreList(List<PlayerScore> ps)
        {
            _playerScores = ps;
            OnUpdatedList();
        }

        /// <summary>
        /// Toggle in between ascending or descending list
        /// </summary>
        /// <param name="b"></param>
        public void ToggleAscendingList(bool b)
        {
            systemStyle = b ? RankingSystemStyle.Ascending : RankingSystemStyle.Descending;
            OnRankingStyleChange();
        }
    }
}
