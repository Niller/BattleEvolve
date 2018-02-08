using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Input]
[Unique]
public class MouseInputStateComponent : IComponent
{
    //public bool IsLeftButtonPressed;
    public bool IsLeftButtonUp;
    public bool IsRightButtonUp;
    public Vector2? MousePosition;
    public bool IsUnderUi;
    //public InteractableComponent interactableObject;
}
