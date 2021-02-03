using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
public class UnitySpawnMono : MonoBehaviour
{
    public GameObject Prefab;
    
    Entity m_prefab;
    private EntityManager _entityManager;

    GameObjectConversionSettings settings;

    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype archetype = _entityManager.CreateArchetype(
            typeof(Translation),
            typeof(EnemyMove)
        );

        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

        m_prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, settings);

        Spawn(Vector3.zero);
    }

    public void Spawn(Vector3 _position)
    {
        Entity myEntity = _entityManager.Instantiate(m_prefab);

        //_entityManager.SetComponentData(myEntity, new )
    }
}
