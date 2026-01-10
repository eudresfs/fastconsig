using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CP.FastConsig.Services
{
    
    [ServiceContract]
    public interface IServicoUsuario
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        List<Consignataria> ListarConsignatarias();

        [OperationContract]
        List<Consignante> ListarConsignantes();

        [OperationContract]
        bool ExisteUsuario(string CPF);

        [OperationContract]
        bool ExcluirUsuarioPorCpf(string cpf, int? idCriador, string tipoCriador);

        [OperationContract]
        Usuario ObtemUsuarioPorLogin(string login);

        [OperationContract]
        Usuario ObtemUsuarioPorCpf(string cpf);

        [OperationContract]
        void AtualizaUsuario(string cpf, string nome, string email, string telefone, string login, string senha, int idConsignataria, string novoCpf, int idConsignante, int? idCriador, string tipoCriador);

        [OperationContract]
        Consignante ObtemConsignante(int idConsignante);

        [OperationContract]
        Consignataria ObtemConsignataria(int idConsignataria);

        [OperationContract]
        bool IncluirUsuario(int idConsignataria, int idConsignante, string cpf, string nome, string email, string fone, string login, string senha, int? idCriador, string tipoCriador);

        [OperationContract]
        bool AlterarUsuario(string cpf, string nome, string email, string fone, string login, string senha, string novoCpf, int? idCriador, string tipoCriador);

        [OperationContract]
        List<Consignante> ConsignantesDoUsuario(string cpf);

        [OperationContract]
        List<Consignante> ConsignantesDaConsignataria(int idConsignataria);

        [OperationContract]
        Consignataria ConsignatariaDoUsuario(string CPF);

        [OperationContract]
        bool AlterarSenhaUsuario(string cpf, string novaSenha, int? idCriador, string tipoCriador);

    }

    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }

    }

}