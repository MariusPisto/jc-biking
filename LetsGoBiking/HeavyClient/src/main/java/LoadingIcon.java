import javax.swing.*;
import java.awt.*;

public class LoadingIcon extends JComponent {
        private int angle = 0;

        public LoadingIcon() {
            setPreferredSize(new Dimension(60, 60));
        }

        @Override
        protected void paintComponent(Graphics g) {
            super.paintComponent(g);
            Graphics2D g2 = (Graphics2D) g.create();
            g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

            int w = getWidth();
            int h = getHeight();
            int cx = w / 2;
            int cy = h / 2;
            int r = Math.min(w, h) / 2 - 5;

            g2.setColor(new Color(50, 50, 50));
            g2.setStroke(new BasicStroke(4, BasicStroke.CAP_ROUND, BasicStroke.JOIN_ROUND));

            angle = (angle + 10) % 360;
            g2.drawArc(cx - r, cy - r, 2 * r, 2 * r, angle, 270);
            g2.dispose();
        }
    }
