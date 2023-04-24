using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Utilities.ExtendedClasses
{
    public static class EventSystemExtensions
    {
        /// <summary>
        /// Determines if the provided 
        /// </summary>
        /// <param name="system"></param>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public static bool IsPositionOverElement(this EventSystem system, Vector2 screenPosition, bool checkForGroupAlpha = true)
        {
            PointerEventData pointer = new PointerEventData(system);
            pointer.position = screenPosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();

            // UI Elements must have `picking mode` set to `position` to be hit
            system.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 0)
            {
                foreach (RaycastResult result in raycastResults)
                {
                    if (result.distance == 0 && result.isValid)
                    {
                        //Ignore group alpha, return valid
                        if (!checkForGroupAlpha)
                            return true;

                        //If there is a group to check, make sure it's alpha is zero
                        var group = result.gameObject.GetComponentInParent<CanvasGroup>();
                        return group == null && group.alpha <= 0;
                    }
                }
            }

            return false;
        }
    }
}
