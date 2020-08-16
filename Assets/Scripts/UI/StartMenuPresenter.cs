using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace HackedDesign.UI
{
    public class StartMenuPresenter : AbstractPresenter
    {
        [SerializeField] GameObject defaultButton = null;

        public override void Repaint()
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);

        }

        public void ResumeEvent()
        {
            GameManager.Instance.SetPlaying();
        }

        public void QuitEvent()
        {
            GameManager.Instance.Reset();
            GameManager.Instance.SetMainMenu();
        }
    }
}