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

        private bool dirty = true;

        public override void Repaint()
        {
            if (this.dirty)
            {
                EventSystem.current.SetSelectedGameObject(button);

                if (GameManager.Instance.Data.currentLevel.currentDialogue < GameManager.Instance.Data.currentLevel.dialogue.Count)
                {
                    dialogText.text = GameManager.Instance.Data.currentLevel.dialogue[GameManager.Instance.Data.currentLevel.currentDialogue];
                }
                this.dirty = false;
            }
        }

        public void NextEvent()
        {
            GameManager.Instance.Data.currentLevel.currentDialogue++;
            this.dirty = true;
        }
    }
}