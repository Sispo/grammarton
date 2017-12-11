/*
 * Copyright (c) 2017 Tymofii Dolenko
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */


using System;
using Grammarton;

namespace Testing
{

    class Program
    {
        static void Main(string[] args)
        {
            var grammarton = new GrammartonService();

            var E = "<E>";
            var T = "<T>";
            var EList = "<E-List>";
            var TList = "<T-List>";
            var F = "<F>";
            var FList = "<F-List>";
            var P = "<P>";
            var c = "c";

            // <E> -> <T><E-List>
            grammarton.AddProductionRule(E, T, EList);

            // <E-List> -> +<T><E-List>
            grammarton.AddProductionRule(EList,"+",T,EList);

            // <E-List> -> ε
            grammarton.AddProductionRule(EList,"");

            // <T> -> <F><T-List>
            grammarton.AddProductionRule(T,F,TList);

            // <T-List> -> *<F><T-List>
            grammarton.AddProductionRule(TList,"*",F,TList);

            // <T-List> -> ε
            grammarton.AddProductionRule(TList,"");

            // <F> -> <P><F-List>
            grammarton.AddProductionRule(F,P,FList);

            // <F-List> -> ^<P><F-List>
            grammarton.AddProductionRule(FList,"^",P,FList);

            // <F-List> -> ε
            grammarton.AddProductionRule(FList,"");

            // <P> -> (<E>)
            grammarton.AddProductionRule(P,"(",E,")");

            // <P> -> c
            grammarton.AddProductionRule(P,c);

            // c = 1,2,3,4,5,6,7,8,9,0
            grammarton.AddWords(c,new string[]{ "1", "2","3","4","5","6","7","8","9","0"});

            //Generating PDA
            var pda = grammarton.GetPushdownAutomaton(E);

            //Recognition result
            var result = pda.Recognize("1 + 2 * 3".Split(' '));

            //Generated string that is part of this formal language
            var generatedString = String.Join(" ", pda.Generate());
        }
    }
}
