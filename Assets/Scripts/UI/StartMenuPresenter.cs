using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{
    public class StartMenuPresenter : AbstractPresenter
    {
        public override void Repaint()
        {
            

        }

        public void ResumeEvent()
        {
            GameManager.Instance.SetPlaying();
        }

        public void QuitEvent()
        {
            GameManager.Instance.SetMainMenu();
        }
    }
}