using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

struct LaborOrder  {

    public int x, y;
    public int ttc;
    public string[] actions;
    public string type;
    public int orderNumber; // this is just for this program's output; not relevant to Unity solution

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

class PawnLevelQueue {

    static void Main(string[] args) {

    }

}