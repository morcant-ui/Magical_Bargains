-> a
-> b
-> c
-> d
-> e
-> f 
//-> g
-> h 
//-> i

=== default ===
This is the default knot, we should not be reading this in game
-> DONE

=== a ===
....
I bring a powerful scroll. 
Of my own making.
It lifts a protective barrier... of quite large dimensions... 
A good offer, i insist...
* "A barrier?"
    ...
    -> DONE
* "Let me decide on that!"
    ...
    -> DONE

=== b ===
//The client accepts your offer
You won't regret it.
-> DONE

=== c ===
// Your offer is not enough for client
... Not enough coins. Bye then.
-> DONE

=== d ===
// You don't have enough money to make this offer
I see you have no money. Bye then.
-> DONE

=== e ===
//You refused the client's offer
... Bye then.
-> DONE

=== f ===
//You trying to bargain
... You'll have to do better.
Do you wish to bargain again?
* "Yes, let's bargain again" 
    -> DONE
* "No thanks!" 
    -> DONE
-> DONE

//=== g ===
// there was a way to bargain but you missed
// not possible here
//-> DONE

=== h ===
// = false -> there was no way to bargain for a decreased offer (but could maybe offer more money) (so you missed also)
... I trust my work. Check your tools.
-> DONE

//=== i ===
// congrats you found the scam
// not possible here
//-> DONE


