using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HackedDesign.UI
{
    public class MissionPresenter : AbstractPresenter
    {
        [SerializeField] UnityEngine.UI.Text corpText = null;
        [SerializeField] UnityEngine.UI.Text targetText = null;
        [SerializeField] UnityEngine.UI.Text difficultyText = null;
        [SerializeField] UnityEngine.UI.Text windowText = null;
        [SerializeField] GameObject defaultButton = null;

        public override void Repaint()
        {
            corpText.text = GameManager.Instance.Data.currentLevel.corp;
            targetText.text = GameManager.Instance.Data.currentLevel.target;
            difficultyText.text = GameManager.Instance.Data.currentLevel.difficulty;
            windowText.text = GameManager.Instance.Data.currentLevel.window.ToString() + "s";
            
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }

        public void StartEvent()
        {
            GameManager.Instance.SetPlaying();
        }
    }
}