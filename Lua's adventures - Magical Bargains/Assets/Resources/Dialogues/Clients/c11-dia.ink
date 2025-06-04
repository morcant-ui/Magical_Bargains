-> a
-> b
-> c
-> d
-> e
-> f 
-> g
//-> h 
-> i


=== default ===
This is the default knot, we should not be reading this in game
-> DONE

=== a ===
Greetings...
This is the Box of Pandora.
You give me money, I give you destiny.
Do you have a question?
* "A lot..."
    Time is not something I trade in.
    -> DONE
* "Nope!"
    Perfect
    -> DONE

=== b ===
//The client accepts your offer
A wise decision. May fate smile upon your recklessness.
-> DONE

=== c ===
//The client thinks your offer is too low and refuses the deal
You dare scorn the power sealed within? Your wallet isn’t ready for greatness.
-> DONE

=== d ===
//You don't have enough money and the client refuses the deal
Ah. A shame. Destiny remains sealed... for now.
-> DONE

=== e ===
//You refused the client's offer
You mock the box, but one day it may mock you. Good luck.
-> DONE

=== f ===
//You trying to bargain
Fate does not haggle. But... perhaps we bend the thread just this once.
Do you wish to bargain again?
* "Yes, let's bargain again" 
    -> DONE
* "No thanks!" 
    -> DONE
-> DONE

=== g ===
// there was a way to bargain but you missed
The moment passed. The box remains... unopened.
-> DONE

//=== h ===
// = false -> there was no way to bargain for a decreased offer (but could maybe offer more money) (so you missed also)
// not possible in this case
//-> DONE

=== i ===
// congrats you found the scam
Alright, alright... it’s just a box. No magic But hey! You bought the performance, didn't you?
-> DONE

