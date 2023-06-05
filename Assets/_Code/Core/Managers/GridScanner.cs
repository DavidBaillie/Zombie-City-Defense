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
            var minVector = new Vector2(-scanArea.x / 2, -scanArea.y / 2);
            var maxVector = new Vector2(scanArea.x / 2, scanArea.y / 2);

            using (Draw.WithDuration(2))
            {
                for (float x = transform.position.x + minVector.x; x <= transform.position.x + maxVector.x; x += gridSize)
                {
                    for (float z = transform.position.z + minVector.y; z <= transform.position.z + maxVector.y; z += gridSize)
                    {
                        if (Physics.Raycast(new Vector3(x, raycastOffset, z), Vector3.down,
                            out var hit, float.MaxValue, raycastMask, QueryTriggerInteraction.Ignore))
                        {
                            validWorldPositions.Add(hit.point);
                            Draw.Ray(new Vector3(x, 0, z), Vector3.up * 2, Color.blue);
                        }
                        else
                        {
                            Draw.Ray(new Vector3(x, 0, z), Vector3.up * 2, Color.red);
                        }
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
            if (!GizmoContext.InSelection(transform))
                return;

            Draw.WireRectangleXZ(transform.position, scanArea, Color.blue);

            //Invalid state
            if (!showGridView || gridData == null || gridData.WorldPositionsArray == null)
            {
                return;
            }

            float boxSize = gridSize / 3f;

            //Draw each valid point
            foreach (var position in gridData.WorldPositionsArray)
            {
                Draw.SolidBox(new Vector3(position.x, position.y, position.z), new Vector3(boxSize, boxSize, boxSize), Color.blue);
            }
        }
    }
}
