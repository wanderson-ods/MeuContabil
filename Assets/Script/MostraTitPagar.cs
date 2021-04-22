using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class MostraTitPagar : MonoBehaviour
{
    public int indice;
    private Button myButao;
    public Button[] btnFunc = new Button[4];
    public Text txtDescriTit,txtValorTit;
    public GameObject objImgBloq, objOptions;
    public GameObject objAtivaTit, objDesativaTit,objSumiu,objDizi,objTitPG,objTitNP,objPagou;

    void Awake() {
        indice = Saidas.indice;
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

        descri = CONFIGMASTER.instance.CapturaTitulo("P",indice,1)+"   |   "+CONFIGMASTER.instance.CapturaTitulo("P",indice,5);
        txtDescriTit.text = descri;
        txtValorTit.text = CONFIGMASTER.instance.itemValorSaidas[indice].ToString("C2", CONFIGMASTER.formatoBrasil)+"   ";

        //print("Titulo invalido "+CONFIGMASTER.instance.CapturaTitulo("R",indice,4));
        if(CONFIGMASTER.instance.CapturaTitulo("P",indice,4) == "-")
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
        if(CONFIGMASTER.instance.CapturaTitulo("P",indice,7) == "NP" && CONFIGMASTER.instance.CapturaTitulo("P",indice,4) == "+")
        {
            objTitPG.SetActive(false);
            objTitNP.SetActive(true);
            objPagou.SetActive(false);
        }
        else 
        {
            if(CONFIGMASTER.instance.CapturaTitulo("P",indice,4) == "+")
            {
                objTitPG.SetActive(true);
                objTitNP.SetActive(false);
                objPagou.SetActive(true);
            }
        }

    }

    void MostraOpitions()
    {
        objOptions.SetActive(true);

        btnFunc[0].onClick.RemoveAllListeners();
        btnFunc[1].onClick.RemoveAllListeners();
        btnFunc[2].onClick.RemoveAllListeners();
        btnFunc[3].onClick.RemoveAllListeners();

        //Validações nos botões---------------------------------------------------------
        if(CONFIGMASTER.instance.CapturaTitulo("P",indice,2) != "Dízimo")
            btnFunc[0].onClick.AddListener(ClickEditar);
        else
            objDizi.SetActive(false);

        btnFunc[1].onClick.AddListener(ClickDesativar);

        if(CONFIGMASTER.instance.CapturaTitulo("P",indice,1) != "Mês Anterior")
            btnFunc[2].onClick.AddListener(ClickExcluir);
        else
            objSumiu.SetActive(false);
        btnFunc[3].onClick.AddListener(ClickPago);
        //-----------------------------------------------------------------------------

        if(CONFIGMASTER.instance.CapturaTitulo("P",indice,4) == "-")
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
        Saidas.Clicou(indice);
        Sair();
    }

    void ClickExcluir()
    {
        CONFIGMASTER.instance.descriSaida.RemoveRange(indice,1);
        CONFIGMASTER.instance.itemValorSaidas.RemoveRange(indice,1);

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

        if(CONFIGMASTER.instance.CapturaTitulo("P",indice,4) == "-")
        {
            x = CONFIGMASTER.instance.descriSaida[indice].IndexOf("-");

            prefixo = CONFIGMASTER.instance.descriSaida[indice].Substring(0,x);
            sufixo = CONFIGMASTER.instance.descriSaida[indice].Substring(x+1);

            CONFIGMASTER.instance.descriSaida[indice] = prefixo + "+" + sufixo;

            //ProcessaData.processa = true;

            objAtivaTit.SetActive(true);
            objDesativaTit.SetActive(false);

            objImgBloq.SetActive(false);
        }
        else
        {
            x = CONFIGMASTER.instance.descriSaida[indice].IndexOf("+");

            prefixo = CONFIGMASTER.instance.descriSaida[indice].Substring(0,x);
            sufixo = CONFIGMASTER.instance.descriSaida[indice].Substring(x+1);

            CONFIGMASTER.instance.descriSaida[indice] = prefixo + "-" + sufixo;

            

            objAtivaTit.SetActive(false);
            objDesativaTit.SetActive(true);

            objImgBloq.SetActive(true);
        }

        CONFIGMASTER.instance.ValorMensal();
        CONFIGMASTER.instance.SaveTitulo();
        ProcessaData.processa = true;
        Sair();
    }

    void ClickPago()
    {
        string prefixo = "";
        int x;
        print("validar "+CONFIGMASTER.instance.CapturaTitulo("P",indice,7)+" validar 2 "+CONFIGMASTER.instance.CapturaTitulo("P",indice,4));
        if(CONFIGMASTER.instance.CapturaTitulo("P",indice,7) == "NP" && CONFIGMASTER.instance.CapturaTitulo("P",indice,4) == "+")
        {
            x = CONFIGMASTER.instance.descriSaida[indice].IndexOf("+");
            x += 2;

            prefixo = CONFIGMASTER.instance.descriSaida[indice].Substring(0,x)+CONFIGMASTER.instance.CapturaTitulo("P",indice,5)+"*"+CONFIGMASTER.instance.CapturaTitulo("P",indice,6)+"*";
            //print("como estava "+CONFIGMASTER.instance.descriSaida[indice]+" -prefixo- "+prefixo);
            CONFIGMASTER.instance.descriSaida[indice] = prefixo + "PG*";
            //print("como ficou "+CONFIGMASTER.instance.descriSaida[indice]);
            //ProcessaData.processa = true;

            objTitPG.SetActive(true);
            objTitNP.SetActive(false);
            objPagou.SetActive(true);
        }
        else 
        {
            if(CONFIGMASTER.instance.CapturaTitulo("P",indice,4) == "+")
            {
                x = CONFIGMASTER.instance.descriSaida[indice].IndexOf("+");
                x += 2;

                prefixo = CONFIGMASTER.instance.descriSaida[indice].Substring(0,x)+CONFIGMASTER.instance.CapturaTitulo("P",indice,5)+"*"+CONFIGMASTER.instance.CapturaTitulo("P",indice,6)+"*";
                //print("como estava2 "+CONFIGMASTER.instance.descriSaida[indice]);
                CONFIGMASTER.instance.descriSaida[indice] = prefixo + "NP*";
                //print("como ficou2 "+CONFIGMASTER.instance.descriSaida[indice]);
                //ProcessaData.processa = true;

                objTitPG.SetActive(false);
                objTitNP.SetActive(true);
                objPagou.SetActive(false);
            }
        }

        CONFIGMASTER.instance.SaveTitulo();
        Sair();
    }

    void Sair()
    {print("sair");
        myButao.onClick.RemoveAllListeners();
        myButao.onClick.AddListener(MostraOpitions);
        objOptions.SetActive(false);
    }
}
