// dotnet build -o .

struct LaborOrder  {

    public int x, y;
    public int ttc;
    public string[] actions;
    public string type;

    public LaborOrder(int x, int y, int ttc, string[] actions, string type) {
        this.x = x;
        this.y = y;
        this.ttc = ttc;
        this.actions = actions;
        this.type = type;
    }

}

class Pawn  {

    public string type;
    public LaborOrder currentOrder;
    
    public Pawn(string type) {
        this.type = type;
    }

}

class Program {

    static void Main(string[] args) {

        // create a list of pawns called "workingPawns"
        List<Pawn> workingPawns = new List<Pawn>();

        // create a list of pawns called "freePawns"
        List<Pawn> freePawns = new List<Pawn>();

        // create a FIFO queue of labor orders called "laborOrders"
        Queue<LaborOrder> laborOrders = new Queue<LaborOrder>();

        // fill the list of free pawns with 10 pawns randomly assigned to either "storage", "forage", or "generic" as their type
        for (int i = 0; i < 10; i++) {
            int type = new Random().Next(0, 3);
            if (type == 0) {
                freePawns.Add(new Pawn("storage"));
            } else if (type == 1) {
                freePawns.Add(new Pawn("forage"));
            } else {
                freePawns.Add(new Pawn("generic"));
            }
        }

        // fill the queue of labor orders with 25 labor orders randomly assigned to either "storage", "forage", or "generic" as their type (other properties can be 0)
        for (int i = 0; i < 25; i++) {
            int type = new Random().Next(0, 3);
            if (type == 0) {
                laborOrders.Enqueue(new LaborOrder(0, 0, 0, new string[0], "storage"));
            } else if (type == 1) {
                laborOrders.Enqueue(new LaborOrder(0, 0, 0, new string[0], "forage"));
            } else {
                laborOrders.Enqueue(new LaborOrder(0, 0, 0, new string[0], "generic"));
            }
        }

        // while there are labor orders in the queue:
        while (laborOrders.Count > 0) {

            // if there are free pawns:
            if (freePawns.Count > 0) {

                // dequeue a labor order from the queue
                LaborOrder laborOrder = laborOrders.Dequeue();

                // assign the labor order to the first free pawn
                freePawns[0].currentOrder = laborOrder;

                // add the pawn to the list of working pawns
                workingPawns.Add(freePawns[0]);

                // remove the pawn from the list of free pawns
                freePawns.RemoveAt(0);
                
                //print the number of free pawns and the number of labor orders in the queue
                Console.WriteLine("Free pawns: " + freePawns.Count + " Labor orders: " + laborOrders.Count);

                // add the working pawn back to the list of free pawns
                freePawns.Add(workingPawns[0]);

            }

        }
    }
}