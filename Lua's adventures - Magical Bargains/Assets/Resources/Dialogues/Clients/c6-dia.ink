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
Good afternoon...
Please, buy this thing from me! It appeared from my hat one day and just won’t leave me alone! It’s scary and annoying!
It keeps teleporting everywhere at home. I’m sure someone would be interested... it’s very powerful magic... yes...
* "It teleports? Sounds cool!"
    I assure you, it really isn’t...
    -> DONE
* "Scary how?"
    It’s the most terrifying thing I’ve ever seen! It won’t leave me alone all day, it’s always watching me!
    -> DONE

=== b ===
You’re my savior, thank you! I can finally sleep at night without hearing teleporting noises everywhere!
-> DONE

=== c ===
Even I have standards... I can’t let it go for so little money after all the trouble it’s caused me...
-> DONE

=== d ===
Did all your money teleport away too? Not my lucky day...
-> DONE

=== e ===
//You refused the client's offer
NOOOOOOOOOOO! Why would you leave me with it?!?!
-> DONE

=== f ===
//You trying to bargain
Mmmmh, grrrr, can’t you offer just a little more? I’m sure it’s still worth its price, no?
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
It is powerful magic, I told you! Why won’t you believe me??? I’m out!
-> DONE

//=== i ===
// congrats you found the scam
// not possible here
//-> DONE

