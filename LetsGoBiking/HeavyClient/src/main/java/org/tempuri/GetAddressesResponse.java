
package org.tempuri;

import com.heavyclient.generated.response.ArrayOfAddress;
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
 *         <element name="GetAddressesResult" type="{http://schemas.datacontract.org/2004/07/Server.Entities.Response}ArrayOfAddress" minOccurs="0"/>
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
    "getAddressesResult"
})
@XmlRootElement(name = "GetAddressesResponse")
public class GetAddressesResponse {

    @XmlElementRef(name = "GetAddressesResult", namespace = "http://tempuri.org/", type = JAXBElement.class, required = false)
    protected JAXBElement<ArrayOfAddress> getAddressesResult;

    /**
     * Gets the value of the getAddressesResult property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfAddress }{@code >}
     *     
     */
    public JAXBElement<ArrayOfAddress> getGetAddressesResult() {
        return getAddressesResult;
    }

    /**
     * Sets the value of the getAddressesResult property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfAddress }{@code >}
     *     
     */
    public void setGetAddressesResult(JAXBElement<ArrayOfAddress> value) {
        this.getAddressesResult = value;
    }

}
