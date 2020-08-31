using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController, ISpaceShipParameters
{

    public float speedModifier = 0.1f;
    public float rotationModifier = 0.1f;

    public GameObject rightThrust;
    public GameObject leftThrust;

    public float maxHealth;

    private AudioSource mainAudioSource;
    private AudioSource impactAudioSource;

    public AudioClip engineStart;
    public AudioClip engineGoing;
    public AudioClip explosion;


    public GameObject explosionController;

    [Serializable]
    public class PartsDictionary : SerializableDictionaryBase<string, GameObject> { }

    public PartsDictionary parts;

    public float health { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
        AudioSource[] audioSources = GetComponents<AudioSource>();
        mainAudioSource = audioSources[0];
        impactAudioSource = audioSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Input.GetAxis("Vertical") >= 0 ? Input.GetAxis("Vertical") : 0;
        float rotation = Input.GetAxis("Horizontal");
        MoveUp(speed);
        Rotate(rotation);
        Sound(speed, rotation);
    }

    public void MoveUp(float speed)
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = this.gameObject.transform.up * speed *speedModifier;
        
    }

    public void Rotate(float strength)
    {
        this.gameObject.transform.Rotate(new Vector3(0, 0, (-1)*rotationModifier * strength));
        if (strength > 0)
        {
            leftThrust.SetActive(true);
            rightThrust.SetActive(false);
        }
        else if (strength < 0)
        {
            leftThrust.SetActive(false);
            rightThrust.SetActive(true);
        }
        else
        {
            leftThrust.SetActive(false);
            rightThrust.SetActive(false);
        }
    }

    private void Sound(float speed, float rotation)
    {
        if (speed > 0 || rotation != 0)
        {
            if (mainAudioSource.clip == null)
            {
                mainAudioSource.clip = engineStart;
                mainAudioSource.loop = false;
                mainAudioSource.Play();
            }
            else if (mainAudioSource.clip == engineStart && !mainAudioSource.isPlaying)
            {
                mainAudioSource.clip = engineGoing;
                mainAudioSource.loop = true;
                mainAudioSource.Play();
            }
        }
        else
        {
            mainAudioSource.Stop();
            mainAudioSource.clip = null;
        }
    }

    public void Damage(float amount)
    {
        impactAudioSource.Play();
        this.health -= amount;
        if (health <= 0)
        {
            health = 0;
            this.gameObject.GetComponent<Renderer>().enabled = false;
            this.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            explosionController.GetComponent<Animator>().Play("Explosion" + UnityEngine.Random.Range(1, 3));
            mainAudioSource.clip = explosion;
            mainAudioSource.loop = false;
            mainAudioSource.Play();
            SpawnParts();
            Stats.isAlive = false;
            this.gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }

    public void SpawnParts()
    {
        int amount = UnityEngine.Random.Range(1, 4);
        for (int i = 0; i < amount; i++)
        {
            GameObject newObj = Instantiate(parts.ElementAt(UnityEngine.Random.Range(0, parts.Count)).Value);

            float xMultiplier = (float)Mathf.Pow(-1, i);
            float yMultiplier = i < 2 ? 1 : -1;
            newObj.transform.localScale = this.transform.localScale / 3;
            newObj.transform.position = this.transform.position + new Vector3(xMultiplier * 0.25f, yMultiplier * 0.25f, 0);
            newObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(xMultiplier * 100, yMultiplier * 100));

        }
    }
}
