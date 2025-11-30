
package com.heavyclient.generated.ors;

import com.microsoft.schemas._2003._10.serialization.arrays.ArrayOfdouble;
import jakarta.xml.bind.JAXBElement;
import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElementRef;
import jakarta.xml.bind.annotation.XmlType;


/**
 * <p>Java class for RouteFeature complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>{@code
 * <complexType name="RouteFeature">
 *   <complexContent>
 *     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       <sequence>
 *         <element name="bbox" type="{http://schemas.microsoft.com/2003/10/Serialization/Arrays}ArrayOfdouble" minOccurs="0"/>
 *         <element name="geometry" type="{http://schemas.datacontract.org/2004/07/Server.Entities.ORS}Geometry" minOccurs="0"/>
 *         <element name="properties" type="{http://schemas.datacontract.org/2004/07/Server.Entities.ORS}Properties" minOccurs="0"/>
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
@XmlType(name = "RouteFeature", propOrder = {
    "bbox",
    "geometry",
    "properties",
    "type"
})
public class RouteFeature {

    @XmlElementRef(name = "bbox", namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", type = JAXBElement.class, required = false)
    protected JAXBElement<ArrayOfdouble> bbox;
    @XmlElementRef(name = "geometry", namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", type = JAXBElement.class, required = false)
    protected JAXBElement<Geometry> geometry;
    @XmlElementRef(name = "properties", namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", type = JAXBElement.class, required = false)
    protected JAXBElement<Properties> properties;
    @XmlElementRef(name = "type", namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", type = JAXBElement.class, required = false)
    protected JAXBElement<String> type;

    /**
     * Gets the value of the bbox property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfdouble }{@code >}
     *     
     */
    public JAXBElement<ArrayOfdouble> getBbox() {
        return bbox;
    }

    /**
     * Sets the value of the bbox property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfdouble }{@code >}
     *     
     */
    public void setBbox(JAXBElement<ArrayOfdouble> value) {
        this.bbox = value;
    }

    /**
     * Gets the value of the geometry property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link Geometry }{@code >}
     *     
     */
    public JAXBElement<Geometry> getGeometry() {
        return geometry;
    }

    /**
     * Sets the value of the geometry property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link Geometry }{@code >}
     *     
     */
    public void setGeometry(JAXBElement<Geometry> value) {
        this.geometry = value;
    }

    /**
     * Gets the value of the properties property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link Properties }{@code >}
     *     
     */
    public JAXBElement<Properties> getProperties() {
        return properties;
    }

    /**
     * Sets the value of the properties property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link Properties }{@code >}
     *     
     */
    public void setProperties(JAXBElement<Properties> value) {
        this.properties = value;
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
