using System.Collections.Generic;
using Entitas;
using UnityEngine;

class ViewResourceSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    public ViewResourceSystem(IContext<GameEntity> context) : base(context) {}

    public ViewResourceSystem(ICollector<GameEntity> collector) : base(collector) {}

    private Transform _parent;

    public void Initialize()
    {
        _parent = new GameObject("view").transform;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.ViewResource);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasViewResource;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var prefabPath = string.Format("Prefabs/{0}", e.viewResource.PrefabPath);
            var go = Object.Instantiate(Resources.Load(prefabPath)) as GameObject;
            
            if (go == null)
            {
                Debug.LogError(string.Format("{0} doesn't exist", prefabPath));
                continue;
            }
            
            go.transform.parent = _parent;
            
            if (e.hasTransform)
                go.transform.position = new Vector3(e.transform.Position.x, 1, e.transform.Position.y);
            
            
            e.AddView(go);
        }
    }

    
}
