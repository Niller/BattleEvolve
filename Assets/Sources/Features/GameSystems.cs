using Entitas;
using Sources.Systems;

public class GameSystems : Feature
{
    public GameSystems(Contexts contexts) : base("Game Systems")
    {
        Add(new GameStartSystem(contexts.game));
        Add(new ViewResourceSystem(contexts.game));
    }

    public sealed override Systems Add(ISystem system)
    {
        return base.Add(system);
    }
}