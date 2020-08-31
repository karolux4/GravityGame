using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StarGenerator : MonoBehaviour
{

    public GameObject objectPrefab;
    public GameObject parentObject;

    [Serializable]
    public class Star
    {
        [SerializeField]
        Sprite sprite;

        public Sprite GetSprite()
        {
            return sprite;
        }
    }

    [Serializable]
    public class StarDictionary : SerializableDictionaryBase<string, Star> { }

    public StarDictionary Stars;

    void Start()
    {
        GenerateWorld(objectPrefab, parentObject);
    }

    void GenerateWorld(GameObject obj, GameObject parent, float startX = -50, float startY = -50, float length = 100, int areaCount = 10, int inAreaCount = 50)
    {
        float areaLength = Mathf.Sqrt((length * length) / (float)areaCount);
        for (float i = startX; i < startX + length; i += areaLength)
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
            newObj.transform.localEulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
            float scale = UnityEngine.Random.Range(0.25f, 1f);
            newObj.transform.localScale = new Vector3(scale, scale, scale);
            newObj.GetComponent<SpriteRenderer>().sprite = Stars.ElementAt(UnityEngine.Random.Range(0, Stars.Count)).Value.GetSprite();

        }
    }
}
