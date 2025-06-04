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
...
......
Strong genie in here... very valuable...
* "..."
    ....... Don't have all day...
    -> DONE
* "So many genies!"
    Only surviving genie... Very rare... Very valuable...
    -> DONE

=== b ===
//The client accepts your offer
I accept this sum... I hope he won't be too much trouble for this shop... 
-> DONE

=== c ===
//The client thinks your offer is too low and refuses the deal
This should sell for higher... You rat!
-> DONE

=== d ===
//You don't have enough money and the client refuses the deal
Don't make propositions you can't honor... Bye.
-> DONE

=== e ===
//You refused the client's offer
I understand... The bad energy submerged you... happens all the time...
-> DONE

=== f ===
//You trying to bargain
This sum won't do...
Do you wish to bargain again?
* "Yes, let's bargain again" 
    -> DONE
* "No thanks!" 
    -> DONE
-> DONE

=== g ===
// there was a way to bargain but you missed
Genie is totally safe, what are you even saying...? Surely you didn't see anything of that sort...
-> DONE

//=== h ===
// = false -> there was no way to bargain for a decreased offer (but could maybe offer more money) (so you missed also)
// not possible here
//-> DONE

=== i ===
// congrats you found the scam
Well... yes... genie a bit evil... take it fast! He doesn't like waiting. I go now!
-> DONE
