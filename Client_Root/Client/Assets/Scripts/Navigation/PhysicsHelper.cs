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

        //  ref : https://en.wikipedia.org/wiki/Line%E2%80%93sphere_intersection
        //  l : direction of line (a unit vector)
        //  o : origin of the line
        //  s : sphere
        public static bool TestLineSphere(Vector3 l, Vector3 o, Sphere s)
        {
            float fValue = Mathf.Pow(Vector3.Dot(l, (o - s.center)), 2f) - Mathf.Pow((o - s.center).magnitude, 2f) + Mathf.Pow(s.radius, 2f);

            //If the value is less than zero, then it is clear that no solutions exist, i.e. the line does not intersect the sphere.
            if (fValue < 0f)
            {
                return false;
            }
            //If it is zero, then exactly one solution exists, i.e. the line just touches the sphere in one point.
            else if (Mathf.Approximately(fValue, 0f))
            {
                return true;
            }
            //If it is greater than zero, two solutions exist, and thus the line touches the sphere in two points.
            else
            {
                return true;
            }
        }

        public static bool TestSpherePolygon(Sphere s, Polygon p)
        {
            // Compute normal for the plane of the polygon
            Vector3 n = Vector3.Normalize(Vector3.Cross(p.m_listVertex[1] - p.m_listVertex[0], p.m_listVertex[2] - p.m_listVertex[0]));

            // Compute the plane equation for p
            Plane m = new Plane(); 
            m.normal = n;
            m.distance = -Vector3.Dot(n, p.m_listVertex[0]);

            // No intersection if sphere not intersecting plane of polygon
            if (!TestSpherePlane(s, m)) return false;

            // Test to see if any one of the polygon edges pierces the sphere   
            for (int k = p.m_listVertex.Count, i = 0, j = k - 1; i < k; j = i, i++)
            {
                // Test if edge (p.v[j], p.v[i]) intersects s
                if (TestLineSphere((p.m_listVertex[i] - p.m_listVertex[j]).normalized, p.m_listVertex[j], s))
                    return true;
            }

            // Test if the orthogonal projection q of the sphere center onto m is inside p
            //Vector3 q1 = ClosestPtPointPlane(s.center, m);
            return PointInConvexPolygon(s.center, p);
        }

        // Test if point p lies inside ccw-specified convex n-gon given by vertices v[]
        public static bool PointInConvexPolygon(Vector3 p, Polygon polygon)
        {
            // Do binary search over polygon vertices to find the fan triangle
            // (v[0], v[low], v[high]) the point p lies within the near sides of
            int low = 0, high = polygon.m_listVertex.Count;
            do {
                int mid = (low + high) / 2;
                if (TriangleIsCCW(polygon.m_listVertex[0], polygon.m_listVertex[mid], p))
                    low = mid;
                else
                    high = mid;
            } while (low + 1 < high);

            // If point outside last (or first) edge, then it is not inside the n-gon
            if (low == 0 || high == polygon.m_listVertex.Count) return false;

            // p is inside the polygon if it is left of
            // the directed edge from v[low] to v[high]
            return TriangleIsCCW(polygon.m_listVertex[low], polygon.m_listVertex[high], p);
        }

        // Determine whether plane p intersects sphere s
        private static bool TestSpherePlane(Sphere s, Plane p)
        {
            // For a normalized plane (|p.n| = 1), evaluating the plane equation
            // for a point gives the signed distance of the point to the plane
            float dist = Vector3.Dot(s.center, p.normal) - p.distance;

            // If sphere center within +/-radius from plane, plane intersects sphere
            return Mathf.Abs(dist) <= s.radius;
        }

        // Intersects ray r = p + td, |d| = 1, with sphere s and, if intersecting,
        // returns t value of intersection and intersection point q
        private static bool IntersectRaySphere(Vector3 p, Vector3 d, Sphere s, ref float t, ref Vector3 q)
        {
            Vector3 m = p - s.center;
            float b = Vector3.Dot(m, d);
            float c = Vector3.Dot(m, m) - s.radius * s.radius;

            // Exit if r’s origin outside s (c > 0)and r pointing away from s (b > 0)
            if (c > 0.0f && b > 0.0f) return false;
            float discr = b*b - c;
            // A negative discriminant corresponds to ray missing sphere
            if (discr < 0.0f) return false;
            // Ray now found to intersect sphere, compute smallest t value of intersection
            t = -b - Mathf.Sqrt(discr);
            // If t is negative, ray started inside sphere so clamp t to zero
            if (t < 0.0f) t = 0.0f;
            q = p + t * d;
            return true;
        }

        private static Vector3 ClosestPtPointPlane(Vector3 q, Plane p)
        {
            float t = Vector3.Dot(p.normal, q) - p.distance;
            return q - t * p.normal;
        }

        private static bool TriangleIsCCW(Vector3 a, Vector3 b, Vector3 c)
        {
            float sum = 0f;

            sum += (b.x - a.x) * (b.z + a.z);
            sum += (c.x - b.x) * (c.z + b.z);
            sum += (a.x - c.x) * (a.z + c.z);

            //  negative : ccw, positive : cw
            return sum < 0;
        }
    }
}

