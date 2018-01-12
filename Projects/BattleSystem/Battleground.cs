using System.Collections.Generic;

namespace BattleSystem
{
    public interface ISimulatable
    {
        void Simulate();
    }
    
    public class Battleground
    {
        private readonly List<ISimulatable> _simulatableUnits = new List<ISimulatable>();

        public void Simulate()
        {
            foreach (var simulatableUnit in _simulatableUnits)
            {
                simulatableUnit.Simulate();
            }
        }

        public void AddUnit()
        {
        
            _simulatableUnits.Add(new Unit(Vector2.Zero()));
        }
        
    }

    public class Vector2
    {
        public static Vector2 Zero()
        {
            throw new System.NotImplementedException();
        }
    }
}