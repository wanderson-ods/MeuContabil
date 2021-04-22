using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class Investido : MonoBehaviour
{
    public static Investido instace;
    public Text txtEntrada;
    public Button btnCriaItem;
    public Button btnVolta;
    public Transform posicaoIncluirItem;
    public RectTransform tamanhoTelaItem;
    public GameObject objItem;
    private List<GameObject> objItensTela = new List<GameObject>();
    public static int indice = 0;
    private static int indiceE = -1;
    public static bool cliqueEdita = false;

    //Edição de titulo
    public Text txtEditErro;
    public InputField txtEditNome,txtEditDescri,txtEditLocal, txtEditValor;
    public GameObject objEditTit;
    public Button btnEditSai, btnConfirmEdit;

    void Start()
    {
        btnCriaItem.onClick.RemoveAllListeners();
        btnVolta.onClick.RemoveAllListeners();

        btnCriaItem.onClick.AddListener(CriarItem);
        btnVolta.onClick.AddListener(Voltar);

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

        if (cliqueEdita)
        {
            cliqueEdita = false;
            Editar();
        }
    }
    void Voltar ()
    {
        indice = 0;
        CONFIGMASTER.instance.TrocaPagina(1);
    }

    void CarregaValor()
    {

        CONFIGMASTER.instance.ValorMensal();

        float val = CONFIGMASTER.instance.valorInvestido;

        txtEntrada.text = val.ToString("C2", CONFIGMASTER.formatoBrasil);
    }

    void CarregaItem()
    {

        if(indice > 0)
        {
            for (int i = 0; i < indice; i++)
            {
                Destroy (objItensTela[i]);
            }

            objItensTela.Clear();
            indice = 0;
        }

        for (int i = 0; i < CONFIGMASTER.instance.descriInvest.Count; i++)
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

       //--------------------//
        txtEditDescri.text = "";//Descrição
        //--------------------//
        
        //--------------------//
        txtEditValor.text = "0,00";
        //--------------------//

        btnConfirmEdit.onClick.RemoveAllListeners();
        btnEditSai.onClick.RemoveAllListeners();
        btnConfirmEdit.onClick.AddListener(ConfirmInclusao);
        btnEditSai.onClick.AddListener(SairEdicao);
    }

    void ConfirmInclusao()
    {
        string descri = "",ativo = "";

        if(txtEditNome.text.IndexOf("+") != -1 || txtEditNome.text.IndexOf("*") != -1 || txtEditNome.text.IndexOf("-") != -1 || txtEditNome.text == "")
        {
            txtEditErro.text = "O nome não pode conter (+), (*) e (-)";
            return;
        }

        if(txtEditDescri.text.IndexOf("+") != -1 || txtEditDescri.text.IndexOf("*") != -1 || txtEditDescri.text.IndexOf("-") != -1 )
        {
            txtEditErro.text = "A descrição não pode conter (+), (*) e (-)";
            return;
        }//

        if(txtEditLocal.text.IndexOf("+") != -1 || txtEditLocal.text.IndexOf("*") != -1 || txtEditLocal.text.IndexOf("-") != -1 || txtEditLocal.text == "")
        {
            txtEditErro.text = "O Local investido não pode estar vazio e conter (+), (*) e (-)";
            return;
        }

        if(txtEditValor.text.IndexOf("+") != -1 || txtEditValor.text.IndexOf("*") != -1 || txtEditValor.text.IndexOf("-") != -1 || txtEditValor.text == "")
        {
            txtEditErro.text = "O valor não pode estar vazio";
            return;
        }

        ativo = "+";

        descri = txtEditNome.text+"*"+txtEditLocal.text+"*"+txtEditDescri.text+"*"+ativo+"*";
            
        CONFIGMASTER.instance.descriInvest.Add(descri);
        CONFIGMASTER.instance.itemValorInvest.Add(float.Parse(txtEditValor.text));

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

        objEditTit.SetActive(true);
        //print("indice e "+indiceE);
        txtEditNome.text = CONFIGMASTER.instance.CapturaTitulo("I",indiceE,1);//Nome
        //--------------------//
        //Local de Investimento
        txtEditLocal.text = CONFIGMASTER.instance.CapturaTitulo("I",indiceE,2);//Local
       //--------------------//
       //Descrição
        txtEditDescri.text = CONFIGMASTER.instance.CapturaTitulo("I",indiceE,3);//Descrição
        //--------------------//
        //Valor
        txtEditValor.text = CONFIGMASTER.instance.itemValorInvest[indiceE].ToString("F2");
        //--------------------//
        btnConfirmEdit.onClick.RemoveAllListeners();
        btnEditSai.onClick.RemoveAllListeners();
        btnConfirmEdit.onClick.AddListener(ConfirmEdicao);
        btnEditSai.onClick.AddListener(SairEdicao);
    }

    void ConfirmEdicao()
    {
        string descri = "",ativo = "";

        if(txtEditNome.text.IndexOf("+") != -1 || txtEditNome.text.IndexOf("*") != -1 || txtEditNome.text.IndexOf("-") != -1 || txtEditNome.text == "")
        {
            txtEditErro.text = "Qual investimento não pode estar vazio e conter (+), (*) e (-)";
            return;
        }

        if(txtEditDescri.text.IndexOf("+") != -1 || txtEditDescri.text.IndexOf("*") != -1 || txtEditDescri.text.IndexOf("-") != -1)
        {
            txtEditErro.text = "A descrição não pode conter (+), (*) e (-)";
            return;
        }

        if(txtEditLocal.text.IndexOf("+") != -1 || txtEditLocal.text.IndexOf("*") != -1 || txtEditLocal.text.IndexOf("-") != -1 || txtEditLocal.text == "")
        {
            txtEditErro.text = "O Local investido não pode estar vazio e conter (+), (*) e (-)";
            return;
        }

        if(txtEditValor.text.IndexOf(" ") != -1 || txtEditValor.text.IndexOf("+") != -1 || txtEditValor.text.IndexOf("*") != -1 || txtEditValor.text.IndexOf("-") != -1 || txtEditValor.text == "")
        {
            txtEditErro.text = "O valor não pode estar vazio, conter caractere ou espaço!";
            return;
        }

        if(CONFIGMASTER.instance.CapturaTitulo("I",indiceE,4) == "+")
            ativo = "+";
        else
            ativo = "-";

        descri = txtEditNome.text+"*"+txtEditLocal.text+"*"+txtEditDescri.text+"*"+ativo+"*";
            
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
        txtEditErro.text = "";
        indiceE = -1;
        objEditTit.SetActive(false);
    }
    
}
