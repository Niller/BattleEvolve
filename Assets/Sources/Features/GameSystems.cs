using Entitas;
using Sources.Systems;

public class GameSystems : Feature
{
    public GameSystems(Contexts contexts) : base("Game Systems")
    {
        Add(new GameStartSystem(contexts.game));
        Add(new ViewResourceSystem(contexts.game));
        Add(new ViewTransformUpdateSystem(contexts.game));
        Add(new PlayerControlSystem(contexts.input));
        Add(new MoveSystem(contexts.game));
    }

    public sealed override Systems Add(ISystem system)
    {
        return base.Add(system);
    }
}