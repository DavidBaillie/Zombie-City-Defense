using Drawing;
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
        [SerializeField, Required, InlineEditor]
        private GridDataTag gridData = null;

        [SerializeField, BoxGroup("Grid Size")]
        private Vector2 scanArea = new Vector2(100, 100);

        [SerializeField, MinValue(0.001), BoxGroup("Grid Size")]
        private float gridSize = 1;

        [SerializeField, MinValue(0.001), BoxGroup("Grid Settings")]
        private float raycastOffset = 100;

        [SerializeField, BoxGroup("Grid Settings")]
        private LayerMask raycastMask;

        [SerializeField, BoxGroup("Grid Settings")]
        private bool showGridView = false;

        /// <summary>
        /// Clears data stashed in the grid tag
        /// </summary>
        [ResponsiveButtonGroup]
        public void ClearData()
        {
            if (gridData == null)
            {
                LogError($"Cannot generate grid data because the provided data container is empty!");
                return;
            }

            gridData.ClearData();
        }


        /// <summary>
        /// Allows devs in the editor to trigger a grid calculation event
        /// </summary>
        [ResponsiveButtonGroup]
        public void GenerateGridData()
        {
            if (gridData == null)
            {
                LogError($"Cannot generate grid data because the provided data container is empty!");
                return;
            }

            var validWorldPositions = new List<Vector3>(100);
            var min = new Vector2(-scanArea.x / 2, -scanArea.y / 2);
            var max = new Vector2(scanArea.x / 2, scanArea.y / 2);

            for (float x = min.x; x <= max.x; x += gridSize)
            {
                for (float y = min.y; y <= max.y; y += gridSize)
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
            Drawing.Draw.WireRectangleXZ(Vector3.zero, scanArea, Color.blue);

            //Invalid state
            if (!showGridView || gridData == null || gridData.WorldPositionsArray == null)
            {
                return;
            }

            float boxSize = gridSize / 3;

            //Draw each valid point
            foreach (var position in gridData.WorldPositionsArray)
            {
                Drawing.Draw.SolidBox(new float3(position.x, position.y, position.z), new float3(boxSize, boxSize, boxSize), Color.blue);
            }
        }
    }
}
