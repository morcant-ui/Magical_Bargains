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
Hello fellow adventurer!
I just returned from my last trip!
I brought back a very interesting item, a compass that always points to a treasure!
Would you buy it? I need money for my next adventures!
* "An adventurer cat?"
    Yes... We pretty much all are adventurers! Oh... maybe not those domesticated cats though...
    -> DONE
* "Isn't it useful to you?"
    No, it makes my adventures too easy. I want dangers, getting lost, not just following a path!
    -> DONE

=== b ===
//The client accepts your offer
This sounds reasonable! I will be able to take the next boat for the Wonderland Land without trouble! 
-> DONE

=== c ===
//The client thinks your offer is too low and refuses the deal
Hm... I will try my luck elsewhere, I need more money to depart!
-> DONE

=== d ===
//You don't have enough money and the client refuses the deal
That's generous, but you don't even have that kind of money! I'll go elsewhere!
-> DONE

=== e ===
//You refused the client's offer
I don't see why you would refuse, this would make you rich!
-> DONE

=== f ===
//You trying to bargain
Can't you go a bit higher? Boat fees are quite expensive and I can't just swim to my next adventure... I hate water...
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
Not as sharp as they say, are you? This compass could make anyone rich and you doubt it? I'm off!
-> DONE

//=== i ===
// congrats you found the scam
// not possible here
//-> DONE
