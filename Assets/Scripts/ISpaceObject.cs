using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpaceObject
{
    float mass { get; set; }

    Vector3 worldPosition { get; set; }

    float CalculateGravitationalForce(ISpaceObject obj);

    float CalculateAcceleration(float force);

    void Move(Vector2 velocity);

}
