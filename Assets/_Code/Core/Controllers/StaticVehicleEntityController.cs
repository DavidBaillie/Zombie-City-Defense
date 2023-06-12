using Assets.Core.Abstract;
using Assets.Core.Interfaces;
using UnityEngine;

namespace Assets.Core.Controllers
{
    /// <summary>
    /// Controller class attached to the player vehicle that creates a fail event when killed
    /// </summary>
    [SelectionBase]
    public class StaticVehicleEntityController : AEntityController, IDamageReceiver
    {
        /// <summary>
        /// Called when the entity takes damage
        /// </summary>
        /// <param name="damage"></param>
        public void ApplyDamage(float damage)
        {

        }
    }
}
