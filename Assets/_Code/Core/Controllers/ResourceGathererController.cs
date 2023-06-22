using Assets.Core.Managers.Static;
using Assets.Core.StaticChannels;
using Assets.Tags.Settings;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Core.Controllers
{
    public class ResourceGathererController : AExtendedMonobehaviour
    {
        [SerializeField, Required]
        private EconomySettingsTag economySettings = null;

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
        /// Called when component enabled
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            SurvivalGameplayChannel.OnGameModeObjectiveFailed += OnObjectiveFailed;
        }

        /// <summary>
        /// Called when component disabled
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            SurvivalGameplayChannel.OnGameModeObjectiveFailed -= OnObjectiveFailed;
        }

        /// <summary>
        /// Called when the game mode fails
        /// </summary>
        private void OnObjectiveFailed()
        {
            this.enabled = false;
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
                        economySettings.GatherResourceRange.x, economySettings.GatherResourceRange.y + 1));
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
