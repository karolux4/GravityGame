using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MeteorGenerator : MonoBehaviour
{

    public GameObject objectPrefab;
    public GameObject parentObject;

    [Serializable]
    public class Meteor
    {
        [SerializeField]
        float min;
        [SerializeField]
        float max;
        [SerializeField]
        Sprite sprite;

        public float GetMin()
        {
            return min;
        }

        public float GetMax()
        {
            return max;
        }

        public Sprite GetSprite()
        {
            return sprite;
        }
    }

    [Serializable]
    public class MeteorDictionary : SerializableDictionaryBase<string, Meteor> { }

    public MeteorDictionary MeteorSprites;

    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld(objectPrefab, parentObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateWorld(GameObject obj, GameObject parent, float startX=-50, float startY=-50, float length=100, int areaCount=10, int inAreaCount=10)
    {
        float areaLength = Mathf.Sqrt((length*length) / (float)areaCount);
        for(float i=startX; i < startX + length; i += areaLength)
        {
            for (float j = startY; j < startY + length; j += areaLength)
            {
                GenerateArea(obj, parent, i, j, areaLength, inAreaCount);
            }
        }
    }
    void GenerateArea(GameObject obj, GameObject parent, float x, float y, float length, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float randX = UnityEngine.Random.Range(0, length);
            float randY = UnityEngine.Random.Range(0, length);
            GameObject newObj = Instantiate(obj, parent.transform);
            newObj.transform.localPosition = new Vector3(x + randX, y + randY);
            float mass = UnityEngine.Random.Range(1, 1000);
            newObj.GetComponent<SpaceObjectController>().massInput = mass;
            MeteorDictionary availableMeteors = GetAvailableMeteorSprites(mass, MeteorSprites);
            newObj.GetComponent<SpriteRenderer>().sprite = availableMeteors.ElementAt(UnityEngine.Random.Range(0, availableMeteors.Count)).Value.GetSprite();
            Destroy(newObj.GetComponent<PolygonCollider2D>());
            newObj.AddComponent<PolygonCollider2D>();
        }
    }

    MeteorDictionary GetAvailableMeteorSprites(float mass, MeteorDictionary meteors)
    {
        MeteorDictionary availableMeteors = new MeteorDictionary();
        foreach(KeyValuePair<string, Meteor> met in meteors)
        {
            if (met.Value.GetMin() <= mass && met.Value.GetMax() >= mass)
            {
                availableMeteors.Add(met.Key, met.Value);
            }
        }
        return availableMeteors;
    }
}
