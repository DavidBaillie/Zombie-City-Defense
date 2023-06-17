using Assets.Core.Abstract;
using Assets.Core.Interfaces;
using Assets.Core.StaticChannels;
using Assets.Tags.Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Core.Controllers
{
    /// <summary>
    /// Controller class attached to the player vehicle that creates a fail event when killed
    /// </summary>
    [SelectionBase]
    public class StaticVehicleEntityController : AEntityController, IDamageReceiver
    {
        [SerializeField, Required]
        private ObjectiveUnitTag objectiveTag = null;


        private float currentHealth = 0;

        /// <summary>
        /// Called when object created
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            currentHealth = objectiveTag.MaxHealth;
        }


        /// <summary>
        /// Called when the entity takes damage
        /// </summary>
        /// <param name="damage">Damage to apply</param>
        public bool ApplyDamage(float damage)
        {
            currentHealth = Mathf.Max(0, currentHealth - damage);
            SurvivalGameplayChannel.RaiseOnUnitHealthChanged(objectiveTag, currentHealth, objectiveTag.MaxHealth);

            if (currentHealth <= 0)
            {
                SurvivalGameplayChannel.RaiseOnStaticUnitDeath(this, objectiveTag);
                return true;
            }

            return false;
        }
    }
}
