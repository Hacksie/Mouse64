using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{
    public class DialogPresenter : AbstractPresenter
    {
        [SerializeField] UnityEngine.UI.Text dialogText = null;

        public override void Repaint()
        {
            if (GameManager.Instance.Data.currentLevel.currentDialogue < GameManager.Instance.Data.currentLevel.dialogue.Length)
            {
                dialogText.text = GameManager.Instance.Data.currentLevel.dialogue[GameManager.Instance.Data.currentLevel.currentDialogue];
            }
        }

        public void NextEvent()
        {
            GameManager.Instance.Data.currentLevel.currentDialogue++;
        }
    }
}