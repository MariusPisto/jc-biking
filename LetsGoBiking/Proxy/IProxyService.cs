using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Proxy
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IProxyService
    {
        [OperationContract]
        APIResponse Call(String url);
    }

    [DataContract]
    public class APIResponse
    {
        int status;
        string reponse;

        [DataMember]
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        [DataMember]
        public string Reponse
        {
            get { return reponse; }
            set { reponse = value; }
        }
    }
}
