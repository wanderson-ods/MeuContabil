using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Configuracao : MonoBehaviour
{
    [Header("Iniciais")]
    public GameObject objConfig;
    public Button[] btnConfig;
    public Text txtTitulo;
    public Button btnVoltar;
    public Button btnSaiObj;
    public GameObject objUsar, objContato;
    [Header("Zera Mês")]
    public GameObject objZeraMes;
    public Button btnZeraSim, btnZeraNao;
    [Header("Novo mês")]
    public GameObject objNewMes;
    public Text txtMesNovo;
    public Button btnMesSim, btnMesNao;
    [Header("Novo usuário")]
    public GameObject objNewUser;
    public InputField inputNomeUser;
    public Text txtErroUser, txtAnoUser;
    public Dropdown dropUserMes;
    public Button btnUserSim, btnUserNao;
    [Header("Excluir usuário")]
    public GameObject objExcluiUser;
    public Button btnDelUserSim, btnDelUserNao;
    [Header("Tipo Entrada")]
    public GameObject objTipEntra;
    public InputField inputEntrada;
    public Text txtEntraErro;
    public Button btnTPEntraSim, btnTPEntraNao;
    [Header("Tipo Saída")]
    public GameObject objTipSaida;
    public InputField inputTPSaida;
    public Text txtSaiErro;
    public Button btnTPSaiSim, btnTPSaiNao;
    [Header("Deleta tudo")]
    public GameObject objDelet;
    public Button btnDeletAllSim, btnDeletAllNao;

    void Start() 
    {
        for (int i = 0; i < btnConfig.Length; i++)
        {
            btnConfig[i].onClick.RemoveAllListeners();
        }

        btnConfig[0].onClick.AddListener(ZeraMes);
        btnConfig[1].onClick.AddListener(NewMes);
        btnConfig[2].onClick.AddListener(NewUser);
        btnConfig[3].onClick.AddListener(DeletUser);
        btnConfig[4].onClick.AddListener(TPSaida);
        btnConfig[5].onClick.AddListener(TPEntrada);
        btnConfig[6].onClick.AddListener(ComoUsar);
        btnConfig[7].onClick.AddListener(Contato);
        btnConfig[8].onClick.AddListener(DeletAll);

        btnVoltar.onClick.RemoveAllListeners();
        btnVoltar.onClick.AddListener(Voltar);
    }

    void Voltar()
    {
        CONFIGMASTER.instance.TrocaPagina(1);
    }

    void Sair()
    {
        objUsar.SetActive(false); 
        objContato.SetActive(false); 
        objZeraMes.SetActive(false);
        objNewMes.SetActive(false);
        objNewUser.SetActive(false); 
        objExcluiUser.SetActive(false);
        objTipEntra.SetActive(false);
        objTipSaida.SetActive(false);
        objDelet.SetActive(false);
        txtTitulo.text="";
        objConfig.SetActive(false);
    }

    void ZeraMes()
    {
        objConfig.SetActive(true);
        objZeraMes.SetActive(true); 
        btnSaiObj.onClick.RemoveAllListeners();
        btnSaiObj.onClick.AddListener(Sair);

        txtTitulo.text="Zerar titulo do mês ativo";

        btnZeraSim.onClick.RemoveAllListeners();
        btnZeraNao.onClick.RemoveAllListeners();
        btnZeraSim.onClick.AddListener(ZerouMes);
        btnZeraNao.onClick.AddListener(Sair);
    }

    void ZerouMes()
    {
        CONFIGMASTER.instance.DeleteTitulo(CONFIGMASTER.instance.mesAtivo[0]);
        

        CONFIGMASTER.instance.itemValorEntrada[0] = 0;
        CONFIGMASTER.instance.itemValorEntrada[1] = 0;
        CONFIGMASTER.instance.itemValorSaidas[0] = 0;
        CONFIGMASTER.instance.startRecebPag();

        CONFIGMASTER.instance.ValorMensal();
        CONFIGMASTER.instance.TrocaPagina(0);
    }

    void NewMes()
    {
        objConfig.SetActive(true);
        objNewMes.SetActive(true); 
        btnSaiObj.onClick.RemoveAllListeners();
        btnSaiObj.onClick.AddListener(Sair);

        txtTitulo.text="Criar mês subsequente";

        int ultimaData = CONFIGMASTER.instance.indiceOrdMes[CONFIGMASTER.instance.indiceOrdMes.Count - 1];
        int mes = Int32.Parse(CONFIGMASTER.instance.dataContabil[ultimaData].Substring(0,2));
        int ano = Int32.Parse(CONFIGMASTER.instance.dataContabil[ultimaData].Substring(3,4));
        string mesStr = "",newData = "";
    
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

        newData = mesStr+"/"+ano;
        txtMesNovo.text = newData;
        
        btnMesSim.onClick.RemoveAllListeners();
        btnMesNao.onClick.RemoveAllListeners();
        btnMesSim.onClick.AddListener(SetaData);
        btnMesNao.onClick.AddListener(Sair);
    }

    void SetaData()
    {
        CONFIGMASTER.instance.CriarData(txtMesNovo.text);
        Sair();
    }

    void NewUser()
    {
        int x = -1;
        objConfig.SetActive(true);
        btnSaiObj.onClick.RemoveAllListeners();
        btnSaiObj.onClick.AddListener(Sair);
        txtErroUser.text = "";
        txtAnoUser.text = "";
        inputNomeUser.text = "";
        
        for (int i = 0; i < CONFIGMASTER.instance.contasAtivas.Length; i++)
        {
            if(CONFIGMASTER.instance.contasAtivas[i] == "x")
                x = i;
        }
        if(x != -1)
        {
            txtTitulo.text="Criar novo usuário";
            objNewUser.SetActive(true); 
            btnUserSim.onClick.RemoveAllListeners();
            btnUserNao.onClick.RemoveAllListeners();
            btnUserSim.onClick.AddListener(UsuarioCheck);
            btnUserNao.onClick.AddListener(Sair);
        }
        else
        {
            txtTitulo.text="Maximo de contas de usuário criadas";
        }
        
    }

    void UsuarioCheck()
    {
        int x = -1;

        for (int i = 0; i < CONFIGMASTER.instance.contasAtivas.Length; i++)
        {
            if(CONFIGMASTER.instance.contasAtivas[i] == "x" && x == -1)
            {
                x = i;
                break;
            }
                
        }
        //print("a "+caixaSelecao.value);
        if(inputNomeUser.text == "" || inputNomeUser.text.Length < 3)
        {
            txtErroUser.text = "Nome está vazio ou muito curto!";
        }
        else if (txtAnoUser.text.Length != 4 || Int32.Parse(txtAnoUser.text) < 1000 || Int32.Parse(txtAnoUser.text) > 9999 )
        {
            txtErroUser.text = "Ano precisa ter quatro dígitos!";
        }
        else if(dropUserMes.value < 1)
        {
            txtErroUser.text = "Selecione um mês valido!";
        }
        else
        {
            string a = "";
            int b = dropUserMes.value + 2;
            if(b < 10)
                a = "0"+b;
            else
                a = b.ToString();
            
            //print(a+"/"+txtAno.text);

            CONFIGMASTER.instance.dataContabil.Add(a+"/"+txtAnoUser.text);
           // print("indice user "+x);
            CONFIGMASTER.instance.contasAtivas[x] = inputNomeUser.text;
            CONFIGMASTER.instance.SaveConta();
            //print("ant acess conta");
            CONFIGMASTER.instance.AcessarConta(x);
            CONFIGMASTER.instance.TrocaPagina(0);
        }
    }

    void DeletUser()
    {
        objConfig.SetActive(true);
        objExcluiUser.SetActive(true); 
        btnSaiObj.onClick.RemoveAllListeners();
        btnSaiObj.onClick.AddListener(Sair);

        txtTitulo.text="Deletar usuário ativo";

        btnDelUserSim.onClick.RemoveAllListeners();
        btnDelUserNao.onClick.RemoveAllListeners();
        btnDelUserSim.onClick.AddListener(DeletouUser);
        btnDelUserNao.onClick.AddListener(Sair);

    }

    void DeletouUser()
    {
        CONFIGMASTER.instance.DeleteConta();        
        CONFIGMASTER.instance.TrocaPagina(0);
    }

    void TPSaida()
    {
        objConfig.SetActive(true);
        objTipSaida.SetActive(true); 
        btnSaiObj.onClick.RemoveAllListeners();
        btnSaiObj.onClick.AddListener(Sair);

        txtSaiErro.text = "";
        inputTPSaida.text = "";
        txtTitulo.text="Cadastro de tipo de saída";

        btnTPSaiSim.onClick.RemoveAllListeners();
        btnTPSaiNao.onClick.RemoveAllListeners();
        btnTPSaiSim.onClick.AddListener(TpSaidaCadastrado);
        btnTPSaiNao.onClick.AddListener(Sair);
    }

    void TpSaidaCadastrado()
    {
        if(inputTPSaida.text == "+" || inputTPSaida.text == "-" || inputTPSaida.text == "*")
        {
            txtSaiErro.text = "Tipo de saída não pode conter (+),(-) e (*)";
            return;
        }
        for (int i = 0; i < CONFIGMASTER.instance.tipoSaida.Count; i++)
        {
            if(CONFIGMASTER.instance.tipoSaida[i] == inputTPSaida.text)
            {
                txtSaiErro.text = "Tipo de saída já cadastrado";
                return;
            }
        }

        CONFIGMASTER.instance.tipoSaida.Add(inputTPSaida.text);
        CONFIGMASTER.instance.SaveConta();
        Sair();
    }

    void TPEntrada()
    {
        objConfig.SetActive(true);
        objTipEntra.SetActive(true); 
        btnSaiObj.onClick.RemoveAllListeners();
        btnSaiObj.onClick.AddListener(Sair);

        inputEntrada.text = "";
        txtEntraErro.text = "";
        txtTitulo.text="Cadastro de tipo de entrada";

        btnTPEntraSim.onClick.RemoveAllListeners();
        btnTPEntraNao.onClick.RemoveAllListeners();
        btnTPEntraSim.onClick.AddListener(TpEntradaCadastrado);
        btnTPEntraNao.onClick.AddListener(Sair);
    }

    void TpEntradaCadastrado()
    {
        if(inputEntrada.text == "+" || inputEntrada.text == "-" || inputEntrada.text == "*")
        {
            txtEntraErro.text = "Tipo de saída não pode conter (+),(-) e (*)";
            return;
        }
        for (int i = 0; i < CONFIGMASTER.instance.tipoEntrada.Count; i++)
        {
            if(CONFIGMASTER.instance.tipoEntrada[i] == inputEntrada.text)
            {
                txtEntraErro.text = "Tipo de saída já cadastrado";
                return;
            }
        }
        CONFIGMASTER.instance.tipoEntrada.Add(inputEntrada.text);
        CONFIGMASTER.instance.SaveConta();
        Sair();
    }

    void ComoUsar()
    {
        objConfig.SetActive(true);
        objUsar.SetActive(true); 
        btnSaiObj.onClick.RemoveAllListeners();
        btnSaiObj.onClick.AddListener(Sair);

        txtTitulo.text="Informações de uso";
    }

    void Contato()
    {
        objConfig.SetActive(true);
        objContato.SetActive(true); 
        btnSaiObj.onClick.RemoveAllListeners();
        btnSaiObj.onClick.AddListener(Sair);

        txtTitulo.text="Dados para contato";
    }

    void DeletAll()
    {
        objConfig.SetActive(true);
        objDelet.SetActive(true); 
        btnSaiObj.onClick.RemoveAllListeners();
        btnSaiObj.onClick.AddListener(Sair);

        txtTitulo.text="Excluir todos os dados salvos";

        btnDeletAllSim.onClick.RemoveAllListeners();
        btnDeletAllNao.onClick.RemoveAllListeners();
        btnDeletAllSim.onClick.AddListener(ExcluiTudo);
        btnDeletAllNao.onClick.AddListener(Sair);

    }

    void ExcluiTudo()
    {
        CONFIGMASTER.instance.DeletaAllSave();
        CONFIGMASTER.instance.TrocaPagina(0);
    }
}
