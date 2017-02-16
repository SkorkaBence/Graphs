using System;

namespace Graphs {
    public class GraphPoint {

        private string id;
        private Random rnd;

        public GraphPoint(Random r) {
            rnd = r;
            id = rnd.Next(10000, 99999) + "-" + rnd.Next(10000, 99999);
        }

        public GraphPoint(Random r, string id) {
            rnd = r;
            this.id = id;
        }

        public string getId() {
            return id;
        }

    }
}