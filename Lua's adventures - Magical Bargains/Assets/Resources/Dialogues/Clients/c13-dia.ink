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
Behold...
Me...
Here is...
Bee pendant...
To summon infinite bees... and restore peace on earth...
Bees communicate... through dance...
So elegant... So powerful...
* "What are you?"
    Rude...
    -> DONE
* "Sounds dangerous?"
    Bees... good creatures... peaceful...
    -> DONE

=== b ===
//The client accepts your offer
Good... trade...
-> DONE

=== c ===
//The client thinks your offer is too low and refuses the deal
Offer... too low...
-> DONE

=== d ===
//You don't have enough money and the client refuses the deal
You... have no coins?
-> DONE

=== e ===
//You refused the client's offer
You... missed great deal...
-> DONE

=== f ===
//You trying to bargain
Me... need more...
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
You... fool... Bees are never the problem...
-> DONE

//=== i ===
// congrats you found the scam
// not possible here
//-> DONE

