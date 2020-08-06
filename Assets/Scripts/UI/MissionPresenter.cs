using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{
    public class MissionPresenter : AbstractPresenter
    {
        [SerializeField] UnityEngine.UI.Text corpText;
        [SerializeField] UnityEngine.UI.Text targetText;
        [SerializeField] UnityEngine.UI.Text difficultyText;

        public override void Repaint()
        {
            corpText.text = GameManager.Instance.Data.currentLevel.corp;
            targetText.text = GameManager.Instance.Data.currentLevel.target;
            difficultyText.text = GameManager.Instance.Data.currentLevel.difficulty;
            //dialogText.text = GameManager.Instance.Data.currentLevel.dialogue[GameManager.Instance.Data.currentLevel.currentDialogue];
        }

        public void StartEvent()
        {
            GameManager.Instance.SetPlaying();
            /*
            if (GameManager.Instance.Data.currentLevel.currentDialogue < GameManager.Instance.Data.currentLevel.dialogue.Length)
            {
                GameManager.Instance.Data.currentLevel.currentDialogue++;
            } else {
                // Next playing level
            }*/

        }
    }
}