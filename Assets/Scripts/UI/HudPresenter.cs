using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign.UI
{

    public class HudPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Text bulletsText = null;
        [SerializeField] private UnityEngine.UI.Slider healthSlider = null;
        [SerializeField] private UnityEngine.UI.Slider energySlider = null;
        [SerializeField] private UnityEngine.UI.Text timerText = null;
        [SerializeField] private UnityEngine.UI.Text alertText = null;

        public override void Repaint()
        {
            bulletsText.text = GameManager.Instance.Data.bullets.ToString();
            alertText.text = GameManager.Instance.Data.alert.ToString("D0");
            healthSlider.value = GameManager.Instance.Data.health;
            energySlider.value = GameManager.Instance.Data.energy;
            
            if (GameManager.Instance.Data.timer > 0)
            {
                timerText.text = GameManager.Instance.Data.timer.ToString("F0");
            }
            else
            {
                timerText.text = "<color=#FF0000> ! </color>";
            }

        }
    }
}