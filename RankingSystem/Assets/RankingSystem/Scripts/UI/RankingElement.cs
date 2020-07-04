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
        [SerializeField] private GameObject _rank;

        public void Initialize(string name, string score, string rank, Sprite spriteToShow = null)
        {
            Font fontToUse = RankingSystemController.Instance.fontToUse;
            if (fontToUse != null)
            {
                _name.font = fontToUse;
                _score.font = fontToUse;
            }
            _name.text = name;

            _score.text = score;

            if (spriteToShow != null)
            {
                var rankImage = _rank.AddComponent<Image>();
                rankImage.sprite = spriteToShow;
                rankImage.preserveAspect = true;
            }
            else
            {
                var rankText = _rank.AddComponent<Text>();
                rankText.text = rank;
                if (fontToUse != null) rankText.font = fontToUse;
                rankText.fontSize = 14;
                rankText.alignment = TextAnchor.MiddleCenter;
            }
        }

        public void ChangeBackgroundColor(Color c)
        {
            _background.color = c;
        }
    }
}
