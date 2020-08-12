using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{
    public class LevelPresenter : AbstractPresenter
    {
        [SerializeField] UnityEngine.UI.Text levelText = null;

        public override void Repaint()
        {
            levelText.text = (GameManager.Instance.Data.currentLevelIndex + 1).ToString("D2");
        }
    }
}