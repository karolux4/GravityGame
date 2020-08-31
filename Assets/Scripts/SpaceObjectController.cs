using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObjectController : MonoBehaviour, ISpaceObject
{

    public float massInput =1f;
    public float detectRadius = 20f;

    public float mass { get; set; }

    public Vector3 worldPosition { get; set; }

    private float timerDelay = 2;

    // Start is called before the first frame update
    void Start()
    {
        this.worldPosition = this.transform.position;
        this.mass = massInput;
    }

    // Update is called once per frame
    void Update()
    {
        this.mass = massInput;
        this.worldPosition = this.transform.position;
        Detect(detectRadius);
        timerDelay += Time.deltaTime;
    }

    private void Detect(float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.worldPosition, radius);
        for(int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject != this.gameObject)
            {
                ISpaceObject obj = hitColliders[i].gameObject.GetComponent<SpaceObjectController>();
                if (obj != null)
                {
                    float force = CalculateGravitationalForce(obj);
                    float acceleration = CalculateAcceleration(force);
                    Vector3 towards = obj.worldPosition - this.worldPosition;
                    Move(towards.normalized * acceleration);
                }
            }
        }
    }

    public float CalculateGravitationalForce(ISpaceObject obj)
    {
        float gravitationalConstant = 6.67408f * (float)Math.Pow(10, -11); // gravitational constant
        // Formula: G*(m1*m2)/d^2
        float force = (float)gravitationalConstant * (float)(this.mass * obj.mass) /
            (float)Math.Pow((float)Vector2.Distance(this.worldPosition, obj.worldPosition),2);
        return force;
    }

    public float CalculateAcceleration(float force)
    {
        // Formula: F=ma => a=F/m
        float acceleration = (float)force / (float)this.mass;
        return acceleration;
    }

    public void Move(Vector2 velocity)
    {
        Rigidbody2D rigidBody=this.gameObject.GetComponent<Rigidbody2D>();
        rigidBody.AddForce(velocity, ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (this.gameObject.tag != "Player")
            {
                collision.gameObject.GetComponent<ISpaceShipParameters>().Damage(10);
                Destroy(this.gameObject);
            }
            else
            {
                this.gameObject.GetComponent<ISpaceShipParameters>().Damage(10);
            }
        }
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<ISpaceShipParameters>().Damage(10);
            Destroy(this.gameObject);
        }
    }

    void OnCollisionStay2D (Collision2D collision)
    {
        if (timerDelay >= 2)
        {
            if (collision.gameObject.tag == "Enemy" && this.gameObject.tag == "Player")
            {
                this.gameObject.GetComponent<ISpaceShipParameters>().Damage(10);
                collision.gameObject.GetComponent<ISpaceShipParameters>().Damage(10);
                timerDelay = 0;
            }
        }
    }
}
