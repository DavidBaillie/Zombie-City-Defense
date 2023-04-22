using Assets.Core.Models;
using Assets.Tags.Abstract;
using Assets.Tags.Common;
using System;
using UnityEngine;

namespace Assets.Tags.Channels
{
    [CreateAssetMenu(menuName = ChannelAssetBaseName + "Gameplay Canvas", fileName = "Gameplay Canvas Channel")]
    public class GameplayCanvasChannel : AChannel
    {
        public event Action<WorldPosition> OnDisplayPlacementSelectionView;
        public event Action<WorldPosition, StaticEntityIdentifier> OnUserSelectedStaticEntityPlacement;
        public event Action OnUserClickedAway;


        public void RaiseOnDisplayPlacementSelectionView(WorldPosition placementPosition) 
            => OnDisplayPlacementSelectionView?.Invoke(placementPosition);

        public void RaiseOnUserSelectedStaticEntityPlacement(WorldPosition placementPosition, StaticEntityIdentifier entityIdentifier) 
            => OnUserSelectedStaticEntityPlacement?.Invoke(placementPosition, entityIdentifier); 

        public void RaiseOnUserClickedAway() => OnUserClickedAway?.Invoke();
    }
}
