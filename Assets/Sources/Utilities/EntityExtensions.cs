using System;
using Entitas;

namespace Sources.Utilities
{
    public static class EntityExtensions
    {
        public static IComponent GetComponentByName(this Entity entity, string name)
        {
            var index = Array.FindIndex(GameComponentsLookup.componentNames,
                c => c == name);
            if (index == -1 || !entity.HasComponent(index))
                return null;
            return entity.GetComponent(index);
        }
    }
}