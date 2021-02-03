using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class EnemyMoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float fDeltaTime = Time.DeltaTime;
        float3 fDirection = new float3();
        fDirection.x = 0.1f;
        Entities.ForEach((ref EnemyMove enemy, ref Translation translation, ref Rotation rotation) =>
        {
            translation.Value += enemy.fSpeedPerSecond * fDeltaTime * fDirection;

        }).Schedule();
    }
}
