using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AcessaConta : MonoBehaviour
{
    [SerializeField]
    Text[] txtCont = new Text[3];
    [SerializeField]
    Button[] btnConta = new Button[3];
    bool[] temConta = new bool[3];
    public Text txtErroUser;
    public Text txtInputUser;
    public Button btnConfirmUser;
    public GameObject objNewUser;
    private int cliqueOK;
    public Dropdown caixaSelecao;
    public Text txtAno;
    
    void Start()
    {
        for (int i = 0; i < CONFIGMASTER.instance.contasAtivas.Length; i++)
        {
            if(CONFIGMASTER.instance.contasAtivas[i] != "x")
            {
                txtCont[i].text = CONFIGMASTER.instance.contasAtivas[i];
                temConta[i] = true;
            }
            else
            {
                txtCont[i].text ="Criar usuário";
                temConta[i] = false;
            }
                
        }
        btnConta[0].onClick.RemoveAllListeners();
        btnConta[1].onClick.RemoveAllListeners();
        btnConta[2].onClick.RemoveAllListeners();

        btnConta[0].onClick.AddListener(CliqueCont0);
        btnConta[1].onClick.AddListener(CliqueCont1);
        btnConta[2].onClick.AddListener(CliqueCont2);
    }

    void CliqueCont0()
    {
        if (temConta[0])
        {
            CONFIGMASTER.instance.AcessarConta(0);
        }
        else
            UsuarioNovo(0);
    }
    void CliqueCont1()
    {
        if (temConta[1])
        {
            CONFIGMASTER.instance.AcessarConta(1);
        }
        else
            UsuarioNovo(1);
    }
    void CliqueCont2()
    {
        if (temConta[2])
        {
            CONFIGMASTER.instance.AcessarConta(2);
        }
        else
            UsuarioNovo(2);
    }

    void UsuarioNovo(int qualConta)
    {
        objNewUser.SetActive(true);
        cliqueOK = qualConta;
        btnConfirmUser.onClick.RemoveAllListeners();
        btnConfirmUser.onClick.AddListener(UsuarioCheck);
    }

    void UsuarioCheck()
    {
        //print("a "+caixaSelecao.value);
        if(txtInputUser.text == "" || txtInputUser.text.Length < 3)
        {
            txtErroUser.text = "Nome está vazio ou muito curto!";
        }
        else if (txtAno.text.Length != 4 || Int32.Parse(txtAno.text) < 1000 || Int32.Parse(txtAno.text) > 9999 )
        {
            txtErroUser.text = "Ano precisa ter quatro dígitos!";
        }
        else if(caixaSelecao.value > 12 || caixaSelecao.value < 0)
        {
            txtErroUser.text = "Selecione um mês valido!";
        }
        else
        {
            string a = "";
            int b = caixaSelecao.value + 1;
            if(b < 10)
                a = "0"+b;
            else
                a = b.ToString();
            
            //print(a+"/"+txtAno.text);

            CONFIGMASTER.instance.dataContabil.Add(a+"/"+txtAno.text);
            //print("ant save conta");
            CONFIGMASTER.instance.contasAtivas[cliqueOK] = txtInputUser.text;
            CONFIGMASTER.instance.SaveConta();
            //print("ant acess conta");
            CONFIGMASTER.instance.AcessarConta(cliqueOK);
        }
    }
}
