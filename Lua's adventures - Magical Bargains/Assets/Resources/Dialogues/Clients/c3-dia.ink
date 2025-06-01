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
Good afternoon, young lady. I'm here to sell my staff... It's a little old, but well cared for. I assure you, it'll fetch a good price!
* "What kind of staff?"
    Hah! Sweetie, some things don’t need explaining. You’ll understand the moment you lay eyes on it.
    -> DONE
* "How old?"
    Oh ho! That’s not something you ask a lady! Just because it’s old doesn’t mean it’s lost its sparkle!
    -> DONE

=== b ===
Right right, that’ll do nicely. Thank you, my dear, I may just come by again sometime!
-> DONE

=== c ===
Oh no no no… that won’t do. I’m not letting this beauty go for pocket change. Nice try, little toadstool!
-> DONE

=== d ===
That won’t do, little toadstool. You don’t have that kind of money. I’ll just flutter away and sell my treasures elsewhere!
-> DONE

=== e ===
//You refused the client's offer
The magic may have faded a little, but this antique still holds its worth, darling!
-> DONE

=== f ===
//You trying to bargain
That won’t do, little one. There’s a better offer waiting somewhere, I’m sure.
Do you wish to bargain again?
* "Yes, let's bargain again" 
    -> DONE
* "No thanks!" 
    -> DONE

=== g ===
// there was a way to bargain but you missed
No, no... this isn’t it. You must’ve made a mistake, little mushbrain! I’ll be back when you’ve regained your senses.
-> DONE

//=== h ===
// = false -> there was no way to bargain for a decreased offer (but could maybe offer more money) (so you missed also)
// not possible here
//-> DONE

=== i ===
// congrats you found the scam
Right right it may have lost a bit of magic over the ages. Alright then, that’ll do nicely. Thank you, my dear, I may just come by again sometime!
-> DONE