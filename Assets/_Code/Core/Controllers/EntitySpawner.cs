﻿using Game.Core.Managers;
using Game.Utilities.BaseObjects;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Controllers
{
    public class EntitySpawner : AExtendedMonobehaviour
    {
        [SerializeField, Required, AssetsOnly]
        private GameObject entityToSpawn = null;

        [SerializeField, Required, SceneObjectsOnly]
        private Transform spawnPoint = null;

        [SerializeField, Required]
        private WaypointCollectionManager waypointManager = null;


        protected override void Start()
        {
            base.Start();

            if (entityToSpawn == null)
                return;

            MEC.Timing.RunCoroutine(EntitySpawnLoop());
        }


        private IEnumerator<float> EntitySpawnLoop()
        {
            while (true)
            {
                yield return MEC.Timing.WaitForSeconds(Random.Range(0.75f, 1.25f));
                var entity = Instantiate(entityToSpawn, spawnPoint.position, spawnPoint.rotation);
                entity.GetComponent<BasicMovingEntity>().AssignPath(waypointManager.GeneratePath());
            }
        }
    }
}
