using Assets.Core.Abstract;
using Assets.Core.Interfaces;
using Assets.Core.StaticChannels;
using Assets.Tags.Channels;
using Assets.Tags.Models;
using Assets.Utilities.ExtendedClasses;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Core.Controllers
{
    [SelectionBase]
    public class StaticObjectiveEntityController : AEntityController, IDamageReceiver
    {
        [SerializeField, Required]
        private ObjectiveUnitTag objectiveTag = null;

        [SerializeField, ReadOnly]
        private float currentHealth = 0;

        /// <summary>
        /// Called when object created
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            objectiveTag.ThrowIfNull("An objective tag is required on the objective controller!");

            currentHealth = objectiveTag.MaxHealth;
        }


        /// <summary>
        /// Applies damage to the objective
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public bool ApplyDamage(float damage)
        {
            currentHealth = Mathf.Max(0, currentHealth - damage);

            if (currentHealth <= 0)
            {
                SurvivalGameplayChannel.RaiseOnStaticUnitDeath(this, objectiveTag);
                Destroy(gameObject);
            }

            return currentHealth <= 0;
        }
    }
}
