using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public static class GeracaoLoopsGeometria
    {
        static double Dot(vec3 a, vec3 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        static double Cross2D(vec3 a, vec3 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        static vec3 Normalize(vec3 v)
        {
            double len = Math.Sqrt(v.x * v.x + v.y * v.y);

            return new vec3(v.x / len,v.y / len,0);
        }

        static vec3 Sub(vec3 a, vec3 b)
        {
            return new vec3(a.x - b.x,
                a.y - b.y,
                0);

        }

        static bool SamePoint(vec3 a, vec3 b)
        {
            const double EPS = 1e-2;

            return
                Math.Abs(a.x - b.x) < EPS &&
                Math.Abs(a.y - b.y) < EPS;
        }

        static LinhaVec3 GetNextEdge(vec3 previousNode,vec3 currentNode,LinhaVec3 currentEdge,List<LinhaVec3> connectedEdges)
        {
            vec3 vin = Normalize(Sub(currentNode, previousNode));

            LinhaVec3 bestEdge = new LinhaVec3(-1);

            double bestAngle = double.MaxValue;

            foreach (var edge in connectedEdges)
            {
                if (edge.id_aresta == currentEdge.id_aresta)
                    continue;

                vec3 nextNode;

                if (SamePoint(edge.p1, currentNode))
                    nextNode = edge.p2;
                else if (SamePoint(edge.p2, currentNode))
                    nextNode = edge.p1;
                else
                    continue;

                vec3 vout = Normalize(Sub(nextNode, currentNode));

                double cross = Cross2D(vin, vout);
                double dot = Dot(vin, vout);

                double angle = Math.Atan2(cross, dot);

                if (angle <= 0.0)
                    angle += 2.0 * Math.PI;

                if (angle < bestAngle)
                {
                    bestAngle = angle;
                    bestEdge = edge;
                }
            }

            return bestEdge;
        }

        public static List<vec3> ExtractLoop(LinhaVec3 startEdge,Dictionary<vec3, List<LinhaVec3>> nodeEdges)
        {
            var loop = new List<vec3>();

            vec3 startNode = startEdge.p1;
            vec3 currentNode = startEdge.p2;
            vec3 previousNode = startEdge.p1;

            LinhaVec3 currentEdge = startEdge;

            loop.Add(startNode);
            loop.Add(currentNode);

            int safety = 0;

            while (safety++ < 100000)
            {
                LinhaVec3 nextEdge = GetNextEdge(previousNode,currentNode,currentEdge,nodeEdges[currentNode]);

                if (nextEdge.id_aresta == -1)
                    return null;

                vec3 nextNode =
                    SamePoint(nextEdge.p1, currentNode)
                    ? nextEdge.p2
                    : nextEdge.p1;

                if (SamePoint(nextNode, startNode))
                    break;

                loop.Add(nextNode);

                previousNode = currentNode;
                currentNode = nextNode;
                currentEdge = nextEdge;
            }

            return loop;
        }

        public static Dictionary<vec3, List<LinhaVec3>> BuildConnectivity(List<LinhaVec3> edges)
        {
            var map =
                new Dictionary<vec3, List<LinhaVec3>>();

            foreach (var edge in edges)
            {
                if (!map.ContainsKey(edge.p1))
                    map[edge.p1] = new List<LinhaVec3>();

                if (!map.ContainsKey(edge.p2))
                    map[edge.p2] = new List<LinhaVec3>();

                map[edge.p1].Add(edge);
                map[edge.p2].Add(edge);
            }

            return map;
        }
 
        public static double PolygonArea(List<vec3> poly)
        {
            double area = 0.0;

            int n = poly.Count;

            for (int i = 0; i < n; i++)
            {
                vec3 p1 = poly[i];
                vec3 p2 = poly[(i + 1) % n];

                area += p1.x * p2.y - p2.x * p1.y;
            }

            return 0.5 * area;
        }

        public class HalfEdge
        {
            public int id_aresta;

            public int origem;
            public int destino;

            public double angulo;

            public bool visitada;

            public HalfEdge twin;
        }

        public class LoopInfo
        {
            public List<HalfEdge> edges;

            public double area;

            public bool externo;
        }


        public static class LoopExtractor
        {
            public static List<List<HalfEdge>> ExtrairLoops(
                List<LinhaVec3> linhas)
            {
                List<HalfEdge> halfEdges =
                    new List<HalfEdge>();

                Dictionary<int, vec3> nodes =
                    new Dictionary<int, vec3>();

                //----------------------------------
                // cria nós
                //----------------------------------

                foreach (LinhaVec3 linha in linhas)
                {
                    if (!nodes.ContainsKey(linha.p1.id))
                        nodes.Add(linha.p1.id, linha.p1);

                    if (!nodes.ContainsKey(linha.p2.id))
                        nodes.Add(linha.p2.id, linha.p2);
                }

                //----------------------------------
                // cria half-edges
                //----------------------------------

                foreach (LinhaVec3 linha in linhas)
                {
                    HalfEdge he1 = new HalfEdge();
                    HalfEdge he2 = new HalfEdge();

                    he1.id_aresta = linha.id_aresta;
                    he1.origem = linha.p1.id;
                    he1.destino = linha.p2.id;

                    he2.id_aresta = linha.id_aresta;
                    he2.origem = linha.p2.id;
                    he2.destino = linha.p1.id;

                    he1.twin = he2;
                    he2.twin = he1;

                    vec3 a = nodes[he1.origem];
                    vec3 b = nodes[he1.destino];

                    he1.angulo =
                        Math.Atan2(
                            b.y - a.y,
                            b.x - a.x);

                    a = nodes[he2.origem];
                    b = nodes[he2.destino];

                    he2.angulo =
                        Math.Atan2(
                            b.y - a.y,
                            b.x - a.x);

                    halfEdges.Add(he1);
                    halfEdges.Add(he2);
                }

                //----------------------------------
                // outgoing
                //----------------------------------

                Dictionary<int, List<HalfEdge>> outgoing =
                    new Dictionary<int, List<HalfEdge>>();

                foreach (HalfEdge he in halfEdges)
                {
                    if (!outgoing.ContainsKey(he.origem))
                    {
                        outgoing.Add(
                            he.origem,
                            new List<HalfEdge>());
                    }

                    outgoing[he.origem].Add(he);
                }

                //----------------------------------
                // ordenação angular
                //----------------------------------

                foreach (KeyValuePair<int, List<HalfEdge>> kv in outgoing)
                {
                    kv.Value.Sort(delegate (HalfEdge a, HalfEdge b)
                    {
                        return a.angulo.CompareTo(b.angulo);
                    });
                }

                //----------------------------------
                // loops
                //----------------------------------

                List<List<HalfEdge>> loops =
                    new List<List<HalfEdge>>();

                foreach (HalfEdge start in halfEdges)
                {
                    if (start.visitada)
                        continue;

                    List<HalfEdge> loop =
                        new List<HalfEdge>();

                    HalfEdge current = start;

                    while (true)
                    {
                        if (current.visitada)
                            break;

                        current.visitada = true;

                        loop.Add(current);

                        current =
                            NextBoundary(
                                current,
                                outgoing);

                        if (Object.ReferenceEquals(
                            current,
                            start))
                            break;
                    }

                    if (loop.Count > 2)
                        loops.Add(loop);
                }

                return loops;
            }

            private static HalfEdge NextBoundary(
                HalfEdge current,
                Dictionary<int, List<HalfEdge>> outgoing)
            {
                HalfEdge twin = current.twin;

                List<HalfEdge> lista =
                    outgoing[twin.origem];

                int idx = lista.IndexOf(twin);

                idx--;

                if (idx < 0)
                    idx = lista.Count - 1;

                return lista[idx];
            }

            public static double CalcularArea(
                List<HalfEdge> loop,
                Dictionary<int, vec3> nodes)
            {
                double area = 0.0;

                int n = loop.Count;

                for (int i = 0; i < n; i++)
                {
                    vec3 p0 =
                        nodes[loop[i].origem];

                    vec3 p1 =
                        nodes[loop[(i + 1) % n].origem];

                    area +=
                        p0.x * p1.y -
                        p1.x * p0.y;
                }

                return area * 0.5;
            }
        }
    }
}
