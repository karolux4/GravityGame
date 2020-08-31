using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpaceShipParameters
{
    float health { get; set; }

    void Damage(float amount);
}
