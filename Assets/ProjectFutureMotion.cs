using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Material = UnityEngine.Material;
using Mesh = UnityEngine.Mesh;

public struct ProjectIntoFutureTrail : IComponentData { }

public class ProjectFutureMotion : MonoBehaviour, IReceiveEntity
{
    public Mesh referenceMesh;
    public Material referenceMaterial;
    public int numSteps = 25;

    public bool renderTrails = false;

    public Entity WhiteBallEntity = Entity.Null;

    private RenderMesh ghostMaterial;
    public float3 ghostScale = 1f;
    private Simulation simulation;
    private PhysicsWorld localWorld;
    private SimulationStepInput stepInput;


    // Use this for initialization
    void Start()
    {
        ghostMaterial = new RenderMesh
        {
            mesh = referenceMesh,
            material = referenceMaterial
        };

        ref PhysicsWorld world = ref World.Active.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;

        localWorld = (PhysicsWorld)world.Clone();

        stepInput = new SimulationStepInput
        {
            World = localWorld,
            TimeStep = Time.fixedDeltaTime,
            ThreadCountHint = Unity.Physics.PhysicsStep.Default.ThreadCountHint,
            NumSolverIterations = Unity.Physics.PhysicsStep.Default.SolverIterationCount,
            Gravity = Unity.Physics.PhysicsStep.Default.Gravity,
            SynchronizeCollisionWorld = true
        };
    }

    // Update is called once per frame
    void Update()
    {
           if (WhiteBallEntity == null)
        {
            return;
        }

        try
        {
            // Sync the collision world first
            localWorld.CollisionWorld.ScheduleUpdateDynamicLayer(ref localWorld, stepInput.TimeStep, stepInput.ThreadCountHint, new JobHandle()).Complete();

            Color color = Color.red;
            for (int i = 0; i < numSteps; i++)
            {
                simulation.Step(stepInput);

                if (renderTrails)
                {
                    createTrails(ref localWorld);
                }

                color.a = 1.0f - ((float)i / numSteps);
            }
        }
        finally
        {
            localWorld.Dispose();
            simulation.Dispose();
        }
    }
    void createTrails(ref PhysicsWorld localWorld)
    {
        var em = World.Active.EntityManager;
        const float minVelocitySq = 0.05f;
        for (int i = 0; i < localWorld.DynamicBodies.Length; i++)
        {
            if (math.lengthsq(localWorld.MotionVelocities[i].LinearVelocity) > minVelocitySq)
            {
                var body = localWorld.DynamicBodies[i];

                var ghost = em.Instantiate(body.Entity);

                em.RemoveComponent<PhysicsCollider>(ghost);
                em.RemoveComponent<PhysicsVelocity>(ghost);

                em.AddComponentData(ghost, new ProjectIntoFutureTrail() );

                em.SetSharedComponentData(ghost, ghostMaterial);

                Translation position = em.GetComponentData<Translation>(ghost);
                position.Value = body.WorldFromBody.pos;
                em.SetComponentData(ghost, position);

                Rotation rotation = em.GetComponentData<Rotation>(ghost);
                rotation.Value = body.WorldFromBody.rot;
                em.SetComponentData(ghost, rotation);

                var scale = new NonUniformScale() { Value = ghostScale };
                if (em.HasComponent<NonUniformScale>(ghost))
                {
                    scale.Value *= em.GetComponentData<NonUniformScale>(ghost).Value;
                    em.SetComponentData(ghost, scale);
                }
                else
                {
                    em.AddComponentData(ghost, scale);
                }
            }
        }
    }
    public void SetReceivedEntity(Entity entity)
    {
        WhiteBallEntity = entity;
    }
}
