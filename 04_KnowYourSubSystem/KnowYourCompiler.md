Integration tests for n branch/loops = 2^n code paths


Joe Duffy (https://www.infoq.com/presentations/csharp-systems-programming)
	Function calls are not cheap
		New stack fram
		save return address
		save register that might be overwrittern
		call
		restore
		--> can add 10s of cycles
		--> Previously property accessors weren't inlined in C#. Oh Snap!
	Range analysis
		bounds checking on array loops
		without optimizations to remove the internal bounds checks on each loop, cant apply other optimizations like vectoization
	Ex. Stack allocation
		int index = custs.Indexof(c=>c.Name == name)
			produces a pointer from the stack to the Func<T, bool>
			Allocates the func
			Allocates a closure (to get access to `name`)
			Generates the functions into the class
		--> but escape alanysis can see that IndexOf never lets the func out of scope so can inline the whole thing.
		Can have Roslyn tell you if it cant stack allocate a function.
		
