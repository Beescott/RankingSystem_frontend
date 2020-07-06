using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RankingSystem
{
    public class RankingElementContainer : MonoBehaviour
    {
        [SerializeField] private GameObject _rankingElementPrefab = null;
        [SerializeField] private List<PlayerScore> _playerScores = null;

        private List<GameObject> _scoreContainerGameObjects;
        private bool _firstEnable = false;
        private int _currentPageIndex = 0;

        private void Start()
        {
            RankingSystemController.Instance.OnRankingStyleChange += UpdateList;
            RankingSystemController.Instance.OnFloatPrecisionChange += UpdateList;
            RankingSystemController.Instance.OnUpdatedList += UpdateList;
        }

        private void OnEnable()
        {
            _scoreContainerGameObjects = new List<GameObject>();
            // Destroy previews element
            if (!_firstEnable)
            {
                RankingElement[] rankingElements = GetComponentsInChildren<RankingElement>();
                for (int i = 0; i < rankingElements.Length; i++)
                {
                    Destroy(rankingElements[i].gameObject);
                }
                _firstEnable = true;
            }

            // Update list every time the object is active
            UpdateList();
        }

        /// <summary>
        /// Destroy all previous elements and replace them with the new fetched list
        /// </summary>
        public void UpdateList()
        {
            DestroyElements();

            _playerScores = RankingSystemController.Instance.GetPlayerScoreList();
            List<Sprite> sprites = RankingSystemController.Instance.rankingSprites;
            _scoreContainerGameObjects = new List<GameObject>();

            int numberOfPlayersToDisplay = RankingSystemController.Instance.displayAmountPlayersPerPage ? RankingSystemController.Instance.numberOfPlayersPerPage : _playerScores.Count;

            for (int i = numberOfPlayersToDisplay * _currentPageIndex; i < Mathf.Min(numberOfPlayersToDisplay * _currentPageIndex + numberOfPlayersToDisplay, _playerScores.Count); i++)
            {
                var playerScore = _playerScores[i];
                var newRankingElement = Instantiate(_rankingElementPrefab);

                RankingElement re = newRankingElement.GetComponent<RankingElement>();

                Sprite rankingSprite = null;
                // Actual rank is the rank of the player, no matter if the list is ascending or descending
                int actualRank = RankingSystemController.Instance.systemStyle == RankingSystemStyle.Ascending ? _playerScores.Count - 1 - i : i;
                if (actualRank < sprites.Count)
                {
                    rankingSprite = sprites[actualRank];
                }

                // Initialize the new ranking element
                re.Initialize(playerScore.name, playerScore.score.ToString(GetPrecision()), (actualRank + 1).ToString(), rankingSprite);

                // Change its background to match the ranking system controller colors
                Color backgroundColor = i % 2 == 0 ? RankingSystemController.Instance.primaryColor : RankingSystemController.Instance.secondaryColor;
                re.ChangeBackgroundColor(backgroundColor);

                // Set new ranking element to this in order to fit the vertical layout group
                newRankingElement.transform.SetParent(transform);
                // Fix its scale (sometime the scale is messed up for some reason?)
                newRankingElement.transform.localScale = Vector3.one;

                _scoreContainerGameObjects.Add(newRankingElement);

                // Give the background image to ranking system so that it can control its color
                if (i % 2 == 0)
                    RankingSystemController.Instance.primaryColorUsers.Add(newRankingElement.GetComponentsInChildren<Image>()[1]);
                else
                    RankingSystemController.Instance.secondaryColorUsers.Add(newRankingElement.GetComponentsInChildren<Image>()[1]);
            }

            // Change the size of the parent to make it match the children counts
            Vector2 size = GetComponent<RectTransform>().sizeDelta;
            GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, numberOfPlayersToDisplay * _rankingElementPrefab.GetComponent<RectTransform>().sizeDelta.y);
        }

        private void OnDisable()
        {
            DestroyElements();
        }

        private void DestroyElements()
        {
            // int numberOfPlayersToDisplay = RankingSystemController.Instance.displayAmountPlayersPerPage ? RankingSystemController.Instance.numberOfPlayersPerPage : _playerScores.Count;
            if (_scoreContainerGameObjects.Count == 0)
                return;

            for (int i = 0; i < _scoreContainerGameObjects.Count; i++)
            {
                Destroy(_scoreContainerGameObjects[i]);
            }
            RankingSystemController.Instance.primaryColorUsers.Clear();
            RankingSystemController.Instance.secondaryColorUsers.Clear();
        }

        private string GetPrecision()
        {
            return "F" + RankingSystemController.Instance.floatPrecision.ToString();
        }

        public void NextPage()
        {
            int numberOfPlayersPerPage = RankingSystemController.Instance.numberOfPlayersPerPage;
            if (!RankingSystemController.Instance.displayAmountPlayersPerPage || _currentPageIndex >= (_playerScores.Count - 1) / numberOfPlayersPerPage)
                return;

            _currentPageIndex++;
            UpdateList();
        }

        public void PreviousPage()
        {
            if (!RankingSystemController.Instance.displayAmountPlayersPerPage || _currentPageIndex == 0)
                return;

            _currentPageIndex--;
            UpdateList();
        }
    }
}
