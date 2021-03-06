//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public ViewResourceComponent viewResource { get { return (ViewResourceComponent)GetComponent(GameComponentsLookup.ViewResource); } }
    public bool hasViewResource { get { return HasComponent(GameComponentsLookup.ViewResource); } }

    public void AddViewResource(string newPrefabPath) {
        var index = GameComponentsLookup.ViewResource;
        var component = CreateComponent<ViewResourceComponent>(index);
        component.PrefabPath = newPrefabPath;
        AddComponent(index, component);
    }

    public void ReplaceViewResource(string newPrefabPath) {
        var index = GameComponentsLookup.ViewResource;
        var component = CreateComponent<ViewResourceComponent>(index);
        component.PrefabPath = newPrefabPath;
        ReplaceComponent(index, component);
    }

    public void RemoveViewResource() {
        RemoveComponent(GameComponentsLookup.ViewResource);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherViewResource;

    public static Entitas.IMatcher<GameEntity> ViewResource {
        get {
            if (_matcherViewResource == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ViewResource);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherViewResource = matcher;
            }

            return _matcherViewResource;
        }
    }
}
