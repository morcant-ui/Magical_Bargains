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
Greetings, young one. How are you today? I bring you a curious treasure.
Legend has it, if you play it just right, it calls forth a horde of invisible critters that will fight for you..
It is hard to demonstrate since the little critters cannot be seen, but believe me, they are there. I've felt them stir.
* "How do you play it?"
    I am merely the bearer of the tale. I don’t know the exact method... That’s why I’m parting with it, in hopes that someone who does will find it.
    -> DONE
* "That's cruel!"
    Mmm… it’s true. Calling on others to fight your battles is not the kindest path... But such is the nature of many old magics.
    -> DONE

=== b ===
Thank you, little one. I hope you grandpa will find a good home for the little creatures.
-> DONE

=== c ===
Oh, honey, I’m afraid I can’t go quite that low. I’ll return another day 
-> DONE

=== d ===
Oh dear... no money today? That’s quite alright. I’ll come back another time, maybe fortune will smile on you then.
-> DONE

=== e ===
//You refused the client's offer
No worries at all! I know not every forgotten bundle finds a home right away.
-> DONE

=== f ===
//You trying to bargain
Oh, sweetie, this amount isn’t quite right. Think on it a bit, won’t you?
Do you wish to bargain again?
* "Yes, let's bargain again" 
    -> DONE
* "No thanks!" 
    -> DONE

//=== g ===
// there was a way to bargain but you missed
// not possible here
//-> DONE

=== h ===
// = false -> there was no way to bargain for a decreased offer (but could maybe offer more money) (so you missed also)
I’m sorry, little one, but truly, there’s nothing amiss with this instrument. Perhaps all you saw were the critters, patiently waiting their turn to protect their next musician.
-> DONE

//=== i ===
// congrats you found the scam
// not possible here
//-> DONE
