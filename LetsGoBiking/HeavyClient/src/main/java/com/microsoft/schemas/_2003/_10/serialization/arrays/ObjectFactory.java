
package com.microsoft.schemas._2003._10.serialization.arrays;

import jakarta.xml.bind.JAXBElement;
import jakarta.xml.bind.annotation.XmlElementDecl;
import jakarta.xml.bind.annotation.XmlRegistry;

import javax.xml.namespace.QName;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the com.microsoft.schemas._2003._10.serialization.arrays package. 
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

    private static final QName _ArrayOfdouble_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/Arrays", "ArrayOfdouble");
    private static final QName _ArrayOfArrayOfdouble_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/Arrays", "ArrayOfArrayOfdouble");
    private static final QName _ArrayOfint_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/Arrays", "ArrayOfint");

    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: com.microsoft.schemas._2003._10.serialization.arrays
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link ArrayOfdouble }
     * 
     * @return
     *     the new instance of {@link ArrayOfdouble }
     */
    public ArrayOfdouble createArrayOfdouble() {
        return new ArrayOfdouble();
    }

    /**
     * Create an instance of {@link ArrayOfArrayOfdouble }
     * 
     * @return
     *     the new instance of {@link ArrayOfArrayOfdouble }
     */
    public ArrayOfArrayOfdouble createArrayOfArrayOfdouble() {
        return new ArrayOfArrayOfdouble();
    }

    /**
     * Create an instance of {@link ArrayOfint }
     * 
     * @return
     *     the new instance of {@link ArrayOfint }
     */
    public ArrayOfint createArrayOfint() {
        return new ArrayOfint();
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfdouble }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfdouble }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", name = "ArrayOfdouble")
    public JAXBElement<ArrayOfdouble> createArrayOfdouble(ArrayOfdouble value) {
        return new JAXBElement<>(_ArrayOfdouble_QNAME, ArrayOfdouble.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfArrayOfdouble }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfArrayOfdouble }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", name = "ArrayOfArrayOfdouble")
    public JAXBElement<ArrayOfArrayOfdouble> createArrayOfArrayOfdouble(ArrayOfArrayOfdouble value) {
        return new JAXBElement<>(_ArrayOfArrayOfdouble_QNAME, ArrayOfArrayOfdouble.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfint }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfint }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", name = "ArrayOfint")
    public JAXBElement<ArrayOfint> createArrayOfint(ArrayOfint value) {
        return new JAXBElement<>(_ArrayOfint_QNAME, ArrayOfint.class, null, value);
    }

}
