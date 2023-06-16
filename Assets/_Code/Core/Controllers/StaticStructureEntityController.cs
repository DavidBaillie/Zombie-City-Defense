using Assets.Core.Abstract;
using Assets.Core.Interfaces;
using Assets.Core.Models;
using Assets.Tags.Abstract;
using Assets.Tags.Models;
using Assets.Utilities.ExtendedClasses;
using System;

namespace Assets.Core.Controllers
{
    public class StaticStructureEntityController : AEntityController, IDamageReceiver, IDeployableEntity
    {
        private StaticStructureTag structureTag;
        private Guid worldPositonId;


        public bool ApplyDamage(float damage)
        {
            return false;
        }

        public AEntityController SetupController(WorldPosition position, AUnitTag tag)
        {
            StaticStructureTag localTag = tag as StaticStructureTag;
            localTag.ThrowIfNull("Cannot setup the static structure controller because the provided unit tag is not a static structure tag!");

            structureTag = localTag;
            worldPositonId = position.Id;
            return this;
        }
    }
}
