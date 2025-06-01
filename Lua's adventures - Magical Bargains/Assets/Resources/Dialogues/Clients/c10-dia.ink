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
Hello there! 
I’m Qiwy, and this cute kitty cat is my sister Quellia.
She’s very shy and won’t speak to you. 
We know our offer is not as great as the ones you normally get… 
But please, help us! We need to feed our family.
* "What about mice?"
    Yes… well, you see, we're not normal cats… we're of the domesticated kind! We’re terrible at chasing mice, so we’re starving…
    -> DONE
* "Can I pet you?"
    Of course you can! But please don't touch Quellia, she would get very uncomfortable!
    -> DONE

=== b ===
//The client accepts your offer
Thank you! You're our savior, this will feed us for weeks!
-> DONE

=== c ===
//The client thinks your offer is too low and refuses the deal
Come on! I could get a better price on the street… Wait, maybe we should try that!
-> DONE

=== d ===
//You don't have enough money and the client refuses the deal
Oh… I didn’t realize you were struggling too. I’m sorry.
-> DONE

=== e ===
//You refused the client's offer
Noo! I won’t forgive you! ... Wait, no... don’t pet me... okay, that’s nice…
-> DONE

=== f ===
//You trying to bargain
Can’t you go just a bit higher? This is for our family!
Do you wish to bargain again?
* "Yes, let's bargain again" 
    -> DONE
* "No thanks!" 
    -> DONE
-> DONE

=== g ===
// there was a way to bargain but you missed
That’s low. You don’t need to make excuses. Just say no if it’s no.
-> DONE

//=== h ===
// = false -> there was no way to bargain for a decreased offer (but could maybe offer more money) (so you missed also)
// not possible here
//-> DONE

=== i ===
// congrats you found the scam
Okay, yes... it is just a normal slice of pizza. Il't take you on your offer, it's better than nothing I guess!
-> DONE

