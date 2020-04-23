using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// The class which manage for updating the color of the toogle
/// </summary>
public class ChangeToggleColor : MonoBehaviour
{
    private Image _image;
    private Toggle _toggle;
    /// <summary>
    /// the function Start() is called before the first frame update
    /// </summary>
    void Start()
    {
        _image=GetComponent<Image>();
        _toggle = GetComponent<Toggle>();
        _image.color=Color.blue;
    }
    /// <summary>
    /// the function ToggleColor() is update the toggle color when the toggle is active and inactive 
    /// </summary>
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
