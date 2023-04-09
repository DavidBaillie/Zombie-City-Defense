using Game.Core.Abstract;
using Game.Core.Interfaces;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Core.Managers
{
    /// <summary>
    /// Class attached to persistent gameobject that allows for controller processing of logic in the game cycle
    /// </summary>
    [SelectionBase]
    public class LogicProcessingManager : ALogicProcessor
    {
        [SerializeField, MinValue(0), BoxGroup("Controls")]
        private int collectionPreallocationSize = 1000;

        [SerializeField, MinValue(1), SuffixLabel("count", Overlay = true), BoxGroup("Controls")]
        private int highPriorityUpdateRate = 50;
        [SerializeField, MinValue(1), SuffixLabel("count", Overlay = true), BoxGroup("Controls")]
        private int lowPriorityUpdateRate = 10;

        [SerializeField, ReadOnly]
        private List<ILogicUpdateProcessor> highPriorityProcessors = null;
        [SerializeField, ReadOnly]
        private List<ILogicUpdateProcessor> lowPriorityProcessors = null;


        private int lastHighPriorityUpdateIndex = 0;
        private int lastLowPriorityUpdateIndex = 0;


        /// <summary>
        /// Called when object created
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(this);
            }

            highPriorityProcessors = new List<ILogicUpdateProcessor>(collectionPreallocationSize);
            lowPriorityProcessors = new List<ILogicUpdateProcessor>(collectionPreallocationSize);
        }

        /// <summary>
        /// Called each frame
        /// </summary>
        protected override void Update()
        {
            base.Update();

            //Process logic
            for (int i = lastHighPriorityUpdateIndex; i < math.min(highPriorityUpdateRate, highPriorityProcessors.Count); i++)
            {
                try { highPriorityProcessors[i].ProcessLogic(); } catch { }
            }
            for (int j = lastLowPriorityUpdateIndex; j < math.min(lowPriorityUpdateRate, lowPriorityProcessors.Count); j++)
            {
                try { lowPriorityProcessors[j].ProcessLogic(); } catch { }
            }

            //Update index for next cycle
            lastHighPriorityUpdateIndex = (lastHighPriorityUpdateIndex + 1) % highPriorityProcessors.Count;
            lastLowPriorityUpdateIndex = (lastLowPriorityUpdateIndex + 1) % lowPriorityProcessors.Count;
        }


        public override void DeregisterHighPriorityProcessor(ILogicUpdateProcessor processor)
        {
            highPriorityProcessors.Remove(processor);
        }

        public override void DeregisterLowPriorityProcessor(ILogicUpdateProcessor processor)
        {
            lowPriorityProcessors.Remove(processor);
        }

        public override void RegisterHighPriorityProcessor(ILogicUpdateProcessor processor)
        {
            highPriorityProcessors.Add(processor);
        }

        public override void RegisterLowPriorityProcessor(ILogicUpdateProcessor processor)
        {
            lowPriorityProcessors.Add(processor);
        }
    }
}
