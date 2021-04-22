﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class Saidas : MonoBehaviour
{
    public static Saidas instace;
    public Text txtEntrada;
    public Button btnCriaItem,btnDizimo,btnSaiDizimo;
    public Button btnVolta;
    public Transform posicaoIncluirItem;
    public RectTransform tamanhoTelaItem;
    public GameObject objItem,objDizimo;
    private List<GameObject> objItensTela = new List<GameObject>();
    public static int indice = 0;
    private static int indiceE = -1;
    public static bool cliqueEdita = false;

    //Edição de titulo
    public Text txtEditErro;
    public InputField txtEditNome,txtEditDescri,txtEditParcela, txtEditValor,txtParcTotal;
    public Dropdown dropTipo,dropCondPG;
    public GameObject objEditParcela, objEditTit,objParcTot;
    public Button btnEditSai, btnConfirmEdit;

    //Mostrar valores por tipo
    private List<float> txtVlEntradaTipo = new List<float>();
    private bool ativoTPEntra = false, editando = false;
    public Text txtVlTPEntra, txtVlPg, txtVlNp;
    public Dropdown dropTPEntra;
    public Button btnVlEntradaTipo,btnVlEntradaSair;
    public GameObject objVlEntradaTipo;

    void Start()
    {
        btnCriaItem.onClick.RemoveAllListeners();
        btnVolta.onClick.RemoveAllListeners();
        btnVlEntradaTipo.onClick.RemoveAllListeners();
        btnDizimo.onClick.RemoveAllListeners();

        btnCriaItem.onClick.AddListener(CriarItem);
        btnVolta.onClick.AddListener(Voltar);
        btnVlEntradaTipo.onClick.AddListener(valorTitEntradas);
        btnDizimo.onClick.AddListener(IncluiDizimo);

        CarregaValor();
        CarregaItem();
    }

    void FixedUpdate() 
    {
        if(ProcessaData.processa)
        {
            ProcessaData.processa = false;
            CarregaValor();
            CarregaItem();
        }

        if(indiceE > -1)
        {
            if(dropCondPG.value == 2)
            {
                objEditParcela.SetActive(true);
                Text tx = objEditParcela.GetComponent<Text>();
           

                if(editando)
                {
                    objParcTot.SetActive(true);
                    
                    tx.text = "Nº de parcelas";
                }
                else
                {
                    tx.text = "Parcelas";
                    objParcTot.SetActive(false);
                }
            }
            else
                objEditParcela.SetActive(false);
        }
        if (cliqueEdita)
        {
            cliqueEdita = false;
            Editar();
        }

        if(ativoTPEntra)
        {
            txtVlTPEntra.text =  txtVlEntradaTipo[dropTPEntra.value].ToString("C2", CONFIGMASTER.formatoBrasil);
        }
    }
    void Voltar ()
    {
        indice = 0;
        CONFIGMASTER.instance.TrocaPagina(1);
    }

    void CarregaValor()
    {
        //calculo do dizimo se tiver
        for (int i = 0; i < CONFIGMASTER.instance.descriSaida.Count; i++)
        {
            if(CONFIGMASTER.instance.CapturaTitulo("P",i,2) == "Dízimo")
            {
                CONFIGMASTER.instance.itemValorSaidas[i] = CONFIGMASTER.instance.CalculaDizimo();
            }
        }

        CONFIGMASTER.instance.ValorMensal();

        int mes = Int32.Parse(CONFIGMASTER.instance.mesAtivo[1]);
        float val = CONFIGMASTER.instance.valorSaidas[mes];

        txtEntrada.text = val.ToString("C2", CONFIGMASTER.formatoBrasil);
    }

    void CarregaItem()
    {
        //calculo do dizimo se tiver
        for (int i = 0; i < CONFIGMASTER.instance.descriSaida.Count; i++)
        {
            if(CONFIGMASTER.instance.CapturaTitulo("P",i,2) == "Dízimo")
            {
                CONFIGMASTER.instance.itemValorSaidas[i] = CONFIGMASTER.instance.CalculaDizimo();
            }
        }

        if(indice > 0)
        {
            for (int i = 0; i < indice; i++)
            {
                Destroy (objItensTela[i]);
            }

            objItensTela.Clear();
            indice = 0;
        }

        for (int i = 0; i < CONFIGMASTER.instance.descriSaida.Count; i++)
        {
            tamanhoTelaItem.sizeDelta = new Vector2 (tamanhoTelaItem.sizeDelta.x, tamanhoTelaItem.sizeDelta.y + 125);

            objItensTela.Add(Instantiate (objItem, posicaoIncluirItem.position,objItem.transform.rotation) as GameObject);

            objItensTela[indice].transform.SetParent(posicaoIncluirItem);
            objItensTela[indice].transform.localScale = new Vector3 (1,1,1);
            indice++;
        }
    }

    void CriarItem()
    {
        indiceE = 0;

        objEditTit.SetActive(true);

        //--------------------//
        txtEditNome.text = "";//Nome
        //--------------------//

        //Forma de recebimento(Tipo)
        List<string> x = new List<string>();
        dropTipo.ClearOptions();
        for (int i = 0; i < CONFIGMASTER.instance.tipoSaida.Count; i++)
        {
            x.Add(CONFIGMASTER.instance.tipoSaida[i]);  
        }
        dropTipo.AddOptions(x);
        dropTipo.value = 0;
       //--------------------//

        txtEditDescri.text = "";//Descrição

        //--------------------//

        //Condição de pagamento(Frequencia)
        List<string> y = new List<string>();
        dropCondPG.ClearOptions();
        for (int i = 0; i < CONFIGMASTER.instance.frequenciaTit.Count; i++)
        {
            y.Add(CONFIGMASTER.instance.frequenciaTit[i]);  
        }
        dropCondPG.AddOptions(y);
        dropCondPG.value = 0;
        
        //--------------------//

        objEditParcela.SetActive(true);
        txtEditParcela.text = "0";
        txtParcTotal.text = "0";
        objEditParcela.SetActive(false);

        //-------------------//

        txtEditValor.text = "";

        //--------------------//
        btnConfirmEdit.onClick.RemoveAllListeners();
        btnEditSai.onClick.RemoveAllListeners();
        btnConfirmEdit.onClick.AddListener(ConfirmInclusao);
        btnEditSai.onClick.AddListener(SairEdicao);
    }

    void ConfirmInclusao()
    {
        string descri = "",ativo = "", parcela = "", parcTot = "";

        if(txtEditNome.text.IndexOf("+") != -1 || txtEditNome.text.IndexOf("*") != -1 || txtEditNome.text.IndexOf("-") != -1 || txtEditNome.text == "")
        {
            txtEditErro.text = "O descrição precisa estar preenchido não pode conter (+), (*) e (-)";
            return;
        }

        if(txtEditDescri.text.IndexOf("+") != -1 || txtEditDescri.text.IndexOf("*") != -1 || txtEditDescri.text.IndexOf("-") != -1)
        {
            txtEditErro.text = "A observações não pode conter (+), (*) e (-)";
            return;
        }

        if(txtEditValor.text.IndexOf("+") != -1 || txtEditValor.text.IndexOf("*") != -1 || txtEditValor.text.IndexOf("-") != -1 || txtEditValor.text.IndexOf(" ") != -1 || txtEditValor.text == "")
        {
            txtEditErro.text = "O valor não pode estar vazio, conter caractere ou espaço!";
            return;
        }

        ativo = "+";

        if(CONFIGMASTER.instance.frequenciaTit[dropCondPG.value] == "Parcelado")
        {
            if(txtEditParcela.text.IndexOf("+") != -1 || txtEditParcela.text.IndexOf("*") != -1 || txtEditParcela.text.IndexOf("-") != -1 || txtEditParcela.text.IndexOf(" ") != -1 || txtEditParcela.text == "" || int.Parse(txtEditParcela.text) < 1)
            {
                txtEditErro.text = "A parcela não pode conter (+), (*), (-), não pode estar vazia e zerado";
                return;
            }
            int x = Int32.Parse(txtEditParcela.text)-1;
            parcela = x.ToString();
            parcTot = txtEditParcela.text;
        }
        else
        {
            parcTot = "X";
            parcela = "X";
        }

        

        descri = txtEditNome.text+"*"+CONFIGMASTER.instance.tipoSaida[dropTipo.value]+"*"+txtEditDescri.text+"*"+ativo+"*"+CONFIGMASTER.instance.frequenciaTit[dropCondPG.value]+"*"+parcela+"*NP*"+parcTot+"*";
//        print("indice "+indiceE+" descri "+descri);
        if(dropCondPG.value == 1 || dropCondPG.value == 2)
        {
            string w = CONFIGMASTER.instance.mesAtivo[0]+"*"+CONFIGMASTER.instance.descriSaida.Count+"*P*"+txtEditValor.text+"*"+txtEditNome.text+"*"+CONFIGMASTER.instance.tipoSaida[dropTipo.value]+"*"+txtEditDescri.text+"*"+CONFIGMASTER.instance.frequenciaTit[dropCondPG.value]+"*"+parcela+"*"+parcTot+"*";
            CONFIGMASTER.instance.tituloRotativo.Add(w);
        }
            
        CONFIGMASTER.instance.descriSaida.Add(descri);
        CONFIGMASTER.instance.itemValorSaidas.Add(float.Parse(txtEditValor.text));

        CONFIGMASTER.instance.SaveTitulo();
        
        CarregaValor();
        CarregaItem();
        SairEdicao();
    }

    public static void Clicou (int y)
    {
        indiceE = y;
        cliqueEdita = true;
    }

    public void Editar()
    {
        editando = true;
        objEditTit.SetActive(true);
        //print("indice e "+indiceE);
        txtEditNome.text = CONFIGMASTER.instance.CapturaTitulo("P",indiceE,1);//Nome
        //--------------------//
        //Forma de recebimento(Tipo)
        List<string> x = new List<string>();
        int w = 0;
        dropTipo.ClearOptions();
        for (int i = 0; i < CONFIGMASTER.instance.tipoSaida.Count; i++)
        {
            x.Add(CONFIGMASTER.instance.tipoSaida[i]);  
            if(CONFIGMASTER.instance.CapturaTitulo("P",indiceE,2) == x[i])
                w=i;
        }
        dropTipo.AddOptions(x);
        dropTipo.value = w;
       //--------------------//
        txtEditDescri.text = CONFIGMASTER.instance.CapturaTitulo("P",indiceE,3);//Descrição

        //--------------------//
        //Condição de pagamento(Frequencia)
        List<string> y = new List<string>();
        int z = 0;
        dropCondPG.ClearOptions();
        for (int i = 0; i < CONFIGMASTER.instance.frequenciaTit.Count; i++)
        {
            y.Add(CONFIGMASTER.instance.frequenciaTit[i]);  
            if(CONFIGMASTER.instance.CapturaTitulo("P",indiceE,5) == y[i])
                z=i;
        }
        dropCondPG.AddOptions(y);
        dropCondPG.value = z;

        //--------------------//
        if(z == 2)
        {
            int val = Int32.Parse(CONFIGMASTER.instance.CapturaTitulo("P",indiceE,8)) - Int32.Parse(CONFIGMASTER.instance.CapturaTitulo("P",indiceE,6));
            objEditParcela.SetActive(true);
            objParcTot.SetActive(true);
            txtEditParcela.text = val.ToString();
            txtParcTotal.text = CONFIGMASTER.instance.CapturaTitulo("P",indiceE,8);
        }
        else
        {
            objEditParcela.SetActive(true);
            txtEditParcela.text = "0";
            txtParcTotal.text = "0";
            objEditParcela.SetActive(false);
        }
        
        //--------------------//

        txtEditValor.text = CONFIGMASTER.instance.itemValorSaidas[indiceE].ToString("F2");

        //--------------------//
        btnConfirmEdit.onClick.RemoveAllListeners();
        btnEditSai.onClick.RemoveAllListeners();
        btnConfirmEdit.onClick.AddListener(ConfirmEdicao);
        btnEditSai.onClick.AddListener(SairEdicao);
    }

    void ConfirmEdicao()
    {
        string descri = "",ativo = "", parcela = "", parcTot = "",rotativo = "";
        int indiceRotativo = -1;

        if(txtEditNome.text.IndexOf("+") != -1 || txtEditNome.text.IndexOf("*") != -1 || txtEditNome.text.IndexOf("-") != -1 || txtEditNome.text == "")
        {
            txtEditErro.text = "O descrição não pode estar vazio e conter (+), (*) e (-)";
            return;
        }

        if(txtEditDescri.text.IndexOf("+") != -1 || txtEditDescri.text.IndexOf("*") != -1 || txtEditDescri.text.IndexOf("-") != -1)
        {
            txtEditErro.text = "A Observação não pode conter (+), (*) e (-)";
            return;
        }

        if(txtEditValor.text.IndexOf(" ") != -1 || txtEditValor.text.IndexOf("+") != -1 || txtEditValor.text.IndexOf("*") != -1 || txtEditValor.text.IndexOf("-") != -1 || txtEditValor.text == "")
        {
            txtEditErro.text = "O valor não pode estar vazio, conter caractere ou espaço!";
            return;
        }

        
        //print ("esta ativo "+CONFIGMASTER.instance.CapturaTitulo("R",indiceE,4));
        if(CONFIGMASTER.instance.CapturaTitulo("P",indiceE,4) == "+")
            ativo = "+";
        else
            ativo = "-";
        //print ("valor var ativo "+ativo);

        if(CONFIGMASTER.instance.frequenciaTit[dropCondPG.value] == "Parcelado")
        {
            if(txtEditParcela.text.IndexOf("+") != -1 || txtEditParcela.text.IndexOf("*") != -1 || txtEditParcela.text.IndexOf("-") != -1 || txtEditParcela.text.IndexOf(" ") != -1 || txtEditParcela.text == "" || int.Parse(txtEditParcela.text) < 1)
            {
                txtEditErro.text = "A parcela atual não pode conter (+), (*), (-), não pode conter espaço, estar vazia e zerado";
                return;
            }
            if(txtParcTotal.text.IndexOf("+") != -1 || txtParcTotal.text.IndexOf("*") != -1 || txtParcTotal.text.IndexOf("-") != -1 || txtParcTotal.text.IndexOf(" ") != -1 || txtParcTotal.text == "" || int.Parse(txtParcTotal.text) < 1 || int.Parse(txtEditParcela.text) > int.Parse(txtParcTotal.text))
            {
                txtEditErro.text = "A parcela total não pode conter (+), (*), (-), não pode conter espaço, estar vazia e zerado";
                return;
            }
            int x = Int32.Parse(txtParcTotal.text)-Int32.Parse(txtEditParcela.text);
            parcela = x.ToString();
            parcTot = txtParcTotal.text;
        }
        else
        {
            parcela = "X";
            parcTot = "X";
        }

        if(parcela.IndexOf(" ") != -1 || parcela.IndexOf("+") != -1 || parcela.IndexOf("*") != -1 || parcela.IndexOf("-") != -1 || parcela == "")
        {
            txtEditErro.text = "A parcela não pode conter (+), (*), (-), não pode estar vazia e zerado";
            return;
        }
        if(parcTot.IndexOf(" ") != -1 || parcTot.IndexOf("+") != -1 || parcTot.IndexOf("*") != -1 || parcTot.IndexOf("-") != -1 || parcTot == "" || parcTot == "0")
        {
            txtEditErro.text = "A parcela total não pode conter (+), (*), (-), não pode conter espaço, estar vazia e zerado";
            return;
        }

        if(CONFIGMASTER.instance.frequenciaTit[dropCondPG.value] == "Parcelado" || CONFIGMASTER.instance.frequenciaTit[dropCondPG.value] == "Mensal")
        {
            //print("condiçao "+CONFIGMASTER.instance.frequenciaTit[dropCondPG.value]);
            for (int i = 0; i < CONFIGMASTER.instance.tituloRotativo.Count; i++)
            {
                //print("validar= "+ CONFIGMASTER.instance.CapturaTitulo("F",i,3) +" compara  R" +" e "+ CONFIGMASTER.instance.CapturaTitulo("F",i,2) +" compara "+ indiceE.ToString());
                if (CONFIGMASTER.instance.CapturaTitulo("F",i,3) == "P" && CONFIGMASTER.instance.CapturaTitulo("F",i,2) == indiceE.ToString())
                    indiceRotativo = i;
            }
            //print(CONFIGMASTER.instance.CapturaTitulo("R",indiceE,4)+" indice "+indiceRotativo);
            if(CONFIGMASTER.instance.CapturaTitulo("P",indiceE,4) == "+" && indiceRotativo > -1)
            {
                rotativo = CONFIGMASTER.instance.mesAtivo[0]+"*"+indiceE+"*P*"+txtEditValor.text+"*"+txtEditNome.text+"*"+CONFIGMASTER.instance.tipoSaida[dropTipo.value]+"*"+txtEditDescri.text+"*"+CONFIGMASTER.instance.frequenciaTit[dropCondPG.value]+"*"+parcela+"*"+parcTot+"*";
                CONFIGMASTER.instance.tituloRotativo[indiceRotativo] = rotativo;
            }
        }
        else
        {   //Se não for rotativo 
            for (int i = 0; i < CONFIGMASTER.instance.tituloRotativo.Count; i++)
            {
                //print("validar= "+ CONFIGMASTER.instance.CapturaTitulo("F",i,3) +" compara  R" +" e "+ CONFIGMASTER.instance.CapturaTitulo("F",i,2) +" compara "+ indiceE.ToString());
                if (CONFIGMASTER.instance.CapturaTitulo("F",i,3) == "P" && CONFIGMASTER.instance.CapturaTitulo("F",i,2) == indiceE.ToString())
                {
                    CONFIGMASTER.instance.tituloRotativo.RemoveRange(i,1);

                    CONFIGMASTER.instance.DeleteTitulo(CONFIGMASTER.instance.mesAtivo[0]);
                    CONFIGMASTER.instance.SaveTitulo();
                }
            }
        }

        descri = txtEditNome.text+"*"+CONFIGMASTER.instance.tipoSaida[dropTipo.value]+"*"+txtEditDescri.text+"*"+ativo+"*"+CONFIGMASTER.instance.frequenciaTit[dropCondPG.value]+"*"+parcela+"*NP*"+parcTot+"*";
        
//        print("indice "+indiceE+" descri "+descri);
        CONFIGMASTER.instance.descriSaida[indiceE] = descri;
        
        CONFIGMASTER.instance.itemValorSaidas[indiceE] = float.Parse(txtEditValor.text);

        CONFIGMASTER.instance.SaveTitulo();
        
        CarregaValor();
        CarregaItem();
        SairEdicao();
    }

    

    void SairEdicao()
    {
        editando = false;
        txtEditErro.text = "";
        indiceE = -1;
        objEditTit.SetActive(false);
    }

    void valorTitEntradas()
    {
        float pago = 0f, naoPago = 0f;
        List<string> lista = new List<string>();

        objVlEntradaTipo.SetActive(true);

        for (int i = 0; i < CONFIGMASTER.instance.descriSaida.Count; i++)
        {
            if(CONFIGMASTER.instance.CapturaTitulo("P",i,4) == "+")
            {
                for (int x = 0; x < CONFIGMASTER.instance.tipoSaida.Count; x++)
                {
                    if(i == 0)
                    {
                        lista.Add(CONFIGMASTER.instance.tipoSaida[x]);
                        txtVlEntradaTipo.Add(0);
                    }
                        

                    if(CONFIGMASTER.instance.CapturaTitulo("P",i,2) == CONFIGMASTER.instance.tipoSaida[x])
                        txtVlEntradaTipo[x] += CONFIGMASTER.instance.itemValorSaidas[i];
                }

                if(CONFIGMASTER.instance.CapturaTitulo("P",i,7) == "NP")
                    naoPago += CONFIGMASTER.instance.itemValorSaidas[i];
                else
                    pago += CONFIGMASTER.instance.itemValorSaidas[i];                    
            }
        }
        dropTPEntra.ClearOptions();
        dropTPEntra.AddOptions(lista);

        txtVlPg.text = pago.ToString("C2", CONFIGMASTER.formatoBrasil);
        txtVlNp.text = naoPago.ToString("C2", CONFIGMASTER.formatoBrasil);

        ativoTPEntra = true;

        btnVlEntradaSair.onClick.RemoveAllListeners();
        btnVlEntradaSair.onClick.AddListener(valorTitSai);
    }

    void valorTitSai()
    {
        ativoTPEntra = false;
        objVlEntradaTipo.SetActive(false);
    }

    void IncluiDizimo()
    {
        bool existe = false;
        for (int i = 0; i < CONFIGMASTER.instance.descriSaida.Count; i++)
        {
            if(CONFIGMASTER.instance.CapturaTitulo("P",i,2) == "Dízimo")
            {
                existe = true;
            }
        }

        if(!existe)
        {
            CONFIGMASTER.instance.descriSaida.Add("Dízimo*Dízimo*Décima parte do lucro*+*Mensal*X*NP*");
            CONFIGMASTER.instance.itemValorSaidas.Add(CONFIGMASTER.instance.CalculaDizimo());
            int x = CONFIGMASTER.instance.itemValorSaidas.Count - 1;
            CONFIGMASTER.instance.tituloRotativo.Add(CONFIGMASTER.instance.mesAtivo[0]+"*"+x+"*P*"+CONFIGMASTER.instance.itemValorSaidas[x]+"*Dízimo*Dízimo*Décima parte do lucro*+*Mensal*X*");

            CONFIGMASTER.instance.SaveTitulo();
        
            CarregaValor();
            CarregaItem();
        }

        objDizimo.SetActive(true);
            
        btnSaiDizimo.onClick.RemoveAllListeners();
        btnSaiDizimo.onClick.AddListener(saiDizimo);
        
    }

    void saiDizimo()
    {
        objDizimo.SetActive(false);
    }
}
