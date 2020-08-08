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
            var time = (GameManager.Instance.Data.currentLevel.window - GameManager.Instance.Data.timer);
            var score = 1000 + (Mathf.FloorToInt(time) * 100) - (GameManager.Instance.Data.alert * -50);
            timeText.text = time.ToString() + "s";
            alertsText.text = GameManager.Instance.Data.alert.ToString();
            scoreText.text = score.ToString();
        }

        public void NextEvent()
        {

            GameManager.Instance.NextLevel();
            GameManager.Instance.SetMissionSelect();
        }
    }
}