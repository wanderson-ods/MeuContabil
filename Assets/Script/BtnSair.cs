using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnSair : MonoBehaviour
{
    public void Sair()
    {
        CONFIGMASTER.instance.Bye();
    }
}
