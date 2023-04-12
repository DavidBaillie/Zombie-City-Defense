using Assets.Core.Interfaces;
using Assets.Tags.Abstract;
using Game.Utilities.Worker;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Tags.Core.Managers
{
    /// <summary>
    /// Class attached to persistent gameobject that allows for controller processing of logic in the game cycle
    /// </summary>
    [CreateAssetMenu(menuName = AssetMenuBaseName + "Processors/Logic Processor")]
    public class LogicProcessingManager : ALogicProcessor
    {
        [SerializeField, MinValue(0), BoxGroup("Controls")]
        private int collectionPreallocationSize = 1000;

        [SerializeField, MinValue(1), SuffixLabel("count", Overlay = true), BoxGroup("Controls")]
        private int highPriorityUpdateRate = 50;
        [SerializeField, MinValue(1), SuffixLabel("count", Overlay = true), BoxGroup("Controls")]
        private int lowPriorityUpdateRate = 10;

        private List<ILogicUpdateProcessor> highPriorityProcessors = null;
        private List<ILogicUpdateProcessor> lowPriorityProcessors = null;


        private int lastHighPriorityUpdateIndex = 0;
        private int lastLowPriorityUpdateIndex = 0;


        /// <summary>
        /// Called when object created
        /// </summary>
        public override void InitializeTag()
        {
            base.InitializeTag();
            UnityEventPassthrough.Instance.OnUpdate += Update;

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
        protected void Update()
        {
            UpdateHighPriorityProcessors();
            UpdateLowPriorityProcessors();
        }

        private void UpdateHighPriorityProcessors()
        {
            if (highPriorityProcessors.Count > 0)
            {
                int lowerBound = math.min(highPriorityUpdateRate, highPriorityProcessors.Count);

                for (int i = lastHighPriorityUpdateIndex; i < lastHighPriorityUpdateIndex + lowerBound; i++)
                {
                    try { highPriorityProcessors[i].ProcessLogic(); } catch { }
                }

                lastHighPriorityUpdateIndex = (lastHighPriorityUpdateIndex + lowerBound) % highPriorityProcessors.Count;
            }
            else
            {
                lastHighPriorityUpdateIndex = 0;
            }
        }

        private void UpdateLowPriorityProcessors()
        {
            if (lowPriorityProcessors.Count > 0)
            {
                int lowerBound = math.min(lowPriorityUpdateRate, lowPriorityProcessors.Count);

                for (int j = lastLowPriorityUpdateIndex; j < lastLowPriorityUpdateIndex + lowerBound; j++)
                {
                    try { lowPriorityProcessors[j].ProcessLogic(); } catch { }
                }

                lastLowPriorityUpdateIndex = (lastLowPriorityUpdateIndex + lowerBound) % lowPriorityProcessors.Count;
            }
            else
            {
                lastLowPriorityUpdateIndex = 0;
            }
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
