# Grammarton
Easy to use context-free grammar to pushdown automaton C# converter

## How to use? 

Let's say you have some context-free grammar and you want to somehow recognize or generate strings that belong to formal language CFG is describing

Grammarton will generate for you pushdown automaton in just few lines of code.

Let's say you have following CFG:

- E → T E-List
- E-List → + T E-List
- E-List → ε
- T → F T-List
- T-List → * F T-List
- T-List → ε
- F → P F-List
- F-List → ^ P F-List
- F-List → ε
- P → (E)
- P → c

Where c = constant = 0,1,2,3,4,5,6,7,8,9

### Example 
To see full example source code please open Program.cs

To init Grammarton Service:
```C#
var grammarton = new GrammartonService();
```

To be type-safe:
```C#
var E = "<E>";
var T = "<T>";
var EList = "<E-List>";
var TList = "<T-List>";
var F = "<F>";
var FList = "<F-List>";
var P = "<P>";
var c = "c";
```

- E → T E-List
```C#
grammarton.AddProductionRule(E, T, EList);
```

- E-List → + T E-List
```C#
grammarton.AddProductionRule(EList,"+",T,EList);
```

- E-List → ε
```C#
grammarton.AddProductionRule(EList,"");
```

- P → (E)
```C#
grammarton.AddProductionRule(P,"(",E,")");
```

- P → c
```C#
grammarton.AddProductionRule(P,c);
```

- To describe that c is set of { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 }
```C#
grammarton.AddWords(c,new string[]{ "1", "2","3","4","5","6","7","8","9","0"});
```

Now get your PDA:
```C#
var pda = grammarton.GetPushdownAutomaton(E);
```

E - is the axiom of the grammar

You can get recognition result:
```C#
var result = pda.Recognize("1 + 2 * 3".Split(' '));
```

Or generate string according to CFG:
```C#
var generatedString = String.Join(" ", pda.Generate());
```

Grammarton works with any LL grammar, which is not left-recursive (You should not have rules such as: A → A...)
