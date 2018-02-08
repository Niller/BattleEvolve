﻿using Entitas;

namespace Sources.Systems
{
    public class GameStartSystem : IInitializeSystem
    {
        readonly GameContext _context;

        public GameStartSystem(GameContext context)
        {
            _context = context;
        }
        
        public void Initialize()
        {
            var entity = _context.CreateEntity();
            entity.AddViewResource("Player");
            /*entity.AddName("current_player");
            entity.AddPosition(Map.instance.data.cells.First(c => c.isSpawnPoint));
            entity.isControllable = true;
            entity.isPlayer = true;
            entity.AddPersonality(Faction.Player, new Dictionary<Faction, UnitAttitude>());
            
            entity.AddMovable(entity, 3.5f);
            entity.AddActionable(10, 10);
            entity.AddLife(30, 30);
            entity.AddInventory(new List<InventoryItemComponent>());*/
        }
    }
}