using Assets.Core.Managers.Static;
using Assets.Core.StaticChannels;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Core.Controllers
{
    public class ResourceGathererController : AExtendedMonobehaviour
    {
        [SerializeField, Required]
        private Transform depotPosition;
        [SerializeField, Required]
        private Transform resourcePosition;

        [SerializeField, MinValue(0.1f)]
        private float movementSpeed;

        private Vector3 targetPosition;


        /// <summary>
        /// Called when object created
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            transform.position = depotPosition.position;
            targetPosition = resourcePosition.position;  
        }

        /// <summary>
        /// Called each frame
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if (transform.position == targetPosition)
            {
                if (targetPosition == depotPosition.position)
                {
                    targetPosition = resourcePosition.position;
                    SurvivalGameplayChannel.RaiseOnResourceGathered(Random.Range(
                        GameSettings.EconomySettings.GatherResourceRange.x,
                        GameSettings.EconomySettings.GatherResourceRange.y + 1));
                }
                else
                {
                    targetPosition = depotPosition.position;
                }
            }
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }
}
