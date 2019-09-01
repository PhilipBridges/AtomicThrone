using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoney : MonoBehaviour
{
    public Text money;
    // Start is called before the first frame update
    void Start()
    {
        money.text = "$" + DudeController.currentMoney;
    }

    // Update is called once per frame
    void Update()
    {
        money.text = "$" + DudeController.currentMoney;
    }
}
