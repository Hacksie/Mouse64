using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    public class DialogAction: AbstractAction
    {
        [Header("Settings")]
        [SerializeField] private Dialog dialog = null;
        
        public override void Invoke()
        {
            GameManager.Instance.Data.currentLevel.currentDialogue = dialog;
            GameManager.Instance.Data.currentLevel.currentDialogueIndex = 0;
            GameManager.Instance.CurrentState.ShowDialog();
        }
    }
}