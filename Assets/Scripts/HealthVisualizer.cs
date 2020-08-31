using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthVisualizer : MonoBehaviour
{
    private float startValue;
    public float healthMax;

    public GameObject spaceShip;
    private ISpaceShipParameters parameters;
    // Start is called before the first frame update
    void Start()
    {
        parameters = spaceShip.GetComponent<ISpaceShipParameters>();
        healthMax = parameters.health;
        startValue = this.transform.localScale.y;

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x, (parameters.health/healthMax)*startValue, this.transform.localScale.z);
    }
}
