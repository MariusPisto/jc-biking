
package com.microsoft.schemas._2003._10.serialization.arrays;

import jakarta.xml.bind.annotation.XmlAccessType;
import jakarta.xml.bind.annotation.XmlAccessorType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlType;

import java.util.ArrayList;
import java.util.List;


/**
 * <p>Java class for ArrayOfArrayOfdouble complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>{@code
 * <complexType name="ArrayOfArrayOfdouble">
 *   <complexContent>
 *     <restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       <sequence>
 *         <element name="ArrayOfdouble" type="{http://schemas.microsoft.com/2003/10/Serialization/Arrays}ArrayOfdouble" maxOccurs="unbounded" minOccurs="0"/>
 *       </sequence>
 *     </restriction>
 *   </complexContent>
 * </complexType>
 * }</pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "ArrayOfArrayOfdouble", propOrder = {
    "arrayOfdouble"
})
public class ArrayOfArrayOfdouble {

    @XmlElement(name = "ArrayOfdouble", nillable = true)
    protected List<ArrayOfdouble> arrayOfdouble;

    /**
     * Gets the value of the arrayOfdouble property.
     * 
     * <p>
     * This accessor method returns a reference to the live list,
     * not a snapshot. Therefore any modification you make to the
     * returned list will be present inside the Jakarta XML Binding object.
     * This is why there is not a {@code set} method for the arrayOfdouble property.
     * 
     * <p>
     * For example, to add a new item, do as follows:
     * <pre>
     *    getArrayOfdouble().add(newItem);
     * </pre>
     * 
     * 
     * <p>
     * Objects of the following type(s) are allowed in the list
     * {@link ArrayOfdouble }
     * 
     * 
     * @return
     *     The value of the arrayOfdouble property.
     */
    public List<ArrayOfdouble> getArrayOfdouble() {
        if (arrayOfdouble == null) {
            arrayOfdouble = new ArrayList<>();
        }
        return this.arrayOfdouble;
    }

}
