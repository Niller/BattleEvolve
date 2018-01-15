using System.Collections.Generic;
using UnityEngine;

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
        
            _simulatableUnits.Add(new Unit(Vector2.zero));
        }
        
    }

}