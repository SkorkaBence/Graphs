using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Graphs {
    public class Graph {

        private List<GraphPoint> points = new List<GraphPoint>();
        private List<GraphConnection> connections = new List<GraphConnection>();
        private Random rand = new Random();
        private bool multygraph = false;

        public Graph() {

        }

        public void reset() {
            points.Clear();
            connections.Clear();
        }

        public void resetConnections() {
            connections.Clear();
        }

        public void createNewPoint() {
            points.Add(new GraphPoint(rand));
        }

        public void createNewPoint(string id) {
            points.Add(new GraphPoint(rand, id));
        }

        public List<GraphPoint> getPoints() {
            return points;
        }

        public List<GraphConnection> getConnections() {
            return connections;
        }

        public bool createNewConnection(string id1, string id2) {
            if (!multygraph) {
                if (id1 == id2) {
                    return false;
                    //throw new Exception("The graph is not a multygraph!");
                }
                for (int i = 0; i < connections.Count(); i++) {
                    if (connections[i].isPointConnected(id1) && connections[i].isPointConnected(id2)) {
                        return false;
                        //throw new Exception("The graph is not a multygraph!");
                    }
                }
            }
            connections.Add(new GraphConnection(id1, id2));
            return true;
        }

        public bool createNewConnection(GraphPoint id1, GraphPoint id2) {
            return createNewConnection(id1.getId(), id2.getId());
        }

        public int getPointDegree(GraphPoint point) {
            int v = 0;
            for (int c = 0; c < connections.Count(); c++) {
                if (connections[c].isPointConnected(point)) {
                    v++;
                }
            }
            return v;
        }

        public int[] getDegree() {
            int[] vl = new int[points.Count()];

            for (int p = 0; p < points.Count(); p++) {
                vl[p] = getPointDegree(points[p]);
            }

            Array.Sort(vl);

            return vl;
        }

        public void createRandomGraph() {
            createRandomGraph(rand.Next(10, 15), rand.Next(10, 15));
        }

        public void createRandomGraph(int v, int e) {
            reset();
            for (int i = 0; i < v; i++) {
                createNewPoint();
            }
            for (int i = 0; i < e; i++) {
                int p1, p2;
                do {
                    p1 = rand.Next(points.Count());
                    p2 = rand.Next(points.Count());
                } while (p1 == p2);

                createNewConnection(points[p1], points[p2]);
            }
        }

        private float generateX(int id, int width) {
            return (float)(width / 2 + (width / 2 - 30) * Math.Cos(((Math.PI * 2) / points.Count()) * id));
        }

        private float generateY(int id, int height) {
            return (float)(height / 2 + (height / 2 - 30) * Math.Sin(((Math.PI * 2) / points.Count()) * id));
        }

        public int getId(string id) {
            for (int i = 0; i < points.Count(); i++) {
                if (points[i].getId() == id) {
                    return i;
                }
            }
            throw new Exception("Graph Id Not Found");
        }

        public Image generateImage() {
            Image img = new Bitmap(500, 500, PixelFormat.Format24bppRgb);
            Bitmap bmp = (Bitmap)img;
            Graphics gp = Graphics.FromImage(img);

            SolidBrush brush = new SolidBrush(Color.White);
            gp.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);

            for (int i = 0; i < points.Count(); i++) {
                float x = generateX(i, bmp.Width);
                float y = generateY(i, bmp.Height);

                gp.DrawEllipse(new Pen(Color.Black, 1), x - 5, y - 5, 10, 10);
            }

            for (int i = 0; i < connections.Count(); i++) {
                float x1 = generateX(getId(connections[i].getId1()), bmp.Width);
                float y1 = generateY(getId(connections[i].getId1()), bmp.Height);
                float x2 = generateX(getId(connections[i].getId2()), bmp.Width);
                float y2 = generateY(getId(connections[i].getId2()), bmp.Height);

                gp.DrawLine(new Pen(Color.Black, 1), x1, y1, x2, y2);
            }

            return img;
        }

        public Graph getComplementer() {
            Graph comp = new Graph();
            for (int i = 0; i < points.Count(); i++) {
                comp.createNewPoint(points[i].getId());
            }
            for (int x = 0; x < points.Count(); x++) {
                for (int y = 0; y < points.Count(); y++) {
                    bool connected = false;
                    for (int i = 0; i < connections.Count(); i++) {
                        if (connections[i].isPointConnected(points[x]) && connections[i].isPointConnected(points[y])) {
                            connected = true;
                        }
                    }
                    if (!connected) {
                        comp.createNewConnection(points[x], points[y]);
                    }
                }
            }
            return comp;
        }

        public bool isIsomorphWith(Graph sample) {
            if (sample.getPoints().Count() != points.Count())
                return false;
            if (sample.getConnections().Count() != connections.Count())
                return false;

            bool degreeok = true;
            int[] degree1 = sample.getDegree();
            int[] degree2 = getDegree();
            for (int i = 0; i < degree1.Length; i++) {
                if (degree1[i] != degree2[i]) {
                    degreeok = false;
                    break;
                }
            }
            if (!degreeok)
                return false;

            Graph copy = new Graph();
            copy.reset();
            for (int i = 0; i < sample.getPoints().Count(); i++) {
                copy.createNewPoint(points[i].getId());
            }

            if (sample.getPoints().Count() > 10) {
                throw new Exception("This Graph is too big, the permutation array wont fit in the memory.");
            }

            List<int[]> permarray = Permutation.getPermutations(sample.getPoints().Count());
            for (int i = 0; i < permarray.Count(); i++) {
                copy.resetConnections();
                int[] pm = permarray[i];
                for (int c = 0; c < connections.Count(); c++) {
                    copy.createNewConnection(points[pm[getId(connections[c].getId1())]], points[pm[getId(connections[c].getId2())]]);
                }
                if (isMatching(copy)) {
                    return true;
                }
            }

            return false;
        }

        public bool isMatching(Graph grp) {
            bool[,] m1 = grp.convertToBoolArray();
            bool[,] m2 = convertToBoolArray();

            for (int x = 0; x < m1.GetLength(0); x++) {
                for (int y = 0; y < m1.GetLength(1); y++) {
                    if (m1[x, y] != m2[x,y]) {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool generateGraphFromDegree(int[] val) {
            reset();

            if (!isGraphic(val)) {
                return false;
            }

            Array.Sort(val);
            val.Reverse();

            for (int i = 0; i < val.Length; i++) {
                createNewPoint();
            }

            bool possible = true;
            for (int i = 0; i < val.Length; i++) {
                while (getPointDegree(points[i]) < val[i] && possible) {
                    int connectto = points.Count() - 1;
                    bool stillgoing = true;
                    while (stillgoing) {
                        if (connectto != i) {
                            if (getPointDegree(points[connectto]) < val[connectto]) {
                                if (createNewConnection(points[i], points[connectto])) {
                                    stillgoing = false;
                                }
                            }
                        }
                        connectto--;
                        if (connectto < 0) {
                            stillgoing = false;
                            possible = false;
                        }
                    }
                }
            }

            return possible;
        }

        public bool[,] convertToBoolArray() {
            bool[,] data = new bool[points.Count(), points.Count()];

            for (int x = 0; x < points.Count(); x++) {
                for (int y = 0; y < points.Count(); y++) {
                    bool t = false;
                    for (int i = 0; i < connections.Count(); i++) {
                        if (x != y) {
                            if (connections[i].isPointConnected(points[x]) && connections[i].isPointConnected(points[y])) {
                                t = true;
                            }
                        } else {
                            if (connections[i].getId1() == points[x].getId() && connections[i].getId2() == points[y].getId()) {
                                t = true;
                            }
                        }
                    }
                    data[x, y] = t;
                }
            }

            return data;
        }

        public string boolString() {
            return boolString(true);
        }

        public string boolString(bool triangular) {
            string data = points.Count() + "";
            bool[,] bdata = convertToBoolArray();

            for (int x = 0; x < points.Count(); x++) {
                data += "\n";
                for (int y = 0; y < points.Count(); y++) {
                    data += " ";
                    if (y >= x || !triangular) {
                        if (bdata[x, y]) {
                            data += "1";
                        } else {
                            data += "0";
                        }
                    } else {
                        data += "0";
                    }
                }
            }

            return data;
        }

        public void importFromStringData(string data) {
            reset();
            string[] lines = data.Split('\n');
            int pointcount = Int32.Parse(lines[0]);
            for (int i = 0; i < pointcount; i++) {
                createNewPoint();
            }

            for (int x = 0; x < pointcount; x++) {
                string[] dmt = lines[x + 1].Split(' ');
                for (int y = 0; y < pointcount; y++) {
                    if (dmt[y + 1] != "_") {
                        if (dmt[y + 1] != "0") {
                            createNewConnection(points[x], points[y]);
                        }
                    }
                }
            }
        }

        public Graph clone() {
            Graph gp = new Graph();

            for (int i = 0; i < points.Count(); i++) {
                gp.createNewPoint(points[i].getId());
            }

            for (int i = 0; i < connections.Count(); i++) {
                gp.createNewConnection(connections[i].getId1(), connections[i].getId2());
            }

            return gp;
        }

        public int countComponents() {
            int components = 0;
            List<int> rem = new List<int>();
            for (int i = 0; i < points.Count(); i++)
                rem.Add(i);

            while (rem.Count() > 0) {
                int startpoint = rem[0];
                List<int> callstack = new List<int>();
                callstack.Add(startpoint);
                while (callstack.Count() > 0) {
                    rem.Remove(callstack[0]);
                    for (int i = 0; i < connections.Count(); i++) {
                        if (connections[i].isPointConnected(points[callstack[0]])) {
                            int id = getId(connections[i].getId1());
                            if (rem.Contains(id)) {
                                callstack.Add(id);
                            }
                            id = getId(connections[i].getId2());
                            if (rem.Contains(id)) {
                                callstack.Add(id);
                            }
                        }
                    }
                    callstack.RemoveAt(0);
                }
                components++;
            }

            return components;
        }

        public Graph getLineGraph() {
            Graph re = new Graph();

            for (int i = 0; i < connections.Count(); i++) {
                re.createNewPoint();
            }

            for (int x = 0; x < re.getPoints().Count(); x++) {
                for (int y = 0; y < re.getPoints().Count(); y++) {
                    if (x != y) {
                        if (connections[x].isPointConnected(connections[y].getId1()) || connections[x].isPointConnected(connections[y].getId2())) {
                            re.createNewConnection(re.getPoints()[x], re.getPoints()[y]);
                        }
                    }
                }
            }

            return re;
        }

        public bool isGraphic(int[] graphpoints) {
            Array.Sort(graphpoints);
            Array.Reverse(graphpoints);

            if (graphpoints.Length == 0) {
                return true;
            }

            List<int> temp = graphpoints.ToList();
            temp.RemoveAt(0);
            if (graphpoints[0] > temp.Count()) {
                return false;
            }
            for (int i = 0; i < graphpoints[0]; i++) {
                temp[i]--;
            }
            int[] newpoints = temp.ToArray();

            return isGraphic(newpoints);
        }

        public static bool operator ==(Graph c1, Graph c2) {
            return c1.isMatching(c2);
        }

        public static bool operator !=(Graph c1, Graph c2) {
            return !c1.isMatching(c2);
        }

        public static bool operator ^(Graph c1, Graph c2) {
            return c1.isIsomorphWith(c2);
        }

        public static Graph operator ~(Graph c1) {
            return c1.getComplementer();
        }

        public override bool Equals(object c1) {
            if (c1.GetType() == typeof(Graph)) {
                return this ^ (Graph)c1;
            }
            return false;
        }

        public override int GetHashCode() {
            return convertToBoolArray().GetHashCode();
        }

    }
}
