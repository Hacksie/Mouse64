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

            if (GameManager.Instance.Data.currentLevel.currentDialogueIndex < GameManager.Instance.Data.currentLevel.currentDialogue.text.Count)
            {
                dialogText.text = GameManager.Instance.Data.currentLevel.currentDialogue.text[GameManager.Instance.Data.currentLevel.currentDialogueIndex];
            }
        }

        public void NextEvent()
        {
            GameManager.Instance.Data.currentLevel.currentDialogueIndex++;
            if (GameManager.Instance.Data.currentLevel.currentDialogueIndex >= GameManager.Instance.Data.currentLevel.currentDialogue.text.Count)
            {
                GameManager.Instance.CurrentState.EndDialog();
                return;
            }

            Repaint();
        }
    }
}