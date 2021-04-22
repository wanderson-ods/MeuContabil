using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Globalization;

public class CONFIGMASTER : MonoBehaviour
{
    public static CONFIGMASTER instance;
    public static NumberFormatInfo formatoBrasil = new CultureInfo("pt-BR", false).NumberFormat;

    public static int indiceCena;
    public string[] contasAtivas = new string[3];//Valida contas cadastradas
    public List<string> dataContabil = new List<string>();//Datas que fora criadas com para conta
    public List<int> indiceOrdMes = new List<int>();
    public int entrouConta;
    public float valorInvestido;
    public List<string> frequenciaTit = new List<string>();
    public List<string> tipoEntrada = new List<string>();
    public List<string> tipoSaida = new List<string>();
    public List<string> descriInvest = new List<string>();
    public List<float> itemValorInvest = new List<float>();
    public List<string> tituloRotativo = new List<string>();

    //Calculos do Mês
    public List<float> valorEntradas = new List<float>();
    public List<float> valorSaidas = new List<float>();
    public List<string> descriEntrada = new List<string>();
    public List<string> descriSaida = new List<string>();
    public List<float> itemValorEntrada = new List<float>();
    public List<float> itemValorSaidas = new List<float>();
    //public List<float> valorSando = new List<float>();
    public List<string> mesAtivo;//new string[2]; //Posição 1 o a data ativa 2 
    
    private bool sai = false;

    //Transição de cena
    public GameObject objCarrega;
    public Transform posiCarrega;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad (this.gameObject);
        }else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += Carrega;
    }
    void Carrega (Scene cena, LoadSceneMode modo)
    {
        indiceCena = SceneManager.GetActiveScene().buildIndex;

        //DeletaAllSave();

        if(indiceCena == 0)
        {
            LimpaListMes();
            LimpaListContas();
            LimpaListUser();
            StartSaveConfig();
        }
 
    }

    public void Bye()
    {
        sai = true;
    }
    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            DeletaAllSave();
        }

        if(indiceCena == 1)
        {
            if (Input.GetKey("escape"))
            {
                TrocaPagina(0);
            }
        }
        else if(indiceCena > 1)
        {
            if (Input.GetKey("escape"))
            {
                TrocaPagina(1);
            }
        }
        else if(indiceCena == 0)
        {
            if (Input.GetKey("escape") || sai)
            {
                FecharApp();
            }
        }
    }

    public void FecharApp()
    {
        Application.Quit();
    }

        //Startar Salvar Player ///////////////////////////////////////////////////////////////////////////////// 
    public void StartSaveConfig()
    {
        
        if(PlayerPrefs.HasKey("contasAtivas0"))
        {//print("achou");
            for(int i=0;i<contasAtivas.Length;i++)
            {
                contasAtivas[i] = PlayerPrefs.GetString("contasAtivas"+i);
            }             
        }
        else
        {//print("não achou");
            for(int i=0;i<contasAtivas.Length;i++)
            {
                contasAtivas[i] = "x";
                PlayerPrefs.SetString("contasAtivas"+i, contasAtivas[i]);
            } 
        }
        ////////////////////////////////////frequenciaTit
        if(PlayerPrefs.HasKey("frequenciaTit"+0))
        {
            frequenciaTit.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("frequenciaTit"+i))
            {
                frequenciaTit.Add(PlayerPrefs.GetString("frequenciaTit"+i));
                i++;
            }                        
        }
        else
        {
            frequenciaTit.Add("Único");
            frequenciaTit.Add("Mensal");
            frequenciaTit.Add("Parcelado");
            
            for (int i = 0; i < frequenciaTit.Count; i++)
            {
                PlayerPrefs.SetString("frequenciaTit"+i, frequenciaTit[i]);
            }
        }
        ////////////////////////////////////tipoEntrada
        if(PlayerPrefs.HasKey("tipoEntrada"+0))
        {
            tipoEntrada.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("tipoEntrada"+i))
            {
                tipoEntrada.Add(PlayerPrefs.GetString("tipoEntrada"+i));
                i++;
            }                        
        }
        else
        {
            tipoEntrada.Add("Banco");
            tipoEntrada.Add("Dinheiro");
            tipoEntrada.Add("Carteira digital");
            tipoEntrada.Add("Retorno");
            tipoEntrada.Add("Outros");
            for (int i = 0; i < tipoEntrada.Count; i++)
            {
                PlayerPrefs.SetString("tipoEntrada"+i, tipoEntrada[i]);
            }
            
        }
        ////////////////////////////////////tipoSaida
        if(PlayerPrefs.HasKey("tipoSaida"+0))
        {
            tipoSaida.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("tipoSaida"+i))
            {
                tipoSaida.Add(PlayerPrefs.GetString("tipoSaida"+i));
                i++;
            }                        
        }
        else
        {
            tipoSaida.Add("Cartão");
            tipoSaida.Add("Boleto");
            tipoSaida.Add("Pessoa física");
            tipoSaida.Add("Banco");
            tipoSaida.Add("Dízimo");
            tipoSaida.Add("Outros");
            for (int i = 0; i < tipoSaida.Count; i++)
            {
                PlayerPrefs.SetString("tipoSaida"+i, tipoSaida[i]);
            }
            
        }
    }
//____________________________________//___________________________________________//
    void StartData()
    {
        //print("veio data");
        if(PlayerPrefs.HasKey("dataContabil"+entrouConta+0))
        {
            dataContabil.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("dataContabil"+entrouConta+i))
            {
                dataContabil.Add(PlayerPrefs.GetString("dataContabil"+entrouConta+i));
                i++;
            }                        
        }
        else
        {
            PlayerPrefs.SetString("dataContabil"+entrouConta+0,dataContabil[0]);
        }
        //print("foi ordena "+dataContabil[0]);
        OrdenarData();
        //print("saiu ordena data");

        ////////////valorEntradas//////////////////////////////////////////
        if(PlayerPrefs.HasKey("valorEntradas"+entrouConta+0))
        {
            valorEntradas.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("valorEntradas"+entrouConta+i))
            {
                valorEntradas.Add(PlayerPrefs.GetFloat("valorEntradas"+entrouConta+i));
                i++;
            }                        
        }
        else
        {
            valorEntradas.Add(0);
            PlayerPrefs.SetFloat("valorEntradas"+entrouConta+0,valorEntradas[0]);
        }

        /////////////////////tituloRotativo/////////////////////////
        if(PlayerPrefs.HasKey("tituloRotativo"+entrouConta+0))
        {
            tituloRotativo.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("tituloRotativo"+entrouConta+i))
            {
                tituloRotativo.Add(PlayerPrefs.GetString("tituloRotativo"+entrouConta+i));
                i++;
            }                        
        }
        else
        {
            tituloRotativo.Add(mesAtivo[0]+"*1*R*0*Slario*Banco*Salario deste Mês*Mensal*X*X*");
            PlayerPrefs.SetString("tituloRotativo"+entrouConta+0,tituloRotativo[0]);
        }
        
         ////////////valorSaidas//////////////////////////////////////////
        if(PlayerPrefs.HasKey("valorSaidas"+entrouConta+0))
        {
            valorSaidas.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("valorSaidas"+entrouConta+i))
            {
                valorSaidas.Add(PlayerPrefs.GetFloat("valorSaidas"+entrouConta+i));
                i++;
            }                        
        }
        else
        {
            valorSaidas.Add(0);
            PlayerPrefs.SetFloat("valorSaidas"+entrouConta+0,valorSaidas[0]);
        }
        ////////////////////////////////////descriInvest
        if(PlayerPrefs.HasKey("descriInvest"+entrouConta+0))
        {
            descriInvest.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("descriInvest"+entrouConta+i))
            {
                descriInvest.Add(PlayerPrefs.GetString("descriInvest"+entrouConta+i));
                i++;
            }                        
        }
        else
        {
            descriInvest.Add("Poupança*Banco Exemplo*Valor rendendo desde 2000*+*");
            PlayerPrefs.SetString("descriInvest"+entrouConta+0,descriInvest[0]);
        }

        ////////////////////////////////////itemValorInvest
        if(PlayerPrefs.HasKey("itemValorInvest"+entrouConta+0))
        {
            itemValorInvest.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("itemValorInvest"+entrouConta+i))
            {
                itemValorInvest.Add(PlayerPrefs.GetFloat("itemValorInvest"+entrouConta+i));
                i++;
            }                        
        }
        else
        {
            itemValorInvest.Add(0);
            PlayerPrefs.SetFloat("itemValorInvest"+entrouConta+0, itemValorInvest[0]);
        }
    }
//________________________________//__________________________________________//
    public void startRecebPag()
    {
        ////////////////////////////////////descriEntrada
        //print("receb pag");
        if(PlayerPrefs.HasKey("descriEntrada"+entrouConta+mesAtivo[0]+0))
        {
            descriEntrada.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("descriEntrada"+entrouConta+mesAtivo[0]+i))
            {
                descriEntrada.Add(PlayerPrefs.GetString("descriEntrada"+entrouConta+mesAtivo[0]+i));
                i++;
            }                        
        }
        else
        {
            descriEntrada.Add("Mês Anterior*Outros*Valor que sobrou do mês anterior*+*Mensal*X*X*");
            descriEntrada.Add("Salario*Banco*Salario deste Mês*+*Mensal*X*X*");
            PlayerPrefs.SetString("descriEntrada"+entrouConta+mesAtivo[0]+0,descriEntrada[0]);
            PlayerPrefs.SetString("descriEntrada"+entrouConta+mesAtivo[0]+1,descriEntrada[1]);
        } 

        ////////////////////////////////////descriSaida

        if(PlayerPrefs.HasKey("descriSaida"+entrouConta+mesAtivo[0]+0))
        {
            descriSaida.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("descriSaida"+entrouConta+mesAtivo[0]+i))
            {
                descriSaida.Add(PlayerPrefs.GetString("descriSaida"+entrouConta+mesAtivo[0]+i));
                i++;
            }                        
        }
        else
        {
            descriSaida.Add("Mês Anterior*Outros*Valor que faltou do mês anterior*+*Mensal*X*NP*X*");
            PlayerPrefs.SetString("descriSaida"+entrouConta+mesAtivo[0]+0,descriSaida[0]);
        }

        ////////////////////////////////////itemValorEntrada

        if(PlayerPrefs.HasKey("itemValorEntrada"+entrouConta+mesAtivo[0]+0))
        {
            itemValorEntrada.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("itemValorEntrada"+entrouConta+mesAtivo[0]+i))
            {
                itemValorEntrada.Add(PlayerPrefs.GetFloat("itemValorEntrada"+entrouConta+mesAtivo[0]+i));
                i++;
            }                        
        }
        else
        {
            itemValorEntrada.Add(0);
            itemValorEntrada.Add(0);
            PlayerPrefs.SetFloat("itemValorEntrada"+entrouConta+mesAtivo[0]+0, itemValorEntrada[0]);
            PlayerPrefs.SetFloat("itemValorEntrada"+entrouConta+mesAtivo[0]+1, itemValorEntrada[1]);
        }

        ////////////////////////////////////itemValorSaidas

        if(PlayerPrefs.HasKey("itemValorSaidas"+entrouConta+mesAtivo[0]+0))
        {
            itemValorSaidas.Clear();
            int i = 0;
            while (PlayerPrefs.HasKey("itemValorSaidas"+entrouConta+mesAtivo[0]+i))
            {
                itemValorSaidas.Add(PlayerPrefs.GetFloat("itemValorSaidas"+entrouConta+mesAtivo[0]+i));
                i++;
            }                        
        }
        else
        {
            itemValorSaidas.Add(0);
            PlayerPrefs.SetFloat("itemValorSaidas"+entrouConta+mesAtivo[0]+0, itemValorSaidas[0]);
        }
    }

    public void SaveConta()
    {
        for(int i=0;i<contasAtivas.Length;i++)
        {
            PlayerPrefs.SetString("contasAtivas"+i, contasAtivas[i]);
            //print("conta "+i+" "+contasAtivas[i]);
        } 
        //Tipos de saida-----------------------------------------------------
        for (int i = 0; i < tipoSaida.Count; i++)
        {
            PlayerPrefs.SetString("tipoSaida"+i, tipoSaida[i]);
        }
        //Tipos de entrada---------------------------------------------------
        for (int i = 0; i < tipoEntrada.Count; i++)
        {
            PlayerPrefs.SetString("tipoEntrada"+i, tipoEntrada[i]);
        }
        //Tipos de Condição de PG/REC (Frequencia)---------------------------
        for (int i = 0; i < frequenciaTit.Count; i++)
        {
            PlayerPrefs.SetString("frequenciaTit"+i, frequenciaTit[i]);
        }
            
    }
    public void SaveData()
    {
        for(int i=0;i<dataContabil.Count;i++)
        {
            PlayerPrefs.SetString("dataContabil"+entrouConta+i,dataContabil[i]);
        } 
    }

    public void SaveTitulo()
    {
        for(int i=0;i < valorEntradas.Count;i++)
        {
            PlayerPrefs.SetFloat("valorEntradas"+entrouConta+i,valorEntradas[i]);
        }
        //valorSaidas-------------------------------------------------
        for(int i=0;i < valorSaidas.Count;i++)
        {
            PlayerPrefs.SetFloat("valorSaidas"+entrouConta+i,valorSaidas[i]);
        }
        //descriInvest-------------------------------------------------
        for(int i=0;i < descriInvest.Count;i++)
        {
            PlayerPrefs.SetString("descriInvest"+entrouConta+i,descriInvest[i]);
        }
        //itemValorInvest-------------------------------------------------
        for(int i=0;i < itemValorInvest.Count;i++)
        {
            PlayerPrefs.SetFloat("itemValorInvest"+entrouConta+i, itemValorInvest[i]);
        }
        //tituloRotativo-------------------------------------------------
        for(int i=0;i < tituloRotativo.Count;i++)
        {
            PlayerPrefs.SetString("tituloRotativo"+entrouConta+i,tituloRotativo[i]);
        }
        //descriEntrada-------------------------------------------------
        for(int i=0;i < descriEntrada.Count;i++)
        {
            PlayerPrefs.SetString("descriEntrada"+entrouConta+mesAtivo[0]+i,descriEntrada[i]);
        }
        //descriSaida-------------------------------------------------
        for(int i=0;i < descriSaida.Count;i++)
        {
            PlayerPrefs.SetString("descriSaida"+entrouConta+mesAtivo[0]+i,descriSaida[i]);
        }
        //itemValorEntrada-------------------------------------------------
        for(int i=0;i < itemValorEntrada.Count;i++)
        {
            PlayerPrefs.SetFloat("itemValorEntrada"+entrouConta+mesAtivo[0]+i, itemValorEntrada[i]);
        }
        //itemValorSaidas-------------------------------------------------
        for(int i=0;i < itemValorSaidas.Count;i++)
        {
            PlayerPrefs.SetFloat("itemValorSaidas"+entrouConta+mesAtivo[0]+i, itemValorSaidas[i]);
        }
    }

    //deletar titulos de um mês especifico
     public void DeleteTitulo(string mes)
    {
        
        //descriEntrada-------------------------------------------------
        int i = 0;
        while (PlayerPrefs.HasKey("descriEntrada"+entrouConta+mes+i))
        {
            PlayerPrefs.DeleteKey("descriEntrada"+entrouConta+mes+i);
            i++;
        }
        //descriSaida-------------------------------------------------
        i = 0;
        while (PlayerPrefs.HasKey("descriSaida"+entrouConta+mes+i))
        {
            PlayerPrefs.DeleteKey("descriSaida"+entrouConta+mes+i);
            i++;
        }
        //itemValorEntrada-------------------------------------------------
        i = 0;
        while (PlayerPrefs.HasKey("itemValorEntrada"+entrouConta+mes+i))
        {
            PlayerPrefs.DeleteKey("itemValorEntrada"+entrouConta+mes+i);
            i++;
        }
        //itemValorSaidas-------------------------------------------------
        i = 0;
        while (PlayerPrefs.HasKey("itemValorSaidas"+entrouConta+mes+i))
        {
            PlayerPrefs.DeleteKey("itemValorSaidas"+entrouConta+mes+i);
            i++;
        }
        
        //tituloRotativo-------------------------------------------------
        i = 0;
        while (PlayerPrefs.HasKey("tituloRotativo"+entrouConta+i))
        {
            PlayerPrefs.DeleteKey("tituloRotativo"+entrouConta+i);
            i++;
        }
    }

    //Deleta conta ativa
    public void DeleteConta()
    {
        PlayerPrefs.DeleteKey("contasAtivas"+entrouConta);

        for (int i = 0; i < dataContabil.Count; i++)
        {
            DeleteMes(i);
        }
        contasAtivas[entrouConta] = "x";
        contasAtivas[entrouConta] = PlayerPrefs.GetString("contasAtivas"+entrouConta);
    }

    //Deletar mes especifico
    public void DeleteMes(int indiceMes)
    {
        DeleteTitulo(dataContabil[indiceMes]);

        //valorEntradas----------------------------------------------------
        PlayerPrefs.DeleteKey("valorEntradas"+entrouConta+indiceMes);
        
        //valorSaidas-------------------------------------------------
        PlayerPrefs.DeleteKey("valorSaidas"+entrouConta+indiceMes);
          
        //descriInvest-------------------------------------------------
        PlayerPrefs.DeleteKey("descriInvest"+entrouConta+indiceMes);
        
        //itemValorInvest-------------------------------------------------
        PlayerPrefs.DeleteKey("itemValorInvest"+entrouConta+indiceMes);
        
        //dataContabil------------------------------------------------------
        PlayerPrefs.DeleteKey("dataContabil"+entrouConta+indiceMes);
    }

    //Recebe a data para criar nova data
    public void CriarData(string Data)
    {
        int ultimaData = indiceOrdMes[indiceOrdMes.Count - 1];
        List<string> titRecebe = new List<string>();
        float ultimasaldo;

        //validar se data não existe
        for (int i = 0; i < dataContabil.Count; i++)
        {
            if(dataContabil[i] == Data)
                return;
        }

        //Carregar novo Mês e deixar ele setado
        dataContabil.Add(Data);
        OrdenarData();
        SaveData();
        int z = dataContabil.Count - 1;
        mesAtivo[0] = dataContabil[z];
        mesAtivo[1] = z.ToString();
        
        //Limpa lista de Mês
        LimpaListMes();

        //Criando titulo com valor sobra Mês passado
        ultimasaldo = valorEntradas[ultimaData] - valorSaidas[ultimaData];
        if(ultimasaldo > 0)
        {
            descriEntrada.Add("Mês Anterior*Outros*Valor que sobrou do mês anterior*+*Mensal*X*X*");
            itemValorEntrada.Add(ultimasaldo);

            descriSaida.Add("Mês Anterior*Outros*Valor que faltou do mês anterior*+*Mensal*X*NP*X*");
            itemValorSaidas.Add(0);
        }
        else if(ultimasaldo < 0)
        {
            descriEntrada.Add("Mês Anterior*Outros*Valor que sobrou do mês anterior*+*Mensal*X*X*");
            itemValorEntrada.Add(0);

            descriSaida.Add("Mês Anterior*Outros*Valor que faltou do mês anterior*+*Mensal*X*NP*X*");
            itemValorSaidas.Add(ultimasaldo * -1);
        }
        else
        {
            descriEntrada.Add("Mês Anterior*Outros*Valor que sobrou do mês anterior*+*Mensal*X*X*");
            itemValorEntrada.Add(0);

            descriSaida.Add("Mês Anterior*Outros*Valor que faltou do mês anterior*+*Mensal*X*NP*X*");
            itemValorSaidas.Add(0);
        }

        //Limpa parcela que finalizou do rotativo
        if(tituloRotativo.Count > 1)
        {
            string[] copy = new string[tituloRotativo.Count];
            int parcela = 0;

            for (int i = 0; i < tituloRotativo.Count; i++)
            {
                if(CapturaTitulo("F",i,9) == "0")
                    copy[i] = "X";
                else if(CapturaTitulo("F",i,8) == "Parcelado")
                {
                    parcela = Int32.Parse(CapturaTitulo("F",i,9));
                    parcela--;
                    copy[i] = CapturaTitulo("F",i,1)+"*"+CapturaTitulo("F",i,2)+"*"+CapturaTitulo("F",i,3)+"*"+CapturaTitulo("F",i,4)+"*"+CapturaTitulo("F",i,5)+"*"+CapturaTitulo("F",i,6)+"*"+CapturaTitulo("F",i,7)+"*"+CapturaTitulo("F",i,8)+"*"+parcela+"*"+CapturaTitulo("F",i,10)+"*";
                }
                else
                    copy[i] = tituloRotativo[i];
                    
            }
            tituloRotativo.Clear();
            for (int i = 0; i < copy.Length; i++)
            {
                if(copy[i] != "X")
                    tituloRotativo.Add(copy[i]);
            }
        }

        //Cria titulos validando se vai continuar este mês
        for (int i = 0; i < tituloRotativo.Count; i++)
        {
            string tit = CapturaTitulo("F",i,8);
            string titTip = CapturaTitulo("F",i,3);
            float valor = float.Parse(CapturaTitulo("F",i,4));
           
            if(tit == "Parcelado")
            {
                int qtd = Int32.Parse(CapturaTitulo("F",i,9));

                if(titTip == "R")
                {
                    descriEntrada.Add(CapturaTitulo("F",i,5)+"*"+CapturaTitulo("F",i,6)+"*"+CapturaTitulo("F",i,7)+"*+*Parcelado*"+qtd+"*"+CapturaTitulo("F",i,10)+"*");
                    itemValorEntrada.Add(valor);
                }
                else if(titTip == "P")
                {
                    descriSaida.Add(CapturaTitulo("F",i,5)+"*"+CapturaTitulo("F",i,6)+"*"+CapturaTitulo("F",i,7)+"*+*Parcelado*"+qtd+"*NP*"+CapturaTitulo("F",i,10)+"*");
                    itemValorSaidas.Add(valor);
                }
            }
            else if("Mensal" == tit)
            {
                if(titTip == "R")
                {
                    descriEntrada.Add(CapturaTitulo("F",i,5)+"*"+CapturaTitulo("F",i,6)+"*"+CapturaTitulo("F",i,7)+"*+*Mensal*X*X*");
                    itemValorEntrada.Add(valor);
                }
                else if(titTip == "P")
                {
                    descriSaida.Add(CapturaTitulo("F",i,5)+"*"+CapturaTitulo("F",i,6)+"*"+CapturaTitulo("F",i,7)+"*+*Mensal*X*NP*X*");
                    itemValorSaidas.Add(valor);
                }
            }
            
        }

        float vlEntra = 0, vlSai = 0;
        for (int i = 0; i < itemValorEntrada.Count; i++)
        {
            vlEntra += itemValorEntrada[i];
        }
        for (int i = 0; i < itemValorSaidas.Count; i++)
        {
            vlSai += itemValorSaidas[i];
        }
        valorEntradas.Add(vlEntra);
        valorSaidas.Add(vlSai);

        SaveTitulo();

    }

    //Zerar o list com informações do mês em questão
    void LimpaListMes()
    {
        descriEntrada.Clear();
        itemValorEntrada.Clear();
        descriSaida.Clear();
        itemValorSaidas.Clear();
    }

    void LimpaListUser()
    {
        valorEntradas.Clear();
        //valorSaidas-------------------------------------------------
        valorSaidas.Clear();
        //descriInvest-------------------------------------------------
        descriInvest.Clear();
        //itemValorInvest-------------------------------------------------
        itemValorInvest.Clear();
        //tituloRotativo-------------------------------------------------
        tituloRotativo.Clear();
        dataContabil.Clear();
        
    }

    void LimpaListContas()
    {
        for(int i=0;i<contasAtivas.Length;i++)
        {
            contasAtivas[i] = "x";
        }             
        ////////////////////////////////////frequenciaTit
        frequenciaTit.Clear();
        ////////////////////////////////////tipoEntrada
       tipoEntrada.Clear();
        ////////////////////////////////////tipoSaida
        tipoSaida.Clear();
    }

    //Captura uma informação do titulo retotna como string
    //qual(P-pagar/R-receber/F-frequencia/I-investimento)---indice informa indice do item-
    //posInfo posição que a informação está na descri info vai de 1(= nome) em diante
    public string CapturaTitulo(string qual, int indice, int posInfo)
    {
        string infoTit = "", descriTit0 = "", descriTit1 = "";
        int posCampos = 0;


        if(qual == "P")
        {
            descriTit0 = descriSaida[indice];
            if(posInfo > 8 || posInfo < 1)
                return null; 
        }
        else if(qual == "R")
        {//print("indice "+indice);
            descriTit0 = descriEntrada[indice];
            if(posInfo > 7 || posInfo < 1)
                return null; 
        }
        else if(qual == "F")
        {
            descriTit0 = tituloRotativo[indice];
            if(posInfo > 10 || posInfo < 1)
                return null; 
        }
        else if(qual == "I")
        {
            descriTit0 = descriInvest[indice];
            if(posInfo > 4 || posInfo < 1)
                return null; 
        }

        for (int i = 0; i < posInfo; i++)
        {
            descriTit1 = descriTit0;
            posCampos = descriTit0.IndexOf("*");
            infoTit = descriTit0.Substring(0,posCampos);
            posCampos++;
            descriTit0 = descriTit1.Substring(posCampos);
        }

        return infoTit;
    }

    //Ordem o indice menor fica com data mais antiga e maior com data mais atual
    public void OrdenarData()
    {
        if (mesAtivo.Count != 2)
        {
            mesAtivo.Add("");
            mesAtivo.Add("");
        }

        int[] mes = new int[dataContabil.Count ];
        int[] ano = new int[dataContabil.Count ];
        indiceOrdMes = new List<int>();
        //print("viu param");
        for (int i = 0; i < dataContabil.Count; i++)
        {//print("entra for");
            mes[i] = Int32.Parse(dataContabil[i].Substring(0,2));
            ano[i] = Int32.Parse(dataContabil[i].Substring(3,4));
            indiceOrdMes.Add(i);

            if(i != 0)
            {//print("i maior que zer");
                int x = i;
                while (x == 0)//ir subindo o indice se a data for menor
                {
                    x --;

                    if (ano[indiceOrdMes[x]] > ano[indiceOrdMes[x]+1])
                    {
                        int k = indiceOrdMes[x];
                        indiceOrdMes[x] = indiceOrdMes[x+1];
                        indiceOrdMes[x+1] = k;
                    }
                    else if (ano[indiceOrdMes[x]] == ano[indiceOrdMes[x]+1])
                    {
                        if(mes[indiceOrdMes[x]] > mes[indiceOrdMes[x]+1])
                        {
                            int k = indiceOrdMes[x];
                            indiceOrdMes[x] = indiceOrdMes[x+1];
                            indiceOrdMes[x+1] = k;
                        }
                        else
                            x=0;
                    }
                    else
                        x=0;
                }//print("sai ordena");
            }
        }
        //print("antes mes 0 "+dataContabil[indiceOrdMes[indiceOrdMes.Count-1]]);
        mesAtivo[0] = dataContabil[indiceOrdMes[indiceOrdMes.Count-1]];
        int z = indiceOrdMes.Count - 1;
        //print("antes mes 1");
        mesAtivo[1] = z.ToString();
        //print("apos tudo");
    }

    public void TrocaPagina(int pagina)
    {
        StartCoroutine(Loading(pagina));
    }

    public void AcessarConta(int qualConta)
    {
        entrouConta = qualConta;
        StartData();
        startRecebPag();
        StartCoroutine(Loading(1));
    }

    IEnumerator Loading (int i)
    {
        posiCarrega = GameObject.Find("Main Camera").GetComponent<Transform>();
        System.GC.Collect();
        AsyncOperation async = SceneManager.LoadSceneAsync(i);

        if (!async.isDone)
        {
            Vector3 aux = new Vector3(posiCarrega.position.x,posiCarrega.position.y,0);
            GameObject carregando = Instantiate (objCarrega, aux, Quaternion.identity) as GameObject;
            yield return null;
        }
    }

    //Deleta todos os saves//////////////////////////////////////////////////////////////////////////////////
    public void DeletaAllSave()
    {
        PlayerPrefs.DeleteAll();      
    }


    //////////Saldo do Mês///////
    public void ValorMensal()
    {
        
        int indiceMes = Int32.Parse(mesAtivo[1]); 

        valorEntradas[indiceMes] = 0f;
        valorSaidas[indiceMes] = 0f;
        valorInvestido = 0f;

        for (int i = 0; i < itemValorEntrada.Count; i++)
        {
            if(CapturaTitulo("R",i,4) == "+")
                valorEntradas[indiceMes] += itemValorEntrada[i];
        }

        for (int i = 0; i < itemValorSaidas.Count; i++)
        {
            if(CapturaTitulo("P",i,4) == "+")
                valorSaidas[indiceMes] += itemValorSaidas[i];
        }
        for (int i = 0; i < itemValorInvest.Count; i++)
        {
            if(CapturaTitulo("I",i,4) == "+")
                valorInvestido += itemValorInvest[i];
        }
    }

    public float CalculaDizimo()
    {
        float valor = 0f;

        for (int i = 0; i < itemValorEntrada.Count; i++)
        {
            if(CapturaTitulo("R",i,2) != "Retorno" && CapturaTitulo("R",i,4) != "-" && CapturaTitulo("R",i,1) != "Mês Anterior")
                valor += itemValorEntrada[i];
        }

        valor = (valor*10)/100;

        return valor;
    }
}
