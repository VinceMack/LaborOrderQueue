struct LaborOrder  {

    public int x, y;
    public int ttc;
    public string[] actions;
    public string type;
    public int orderNumber;

    public LaborOrder(int x, int y, int ttc, string[] actions, string type, int orderNumber) {
        this.x = x;
        this.y = y;
        this.ttc = ttc;
        this.actions = actions;
        this.type = type;
        this.orderNumber = orderNumber;
    }

}

class Pawn  {

    enum PawnType { storage, forage, generic };

    public string name;
    public string type = "generic";
    public LaborOrder currentOrder;
    public Boolean isTypeExclusive;
    
    public Pawn(string type, string name, Boolean isTypeExclusive) {
        this.type = type;
        this.name = name;
        this.isTypeExclusive = isTypeExclusive;

        if(isTypeExclusive){
            this.name = name + "_te";
        }
    }

}

class Program {

    static void Main(string[] args) {

        // list of names no longer than 7 characters
        string[] firstNames = new string[] {
            "Aiden", "Alex", "Alexis", "Alyssa", "Andrew", "Anthony", "Austin", "Benjamin", "Blake", "Brandon", "Brayden", "Brianna", "Brooke", "Caleb", "Cameron", "Carson", "Carter", "Chloe", "Christian", "Christopher", "Daniel", "David", "Dylan", "Elijah", "Ethan", "Evan", "Gabriel", "Gavin", "Grace", "Hailey", "Hannah", "Hunter", "Isaac", "Isabella", "Jacob", "Jaden", "Jasmine", "Jayden", "Jessica", "John", "Jonathan", "Jordan", "Joseph", "Joshua", "Julia", "Justin", "Kaitlyn", "Kayla", "Kevin", "Landon", "Lauren", "Liam", "Logan", "Lucas", "Luke", "Madison", "Mason", "Matthew", "Megan", "Michael", "Morgan", "Nathan", "Nicholas", "Noah", "Olivia", "Owen", "Peyton", "Rachel", "Riley", "Robert", "Ryan", "Samantha", "Sarah", "Savannah", "Sean", "Sophia", "Sophie", "Taylor", "Thomas", "Tyler", "Victoria", "William", "Zachary"
        };

        // create a list of pawns called "workingPawns"
        List<Pawn> workingPawns = new List<Pawn>();

        // create a list of pawns called "freePawns"
        List<Pawn> freePawns = new List<Pawn>();

        // create a FIFO queue of labor orders called "laborOrders"
        Queue<LaborOrder> laborOrders = new Queue<LaborOrder>();

        // fill the list of 10 free pawns: give pawns random names. make 2 storage type pawns, make 1 type exclusive. make 2 forage type pawns, make 1 type exclusive. the rest are generic type pawns.
        for (int i = 0; i < 10; i++) {
            if (i == 0) {
                freePawns.Add(new Pawn("storage", firstNames[new Random().Next(0, firstNames.Length)], true));
            } else if (i == 1) {
                freePawns.Add(new Pawn("storage", firstNames[new Random().Next(0, firstNames.Length)], false));
            } else if (i == 2) {
                freePawns.Add(new Pawn("forage", firstNames[new Random().Next(0, firstNames.Length)], true));
            } else if (i == 3) {
                freePawns.Add(new Pawn("forage", firstNames[new Random().Next(0, firstNames.Length)], false));
            } else {
                freePawns.Add(new Pawn("generic", firstNames[new Random().Next(0, firstNames.Length)], false));
            }
        }


        // fill the queue of labor orders with 25 labor orders randomly assigned to either "storage", "forage", or "generic" as their type (other properties can be 0; other than tcc which is random between 1000 and 10000)
        for (int i = 0; i < 25; i++) {
            int type = new Random().Next(0, 3);
            if (type == 0) {
                laborOrders.Enqueue(new LaborOrder(0, 0, new Random().Next(3000, 5000), new string[] { }, "storage", i+1));
            } else if (type == 1) {
                laborOrders.Enqueue(new LaborOrder(0, 0, new Random().Next(3000, 5000), new string[] { }, "forage", i+1));
            } else {
                laborOrders.Enqueue(new LaborOrder(0, 0, new Random().Next(3000, 5000), new string[] { }, "generic", i+1));
            }
        }

        // while there are labor orders in the queue:
        // assign the first labor order in the queue to the first free pawn in the list of free pawns
        // store a temporary reference to the pawn and add it to the list of working pawns; remove it from the list of free pawns.
        // start a new thread and have it sleep for the time to complete of the labor order for the pawn
        // when the thread wakes up, add the pawn back to the list of free pawns and remove it from the list of working pawns
        Mutex mutex = new Mutex();
        while (laborOrders.Count > 0) {

            while(!freePawns.Any()){
                // wait for a free pawn
                //Console.WriteLine("Waiting for a free pawn...");
            }

            mutex.WaitOne();

            // find a free pawn that matches the next order's type and is type exclusive (i.e. give priority to type exclusive pawns)
            Pawn? pawn = freePawns.Find(p => p.type == laborOrders.Peek().type && p.isTypeExclusive);

            // find a free pawn that matches the next order's type
            if(pawn == null){
                pawn = freePawns.Find(p => p.type == laborOrders.Peek().type);
            }

            // if no free pawn matches the next order's type, find a free pawn that is generic
            if(pawn == null){
                pawn = freePawns.Find(p => p.type == "generic");
            }

            // no valid pawns; skip this iteration and try again.
            if(pawn == null){
                // no free pawn matches the next order's type or is generic
                mutex.ReleaseMutex();
                continue;
            }

            // print out the number of free pawns of each type
            Console.WriteLine("Free pawns: " + freePawns.Count(p => p.type == "storage") + " storage, " + freePawns.Count(p => p.type == "forage") + " forage, " + freePawns.Count(p => p.type == "generic") + " generic");

            // if we reached this point: we have a valid pawn to work on the next order
            pawn.currentOrder = laborOrders.Dequeue();
            freePawns.Remove(pawn);
            workingPawns.Add(pawn);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0,-25}{1,-40}{2}", pawn.name + " (" + pawn.type + ")", " is working on a " + pawn.type + " order", "("+pawn.currentOrder.orderNumber+")");
            Console.ResetColor();
            mutex.ReleaseMutex();

            Thread thread = new Thread(() => { // this where the LaborOrder would be passed to the PawnAI to process/complete.
                Thread.Sleep(pawn.currentOrder.ttc);
                mutex.WaitOne();
                freePawns.Add(pawn);
                workingPawns.Remove(pawn);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0,-25}{1,-40}{2}", pawn.name + " (" + pawn.type + ")", " is done working on a " + pawn.type + " order", "("+pawn.currentOrder.orderNumber+")");
                Console.ResetColor();
                mutex.ReleaseMutex();
            }); 
            thread.Start();
        }
    }
}