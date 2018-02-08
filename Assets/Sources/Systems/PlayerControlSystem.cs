using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

namespace Sources.Systems
{
    public class PlayerControlSystem : ReactiveSystem<InputEntity>
    {        
        public PlayerControlSystem(IContext<InputEntity> context) : base(context)
        {
            
        }

        public PlayerControlSystem(ICollector<InputEntity> collector) : base(collector)
        {
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.MouseInputState);
        }

        protected override bool Filter(InputEntity entity)
        {
            return true;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            var mouseInputState = entities.First().mouseInputState;
            var controllableEntity = Contexts.sharedInstance.game.GetGroup(GameMatcher.Controllable).First();
            if (controllableEntity.hasMove)
            {
                controllableEntity.RemoveMove();
            }
                
            if (mouseInputState.IsLeftButtonDown && mouseInputState.MousePosition.HasValue)
            {
                bool pointInControllable =
                    Vector2.Distance(controllableEntity.transform.Position, mouseInputState.MousePosition.Value) <
                    controllableEntity.circleBounds.Radius;
                
                if (!pointInControllable)
                {
                    var direction = (mouseInputState.MousePosition.Value - controllableEntity.transform.Position)
                        .normalized;
                    controllableEntity.AddMove(direction, 0);
                }
            }
        }
    }
}