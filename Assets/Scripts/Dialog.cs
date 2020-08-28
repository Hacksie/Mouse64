
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "Mouse/Dialog/Dialog")]
    public class Dialog : ScriptableObject
    {
        [SerializeField] public List<string> text;
        [SerializeField] public bool allowRepeats = false;
    }
}