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
Goooood evening young little child!
I bring you a powerful magical staff...
It was a toothpick, yes. But now? It’s a staff!
Very powerful, it could break the sea in two! True fact! For real now. Pleaaase buy it?
AAAaaaA! Be very careful while handling it, yes?
* "A toothpick?"
    Yes, you know? That thing you use to remove any extra food in between your teeth after a meal? It's never been used, I swear!
    -> DONE
* "Careful?"
    Yes, please avoid touching the tip of it!
    -> DONE

=== b ===
//The client accepts your offer
This amount satisfies me, child. Do me a favor... don’t put it in your mouth... unless you enjoy instant regret.
-> DONE

=== c ===
//The client thinks your offer is too low and refuses the deal
What? Only this? Do you think my mighty staff is a joke? Hmph, I’m leaving!
-> DONE

=== d ===
//You don't have enough money and the client refuses the deal
You offer coins you do not have! This is blasphemy! Or worse, rudeness! I cannot bargain with such unprofessional people!
-> DONE

=== e ===
//You refused the client's offer
... Why would you refuse? It is so powerful... Give it back! WAIT! Don't touch the tip of it!
-> DONE

=== f ===
//You trying to bargain
Hmm... this amount does not satisfy me... Is there a way to do better?
Do you wish to bargain again?
* "Yes, let's bargain again" 
    -> DONE
* "No thanks!" 
    -> DONE
-> DONE

=== g ===
// there was a way to bargain but you missed
No no, that’s truly not the case! My staff is lacking nothing in this area!
-> DONE

//=== h ===
// = false -> there was no way to bargain for a decreased offer (but could maybe offer more money) (so you missed also)
//-> DONE

=== i ===
// congrats you found the scam
Fine... you win. There is poison on it... I will give it to you for the offered amount...
-> DONE

