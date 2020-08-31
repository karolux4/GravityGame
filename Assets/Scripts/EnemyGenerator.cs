using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Serializable]
    public class EnemyShip
    {
        [SerializeField]
        int size;
        [SerializeField]
        float health;
        [SerializeField]
        Sprite sprite;

        public int GetSize()
        {
            return size;
        }

        public float GetHealth()
        {
            return health;
        }

        public Sprite GetSprite()
        {
            return sprite;
        }
    }

    public GameObject enemy;
    public GameObject parent;

    [Serializable]
    public class EnemyShipDictionary : SerializableDictionaryBase<string, EnemyShip> { }

    public EnemyShipDictionary enemyShips;


    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld(enemy, parent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateWorld(GameObject obj, GameObject parent, float startX = -50, float startY = -50, float length = 100, int areaCount = 10, int inAreaCount = 1)
    {
        float areaLength = Mathf.Sqrt((length * length) / (float)areaCount);
        for (float i = startX; i < startX + length-areaLength; i += areaLength)
        {
            for (float j = startY; j < startY + length-areaLength; j += areaLength)
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
            int sizeCategory = UnityEngine.Random.Range(1, 4);
            EnemyShipDictionary availableEnemies = GetAvailableEnemiesSprites(sizeCategory, enemyShips);
            EnemyShip ship = availableEnemies.ElementAt(UnityEngine.Random.Range(0, availableEnemies.Count)).Value;
            newObj.GetComponent<SpriteRenderer>().sprite = ship.GetSprite();
            EnemyController shipController = newObj.GetComponent<EnemyController>();
            shipController.health = ship.GetHealth();
            shipController.maxHealth = ship.GetHealth();
            newObj.GetComponentInChildren<HealthVisualizer>().healthMax = ship.GetHealth();
            shipController.detectionRadius = ship.GetSize()*12.5f;
            shipController.speedModifier = (5 - ship.GetSize())/4.0f;
            newObj.transform.localScale = new Vector3(sizeCategory+1, sizeCategory+1, 1);

            Destroy(newObj.GetComponent<PolygonCollider2D>());
            newObj.AddComponent<PolygonCollider2D>();
        }
    }

    EnemyShipDictionary GetAvailableEnemiesSprites(int sizeCategory, EnemyShipDictionary ships)
    {
        EnemyShipDictionary availableEnemies = new EnemyShipDictionary();
        foreach (KeyValuePair<string, EnemyShip> ship in ships)
        {
            if (ship.Value.GetSize()==sizeCategory)
            {
                availableEnemies.Add(ship.Key, ship.Value);
            }
        }
        return availableEnemies;
    }
}
