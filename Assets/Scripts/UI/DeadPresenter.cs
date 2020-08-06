using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{
    public class DeadPresenter : AbstractPresenter
    {
        public override void Repaint()
        {
            
        }

        public void QuitEvent()
        {
            GameManager.Instance.SetMainMenu();
        }
    }
}