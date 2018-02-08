using System.Collections.Generic;
using Entitas;
using UnityEngine;

namespace Sources.Systems
{
    public class ViewTransformUpdateSystem : ReactiveSystem<GameEntity>
    {
        public ViewTransformUpdateSystem(IContext<GameEntity> context) : base(context)
        {
        }

        public ViewTransformUpdateSystem(ICollector<GameEntity> collector) : base(collector)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Transform);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasView;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var e in entities)
            {
                e.view.View.transform.localPosition = e.transform.Position.ConvertToGroundVector3();
                e.view.View.transform.localRotation = Quaternion.Euler(0, e.transform.Rotation, 0);
            }
        }
    }
}