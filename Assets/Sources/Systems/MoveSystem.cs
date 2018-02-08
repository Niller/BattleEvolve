using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Sources.Systems
{
    public class MoveSystem : ReactiveSystem<GameEntity>
    {
        public MoveSystem(IContext<GameEntity> context) : base(context)
        {
        }

        public MoveSystem(ICollector<GameEntity> collector) : base(collector)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Move);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasMovable;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var e in entities)
            {
                e.ReplaceTransform(e.transform.Position + e.move.Velocity * e.movable.Speed * Time.deltaTime, -Vector2.SignedAngle(Vector2.up, e.move.Velocity));
                
            }
        }
    }
}