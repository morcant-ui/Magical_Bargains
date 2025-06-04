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
Good day to you, I brought you quite the rare artifact today, please take a look.
They say stroking it gently with a cloth will grant you wishes... 
It might fetch a high price at auctions, if you're lucky.
* "For real?"
    Oh, absolutely. Look at me, don't I seem like someone who got everything they wished for?
    -> DONE
* "Why would you sell it?"
    Because every wish I made was granted... just not in ways I wanted.

=== b ===
Thank you for your offer... May it bring you better fortune than it brought me.
-> DONE

=== c ===
I’m sorry... I truly believe it deserves a higher price.
-> DONE

=== d ===
Your coin pouch seems rather light... not the best day for wish-making?
-> DONE

=== e ===
//You refused the client's offer
Maybe you're not as naive as I thought you would be... Good for you.
-> DONE

=== f ===
//You trying to bargain
Surely you can do better? Artifacts like this don’t come around every day...
Do you wish to bargain again?
* "Yes, let's bargain again" 
    -> DONE
* "No thanks!" 
    -> DONE
-> DONE

=== g ===
// there was a way to bargain but you missed
No, no, it’s not broken, nor is it lacking in magic... I can assure you of that! A shame you couldn’t see it.
-> DONE

//=== h ===
// = false -> there was no way to bargain for a decreased offer (but could maybe offer more money) (so you missed also)
//-> DONE

=== i ===
// congrats you found the scam
You... saw it? I underestimated you. Take it... just get it far, far away from me.
-> DONE