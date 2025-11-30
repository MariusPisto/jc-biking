
package org.tempuri;

import com.heavyclient.generated.response.ArrayOfAddress;
import jakarta.xml.bind.JAXBElement;
import jakarta.xml.bind.annotation.XmlElementDecl;
import jakarta.xml.bind.annotation.XmlRegistry;

import javax.xml.namespace.QName;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the org.tempuri package. 
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

    private static final QName _ItineraryResponseItineraryResult_QNAME = new QName("http://tempuri.org/", "ItineraryResult");
    private static final QName _GetAddressesText_QNAME = new QName("http://tempuri.org/", "text");
    private static final QName _GetAddressesResponseGetAddressesResult_QNAME = new QName("http://tempuri.org/", "GetAddressesResult");

    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: org.tempuri
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link GetOptions }
     * 
     * @return
     *     the new instance of {@link GetOptions }
     */
    public GetOptions createGetOptions() {
        return new GetOptions();
    }

    /**
     * Create an instance of {@link GetOptionsResponse }
     * 
     * @return
     *     the new instance of {@link GetOptionsResponse }
     */
    public GetOptionsResponse createGetOptionsResponse() {
        return new GetOptionsResponse();
    }

    /**
     * Create an instance of {@link Itinerary }
     * 
     * @return
     *     the new instance of {@link Itinerary }
     */
    public Itinerary createItinerary() {
        return new Itinerary();
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
     * Create an instance of {@link GetAddresses }
     * 
     * @return
     *     the new instance of {@link GetAddresses }
     */
    public GetAddresses createGetAddresses() {
        return new GetAddresses();
    }

    /**
     * Create an instance of {@link GetAddressesResponse }
     * 
     * @return
     *     the new instance of {@link GetAddressesResponse }
     */
    public GetAddressesResponse createGetAddressesResponse() {
        return new GetAddressesResponse();
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link com.heavyclient.generated.response.ItineraryResponse }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link com.heavyclient.generated.response.ItineraryResponse }{@code >}
     */
    @XmlElementDecl(namespace = "http://tempuri.org/", name = "ItineraryResult", scope = ItineraryResponse.class)
    public JAXBElement<com.heavyclient.generated.response.ItineraryResponse> createItineraryResponseItineraryResult(com.heavyclient.generated.response.ItineraryResponse value) {
        return new JAXBElement<>(_ItineraryResponseItineraryResult_QNAME, com.heavyclient.generated.response.ItineraryResponse.class, ItineraryResponse.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     */
    @XmlElementDecl(namespace = "http://tempuri.org/", name = "text", scope = GetAddresses.class)
    public JAXBElement<String> createGetAddressesText(String value) {
        return new JAXBElement<>(_GetAddressesText_QNAME, String.class, GetAddresses.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfAddress }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfAddress }{@code >}
     */
    @XmlElementDecl(namespace = "http://tempuri.org/", name = "GetAddressesResult", scope = GetAddressesResponse.class)
    public JAXBElement<ArrayOfAddress> createGetAddressesResponseGetAddressesResult(ArrayOfAddress value) {
        return new JAXBElement<>(_GetAddressesResponseGetAddressesResult_QNAME, ArrayOfAddress.class, GetAddressesResponse.class, value);
    }

}
