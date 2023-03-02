using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

// clear ; dotnet run ; dotnet clean > output_redirect.txt

struct LaborOrder  {

    // create a types enum using basic, doctor, haul, craft, forage, or farm
    public enum Types { basic, doctor, haul, craft, forage, farm };

    public int x;                       // 0 to 100
    public int y;                       // 0 to 100
    public int ttc;                     // time to complete, in ticks, 1 to 2 seconds
    public Types type;                  // the type of labor order 
    public int orderNumber;             // the order number of the labor order
    public static int orderCount = 0;   // this is just for this program's output; not relevant to Unity solution
    //public string[] actions;          // list of actions that needs to be performed

    // constructor
    public LaborOrder(int x, int y, int ttc, Types type) {
        this.x = x;
        this.y = y;
        this.ttc = ttc;
        this.type = type;
        orderNumber = orderCount++;
    }

    // randomized constructor
    public LaborOrder(bool isRandom) {
        x = new Random().Next(0, 101);
        y = new Random().Next(0, 101);
        ttc = new Random().Next(100, 500);
        type = (Types)new Random().Next(0, 6);
        orderNumber = orderCount++;
    }
}

class Pawn  {

    public string name;
    public LaborOrder? currentOrder = null;
    public List<LaborOrder.Types> priorities = new List<LaborOrder.Types>();

    // randomized constructor
    public Pawn(bool isRandom){
        string[] firstNames = {"James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas", "Christopher", "Daniel", "Paul", "Mark", "Donald", "George", "Kenneth", "Steven", "Edward", "Brian", "Ronald", "Anthony", "Kevin", "Jason", "Matthew", "Gary", "Timothy", "Jose", "Larry", "Jeffrey", "Frank", "Scott", "Eric", "Stephen", "Andrew", "Raymond", "Gregory", "Joshua", "Jerry", "Dennis", "Walter", "Patrick", "Peter", "Harold", "Douglas", "Henry", "Carl", "Arthur", "Ryan", "Roger", "Joe", "Juan", "Jack", "Albert", "Jonathan", "Justin", "Terry", "Gerald", "Keith", "Samuel", "Willie", "Ralph", "Lawrence", "Nicholas", "Roy", "Benjamin", "Bruce", "Brandon", "Adam", "Harry", "Fred", "Wayne", "Billy", "Steve", "Louis", "Jeremy", "Aaron", "Randy", "Howard", "Eugene", "Carlos", "Russell", "Bobby", "Victor", "Martin", "Ernest", "Phillip", "Todd", "Jesse", "Craig", "Alan", "Shawn", "Clarence", "Sean", "Philip", "Chris", "Johnny", "Earl", "Jimmy", "Antonio", "Danny", "Bryan", "Tony", "Luis", "Mike", "Stanley", "Leonard", "Nathan", "Dale", "Manuel", "Rodney", "Curtis", "Norman", "Allen", "Marvin", "Glenn", "Jeffery", "Travis", "Jeff", "Chad", "Jacob", "Lee", "Melvin", "Alfred", "Kyle", "Francis", "Bradley", "Jesus", "Herbert", "Frederick", "Ray", "Joel", "Edwin", "Don", "Eddie", "Ricky", "Troy", "Randall", "Barry", "Alexander" };
        name = firstNames[new Random().Next(0, firstNames.Length)];
        for (int i = 0; i < 6; i++) {
            priorities.Add((LaborOrder.Types)new Random().Next(0, 6));
        }
    }

}

class LaborOrderQueue {

    static void Main(string[] args) {

        ConsoleColor[] colors = {
            ConsoleColor.Black,
            ConsoleColor.DarkBlue,
            ConsoleColor.DarkCyan,
            ConsoleColor.DarkRed,
            ConsoleColor.DarkGreen,
            ConsoleColor.DarkMagenta,
            ConsoleColor.DarkYellow,
            ConsoleColor.Gray,
            ConsoleColor.DarkGray,
            ConsoleColor.Blue,
            ConsoleColor.Cyan,
            ConsoleColor.Red,
            ConsoleColor.Magenta,
            ConsoleColor.Yellow,
        };


        Mutex mutex = new Mutex();

        // create a list of labor orders
        List<LaborOrder> orders = new List<LaborOrder>();

        // create a list of pawns
        List<Pawn> pawns = new List<Pawn>();

        // populate the lists with randomized data (using randomized constructors)
        for (int i = 0; i < 100; i++) {
            orders.Add(new LaborOrder(true));
        }
        for (int i = 0; i < 10; i++) {
            pawns.Add(new Pawn(true));
        }
        
        // Iterate through the list of pawns forever.
        // For each pawn that doesn't have a currentOrder, iterate through their list of priorities.
        // For each priority, iterate through the list of labor orders.
        // If the current labor order matches the current priority, assign the labor order to the pawn and complete the labor order using a thread. Continue to the next pawn.
        // If the current labor order doesn't match the current priority, continue to the next labor order.
        // If all the labor orders don't match any priority, do nothing. Continue to the next pawn.
        // If all the pawns have currentOrders, do nothing. Continue to the next iteration and wait for one to be available.
        int colorCount = 0;
        while (orders.Count > 0) {
            foreach (Pawn pawn in pawns) {
                if (pawn.currentOrder == null) {
                    foreach (LaborOrder.Types priority in pawn.priorities) {
                        foreach (LaborOrder order in orders) {
                            if (order.type == priority) {
                                pawn.currentOrder = order;
                                orders.Remove(order);
                                Thread thread = new Thread(() => {
                                
                                    // simulate the pawn working on the order
                                    mutex.WaitOne();
                                    ConsoleColor color = colors[colorCount++ % colors.Length];
                                    Console.ForegroundColor = color;
                                    Console.WriteLine($"STARTING: {pawn.name,-11} is working on {order.type, -6} order {order.orderNumber}");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    mutex.ReleaseMutex();
                                    Thread.Sleep(order.ttc);
                                    mutex.WaitOne();
                                    Console.ForegroundColor = color;
                                    Console.WriteLine($"ENDING:   {pawn.name,-11} has finished  {order.type, -6} order {order.orderNumber}");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    mutex.ReleaseMutex();
                                    pawn.currentOrder = null;
                                });
                                thread.Start();
                                break;
                            }
                        }
                        if (pawn.currentOrder != null) {
                            break;
                        }
                    }
                }
            }
            
            //Console.WriteLine(orders.Count);
        }

        // Thread.Sleep(1000); // janky sleep to wait for the last thread to finish
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\x1b[1mThere are no more orders left to assign.\x1b[0m");
        Console.ForegroundColor = ConsoleColor.White;
    }
}