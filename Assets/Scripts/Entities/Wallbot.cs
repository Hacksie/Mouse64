using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Wallbot : Guard
    {
        [SerializeField] private float height = 1;

        protected override void Kill()
        {
            //base.Kill();
            //GameManager.Instance.Data.currentLevel.completed = true;
            //Logger.Log(this, "completed");
        }
    }
}