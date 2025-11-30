
package com.heavyclient.generated.response;

import com.heavyclient.generated.ors.RouteFeature;
import jakarta.xml.bind.JAXBElement;
import jakarta.xml.bind.annotation.*;
import org.datacontract.schemas._2004._07.server.Location;


/**
 * <p>Java class for Route complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>{@code
 * <complexType name="Route">
 *   <complexContent>
 *     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       <sequence>
 *         <element name="end" type="{http://schemas.datacontract.org/2004/07/Server.Entities}Location" minOccurs="0"/>
 *         <element name="feature" type="{http://schemas.datacontract.org/2004/07/Server.Entities.ORS}RouteFeature" minOccurs="0"/>
 *         <element name="start" type="{http://schemas.datacontract.org/2004/07/Server.Entities}Location" minOccurs="0"/>
 *         <element name="type" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *       </sequence>
 *     </restriction>
 *   </complexContent>
 * </complexType>
 * }</pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "Route", propOrder = {
    "end",
    "feature",
    "start",
    "type"
})
@XmlSeeAlso({
    BikeRoute.class
})
public class Route {

    @XmlElementRef(name = "end", namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", type = JAXBElement.class, required = false)
    protected JAXBElement<Location> end;
    @XmlElementRef(name = "feature", namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", type = JAXBElement.class, required = false)
    protected JAXBElement<RouteFeature> feature;
    @XmlElementRef(name = "start", namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", type = JAXBElement.class, required = false)
    protected JAXBElement<Location> start;
    @XmlElementRef(name = "type", namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.Response", type = JAXBElement.class, required = false)
    protected JAXBElement<String> type;

    /**
     * Gets the value of the end property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link Location }{@code >}
     *     
     */
    public JAXBElement<Location> getEnd() {
        return end;
    }

    /**
     * Sets the value of the end property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link Location }{@code >}
     *     
     */
    public void setEnd(JAXBElement<Location> value) {
        this.end = value;
    }

    /**
     * Gets the value of the feature property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link RouteFeature }{@code >}
     *     
     */
    public JAXBElement<RouteFeature> getFeature() {
        return feature;
    }

    /**
     * Sets the value of the feature property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link RouteFeature }{@code >}
     *     
     */
    public void setFeature(JAXBElement<RouteFeature> value) {
        this.feature = value;
    }

    /**
     * Gets the value of the start property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link Location }{@code >}
     *     
     */
    public JAXBElement<Location> getStart() {
        return start;
    }

    /**
     * Sets the value of the start property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link Location }{@code >}
     *     
     */
    public void setStart(JAXBElement<Location> value) {
        this.start = value;
    }

    /**
     * Gets the value of the type property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getType() {
        return type;
    }

    /**
     * Sets the value of the type property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setType(JAXBElement<String> value) {
        this.type = value;
    }

}
