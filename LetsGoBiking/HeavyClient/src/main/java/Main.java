import com.heavyclient.generated.ors.Geometry;
import com.heavyclient.generated.ors.RouteFeature;
import com.heavyclient.generated.response.BikeRoute;
import com.heavyclient.generated.response.ItineraryResponse;
import com.heavyclient.generated.response.Route;
import com.microsoft.schemas._2003._10.serialization.arrays.ArrayOfArrayOfdouble;
import com.microsoft.schemas._2003._10.serialization.arrays.ArrayOfdouble;
import org.jxmapviewer.JXMapViewer;
import org.jxmapviewer.viewer.DefaultTileFactory;
import org.jxmapviewer.viewer.GeoPosition;
import org.jxmapviewer.viewer.TileFactoryInfo;
import org.jxmapviewer.viewer.DefaultWaypoint;
import org.jxmapviewer.viewer.Waypoint;
import org.jxmapviewer.viewer.WaypointPainter;
import org.jxmapviewer.painter.Painter;
import org.jxmapviewer.painter.CompoundPainter;
import org.jxmapviewer.input.PanMouseInputListener;
import org.jxmapviewer.input.ZoomMouseWheelListenerCenter;
import org.tempuri.ServerService;

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

public class Main {

    private static JXMapViewer mapViewer;
    private static JTextField startLatField;
    private static JTextField startLngField;
    private static JTextField endLatField;
    private static JTextField endLngField;
    private static JFrame frame;
    private static JPanel glassPane;
    private static Timer loadingTimer;

    public static void main(String[] args) {
        // Set a user agent to avoid 403 errors from tile servers. it took me a long time to figure this out there is no doc about it :)
        System.setProperty("http.agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/119.0");

        mapViewer = new JXMapViewer();

        TileFactoryInfo info = new TileFactoryInfo(1, 19, 19,
                256, true, true,
                "https://tile.openstreetmap.org",
                "x", "y", "z") {
            @Override
            public String getTileUrl(int x, int y, int zoom) {
                zoom = 19 - zoom;
                return "https://tile.openstreetmap.org/" + zoom + "/" + x + "/" + y + ".png";
            }
        };

        var tileFactory = new DefaultTileFactory(info);
        mapViewer.setTileFactory(tileFactory);
        tileFactory.setThreadPoolSize(8);

        var mia = new PanMouseInputListener(mapViewer);
        mapViewer.addMouseListener(mia);
        mapViewer.addMouseMotionListener(mia);
        mapViewer.addMouseWheelListener(new ZoomMouseWheelListenerCenter(mapViewer));

        mapViewer.setZoom(15);
        mapViewer.setAddressLocation(new GeoPosition(43.615775, 7.071988));
        mapViewer.setLayout(new GridBagLayout());

        var zoomPanel = getZoomPanel();
        var gbc = new GridBagConstraints();
        gbc.gridx = 0;
        gbc.gridy = 0;
        gbc.weightx = 1.0;
        gbc.weighty = 1.0;
        gbc.anchor = GridBagConstraints.SOUTHEAST;
        gbc.insets = new Insets(0, 0, 20, 20);
        mapViewer.add(zoomPanel, gbc);

        frame = new JFrame("JCBiking");
        frame.setLayout(new BorderLayout());

        var controlPanel = new JPanel();
        controlPanel.setLayout(new FlowLayout());

        startLatField = new JTextField("45.764043", 8);
        startLngField = new JTextField("4.835659", 8);
        endLatField = new JTextField("43.127664", 8);
        endLngField = new JTextField("5.930362", 8);

        var calcButton = new JButton("Calculate Route");
        var demoButton = new JButton("Demo");

        controlPanel.add(new JLabel("Start Lat:"));
        controlPanel.add(startLatField);
        controlPanel.add(new JLabel("Lng:"));
        controlPanel.add(startLngField);
        controlPanel.add(new JLabel("End Lat:"));
        controlPanel.add(endLatField);
        controlPanel.add(new JLabel("Lng:"));
        controlPanel.add(endLngField);
        controlPanel.add(calcButton);
        controlPanel.add(demoButton);

        frame.add(controlPanel, BorderLayout.NORTH);
        frame.add(mapViewer, BorderLayout.CENTER);

        setupGlassPane();

        calcButton.addActionListener(e -> calculateAndDrawRoute());

        demoButton.addActionListener(e -> {
            startLatField.setText("50.8998481");
            startLngField.setText("4.2808363");
            endLatField.setText("49.5806013");
            endLngField.setText("6.1321121");
            calculateAndDrawRoute();
        });

        frame.setSize(1100, 700);
        frame.setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);
        frame.setVisible(true);
    }

    private static JPanel getZoomPanel() {
        var zoomPanel = new JPanel(new GridLayout(2, 1, 5, 5));
        zoomPanel.setOpaque(false);

        var zoomInBtn = new JButton("+");
        var zoomOutBtn = new JButton("-");

        var btnFont = new Font("Arial", Font.BOLD, 16);
        zoomInBtn.setFont(btnFont);
        zoomOutBtn.setFont(btnFont);

        zoomInBtn.setFocusable(false);
        zoomOutBtn.setFocusable(false);

        zoomInBtn.addActionListener(e -> mapViewer.setZoom(mapViewer.getZoom() - 1));
        zoomOutBtn.addActionListener(e -> mapViewer.setZoom(mapViewer.getZoom() + 1));

        zoomPanel.add(zoomInBtn);
        zoomPanel.add(zoomOutBtn);
        return zoomPanel;
    }

    private static void setupGlassPane() {
        glassPane = new JPanel() {
            @Override
            protected void paintComponent(Graphics g) {
                g.setColor(new Color(255, 255, 255, 150));
                g.fillRect(0, 0, getWidth(), getHeight());
                super.paintComponent(g);
            }
        };
        glassPane.setOpaque(false);
        glassPane.setLayout(new GridBagLayout());

        glassPane.addMouseListener(new MouseAdapter() {});
        glassPane.addMouseMotionListener(new MouseMotionAdapter() {});
        glassPane.addKeyListener(new KeyAdapter() {});

        var loadingIcon = new LoadingIcon();
        glassPane.add(loadingIcon);

        frame.setGlassPane(glassPane);

        loadingTimer = new Timer(50, e -> loadingIcon.repaint());
    }

    private static void calculateAndDrawRoute() {
        mapViewer.setOverlayPainter(null);

        double startLat, startLng, endLat, endLng;
        try {
            startLat = Double.parseDouble(startLatField.getText());
            startLng = Double.parseDouble(startLngField.getText());
            endLat = Double.parseDouble(endLatField.getText());
            endLng = Double.parseDouble(endLngField.getText());
        } catch (NumberFormatException e) {
            JOptionPane.showMessageDialog(null, "Invalid coordinates format.");
            return;
        }

        glassPane.setVisible(true);
        loadingTimer.start();

        double finalStartLat = startLat;
        double finalStartLng = startLng;
        double finalEndLat = endLat;
        double finalEndLng = endLng;

        var worker = new SwingWorker<ItineraryResponse, Void>() {
            @Override
            protected ItineraryResponse doInBackground() {
                var service = new ServerService();
                var port = service.getBasicHttpBindingIServerService();
                return port.itinerary(finalStartLat, finalStartLng, finalEndLat, finalEndLng);
            }

            @Override
            protected void done() {
                loadingTimer.stop();
                glassPane.setVisible(false);
                try {
                    var resp = get();
                    processResponse(resp);
                } catch (Exception ex) {
                    System.out.println(ex.getMessage());
                    JOptionPane.showMessageDialog(null, "Error fetching route: " + ex.getMessage());
                }
            }
        };

        worker.execute();
    }

    private static void processResponse(ItineraryResponse resp) {
        List<Painter<JXMapViewer>> painters = new ArrayList<>();
        Set<GeoPosition> allPositions = new HashSet<>();
        Set<Waypoint> waypoints = new HashSet<>();
        GeoPosition lastPos = null;

        List<BikeRoute> bikeList = new ArrayList<>();
        List<Route> walkList = new ArrayList<>();

        if (resp != null && resp.getBikeRoutes() != null && resp.getBikeRoutes().getValue() != null) {
            bikeList = resp.getBikeRoutes().getValue().getBikeRoute();
        }
        if (resp != null && resp.getWalkRoutes() != null && resp.getWalkRoutes().getValue() != null) {
            walkList = resp.getWalkRoutes().getValue().getRoute();
        }

        int maxSteps = Math.max(bikeList.size(), walkList.size());

        for (int i = 0; i < maxSteps; i++) {
            if (i < walkList.size()) {
                var walkParams = walkList.get(i);
                if (walkParams.getFeature() != null && walkParams.getFeature().getValue() != null) {
                    var segment = extractCoords(walkParams.getFeature().getValue());
                    if (!segment.isEmpty()) {
                        painters.add(new RoutePainter(segment, Color.BLUE, true, 3f, 0.9f, true));
                        allPositions.addAll(segment);
                        waypoints.add(new DefaultWaypoint(segment.getFirst()));
                        lastPos = segment.getLast();
                    }
                }
            }

            if (i < bikeList.size()) {
                var bikeParams = bikeList.get(i);
                if (bikeParams.getFeature() != null && bikeParams.getFeature().getValue() != null) {
                    var segment = extractCoords(bikeParams.getFeature().getValue());
                    if (!segment.isEmpty()) {
                        painters.add(new RoutePainter(segment, Color.RED, true, 4f, 1f, false));
                        allPositions.addAll(segment);
                        waypoints.add(new DefaultWaypoint(segment.getFirst()));
                        lastPos = segment.getLast();
                    }
                }
            }
        }

        if (lastPos != null) {
            waypoints.add(new DefaultWaypoint(lastPos));
        }

        if (!allPositions.isEmpty()) {
            var waypointPainter = new WaypointPainter<Waypoint>();
            waypointPainter.setWaypoints(waypoints);
            painters.add(waypointPainter);

            var compoundPainter = new CompoundPainter<>(painters);
            mapViewer.setOverlayPainter(compoundPainter);
            mapViewer.zoomToBestFit(allPositions, 0.7);
        } else {
            JOptionPane.showMessageDialog(null, "No route found.");
        }
    }

    private static List<GeoPosition> extractCoords(RouteFeature feature) {
        List<GeoPosition> list = new ArrayList<>();
        if (feature.getGeometry() != null && feature.getGeometry().getValue() != null) {
            Geometry geom = feature.getGeometry().getValue();
            if (geom.getCoordinates() != null && geom.getCoordinates().getValue() != null) {
                ArrayOfArrayOfdouble coords = geom.getCoordinates().getValue();
                if (coords.getArrayOfdouble() != null) {
                    for (ArrayOfdouble pair : coords.getArrayOfdouble()) {
                        if (pair.getDouble() != null && pair.getDouble().size() >= 2) {
                            double lng = pair.getDouble().get(0);
                            double lat = pair.getDouble().get(1);
                            list.add(new GeoPosition(lat, lng));
                        }
                    }
                }
            }
        }
        return list;
    }
}
