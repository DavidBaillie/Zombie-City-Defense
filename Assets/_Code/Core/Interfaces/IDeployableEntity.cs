using Assets.Core.Abstract;
using Assets.Core.Models;
using Assets.Tags.Abstract;

namespace Assets.Core.Interfaces
{
    public interface IDeployableEntity
    {
        AEntityController SetupController(WorldPosition position, AUnitTag tag);
    }
}
