using Server.Entities.Response;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Server
{
    [ServiceContract]
    public interface IServerService
    {
        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "*")]
        void GetOptions();

        [OperationContract]
        [WebGet(UriTemplate = "itinerary?originLat={OriginLat}&originLng={OriginLng}&destLat={DestLat}&destLng={DestLng}",
            ResponseFormat = WebMessageFormat.Json)]
        Task<ItineraryResponse> ItineraryAsync(double OriginLat, double OriginLng, double DestLat, double DestLng);
    }
}
