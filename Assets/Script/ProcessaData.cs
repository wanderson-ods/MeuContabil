using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Globalization;

public class ProcessaData : MonoBehaviour
{
    private Button btnData;
    public Text txtData, txtProxData, txtValorTela,txtInvestTela;
    public GameObject objData,objCriaData;
    public Button btnOk, btnConfirmData, btnCriaData, btnSair;
    public Dropdown dropDarta;
    string newData = "";
    public static bool processa = false;
    public bool entrada, home,saida;
    void Start()
    {
        btnData = GetComponent<Button>();
        txtData.text = CONFIGMASTER.instance.mesAtivo[0];

        btnData.onClick.RemoveAllListeners();

        btnData.onClick.AddListener(AbreData);
        
    }

    void AbreData()
    {
        //Dropdown[] dropSave = new Dropdown[CONFIGMASTER.instance.dataContabil.Count];
        List<string> x = new List<string>();
        objData.SetActive(true);

        btnOk.onClick.RemoveAllListeners();
        btnConfirmData.onClick.RemoveAllListeners();
        btnSair.onClick.RemoveAllListeners();

        btnOk.onClick.AddListener(ConfirmData);
        btnConfirmData.onClick.AddListener(CriaData);
        btnSair.onClick.AddListener(Sair);

        //dropSave[0].options("1");

        //dropDarta.options.RemoveRange(0,dropDarta.options.Count);
        dropDarta.ClearOptions();

        int y = int.Parse( CONFIGMASTER.instance.mesAtivo[1]);
        for (int i = 0; i < CONFIGMASTER.instance.dataContabil.Count; i++)
        {
            x.Add(CONFIGMASTER.instance.dataContabil[i]);
        }

        dropDarta.AddOptions(x);
        dropDarta.value = y;
    }

    void ConfirmData()
    {
        int indice = CONFIGMASTER.instance.indiceOrdMes[dropDarta.value];
        CONFIGMASTER.instance.mesAtivo[0] = CONFIGMASTER.instance.dataContabil[indice];
        CONFIGMASTER.instance.mesAtivo[1] = indice.ToString();
        CarregaValor();
        objData.SetActive(false);
        txtData.text = CONFIGMASTER.instance.mesAtivo[0];

    }

    void CriaData()
    {
        int ultimaData = CONFIGMASTER.instance.indiceOrdMes[CONFIGMASTER.instance.indiceOrdMes.Count - 1];

        int mes = Int32.Parse(CONFIGMASTER.instance.dataContabil[ultimaData].Substring(0,2));
        int ano = Int32.Parse(CONFIGMASTER.instance.dataContabil[ultimaData].Substring(3,4));
        string mesStr = "";

        if(mes == 12)
        {
            mesStr = "01";
            ano++;
        }
        else if (mes < 9)
        {
            mes++;
            mesStr = "0"+mes;
        }
        else
        {
            mes++;
            mesStr = mes.ToString();
        }

        objCriaData.SetActive(true);
        newData = mesStr+"/"+ano;
        txtProxData.text = newData;
        btnCriaData.onClick.RemoveAllListeners();
        btnCriaData.onClick.AddListener(SetaData);
    }

    void SetaData()
    {
        CONFIGMASTER.instance.CriarData(newData);
        objData.SetActive(false);
        objCriaData.SetActive(false);
        txtData.text = CONFIGMASTER.instance.mesAtivo[0];
        CarregaValor();

    }

    void Sair()
    {
        objData.SetActive(false);
        objCriaData.SetActive(false);
        CarregaValor();
    }

    void CarregaValor()
    {
        CONFIGMASTER.instance.startRecebPag();

        if(entrada)
        {
            CONFIGMASTER.instance.ValorMensal();

            int mes = Int32.Parse(CONFIGMASTER.instance.mesAtivo[1]);
            float val = CONFIGMASTER.instance.valorEntradas[mes];

            txtValorTela.text = val.ToString("C2", CONFIGMASTER.formatoBrasil);

            processa = true;
        }
        else if(home)
        {
            CONFIGMASTER.instance.ValorMensal();

            int mes = Int32.Parse(CONFIGMASTER.instance.mesAtivo[1]);
            float val = CONFIGMASTER.instance.valorEntradas[mes] - CONFIGMASTER.instance.valorSaidas[mes];

            txtValorTela.text = val.ToString("C2", CONFIGMASTER.formatoBrasil);

            txtInvestTela.text = CONFIGMASTER.instance.valorInvestido.ToString("C2", CONFIGMASTER.formatoBrasil);
        }
        else if(saida)
        {
            CONFIGMASTER.instance.ValorMensal();

            int mes = Int32.Parse(CONFIGMASTER.instance.mesAtivo[1]);
            float val = CONFIGMASTER.instance.valorSaidas[mes];

            txtValorTela.text = val.ToString("F2");

            processa = true;
        }

    }
}
