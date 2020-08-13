using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HackedDesign.UI
{
    public class DialogPresenter : AbstractPresenter
    {
        [SerializeField] UnityEngine.UI.Text dialogText = null;
        [SerializeField] GameObject button = null;

        public override void Repaint()
        {
            EventSystem.current.SetSelectedGameObject(button);
            
            if (GameManager.Instance.Data.currentLevel.currentDialogue < GameManager.Instance.Data.currentLevel.dialogue.Count)
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