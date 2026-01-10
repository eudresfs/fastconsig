using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Web.Security;

[ServiceContract(Namespace = "")]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class AverbacaoService
{

    [OperationContract]
    public string SalvarAverbacao(int mes, int dia, int idEvento, int idCalendario)
    {
        return null; // ControleCalendario.RegistraEvento(mes, dia, idEvento, idCalendario);
    }

}

