-> a
-> b
-> c
-> d
-> e
-> f
-> g
// -> h


=== default ===
This is the default knot, we should not be reading this in game
-> DONE

=== a ===
Hellooo! How are you?
I'm here to sell my wand, pleasee buy it!!
It’s very good… does all sorts of things… very useful, yes yes..
* "What kind of useful?"
    Ah! uh well you can throw it and… it comes back?
    -> DONE
* "Talking dog?"
    Of course we talk! You just don’t notice when you're not paying attention. How else do you think you understand when we ask for food?!
    -> DONE

=== b ===
Yesssss, thank you!! Sorry, I might have chewed on it a little... But it’s still a very good wand, yes yes!! Just a tiny bit chewed, no problem!! Totally fine!!
-> DONE

=== c ===
Mmm yes... well... I need money! At least a little more money! If not, I guess I’ll have to fetch my luck elsewhere.
-> DONE

=== d ===
No money?? It seems like you need more money than I do... 
-> DONE

=== e ===
//You refused the client's offer
You refused the best wand on the planet?! Just look at its shape! Elegance! Power! Slightly chewed... but still!!
-> DONE

=== f ===
//You trying to bargain
Only this...? No more money...?
Do you wish to bargain again?
* "Yes, let's bargain again" 
    -> DONE
* "No thanks!" 
    -> DONE
-> DONE

=== g ===
// there was a way to bargain but you missed
How dare you! It’s only been lightly chewed... out of love! I see I’m not welcome here. I leave!
-> DONE

//=== h ===
// = false -> there was no way to bargain for a decreased offer (but could maybe offer more money) (so you missed also)
// no need in this case
//-> DONE
