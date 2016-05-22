using UnityEngine;
using System.Collections.Generic;

namespace PhysicsHelper
{
    public class Collision
    {
        public static bool FindLineAndLineIntersection(Line2D line1, Line2D line2, ref Vector2 intersectionPos)
        {
            float denom = ((line1.end.x - line1.start.x) * (line2.end.y - line2.start.y)) - ((line1.end.y - line1.start.y) * (line2.end.x - line2.start.x));

            //  AB & CD are parallel 
            if (denom == 0)
                return false;

            float numer = ((line1.start.y - line2.start.y) * (line2.end.x - line2.start.x)) - ((line1.start.x - line2.start.x) * (line2.end.y - line2.start.y));

            float r = numer / denom;

            float numer2 = ((line1.start.y - line2.start.y) * (line1.end.x - line1.start.x)) - ((line1.start.x - line2.start.x) * (line1.end.y - line1.start.y));

            float s = numer2 / denom;

            if ((r < 0 || r > 1) || (s < 0 || s > 1))
                return false;

            // Find intersection point

            intersectionPos.x = line1.start.x + (r * (line1.end.x - line1.start.x));
            intersectionPos.y = line1.start.y + (r * (line1.end.y - line1.start.y));

            return true;
        }
        //  FindLineIntersection function
        //  reference site : http://stackoverflow.com/questions/1119451/how-to-tell-if-a-line-intersects-a-polygon-in-chttp://stackoverflow.com/questions/1119451/how-to-tell-if-a-line-intersects-a-polygon-in-c

        public static bool FindLineAndPolygonIntersection(Line2D line, Polygon polygon, ref Vector2 intersectionPos)
        {
            if (polygon.m_listVertex.Count < 3)
            {
                return false;
            }

            List<Line2D> listLine = new List<Line2D>();

            for (int i = 1; i < polygon.m_listVertex.Count; ++i)
            {
                Vector2 start = new Vector2(polygon.m_listVertex[i - 1].x, polygon.m_listVertex[i - 1].z);
                Vector2 end = new Vector2(polygon.m_listVertex[i].x, polygon.m_listVertex[i].z);

                listLine.Add(new Line2D(start, end));
            }

            //  first and last vertex
            {
                Vector2 start = new Vector2(polygon.m_listVertex[polygon.m_listVertex.Count - 1].x, polygon.m_listVertex[polygon.m_listVertex.Count - 1].z);
                Vector2 end = new Vector2(polygon.m_listVertex[0].x, polygon.m_listVertex[0].z);

                listLine.Add(new Line2D(start, end));
            }
                
            foreach(Line2D polygonLine in listLine)
            {
                if (FindLineAndLineIntersection(line, polygonLine, ref intersectionPos))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

