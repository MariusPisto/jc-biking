
package org.tempuri;

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
 *         <element name="OriginLat" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/>
 *         <element name="OriginLng" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/>
 *         <element name="DestLat" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/>
 *         <element name="DestLng" type="{http://www.w3.org/2001/XMLSchema}double" minOccurs="0"/>
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
    "originLat",
    "originLng",
    "destLat",
    "destLng"
})
@XmlRootElement(name = "Itinerary")
public class Itinerary {

    @XmlElement(name = "OriginLat")
    protected Double originLat;
    @XmlElement(name = "OriginLng")
    protected Double originLng;
    @XmlElement(name = "DestLat")
    protected Double destLat;
    @XmlElement(name = "DestLng")
    protected Double destLng;

    /**
     * Gets the value of the originLat property.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getOriginLat() {
        return originLat;
    }

    /**
     * Sets the value of the originLat property.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setOriginLat(Double value) {
        this.originLat = value;
    }

    /**
     * Gets the value of the originLng property.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getOriginLng() {
        return originLng;
    }

    /**
     * Sets the value of the originLng property.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setOriginLng(Double value) {
        this.originLng = value;
    }

    /**
     * Gets the value of the destLat property.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getDestLat() {
        return destLat;
    }

    /**
     * Sets the value of the destLat property.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setDestLat(Double value) {
        this.destLat = value;
    }

    /**
     * Gets the value of the destLng property.
     * 
     * @return
     *     possible object is
     *     {@link Double }
     *     
     */
    public Double getDestLng() {
        return destLng;
    }

    /**
     * Sets the value of the destLng property.
     * 
     * @param value
     *     allowed object is
     *     {@link Double }
     *     
     */
    public void setDestLng(Double value) {
        this.destLng = value;
    }

}
