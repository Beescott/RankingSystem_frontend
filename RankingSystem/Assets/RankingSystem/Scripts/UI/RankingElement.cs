using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RankingSystem
{
    public class RankingElement : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Text _name;
        [SerializeField] private Text _score;
        [SerializeField] private Button _trash;
        [SerializeField] private GameObject _rank;

        /// <summary>
        /// Create a gameobject containing all players information
        /// </summary>
        /// <param name="name"></param>
        /// <param name="score"></param>
        /// <param name="rank"></param>
        /// <param name="spriteToShow"></param>
        public void Initialize(string name, string score, string rank, Sprite spriteToShow = null)
        {
            // Use the right font for texts
            Font fontToUse = RankingSystemController.Instance.fontToUse;
            if (fontToUse != null)
            {
                _name.font = fontToUse;
                _score.font = fontToUse;
            }

            // Assign name and score
            _name.text = name;
            _score.text = score;

            // If there is a sprite to show then add an image component
            if (spriteToShow != null)
            {
                var rankImage = _rank.AddComponent<Image>();
                rankImage.sprite = spriteToShow;
                rankImage.preserveAspect = true;
            }
            // Otherwise, display the rank
            else
            {
                var rankText = _rank.AddComponent<Text>();
                rankText.text = rank;
                if (fontToUse != null) rankText.font = fontToUse;
                rankText.fontSize = 14;
                rankText.alignment = TextAnchor.MiddleCenter;
            }

            // Add a listener to the remove button
            _trash.onClick.AddListener(() => {
                NetworkController.Instance.RemovePlayer(_name.text);
            });
        }

        // Change the background color to the one specified by the ranking system controller
        public void ChangeBackgroundColor(Color c)
        {
            _background.color = c;
        }

        // Toggle active or not the trash button
        public void ToggleTrashIcon()
        {
            _trash.gameObject.SetActive(!_trash.gameObject.activeSelf);
        }
    }
}
