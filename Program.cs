using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

// clear ; dotnet run ; dotnet clean > output_redirect.txt

// enum for labor types
public enum LaborTypes { FireFight, Patient, Doctor, Sleep, Basic, Warden, Handle, Cook, Hunt, Construct, Grow, Mine, Farm, Woodcut, Smith, Tailor, Art, Craft, Haul, Clean, Research };

public struct LaborOrder  {

    // labor type
    public LaborTypes laborType;

    // time to complete the labor order
    public int timeToComplete;

    // random (default) constructor
    public LaborOrder() {
        // assign a random labor type
        laborType = (LaborTypes)new Random().Next(0, Enum.GetValues(typeof(LaborTypes)).Length);
        // assign a random time to complete
        timeToComplete = new Random().Next(5, 50);
    }
}

public class Pawn  {

    // name
    public string? name;

    // current labor order to fulfill
    public LaborOrder? currentLaborOrder;

    // array of lists of labor types
    public List<LaborTypes>[] laborTypeLists = new List<LaborTypes>[4];

    public Pawn(){

        // assign a random name
        string[] names = {"James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas", "Christopher", "Daniel", "Paul", "Mark", "Donald", "George", "Kenneth", "Steven", "Edward", "Brian", "Ronald", "Anthony", "Kevin", "Jason", "Matthew", "Gary", "Timothy", "Jose", "Larry", "Jeffrey", "Frank", "Scott", "Eric", "Stephen", "Andrew", "Raymond", "Gregory", "Joshua", "Jerry", "Dennis", "Walter", "Patrick", "Peter", "Harold", "Douglas", "Henry", "Carl", "Arthur", "Ryan", "Roger", "Joe", "Juan", "Jack", "Albert", "Jonathan", "Justin", "Terry", "Gerald", "Keith", "Samuel", "Willie", "Ralph", "Lawrence", "Nicholas", "Roy", "Benjamin", "Bruce", "Brandon", "Adam", "Harry", "Fred", "Wayne", "Billy", "Steve", "Louis", "Jeremy", "Aaron", "Randy", "Howard", "Eugene", "Carlos", "Russell", "Bobby", "Victor", "Martin", "Ernest", "Phillip", "Todd", "Jesse", "Craig", "Alan", "Shawn", "Clarence", "Sean", "Philip", "Chris", "Johnny", "Earl", "Jimmy", "Antonio", "Danny", "Bryan", "Tony", "Luis", "Mike", "Stanley", "Leonard", "Nathan", "Dale", "Manuel", "Rodney", "Curtis", "Norman", "Allen", "Marvin", "Glenn", "Jeffery", "Travis", "Jeff", "Chad", "Jacob", "Lee", "Melvin", "Alfred", "Kyle", "Francis", "Bradley", "Jesus", "Herbert", "Frederick", "Ray", "Joel", "Edwin", "Don", "Eddie", "Ricky", "Troy", "Randall", "Barry", "Alexander" };
        name = names[new Random().Next(0, names.Length)];

        // set default labor order to null
        currentLaborOrder = null;

        // initialize the labor type lists
        for (int i = 0; i < laborTypeLists.Length; i++) {
            laborTypeLists[i] = new List<LaborTypes>();
        }

        // Loop through the labor types and append them to a random labor type list
        foreach (LaborTypes laborType in Enum.GetValues(typeof(LaborTypes))) {
            laborTypeLists[new Random().Next(0, laborTypeLists.Length)].Add(laborType);
        }

        //// print out the pawn's name and labor type lists
        //Console.WriteLine("Pawn name: " + name);
        //for (int i = 0; i < laborTypeLists.Length; i++) {
        //    Console.WriteLine("Labor type list " + (i+1) + ": " + string.Join(", ", laborTypeLists[i]));
        //}
    }
}

class QueueManager {

    // list of queues
    public List<Queue<LaborOrder>> laborQueues = new List<Queue<LaborOrder>>();

    // create a queue for each labor type
    public QueueManager() {

        // create a queue for each labor type
        foreach (LaborTypes laborType in Enum.GetValues(typeof(LaborTypes))) {
            laborQueues.Add(new Queue<LaborOrder>());
        }

        /// generate 100 random labor orders and add them to the queues that match their labor type
        for (int i = 0; i < 100; i++) {
            LaborOrder laborOrder = new LaborOrder();
            laborQueues[(int)laborOrder.laborType].Enqueue(laborOrder);
        }

        /// Iterate through the laborQueues list. For each queue, print out the labor type and the number of labor orders in the queue.
        //for (int i = 0; i < laborQueues.Count; i++) {
        //    Console.WriteLine($"Labor type: {(LaborTypes)i,-9} | Queue count: {laborQueues[i].Count}");
        //}
    }

}

class Program {

    static void Main(string[] args) {

        // create a list of pawns
        List<Pawn> pawns = new List<Pawn>();

        // add 15 pawns to the list of pawns
        for (int i = 0; i < 15; i++) {
            pawns.Add(new Pawn());
        }

        // create a queue manager
        QueueManager queueManager = new QueueManager();

        // While there are still labor orders to be fulfilled:
        //  Loop through the list of pawns. For each pawn:
        //      If the pawn's current labor order is null, find a labor order for the pawn.
        //          search the labor queues for a labor order that matches the first labor type in the pawn's labor type list at index 0
        //          if found, assign the labor order to the pawn's current labor order and remove the labor order from the queue
        //          create a thread to simulate the pawn completing the labor order by sleeping for the time to complete
        //          if not found, increment the index by 1 and search that labor type queue
        //          if the index is ever greater than the number of labor type lists, do nothing and move on to the next pawn
        //      If the pawn's current labor order is not null, do nothing and move on to the next pawn
        //  Sleep for 1 second
        
        // While there are still labor orders to be fulfilled:
        while (queueManager.laborQueues.Any(queue => queue.Count > 0)) {

            // Loop through the list of pawns. For each pawn:
            foreach (Pawn pawn in pawns) {

                // If the pawn's current labor order is null, find a labor order for the pawn.
                if (pawn.currentLaborOrder == null) {

                    // search the labor queues for a labor order that matches the first labor type in the pawn's labor type list at index 0
                    for (int i = 0; i < pawn.laborTypeLists.Length; i++) {

                        // if found, assign the labor order to the pawn's current labor order and remove the labor order from the queue
                        if (queueManager.laborQueues[(int)pawn.laborTypeLists[i][0]].Count > 0) {
                            pawn.currentLaborOrder = queueManager.laborQueues[(int)pawn.laborTypeLists[i][0]].Dequeue();
                            Console.WriteLine($"{pawn.name} is now fulfilling a labor order of type {pawn.currentLaborOrder.laborType}.");
                            Thread thread = new Thread(() => {
                                Thread.Sleep(pawn.currentLaborOrder.timeToComplete);
                                Console.WriteLine($"{pawn.name} has completed a labor order of type {pawn.currentLaborOrder.laborType}.");
                                pawn.currentLaborOrder = null;
                            });
                            break;
                        }
                    }
                }
            }

            // Sleep for 1 second
            Thread.Sleep(1000);
        }

    }
}