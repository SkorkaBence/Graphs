namespace Graphs {
    public class GraphConnection {

        private string id1;
        private string id2;

        public GraphConnection(string id1, string id2) {
            this.id1 = id1;
            this.id2 = id2;
        }

        public GraphConnection(GraphPoint gp1, GraphPoint gp2) {
            this.id1 = gp1.getId();
            this.id2 = gp2.getId();
        }

        public string getId1() {
            return id1;
        }

        public string getId2() {
            return id2;
        }

        public bool isPointConnected(GraphPoint id) {
            return isPointConnected(id.getId());
        }

        public bool isPointConnected(string id) {
            return (id1 == id) || (id2 == id);
        }

    }
}