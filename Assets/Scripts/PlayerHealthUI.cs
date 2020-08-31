using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Slider slider;
    public GameObject playerObject;
    public GameObject deathScreen;
    public TextMeshProUGUI killCount;
    private ISpaceShipParameters player;
    // Start is called before the first frame update
    void Start()
    {
        player = playerObject.GetComponent<ISpaceShipParameters>();
        slider.maxValue = player.health;
        slider.value = player.health;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = player.health;
        if (player.health == 0)
        {
            deathScreen.SetActive(true);
        }
        killCount.text = Stats.kills.ToString();
    }



}
