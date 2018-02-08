using Entitas;
using UnityEngine;

public class MoveComponent : IComponent
{
    public Vector2 Velocity;
    public float CurrentAcceleration;
}