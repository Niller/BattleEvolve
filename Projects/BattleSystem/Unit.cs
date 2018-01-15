using System.Collections.Specialized;
using UnityEngine;

namespace BattleSystem
{
    public class Unit : ISimulatable
    {
        private readonly Transform _transform;

        public Unit(Vector2 position)
        {
            _transform = new Transform();
            _transform.Position = position;
        }
        
        public void Simulate()
        {
            
        }
    }
}