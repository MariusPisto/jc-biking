
package com.heavyclient.generated.response;

import jakarta.xml.bind.JAXBElement;
import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElementRef;
import jakarta.xml.bind.annotation.XmlType;


/**
 * <p>Java class for ItineraryResponse complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>{@code
 * <complexType name="ItineraryResponse">
 *   <complexContent>
 *     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       <sequence>
 *         <element name="bikeRoutes" type="{http://schemas.datacontract.org/2004/07/Server.Entities.Response}ArrayOfBikeRoute" minOccurs="0"/>
 *         <element name="walkRoutes" type="{http://schemas.datacontract.org/2004/07/Server.Entities.Response}ArrayOfRoute" minOccurs="0"/>
 *       </sequence>
 *     </restriction>
 *   </complexContent>
 * </complexType>
 * }</pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "ItineraryResponse", propOrder = {
    "bikeRoutes",
    "walkRoutes"
})
public class ItineraryResponse {

    @XmlElementRef(name = "bikeRoutes", namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", type = JAXBElement.class, required = false)
    protected JAXBElement<ArrayOfBikeRoute> bikeRoutes;
    @XmlElementRef(name = "walkRoutes", namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", type = JAXBElement.class, required = false)
    protected JAXBElement<ArrayOfRoute> walkRoutes;

    /**
     * Gets the value of the bikeRoutes property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfBikeRoute }{@code >}
     *     
     */
    public JAXBElement<ArrayOfBikeRoute> getBikeRoutes() {
        return bikeRoutes;
    }

    /**
     * Sets the value of the bikeRoutes property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfBikeRoute }{@code >}
     *     
     */
    public void setBikeRoutes(JAXBElement<ArrayOfBikeRoute> value) {
        this.bikeRoutes = value;
    }

    /**
     * Gets the value of the walkRoutes property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfRoute }{@code >}
     *     
     */
    public JAXBElement<ArrayOfRoute> getWalkRoutes() {
        return walkRoutes;
    }

    /**
     * Sets the value of the walkRoutes property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfRoute }{@code >}
     *     
     */
    public void setWalkRoutes(JAXBElement<ArrayOfRoute> value) {
        this.walkRoutes = value;
    }

}
