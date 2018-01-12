namespace BattleSystem.Actions
{

    public class MoveForward : MoveAction
    {
        public MoveForward(Transform transform, params object[] args) : base(transform, args)
        {
        }


        public override void Perform()
        {
            
        }
    }
    
    public abstract class MoveAction : BaseAction
    {
        protected Transform _transform;

        protected Transform _target;
        
        protected MoveAction(Transform transform, params object[] args) : base(args)
        {
            _transform = transform;
            _target = (Transform) args[0];
        }
        
        public override bool IsParallel()
        {
            return true;
        }
        
        //protected abstract Vector2 Direction
    }
}