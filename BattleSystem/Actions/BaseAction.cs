    namespace BattleSystem.Actions
{
    public abstract class BaseAction
    {
        protected BaseAction(params object[] args)
        {
            
        }
        
        public int Time { get; protected set; }
        public abstract bool IsParallel();
        public abstract void Perform();

    }
}