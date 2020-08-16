using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HackedDesign.UI
{
    public class DeadPresenter : AbstractPresenter
    {
        [SerializeField] private GameObject defaultButton = null;
        public override void Repaint()
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }

        public void ResetEvent()
        {
            GameManager.Instance.SetPlaying();
        }

        public void QuitEvent()
        {
            GameManager.Instance.SetMainMenu();
        }
    }
}