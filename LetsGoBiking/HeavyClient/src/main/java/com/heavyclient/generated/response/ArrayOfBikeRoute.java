
package com.heavyclient.generated.response;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlType;

import java.util.ArrayList;
import java.util.List;


/**
 * <p>Java class for ArrayOfBikeRoute complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>{@code
 * <complexType name="ArrayOfBikeRoute">
 *   <complexContent>
 *     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       <sequence>
 *         <element name="BikeRoute" type="{http://schemas.datacontract.org/2004/07/Server.Entities.Response}BikeRoute" maxOccurs="unbounded" minOccurs="0"/>
 *       </sequence>
 *     </restriction>
 *   </complexContent>
 * </complexType>
 * }</pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "ArrayOfBikeRoute", propOrder = {
    "bikeRoute"
})
public class ArrayOfBikeRoute {

    @XmlElement(name = "BikeRoute", nillable = true)
    protected List<BikeRoute> bikeRoute;

    /**
     * Gets the value of the bikeRoute property.
     * 
     * <p>
     * This accessor method returns a reference to the live list,
     * not a snapshot. Therefore any modification you make to the
     * returned list will be present inside the Jakarta XML Binding object.
     * This is why there is not a {@code set} method for the bikeRoute property.
     * 
     * <p>
     * For example, to add a new item, do as follows:
     * <pre>
     *    getBikeRoute().add(newItem);
     * </pre>
     * 
     * 
     * <p>
     * Objects of the following type(s) are allowed in the list
     * {@link BikeRoute }
     * 
     * 
     * @return
     *     The value of the bikeRoute property.
     */
    public List<BikeRoute> getBikeRoute() {
        if (bikeRoute == null) {
            bikeRoute = new ArrayList<>();
        }
        return this.bikeRoute;
    }

}
