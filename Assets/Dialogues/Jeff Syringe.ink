-> main

=== main ===
You again? I told you notto leave me alone.
    + [I have something that might interest you]
        -> next
=== next ===
GIVE ME THAT! GIVE ME THAT AND I WILL HELP YOU!
    + [I don't think so. I'll keep this for myself.]
        -> break_syringe
    + [Take this and have fun.]
        -> give_syringe
    
=== break_syringe ===
You moron! You don't know how this thing is important for me.
    + [*Smashes it on the ground*\n I know]
Leave... From... My... EYES!
-> END
=== give_syringe ===
Thank you! Thank you! Thank you! Here, the gate is now open!
-> END