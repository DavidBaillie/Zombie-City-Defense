using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Utilities.ExtendedClasses
{
    public static class EventSystemExtensions
    {
        /// <summary>
        /// Determines if the provided screen position has a UI element under it
        /// </summary>
        /// <param name="system">Event system to check with</param>
        /// <param name="screenPosition">Position to check</param>
        /// <param name="checkForGroupAlpha">Determines if a parent canvas group alpha value will affect the results</param>
        /// <returns>If the position is over an active UI element</returns>
        public static bool IsPositionOverElement(this EventSystem system, Vector2 screenPosition, bool checkForGroupAlpha = true)
        {
            //Build the UI state data
            PointerEventData pointer = new PointerEventData(system);
            pointer.position = screenPosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();

            //Raycast elements to see what's under position
            system.RaycastAll(pointer, raycastResults);

            //Check state data on everything that came back
            foreach (RaycastResult result in raycastResults)
            {
                //Check for invalid states
                if (!result.isValid || result.displayIndex != 0)
                    continue;

                //Ignore group alpha, return valid
                if (!checkForGroupAlpha)
                    return true;

                //If there is a group to check, make sure it's alpha is zero
                var group = result.gameObject.GetComponentInParent<CanvasGroup>();
                return group == null || group.alpha > 0;
            }

            return false;
        }
    }
}
