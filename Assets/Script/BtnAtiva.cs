using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BtnAtiva : MonoBehaviour
{
    [SerializeField]
    private GameObject objAtiva;
    private Button btnMy;

    void Start()
    {
        btnMy = GetComponent<Button>();
        btnMy.onClick.AddListener(Ativa);
    }

    void Ativa()
    {
        objAtiva.SetActive(true);
        btnMy.onClick.AddListener(Desativa);
    }

    void Desativa()
    {
        objAtiva.SetActive(false);
        btnMy.onClick.AddListener(Ativa);
    }
}
