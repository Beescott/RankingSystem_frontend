using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RankingSystem
{
    public class RankingElementContainer : MonoBehaviour
    {
        [SerializeField] private GameObject _rankingElementPrefab;
        [SerializeField] private List<PlayerScore> _playerScores;

        private List<GameObject> _scoreContainerGameObjects;
        private bool _firstEnable = false;

        private void Start()
        {
            RankingSystemController.Instance.OnRankingStyleChange += UpdateList;
            RankingSystemController.Instance.OnFloatPrecisionChange += UpdateList;
            RankingSystemController.Instance.OnUpdatedList += UpdateList;
        }

        private void OnEnable()
        {
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

            UpdateList();
        }

        public void UpdateList()
        {
            DestroyElements();

            _playerScores = RankingSystemController.Instance.GetPlayerScoreList();
            List<Sprite> sprites = RankingSystemController.Instance.rankingSprites;
            _scoreContainerGameObjects = new List<GameObject>();

            for (int i = 0; i < _playerScores.Count; i++)
            {
                var playerScore = _playerScores[i];
                var newRankingElement = Instantiate(_rankingElementPrefab);

                RankingElement re = newRankingElement.GetComponent<RankingElement>();

                Sprite rankingSprite = null;
                int actualRank = RankingSystemController.Instance.systemStyle == RankingSystemStyle.Ascending ? _playerScores.Count - 1 - i : i;
                if (actualRank < sprites.Count)
                {
                    rankingSprite = sprites[actualRank];
                }

                re.Initialize(playerScore.name, playerScore.score.ToString(GetPrecision()), (actualRank + 1).ToString(), rankingSprite);

                Color backgroundColor = i % 2 == 0 ? RankingSystemController.Instance.primaryColor : RankingSystemController.Instance.secondaryColor;
                re.ChangeBackgroundColor(backgroundColor);

                newRankingElement.transform.SetParent(transform);
                newRankingElement.transform.localScale = Vector3.one;

                Vector2 size = GetComponent<RectTransform>().sizeDelta;
                GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, _playerScores.Count * _rankingElementPrefab.GetComponent<RectTransform>().sizeDelta.y);

                _scoreContainerGameObjects.Add(newRankingElement);

                // Give the background image to ranking system so that it can control its color
                if (i % 2 == 0)
                    RankingSystemController.Instance.primaryColorUsers.Add(newRankingElement.GetComponentsInChildren<Image>()[1]);
                else
                    RankingSystemController.Instance.secondaryColorUsers.Add(newRankingElement.GetComponentsInChildren<Image>()[1]);
            }
        }

        private void OnDisable()
        {
            DestroyElements();
            // CleanArray();
        }

        private void DestroyElements()
        {
            for (int i = 0; i < _playerScores.Count; i++)
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
    }
}
