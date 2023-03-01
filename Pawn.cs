using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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