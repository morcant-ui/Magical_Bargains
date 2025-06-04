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
Hello there...
Interested in some top-quality magical weaponry? One of a kind, truly!
Please buy it off my hands! I can’t bear to keep something so beautiful and rare!
* "Who made such a beauty?"
    Uh... Merlin? Yeah, totally Merlin.
    -> DONE
* "Is it heavy?"
    Not at all, young lady! Even someone like you could wield it with ease!
    -> DONE

=== b ===
Yesss! Thank you for your business. You’re gonna get rich selling this bad boy!
-> DONE

=== c ===
Come on, that was a steal! 
-> DONE

=== d ===
You don’t even have that much money, do you? Tch. Whatever... I’m taking my offer elsewhere!
-> DONE

=== e ===
//You refused the client's offer
The nerve! I’ll find someone who appreciates real craftsmanship!
-> DONE

=== f ===
//You trying to bargain
Clearly, you don’t understand what you’re looking at. Let’s try this again, shall we?
Do you wish to bargain again?
* "Yes, let's bargain again" 
    -> DONE
* "No thanks!" 
    -> DONE
-> DONE

=== g ===
// there was a way to bargain but you missed
I told you it’s top quality! Well... okay, maybe not legit quality... Anyway, I’m off to find someone else!
-> DONE

//=== h ===
// = false -> there was no way to bargain for a decreased offer (but could maybe offer more money) (so you missed also)
// not possible in this case
//-> DONE

=== i ===
// congrats you found the scam
Why are people so judgmental about things built in China? Everything comes from there. You can’t expect modern weapons to be built anywhere else, yes??
-> DONE

