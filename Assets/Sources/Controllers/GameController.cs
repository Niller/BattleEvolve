using Entitas;
using Sources.Controllers;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Systems _systems;

    void Start()
    {
        // get a reference to the contexts
        var contexts = Contexts.sharedInstance;

        // create the systems by creating individual features
        _systems = new Feature("Systems")
            .Add(new GameSystems(contexts));
        InitInputController<GameInputController>();

        // call Initialize() on all of the IInitializeSystems
        _systems.Initialize();
    }

    protected virtual void InitInputController<T>() where T : MonoBehaviour
    {
        var input = new GameObject("InputController");
        input.AddComponent<T>();
    }

    void Update()
    {
        // call Execute() on all the IExecuteSystems and 
        // ReactiveSystems that were triggered last frame
        _systems.Execute();
        // call cleanup() on all the ICleanupSystems
        _systems.Cleanup();
    }
}