using Assets.Core.Abstract;
using Assets.Core.Interfaces;
using Assets.Tags.Models;
using System;

namespace Assets.Core.Controllers
{
    public class StaticStructureEntityController : AEntityController, IDamageReceiver
    {



        public bool ApplyDamage(float damage)
        {
            return false;
        }

        public void SetupController(Guid positionId, StaticStructureTag structure)
        {

        }
    }
}
