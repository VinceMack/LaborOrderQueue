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

    public string name;
    public string type;
    public LaborOrder currentOrder;
    
    public Pawn(string type, string name) {
        this.type = type;
        this.name = name;
    }

}

class Program {

    static void Main(string[] args) {

        string[] firstNames = {"Oliver", "Emma", "Noah", "Ava", "William", "Sophia", "James", "Isabella", "Benjamin", "Mia", "Lucas", "Charlotte", "Henry", "Amelia", "Alexander", "Harper", "Ethan", "Evelyn", "Michael", "Abigail", "Daniel", "Emily", "Matthew", "Elizabeth", "Jacob", "Mila", "Liam", "Ella", "Mason", "Avery", "Daniel", "Madison", "Logan", "Scarlett", "Caleb", "Grace", "David", "Chloe", "Owen", "Victoria", "Jackson", "Riley", "Luke", "Aria", "Sebastian", "Lily", "Isaac", "Sofia", "Samuel", "Luna"};

        // create a list of pawns called "workingPawns"
        List<Pawn> workingPawns = new List<Pawn>();

        // create a list of pawns called "freePawns"
        List<Pawn> freePawns = new List<Pawn>();

        // create a FIFO queue of labor orders called "laborOrders"
        Queue<LaborOrder> laborOrders = new Queue<LaborOrder>();

        // fill the list of free pawns with 10 pawns randomly assigned to either "storage", "forage", or "generic" as their type and give them a random name
        for (int i = 0; i < 10; i++) {
            int type = new Random().Next(0, 3);
            if (type == 0) {
                freePawns.Add(new Pawn("storage", firstNames[new Random().Next(0, firstNames.Length)]));
            } else if (type == 1) {
                freePawns.Add(new Pawn("forage", firstNames[new Random().Next(0, firstNames.Length)]));
            } else {
                freePawns.Add(new Pawn("generic", firstNames[new Random().Next(0, firstNames.Length)]));
            }
        }

        // fill the queue of labor orders with 25 labor orders randomly assigned to either "storage", "forage", or "generic" as their type (other properties can be 0; other than tcc which is 15 seconds)
        for (int i = 0; i < 25; i++) {
            int type = new Random().Next(0, 3);
            if (type == 0) {
                laborOrders.Enqueue(new LaborOrder(0, 0, 15000, new string[0], "storage"));
            } else if (type == 1) {
                laborOrders.Enqueue(new LaborOrder(0, 0, 15000, new string[0], "forage"));
            } else {
                laborOrders.Enqueue(new LaborOrder(0, 0, 15000, new string[0], "generic"));
            }
        }

        // while there are labor orders in the queue:
        // assign the first labor order in the queue to the first free pawn in the list of free pawns
        // store a temporary reference to the pawn and add it to the list of working pawns; remove it from the list of free pawns.
        // start a new thread and have it sleep for the time to complete of the labor order for the pawn
        // when the thread wakes up, add the pawn back to the list of free pawns and remove it from the list of working pawns
        while (laborOrders.Count > 0 && freePawns.Count > 0) {
            Pawn pawn = freePawns[0];
            pawn.currentOrder = laborOrders.Dequeue();
            workingPawns.Add(pawn);
            freePawns.Remove(pawn);
            new Thread(() => {
                // print the name of the pawn and the type of labor order they are working on
                Console.WriteLine(pawn.name + " is working on a " + pawn.currentOrder.type + " order.");
                Thread.Sleep(pawn.currentOrder.ttc);
                freePawns.Add(pawn);
                workingPawns.Remove(pawn);
            }).Start();
        }
    }
}

/*
Sophia is working on a forage order.
Chloe is working on a storage order.
Emma is working on a forage order.
Alexander is working on a generic order.
Logan is working on a storage order.
Emma is working on a forage order.
Daniel is working on a forage order.
Sofia is working on a forage order.
Grace is working on a storage order.
Lucas is working on a storage order.
Unhandled exception. System.ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')
   at System.Collections.Generic.List`1.RemoveAt(Int32 index)
   at System.Collections.Generic.List`1.Remove(T item)
   at Program.<>c__DisplayClass0_1.<Main>b__0() in C:\Users\VAM\Desktop\Priority Event Queue\queue\Program.cs:line 85
PS C:\Users\VAM\Desktop\Priority Event Queue\queue> 
*/