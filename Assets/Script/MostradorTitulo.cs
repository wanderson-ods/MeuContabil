using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class MostradorTitulo : MonoBehaviour
{
    public int indice;
    private Button myButao;
    public Button[] btnFunc = new Button[3];
    public Text txtDescriTit,txtValorTit;
    public GameObject objImgBloq, objOptions;
    public GameObject objAtivaTit, objDesativaTit,objSumiu;

    void Awake() {
        indice = Entradas.indice;
    }
    void Start() 
    {

        CarregaTitulo();

        myButao = GetComponent<Button>();
        myButao.onClick.RemoveAllListeners();
        myButao.onClick.AddListener(MostraOpitions);

    }

    void CarregaTitulo()
    {
        string descri = "";

        descri = CONFIGMASTER.instance.CapturaTitulo("R",indice,1)+"   |   "+CONFIGMASTER.instance.CapturaTitulo("R",indice,5);
        txtDescriTit.text = descri;
        txtValorTit.text = CONFIGMASTER.instance.itemValorEntrada[indice].ToString("C2", CONFIGMASTER.formatoBrasil)+"   ";

        //print("Titulo invalido "+CONFIGMASTER.instance.CapturaTitulo("R",indice,4));
        if(CONFIGMASTER.instance.CapturaTitulo("R",indice,4) == "-")
        {
            objAtivaTit.SetActive(false);
            objDesativaTit.SetActive(true);
            objImgBloq.SetActive(true);
        }
        else
        {
            objAtivaTit.SetActive(true);
            objDesativaTit.SetActive(false);
            objImgBloq.SetActive(false);
        }

    }

    void MostraOpitions()
    {
        objOptions.SetActive(true);

        btnFunc[0].onClick.RemoveAllListeners();
        btnFunc[1].onClick.RemoveAllListeners();
        btnFunc[2].onClick.RemoveAllListeners();

        btnFunc[0].onClick.AddListener(ClickEditar);
        btnFunc[1].onClick.AddListener(ClickDesativar);

        if(CONFIGMASTER.instance.CapturaTitulo("R",indice,1) != "Mês Anterior")
            btnFunc[2].onClick.AddListener(ClickExcluir);
        else
            objSumiu.SetActive(false);

        if(CONFIGMASTER.instance.CapturaTitulo("R",indice,4) == "-")
        {
            objAtivaTit.SetActive(true);
            objDesativaTit.SetActive(false);
        }
        else
        {
            objAtivaTit.SetActive(false);
            objDesativaTit.SetActive(true);
        }

        myButao.onClick.RemoveAllListeners();
        myButao.onClick.AddListener(Sair);
    }

    void ClickEditar()
    {
        Entradas.Clicou(indice);
        Sair();
    }

    void ClickExcluir()
    {
        CONFIGMASTER.instance.descriEntrada.RemoveRange(indice,1);
        CONFIGMASTER.instance.itemValorEntrada.RemoveRange(indice,1);

        CONFIGMASTER.instance.ValorMensal();
        CONFIGMASTER.instance.DeleteTitulo(CONFIGMASTER.instance.mesAtivo[0]);
        CONFIGMASTER.instance.SaveTitulo();
        Destroy(gameObject,0.01f);
        ProcessaData.processa = true;
        Sair();
    }

    void ClickDesativar()
    {
        string prefixo = "", sufixo = "";
        int x;

        if(CONFIGMASTER.instance.CapturaTitulo("R",indice,4) == "-")
        {
            x = CONFIGMASTER.instance.descriEntrada[indice].IndexOf("-");

            prefixo = CONFIGMASTER.instance.descriEntrada[indice].Substring(0,x);
            sufixo = CONFIGMASTER.instance.descriEntrada[indice].Substring(x+1);

            CONFIGMASTER.instance.descriEntrada[indice] = prefixo + "+" + sufixo;

            ProcessaData.processa = true;

            objAtivaTit.SetActive(true);
            objDesativaTit.SetActive(false);

            objImgBloq.SetActive(false);
        }
        else
        {
            x = CONFIGMASTER.instance.descriEntrada[indice].IndexOf("+");

            prefixo = CONFIGMASTER.instance.descriEntrada[indice].Substring(0,x);
            sufixo = CONFIGMASTER.instance.descriEntrada[indice].Substring(x+1);

            CONFIGMASTER.instance.descriEntrada[indice] = prefixo + "-" + sufixo;

            ProcessaData.processa = true;

            objAtivaTit.SetActive(false);
            objDesativaTit.SetActive(true);

            objImgBloq.SetActive(true);
        }

        CONFIGMASTER.instance.ValorMensal();
        CONFIGMASTER.instance.SaveTitulo();
        Sair();
    }

    void Sair()
    {
        myButao.onClick.RemoveAllListeners();
        myButao.onClick.AddListener(MostraOpitions);
        objOptions.SetActive(false);
    }
}
