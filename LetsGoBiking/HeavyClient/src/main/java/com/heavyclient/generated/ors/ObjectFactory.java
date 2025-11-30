
package com.heavyclient.generated.ors;

import com.microsoft.schemas._2003._10.serialization.arrays.ArrayOfArrayOfdouble;
import com.microsoft.schemas._2003._10.serialization.arrays.ArrayOfdouble;
import com.microsoft.schemas._2003._10.serialization.arrays.ArrayOfint;
import jakarta.xml.bind.JAXBElement;
import jakarta.xml.bind.annotation.XmlElementDecl;
import jakarta.xml.bind.annotation.XmlRegistry;

import javax.xml.namespace.QName;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the com.heavyclient.generated.ors package. 
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

    private static final QName _RouteFeature_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "RouteFeature");
    private static final QName _Geometry_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "Geometry");
    private static final QName _Properties_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "Properties");
    private static final QName _ArrayOfSegment_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "ArrayOfSegment");
    private static final QName _Segment_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "Segment");
    private static final QName _ArrayOfStep_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "ArrayOfStep");
    private static final QName _Step_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "Step");
    private static final QName _Summary_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "Summary");
    private static final QName _StepInstruction_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "instruction");
    private static final QName _StepName_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "name");
    private static final QName _StepWayPoints_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "way_points");
    private static final QName _SegmentSteps_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "steps");
    private static final QName _PropertiesSegments_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "segments");
    private static final QName _PropertiesSummary_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "summary");
    private static final QName _GeometryCoordinates_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "coordinates");
    private static final QName _GeometryType_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "type");
    private static final QName _RouteFeatureBbox_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "bbox");
    private static final QName _RouteFeatureGeometry_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "geometry");
    private static final QName _RouteFeatureProperties_QNAME = new QName("http://schemas.datacontract.org/2004/07/Server.Entities.ORS", "properties");

    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: com.heavyclient.generated.ors
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link RouteFeature }
     * 
     * @return
     *     the new instance of {@link RouteFeature }
     */
    public RouteFeature createRouteFeature() {
        return new RouteFeature();
    }

    /**
     * Create an instance of {@link Geometry }
     * 
     * @return
     *     the new instance of {@link Geometry }
     */
    public Geometry createGeometry() {
        return new Geometry();
    }

    /**
     * Create an instance of {@link Properties }
     * 
     * @return
     *     the new instance of {@link Properties }
     */
    public Properties createProperties() {
        return new Properties();
    }

    /**
     * Create an instance of {@link ArrayOfSegment }
     * 
     * @return
     *     the new instance of {@link ArrayOfSegment }
     */
    public ArrayOfSegment createArrayOfSegment() {
        return new ArrayOfSegment();
    }

    /**
     * Create an instance of {@link Segment }
     * 
     * @return
     *     the new instance of {@link Segment }
     */
    public Segment createSegment() {
        return new Segment();
    }

    /**
     * Create an instance of {@link ArrayOfStep }
     * 
     * @return
     *     the new instance of {@link ArrayOfStep }
     */
    public ArrayOfStep createArrayOfStep() {
        return new ArrayOfStep();
    }

    /**
     * Create an instance of {@link Step }
     * 
     * @return
     *     the new instance of {@link Step }
     */
    public Step createStep() {
        return new Step();
    }

    /**
     * Create an instance of {@link Summary }
     * 
     * @return
     *     the new instance of {@link Summary }
     */
    public Summary createSummary() {
        return new Summary();
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link RouteFeature }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link RouteFeature }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "RouteFeature")
    public JAXBElement<RouteFeature> createRouteFeature(RouteFeature value) {
        return new JAXBElement<>(_RouteFeature_QNAME, RouteFeature.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Geometry }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Geometry }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "Geometry")
    public JAXBElement<Geometry> createGeometry(Geometry value) {
        return new JAXBElement<>(_Geometry_QNAME, Geometry.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Properties }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Properties }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "Properties")
    public JAXBElement<Properties> createProperties(Properties value) {
        return new JAXBElement<>(_Properties_QNAME, Properties.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfSegment }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfSegment }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "ArrayOfSegment")
    public JAXBElement<ArrayOfSegment> createArrayOfSegment(ArrayOfSegment value) {
        return new JAXBElement<>(_ArrayOfSegment_QNAME, ArrayOfSegment.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Segment }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Segment }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "Segment")
    public JAXBElement<Segment> createSegment(Segment value) {
        return new JAXBElement<>(_Segment_QNAME, Segment.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfStep }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfStep }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "ArrayOfStep")
    public JAXBElement<ArrayOfStep> createArrayOfStep(ArrayOfStep value) {
        return new JAXBElement<>(_ArrayOfStep_QNAME, ArrayOfStep.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Step }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Step }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "Step")
    public JAXBElement<Step> createStep(Step value) {
        return new JAXBElement<>(_Step_QNAME, Step.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Summary }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Summary }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "Summary")
    public JAXBElement<Summary> createSummary(Summary value) {
        return new JAXBElement<>(_Summary_QNAME, Summary.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "instruction", scope = Step.class)
    public JAXBElement<String> createStepInstruction(String value) {
        return new JAXBElement<>(_StepInstruction_QNAME, String.class, Step.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "name", scope = Step.class)
    public JAXBElement<String> createStepName(String value) {
        return new JAXBElement<>(_StepName_QNAME, String.class, Step.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfint }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfint }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "way_points", scope = Step.class)
    public JAXBElement<ArrayOfint> createStepWayPoints(ArrayOfint value) {
        return new JAXBElement<>(_StepWayPoints_QNAME, ArrayOfint.class, Step.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfStep }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfStep }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "steps", scope = Segment.class)
    public JAXBElement<ArrayOfStep> createSegmentSteps(ArrayOfStep value) {
        return new JAXBElement<>(_SegmentSteps_QNAME, ArrayOfStep.class, Segment.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfSegment }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfSegment }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "segments", scope = Properties.class)
    public JAXBElement<ArrayOfSegment> createPropertiesSegments(ArrayOfSegment value) {
        return new JAXBElement<>(_PropertiesSegments_QNAME, ArrayOfSegment.class, Properties.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Summary }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Summary }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "summary", scope = Properties.class)
    public JAXBElement<Summary> createPropertiesSummary(Summary value) {
        return new JAXBElement<>(_PropertiesSummary_QNAME, Summary.class, Properties.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfint }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfint }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "way_points", scope = Properties.class)
    public JAXBElement<ArrayOfint> createPropertiesWayPoints(ArrayOfint value) {
        return new JAXBElement<>(_StepWayPoints_QNAME, ArrayOfint.class, Properties.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfArrayOfdouble }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfArrayOfdouble }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "coordinates", scope = Geometry.class)
    public JAXBElement<ArrayOfArrayOfdouble> createGeometryCoordinates(ArrayOfArrayOfdouble value) {
        return new JAXBElement<>(_GeometryCoordinates_QNAME, ArrayOfArrayOfdouble.class, Geometry.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "type", scope = Geometry.class)
    public JAXBElement<String> createGeometryType(String value) {
        return new JAXBElement<>(_GeometryType_QNAME, String.class, Geometry.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfdouble }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link ArrayOfdouble }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "bbox", scope = RouteFeature.class)
    public JAXBElement<ArrayOfdouble> createRouteFeatureBbox(ArrayOfdouble value) {
        return new JAXBElement<>(_RouteFeatureBbox_QNAME, ArrayOfdouble.class, RouteFeature.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Geometry }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Geometry }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "geometry", scope = RouteFeature.class)
    public JAXBElement<Geometry> createRouteFeatureGeometry(Geometry value) {
        return new JAXBElement<>(_RouteFeatureGeometry_QNAME, Geometry.class, RouteFeature.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Properties }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link Properties }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "properties", scope = RouteFeature.class)
    public JAXBElement<Properties> createRouteFeatureProperties(Properties value) {
        return new JAXBElement<>(_RouteFeatureProperties_QNAME, Properties.class, RouteFeature.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     * 
     * @param value
     *     Java instance representing xml element's value.
     * @return
     *     the new instance of {@link JAXBElement }{@code <}{@link String }{@code >}
     */
    @XmlElementDecl(namespace = "http://schemas.datacontract.org/2004/07/Server.Entities.ORS", name = "type", scope = RouteFeature.class)
    public JAXBElement<String> createRouteFeatureType(String value) {
        return new JAXBElement<>(_GeometryType_QNAME, String.class, RouteFeature.class, value);
    }

}
