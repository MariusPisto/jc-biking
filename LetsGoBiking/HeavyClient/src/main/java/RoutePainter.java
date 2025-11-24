import org.jxmapviewer.JXMapViewer;
import org.jxmapviewer.painter.Painter;
import org.jxmapviewer.viewer.GeoPosition;

import java.awt.*;
import java.awt.geom.Point2D;
import java.awt.geom.Rectangle2D;
import java.util.List;

public class RoutePainter implements Painter<JXMapViewer> {
    private final Color color;
    private final boolean antiAlias;
    private final List<GeoPosition> track;
    private final float strokeWidth;
    private final boolean dashed;

    public RoutePainter(List<GeoPosition> track, Color color, boolean antiAlias, float strokeWidth, float alpha, boolean dashed) {
        this.track = track;
        this.antiAlias = antiAlias;
        this.strokeWidth = strokeWidth;
        this.dashed = dashed;
        if (alpha < 0f) alpha = 0f;
        if (alpha > 1f) alpha = 1f;
        int a = Math.round(alpha * 255f);
        this.color = new Color(color.getRed(), color.getGreen(), color.getBlue(), a);
    }

    @Override
    public void paint(Graphics2D g, JXMapViewer map, int w, int h) {
        if (track == null || track.size() < 2) {
            return;
        }

        Rectangle2D viewportBounds = map.getViewportBounds();

        Graphics2D g2 = (Graphics2D) g.create();
        if (antiAlias) {
            g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
        }

        g2.setColor(color);

        if (dashed) {
            g2.setStroke(new BasicStroke(strokeWidth, BasicStroke.CAP_ROUND, BasicStroke.JOIN_ROUND, 10.0f, new float[]{10.0f, 10.0f}, 0.0f));
        } else {
            g2.setStroke(new BasicStroke(strokeWidth, BasicStroke.CAP_ROUND, BasicStroke.JOIN_ROUND));
        }

        Point2D prevPt = null;
        for (GeoPosition gp : track) {
            Point2D pt = map.getTileFactory().geoToPixel(gp, map.getZoom());
            double x = pt.getX() - viewportBounds.getX();
            double y = pt.getY() - viewportBounds.getY();

            if (prevPt != null) {
                g2.drawLine((int) prevPt.getX(), (int) prevPt.getY(), (int) x, (int) y);
            }
            prevPt = new Point2D.Double(x, y);
        }
        g2.dispose();
    }
}
