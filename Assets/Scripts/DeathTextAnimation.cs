using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTextAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(this.gameObject, new Vector3(1, 1, 1), 2f);
    }

    public void Restart()
    {
        Stats.kills = 0;
        Stats.isAlive = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
