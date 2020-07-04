﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RankingSystem
{
    public class RankingElementContainer : MonoBehaviour
    {
        [SerializeField] private GameObject _rankingElementPrefab;
        [SerializeField] private List<PlayerScore> _playerScore;

        private List<GameObject> _scoreContainerGameObjects;

        private void Start()
        {
            RankingSystemController.Instance.OnRankingStyleChange += UpdateList;
        }

        private void OnEnable()
        {
            UpdateList();
        }

        public void UpdateList()
        {
            DestroyElements();
            
            _playerScore = RankingSystemController.Instance.GetPlayerScoreList();
            List<Sprite> sprites = RankingSystemController.Instance.rankingSprites;
            _scoreContainerGameObjects = new List<GameObject>();

            for (int i = 0; i < _playerScore.Count; i++)
            {
                var playerScore = _playerScore[i];
                var newRankingElement = Instantiate(_rankingElementPrefab);

                RankingElement re = newRankingElement.GetComponent<RankingElement>();
                
                Sprite rankingSprite = null;
                if (i < sprites.Count)
                {
                    rankingSprite = sprites[i];
                }

                re.Initialize(playerScore.name, playerScore.score.ToString(GetPrecision()), (i + 1).ToString(), rankingSprite);

                Color backgroundColor = i % 2 == 0 ? RankingSystemController.Instance.primaryColor : RankingSystemController.Instance.secondaryColor;
                re.ChangeBackgroundColor(backgroundColor);

                newRankingElement.transform.SetParent(transform);
                newRankingElement.transform.localScale = Vector3.one;

                Vector2 size = GetComponent<RectTransform>().sizeDelta;
                GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, _playerScore.Count * _rankingElementPrefab.GetComponent<RectTransform>().sizeDelta.y);

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
            for (int i = 0; i < _playerScore.Count; i++)
            {
                if (i % 2 == 0)
                    RankingSystemController.Instance.primaryColorUsers.Remove(_scoreContainerGameObjects[i].GetComponent<Image>());
                else
                    RankingSystemController.Instance.secondaryColorUsers.Remove(_scoreContainerGameObjects[i].GetComponent<Image>());
                Destroy(_scoreContainerGameObjects[i]);
            }
            // CleanArray();
        }

        private void DestroyElements()
        {
            for (int i = 0; i < _playerScore.Count; i++)
            {
                // if (i % 2 == 0)
                //     RankingSystemController.Instance.primaryColorUsers.Remove(_scoreContainerGameObjects[i].GetComponent<Image>());
                // else
                //     RankingSystemController.Instance.secondaryColorUsers.Remove(_scoreContainerGameObjects[i].GetComponent<Image>());
                Destroy(_scoreContainerGameObjects[i]);
            }
        }

        // private void CleanArray()
        // {
        //     for (int i = 0; i < RankingSystemController.Instance.primaryColorUsers.Count; i++)
        //     {
        //         if (RankingSystemController.Instance.primaryColorUsers[i] == null)
        //             RankingSystemController.Instance.primaryColorUsers.RemoveAt(i);
        //     }
        // }

        private string GetPrecision()
        {
            return "F" + RankingSystemController.Instance.floatPrecision.ToString();
        }
    }
}
