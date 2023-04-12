using Game.Tags.Common;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Core.Managers
{
    public class GridScanner : AExtendedMonobehaviour
    {
        [SerializeField, Required]
        private GridDataTag gridData = null;

        [SerializeField, BoxGroup("Grid Size")]
        private Vector2 minGridPosition = new(-100, -100);

        [SerializeField, BoxGroup("Grid Size")]
        private Vector2 maxGridPosition = new(100, 100);

        [SerializeField, MinValue(0.001), BoxGroup("Grid Size")]
        private float gridSize = 1;

        [SerializeField, MinValue(0.001), BoxGroup("Grid Settings")]
        private float raycastOffset = 100;

        [SerializeField, BoxGroup("Grid Settings")]
        private LayerMask raycastMask;

        [SerializeField, BoxGroup("Grid Settings")]
        private bool showGridView = false;


        /// <summary>
        /// Allows devs in the editor to trigger a grid calculation event
        /// </summary>
        [Button("Generate Grid Data")]
        public void GenerateGridData()
        {
            if (gridData == null)
            {
                LogError($"Cannot generate grid data because the provided data container is empty!");
                return;
            }

            List<Vector3> validWorldPositions = new List<Vector3>(100);

            for (float x = minGridPosition.x; x <= maxGridPosition.x; x += gridSize)
            {
                for (float y = minGridPosition.y; y <= maxGridPosition.y; y += gridSize)
                {
                    if (Physics.Raycast(new Vector3(x, raycastOffset, y), Vector3.down, out var hit, float.MaxValue, raycastMask, QueryTriggerInteraction.Ignore))
                    {
                        validWorldPositions.Add(hit.point);
                    }
                }
            }

            LogInformation($"Generated {validWorldPositions.Count} positions.", gameObject);
            gridData.SetWorldPositions(validWorldPositions);
        }

        /// <summary>
        /// Called in editor to draw debug view
        /// </summary>
        public override void DrawGizmos()
        {
            //Invalid state
            if (!showGridView || gridData == null || gridData.WorldPositions == null)
            {
                return;
            }

            float boxSize = gridSize / 3;

            //Draw each valid point
            foreach (var position in gridData.WorldPositions)
            {
                Drawing.Draw.SolidBox(new float3(position.x, position.y, position.z), new float3(boxSize, boxSize, boxSize), Color.blue);
            }
        }
    }
}
