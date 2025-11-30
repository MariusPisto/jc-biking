
package org.tempuri;

import jakarta.xml.bind.JAXBElement;
import jakarta.xml.bind.annotation.*;


/**
 * <p>Java class for anonymous complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>{@code
 * <complexType>
 *   <complexContent>
 *     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       <sequence>
 *         <element name="ItineraryResult" type="{http://schemas.datacontract.org/2004/07/Server.Entities.Response}ItineraryResponse" minOccurs="0"/>
 *       </sequence>
 *     </restriction>
 *   </complexContent>
 * </complexType>
 * }</pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = {
    "itineraryResult"
})
@XmlRootElement(name = "ItineraryResponse")
public class ItineraryResponse {

    @XmlElementRef(name = "ItineraryResult", namespace = "http://tempuri.org/", type = JAXBElement.class, required = false)
    protected JAXBElement<com.heavyclient.generated.response.ItineraryResponse> itineraryResult;

    /**
     * Gets the value of the itineraryResult property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link com.heavyclient.generated.response.ItineraryResponse }{@code >}
     *     
     */
    public JAXBElement<com.heavyclient.generated.response.ItineraryResponse> getItineraryResult() {
        return itineraryResult;
    }

    /**
     * Sets the value of the itineraryResult property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link com.heavyclient.generated.response.ItineraryResponse }{@code >}
     *     
     */
    public void setItineraryResult(JAXBElement<com.heavyclient.generated.response.ItineraryResponse> value) {
        this.itineraryResult = value;
    }

}
