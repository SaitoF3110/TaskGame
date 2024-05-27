using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTask : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] Text _bounsText;
    float bounus;
    void Start()
    {
        Bonus(_gameManager._cityLevel);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bounus = Bonus(_gameManager._cityLevel);
            Debug.Log(bounus + "%");
        }
        _bounsText.text = "現在のボーナス" + bounus.ToString("F2") + "%";
    }
    float Bonus(int level)
    {
        float sam = 1;
        sam += 0.65f * level;
        return sam;
    }
}
