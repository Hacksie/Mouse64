using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Suit : Guard
    {
        protected override void Kill()
        {
            base.Kill();
            GameManager.Instance.Data.currentLevel.completed = true;
            Logger.Log(this, "completed");
        }
    }
}