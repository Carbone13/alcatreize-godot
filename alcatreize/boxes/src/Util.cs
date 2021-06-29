using System;
using System.Drawing;
using System.Numerics;

namespace Dcrew.Spatial {
    public static class Util {
        public static Rectangle Rotate(Vector2 xy, Vector2 size, float angle, Vector2 origin) {
            float cos = (float) Math.Cos(angle),
                sin = (float)Math.Sin(angle),
                x = -origin.X,
                y = -origin.Y,
                w = size.X + x,
                h = size.Y + y,
                xcos = x * cos,
                ycos = y * cos,
                xsin = x * sin,
                ysin = y * sin,
                wcos = w * cos,
                wsin = w * sin,
                hcos = h * cos,
                hsin = h * sin,
                tlx = xcos - ysin,
                tly = xsin + ycos,
                trx = wcos - ysin,
                tr_y = wsin + ycos,
                brx = wcos - hsin,
                bry = wsin + hcos,
                blx = xcos - hsin,
                bly = xsin + hcos,
                minx = tlx,
                miny = tly,
                maxx = minx,
                maxy = miny;
            if (trx < minx)
                minx = trx;
            if (brx < minx)
                minx = brx;
            if (blx < minx)
                minx = blx;
            if (tr_y < miny)
                miny = tr_y;
            if (bry < miny)
                miny = bry;
            if (bly < miny)
                miny = bly;
            if (trx > maxx)
                maxx = trx;
            if (brx > maxx)
                maxx = brx;
            if (blx > maxx)
                maxx = blx;
            if (tr_y > maxy)
                maxy = tr_y;
            if (bry > maxy)
                maxy = bry;
            if (bly > maxy)
                maxy = bly;
            var r = new Rectangle((int)minx, (int)miny, (int)Math.Ceiling(maxx - minx), (int)Math.Ceiling(maxy - miny));
            r.Offset( new Point((int)xy.X, (int)xy.Y));
            return r;
        }
    }
}