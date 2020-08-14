using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{
    public class MissionCompletePresenter : AbstractPresenter
    {
        [SerializeField] UnityEngine.UI.Text timeText = null;
        [SerializeField] UnityEngine.UI.Text alertsText = null;
        [SerializeField] UnityEngine.UI.Text scoreText = null;

        public override void Repaint()
        {
            timeText.text = (GameManager.Instance.Data.currentLevel.window - GameManager.Instance.Data.timer).ToString("F0") + "s";
            alertsText.text = GameManager.Instance.Data.alert.ToString();
            scoreText.text = GameManager.Instance.Data.currentLevel.score.ToString();
        }

        public void NextEvent()
        {
            GameManager.Instance.NextLevel();
            GameManager.Instance.SetMissionSelect();
        }
    }
}