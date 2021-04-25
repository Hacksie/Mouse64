using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace HackedDesign.UI
{
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private RectTransform uiCrosshair = null;

        private Vector2 mousePos;

        void Awake()
        {
            uiCrosshair = GetComponent<RectTransform>();
        }

        void Update()
        {
            PositionCrosshair();
        }

        public void MousePosEvent(InputAction.CallbackContext context)
        {
            mousePos = context.ReadValue<Vector2>();
        }

        private void PositionCrosshair()
        {
            uiCrosshair.anchoredPosition = new Vector2(Mathf.FloorToInt(128f * (mousePos.x / Screen.width)), Mathf.FloorToInt(72f * (mousePos.y / Screen.height)));
        }
    }
}