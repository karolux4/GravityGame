using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour, ISpaceShipParameters
{
    public float health { get; set; }
    public float maxHealth;

    public float detectionRadius;
    public float shootingRadius;
    public float speedModifier;

    public GameObject bullet;
    public float bulletSpeed = 5f;

    private AudioSource mainAudioSource;

    public AudioClip explosion;

    public GameObject explosionController;

    [Serializable]
    public class PartsDictionary : SerializableDictionaryBase<string, GameObject> { }

    public PartsDictionary parts;

    private float timer = 0;
    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
        AudioSource[] audioSources = GetComponents<AudioSource>();
        mainAudioSource = audioSources[0];
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer(detectionRadius);
        timer += Time.deltaTime;
    }

    private void DetectPlayer(float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.transform.position, radius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject != this.gameObject)
            {
                PlayerController obj = hitColliders[i].gameObject.GetComponent<PlayerController>();
                if (obj != null)
                {
                    Vector3 towards = obj.transform.position - this.transform.position;
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = towards.normalized * speedModifier;
                    if (timer > 3f && hitColliders[i].Distance(this.GetComponent<PolygonCollider2D>()).distance<shootingRadius)
                    {
                        Shoot(bullet, towards, bulletSpeed);
                    }
                }
            }
        }
    }

    private void Shoot(GameObject bullet, Vector3 direction, float power)
    {
        mainAudioSource.Play();
        GameObject newObj = Instantiate(bullet);
        newObj.transform.localPosition = this.transform.localPosition+direction.normalized*0.8f*this.transform.localScale.x;
        float zAngle = 0;
        zAngle = direction.x==0&&direction.y<0 ? 180*Mathf.Deg2Rad : Mathf.Atan(direction.y / direction.x);
        newObj.transform.eulerAngles = new Vector3(0,0, (Mathf.Rad2Deg*zAngle)-90);
        newObj.GetComponent<Rigidbody2D>().velocity = direction.normalized * power;
        timer = 0f;
    }

    public void Damage(float amount)
    {
        this.health -= amount;
        if (health <= 0)
        {
            health = 0;
            this.gameObject.GetComponent<Renderer>().enabled = false;
            this.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            explosionController.GetComponent<Animator>().Play("Explosion"+UnityEngine.Random.Range(1,3));
            mainAudioSource.clip = explosion;
            mainAudioSource.Play();
            SpawnParts();
            if (Stats.isAlive)
            {
                Stats.kills++;
            }
            Destroy(this.gameObject,2f);
        }
    }

    public void SpawnParts()
    {
        int amount = UnityEngine.Random.Range(1, 4);
        for(int i = 0; i < amount; i++)
        {
            GameObject newObj = Instantiate(parts.ElementAt(UnityEngine.Random.Range(0,parts.Count)).Value);

            float xMultiplier = (float)Math.Pow(-1, i);
            float yMultiplier= i < 2 ? 1 : -1;
            newObj.transform.localScale = this.transform.localScale / 3;
            newObj.transform.position = this.transform.position + new Vector3( xMultiplier* 0.25f, yMultiplier*0.25f,0);
            newObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(xMultiplier*100, yMultiplier*100));

        }
    }
}
