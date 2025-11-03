using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Proxy
{
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
        string response;

        [DataMember]
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        [DataMember]
        public string Response
        {
            get { return response; }
            set { response = value; }
        }
    }
}
