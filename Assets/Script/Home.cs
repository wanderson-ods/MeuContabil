using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class Home : MonoBehaviour
{
    public Text txtSaldo, txtInvest, txtNome;
    public Button[] btnFuncionalidade = new Button[4];
    public Button btnVolta;

    void Start()
    {
        btnFuncionalidade[0].onClick.RemoveAllListeners();
        btnFuncionalidade[1].onClick.RemoveAllListeners();
        btnFuncionalidade[2].onClick.RemoveAllListeners();
        btnFuncionalidade[3].onClick.RemoveAllListeners();
        btnVolta.onClick.RemoveAllListeners();

        btnFuncionalidade[0].onClick.AddListener(Pagina0);
        btnFuncionalidade[1].onClick.AddListener(Pagina1);
        btnFuncionalidade[2].onClick.AddListener(Pagina2);
        btnFuncionalidade[3].onClick.AddListener(Pagina3);
        btnVolta.onClick.AddListener(Voltar);

        CarregaValor();

        txtNome.text = CONFIGMASTER.instance.contasAtivas[CONFIGMASTER.instance.entrouConta];
    }

    void Pagina0 ()
    {
        CONFIGMASTER.instance.TrocaPagina(2);
    }
    void Pagina1 ()
    {
        CONFIGMASTER.instance.TrocaPagina(3);
    }
    void Pagina2 ()
    {
        CONFIGMASTER.instance.TrocaPagina(4);
    }
    void Pagina3 ()
    {
        CONFIGMASTER.instance.TrocaPagina(5);
    }
    void Voltar ()
    {
        CONFIGMASTER.instance.TrocaPagina(0);
    }

    void CarregaValor()
    {
        for (int i = 0; i < CONFIGMASTER.instance.descriSaida.Count; i++)
        {
            if(CONFIGMASTER.instance.CapturaTitulo("P",i,2) == "Dízimo")
            {
                CONFIGMASTER.instance.itemValorSaidas[i] = CONFIGMASTER.instance.CalculaDizimo();
            }
        }

        CONFIGMASTER.instance.ValorMensal();

        int mes = Int32.Parse(CONFIGMASTER.instance.mesAtivo[1]);
        float val = CONFIGMASTER.instance.valorEntradas[mes] - CONFIGMASTER.instance.valorSaidas[mes];

        txtSaldo.text = val.ToString("C2", CONFIGMASTER.formatoBrasil);

        txtInvest.text = CONFIGMASTER.instance.valorInvestido.ToString("C2", CONFIGMASTER.formatoBrasil);
    }
}
