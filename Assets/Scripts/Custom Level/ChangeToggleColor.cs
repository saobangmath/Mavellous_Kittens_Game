using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeToggleColor : MonoBehaviour
{
    private Image _image;
    private Toggle _toggle;

    // Start is called before the first frame update
    void Start()
    {
        _image=GetComponent<Image>();
        _toggle = GetComponent<Toggle>();
        _image.color=Color.blue;
    }

    public void ToggleColor()
    {
        if (_toggle.isOn)
        {
            _image.color = Color.green;
        }
        else
        {
            _image.color=Color.blue;
        }
    }
}
