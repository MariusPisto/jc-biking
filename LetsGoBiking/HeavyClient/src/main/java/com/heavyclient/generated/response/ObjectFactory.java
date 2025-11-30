
package com.heavyclient.generated.response;

import com.heavyclient.generated.ors.RouteFeature;
import jakarta.xml.bind.JAXBElement;
import jakarta.xml.bind.annotation.XmlElementDecl;
import jakarta.xml.bind.annotation.XmlRegistry;
import org.datacontract.schemas._2004._07.server.Location;

import javax.xml.namespace.QName;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the com.heavyclient.generated.response package. 
 * <p>An ObjectFactory allows you to programatically 
 * construct new instances of the Java representation 
 * for XML content. The Java representation of XML 
 * content can consist of schema derived interfaces 
 * and classes representing the binding of schema 
 * type definitions, element declarations and model 
 * groups.  Factory methods for each of these are 
 * provided in this class.
 * 
 */
@XmlRegistry
public class ObjectFactory {

    private static final QName _ItineraryResponse_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "ItineraryResponse");
    private static final QName _ArrayOfBikeRoute_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "ArrayOfBikeRoute");
    private static final QName _BikeRoute_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "BikeRoute");
    private static final QName _Route_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "Route");
    private static final QName _ArrayOfRoute_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "ArrayOfRoute");
    private static final QName _ArrayOfAddress_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "ArrayOfAddress");
    private static final QName _Address_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "Address");
    private static final QName _AddressLabel_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "Label");
    private static final QName _RouteEnd_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "end");
    private static final QName _RouteFeature_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "feature");
    private static final QName _RouteStart_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "start");
    private static final QName _RouteType_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "type");
    private static final QName _BikeRouteAddressEnd_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "addressEnd");
    private static final QName _BikeRouteAddressStart_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "addressStart");
    private static final QName _ItineraryResponseBikeRoutes_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "bikeRoutes");
    private static final QName _ItineraryResponseWalkRoutes_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.Response", "walkRoutes");

    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: com.heavyclient.generated.response
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link ItineraryResponse }
     * 
     * @return
     *     the new instance of {@link ItineraryResponse }
     */
    public ItineraryResponse createItineraryResponse() {
        return new ItineraryResponse();
    }

    /**
     * Create an instance of {@link ArrayOfAddress }
     * 
     * @return
     *     the new instance of {@link ArrayOfAddress }
     */
    public ArrayOfAddress createArrayOfAddress() {
        return new ArrayOfAddress();
    }

    /**
     * Create an instance of {@link ArrayOfBikeRoute }
     * 
     * @return
     *     the new instance of {@link ArrayOfBikeRoute }
     */
    public ArrayOfBikeRoute createArrayOfBikeRoute() {
        return new ArrayOfBikeRoute();
    }

    /**
     * Create an instance of {@link BikeRoute }
     * 
     * @return
     *     the new instance of {@link BikeRoute }
     */
    public BikeRoute createBikeRoute() {
        return new BikeRoute();
    }

    /**
     * Create an instance of {@link Route }
     * 
     * @return
     *     the new instance of {@link Route }
     */
    public Route createRoute() {
        return new Route();
    }

    /**
     * Create an instance of {@link ArrayOfRoute }
     * 
     * @return
     *     the new instance of {@link ArrayOfRoute }
     */
    public ArrayOfRoute createArrayOfRoute() {
        return new ArrayOfRoute();
    }

    /**
     * Create an instance of {@link Address }
     * 
     * @return
     *     the new instance of {@link Address }
     */
    public Address createAddress() {
        return new Address();
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ItineraryResponse }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ItineraryResponse }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "ItineraryResponse")
    public JAXBElement<ItineraryResponse> createItineraryResponse(ItineraryResponse value) {
        return new JAXBElement<>(_ItineraryResponse_QNAME, ItineraryResponse.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfBikeRoute }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfBikeRoute }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "ArrayOfBikeRoute")
    public JAXBElement<ArrayOfBikeRoute> createArrayOfBikeRoute(ArrayOfBikeRoute value) {
        return new JAXBElement<>(_ArrayOfBikeRoute_QNAME, ArrayOfBikeRoute.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link BikeRoute }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link BikeRoute }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "BikeRoute")
    public JAXBElement<BikeRoute> createBikeRoute(BikeRoute value) {
        return new JAXBElement<>(_BikeRoute_QNAME, BikeRoute.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Route }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Route }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "Route")
    public JAXBElement<Route> createRoute(Route value) {
        return new JAXBElement<>(_Route_QNAME, Route.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfRoute }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfRoute }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "ArrayOfRoute")
    public JAXBElement<ArrayOfRoute> createArrayOfRoute(ArrayOfRoute value) {
        return new JAXBElement<>(_ArrayOfRoute_QNAME, ArrayOfRoute.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfAddress }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfAddress }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "ArrayOfAddress")
    public JAXBElement<ArrayOfAddress> createArrayOfAddress(ArrayOfAddress value) {
        return new JAXBElement<>(_ArrayOfAddress_QNAME, ArrayOfAddress.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Address }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Address }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "Address")
    public JAXBElement<Address> createAddress(Address value) {
        return new JAXBElement<>(_Address_QNAME, Address.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "Label", scope = Address.class)
    public JAXBElement<String> createAddressLabel(String value) {
        return new JAXBElement<>(_AddressLabel_QNAME, String.class, Address.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Location }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Location }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "end", scope = Route.class)
    public JAXBElement<Location> createRouteEnd(Location value) {
        return new JAXBElement<>(_RouteEnd_QNAME, Location.class, Route.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link RouteFeature }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link RouteFeature }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "feature", scope = Route.class)
    public JAXBElement<RouteFeature> createRouteFeature(RouteFeature value) {
        return new JAXBElement<>(_RouteFeature_QNAME, RouteFeature.class, Route.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Location }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Location }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "start", scope = Route.class)
    public JAXBElement<Location> createRouteStart(Location value) {
        return new JAXBElement<>(_RouteStart_QNAME, Location.class, Route.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "type", scope = Route.class)
    public JAXBElement<String> createRouteType(String value) {
        return new JAXBElement<>(_RouteType_QNAME, String.class, Route.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "addressEnd", scope = BikeRoute.class)
    public JAXBElement<String> createBikeRouteAddressEnd(String value) {
        return new JAXBElement<>(_BikeRouteAddressEnd_QNAME, String.class, BikeRoute.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "addressStart", scope = BikeRoute.class)
    public JAXBElement<String> createBikeRouteAddressStart(String value) {
        return new JAXBElement<>(_BikeRouteAddressStart_QNAME, String.class, BikeRoute.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfBikeRoute }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfBikeRoute }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "bikeRoutes", scope = ItineraryResponse.class)
    public JAXBElement<ArrayOfBikeRoute> createItineraryResponseBikeRoutes(ArrayOfBikeRoute value) {
        return new JAXBElement<>(_ItineraryResponseBikeRoutes_QNAME, ArrayOfBikeRoute.class, ItineraryResponse.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfRoute }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfRoute }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", name = "walkRoutes", scope = ItineraryResponse.class)
    public JAXBElement<ArrayOfRoute> createItineraryResponseWalkRoutes(ArrayOfRoute value) {
        return new JAXBElement<>(_ItineraryResponseWalkRoutes_QNAME, ArrayOfRoute.class, ItineraryResponse.class, value);
    }

}
