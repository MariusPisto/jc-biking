
package com.heavyclient.generated.response;

import jakarta.xml.bind.JAXBElement;
import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElementRef;
import jakarta.xml.bind.annotation.XmlType;


/**
 * <p>Java class for BikeRoute complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>{@code
 * <complexType name="BikeRoute">
 *   <complexContent>
 *     <extension base="{http://schemas.datacontract.org/2004/07/Server.Entities.Response}Route">
 *       <sequence>
 *         <element name="addressEnd" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         <element name="addressStart" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         <element name="availableBikes" type="{http://www.w3.org/2001/XMLSchema}int" minOccurs="0"/>
 *         <element name="availableDropPlace" type="{http://www.w3.org/2001/XMLSchema}int" minOccurs="0"/>
 *       </sequence>
 *     </extension>
 *   </complexContent>
 * </complexType>
 * }</pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "BikeRoute", propOrder = {
    "addressEnd",
    "addressStart",
    "availableBikes",
    "availableDropPlace"
})
public class BikeRoute
    extends Route
{

    @XmlElementRef(name = "addressEnd", namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", type = JAXBElement.class, required = false)
    protected JAXBElement<String> addressEnd;
    @XmlElementRef(name = "addressStart", namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", type = JAXBElement.class, required = false)
    protected JAXBElement<String> addressStart;
    protected Integer availableBikes;
    protected Integer availableDropPlace;

    /**
     * Gets the value of the addressEnd property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getAddressEnd() {
        return addressEnd;
    }

    /**
     * Sets the value of the addressEnd property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setAddressEnd(JAXBElement<String> value) {
        this.addressEnd = value;
    }

    /**
     * Gets the value of the addressStart property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getAddressStart() {
        return addressStart;
    }

    /**
     * Sets the value of the addressStart property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setAddressStart(JAXBElement<String> value) {
        this.addressStart = value;
    }

    /**
     * Gets the value of the availableBikes property.
     * 
     * @return
     *     possible object is
     *     {@link Integer }
     *     
     */
    public Integer getAvailableBikes() {
        return availableBikes;
    }

    /**
     * Sets the value of the availableBikes property.
     * 
     * @param value
     *     allowed object is
     *     {@link Integer }
     *     
     */
    public void setAvailableBikes(Integer value) {
        this.availableBikes = value;
    }

    /**
     * Gets the value of the availableDropPlace property.
     * 
     * @return
     *     possible object is
     *     {@link Integer }
     *     
     */
    public Integer getAvailableDropPlace() {
        return availableDropPlace;
    }

    /**
     * Sets the value of the availableDropPlace property.
     * 
     * @param value
     *     allowed object is
     *     {@link Integer }
     *     
     */
    public void setAvailableDropPlace(Integer value) {
        this.availableDropPlace = value;
    }

}
