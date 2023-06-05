using Assets.Tags.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Tags.Processors
{
    /// <summary>
    /// Processor class used to display visuals about the gameplay grid to the player via scene objects
    /// </summary>
    [CreateAssetMenu(menuName = ProcessorAssetBaseName + "Grid Visuals", fileName = "Grid Visuals Processor")]
    public class GridVisualsProcessorTag : AProcessorTag
    {
        [SerializeField, AssetsOnly, Required]
        private GameObject selectedPositionVisual = null;

        private GameObject positionVisualInstance = null;

        /// <summary>
        /// Initializes the tag for use
        /// </summary>
        public override void InitializeTag()
        {
            base.InitializeTag();

            positionVisualInstance = Instantiate(selectedPositionVisual);
            positionVisualInstance.SetActive(false);  
        }

        /// <summary>
        /// Cleans up the tag when it is no longer needed
        /// </summary>
        public override void CleanupTag()
        {
            base.CleanupTag();

            if (positionVisualInstance != null )
            {
                Destroy(positionVisualInstance);
                positionVisualInstance = null;
            }
        }


        /// <summary>
        /// Hides the scene visual from view
        /// </summary>
        public void HideVisual()
        {
            positionVisualInstance.SetActive(false);
        }

        /// <summary>
        /// Shows and sets the position of the scene visual
        /// </summary>
        /// <param name="position">Where to place the visual</param>
        public void ShowVisual(Vector3 position)
        {
            positionVisualInstance.transform.position = position;
            positionVisualInstance.SetActive(true);
        }
    }
}
