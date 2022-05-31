using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI damageText = null;
    public void DestroyText()
    {
        Destroy(gameObject);
    }
    public void SetValue(float amount)
    {
        damageText.text = String.Format("{0:0}", amount);
    }
}
