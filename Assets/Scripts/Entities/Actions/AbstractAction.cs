using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    public abstract class AbstractAction : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] public UnityAction action;

        protected void Awake()
        {
            action+=Invoke;
            var interactTrigger = GetComponent<InteractTrigger>();
            if(interactTrigger != null)
            {
                interactTrigger.AddEvent(action);
            }
        }

        public abstract void Invoke();
    }
}