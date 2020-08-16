using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HackedDesign.UI
{
    public class MissionCompletePresenter : AbstractPresenter
    {
        [SerializeField] UnityEngine.UI.Text timeText = null;
        [SerializeField] UnityEngine.UI.Text alertsText = null;
        [SerializeField] UnityEngine.UI.Text scoreText = null;
        [SerializeField] GameObject defaultButton = null;

        public override void Repaint()
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
            timeText.text = (GameManager.Instance.Data.currentLevel.window - GameManager.Instance.Data.timer).ToString("F0") + "s";
            alertsText.text = GameManager.Instance.Data.alert.ToString();
            scoreText.text = GameManager.Instance.Data.currentLevel.score.ToString();
        }

        public void NextEvent()
        {
            GameManager.Instance.NextLevel();
        }
    }
}