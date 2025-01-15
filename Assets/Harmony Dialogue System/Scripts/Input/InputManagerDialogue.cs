using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HarmonyDialogueSystem
{
    public class InputManagerDialogue : MonoBehaviour
    {
        public static InputManagerDialogue instance;

        public Action onInteract;
        public Action onContinue;

        private bool _interactPressed = false;
        private bool _continuePressed = false;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(instance);
            }

            instance = this;
        }

        public void InteractPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _interactPressed = true;
                onInteract?.Invoke();
            }
            else if (context.canceled)
            {
                _interactPressed = false;
            } 
        }

        public void ContinuePressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _continuePressed = true;
                onContinue?.Invoke();
            }
            else if (context.canceled)
            {
                _continuePressed = false;
            } 
        }

        /// <summary>
        /// If the User is interacting with a trigger
        /// </summary>
        /// <returns></returns>
        public bool isInteracting()
        {
            bool result = _interactPressed;
            _interactPressed = false;
            return result;
        }

        /// <summary>
        /// If the user is continuing on with the dialogue flow
        /// </summary>
        /// <returns></returns>
        public bool isContinuing()
        {
            bool result = _continuePressed;
            _continuePressed= false;
            return result;
        }
    }
}
