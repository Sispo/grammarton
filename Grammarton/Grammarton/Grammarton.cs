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
using System.Collections.Generic;
using System.Linq;
using PushdownAutomaton;

namespace Grammarton
{
    public class GrammartonService
    {
        public HashSet<string> stackAlphabet { get; private set; }
        public List<PDATransition> transitions { get; private set; }
        public Dictionary<string, IEnumerable<string>> terminals { get; private set; }

        public GrammartonService() {
            stackAlphabet = new HashSet<string>();
            transitions = new List<PDATransition>();
            terminals = new Dictionary<string, IEnumerable<string>>();
        }

        public PDA GetPushdownAutomaton(string axiom)
        {
            //Checks if axiom belongs to stack alphabet
            if (!stackAlphabet.Contains(axiom)) {
                throw new Exception("Invalid axiom non-terminal");
            }

            stackAlphabet.Add(PDA.initialStackSymbol);

            var inputAlphabet = new HashSet<string>(from transition in transitions
                                                from element in transition.pushToStack
                                                where !stackAlphabet.Contains(element)
                                                select element, null);

            foreach(var element in inputAlphabet) {
                if (terminals.ContainsKey(element)) {
                    transitions.AddRange(GetTransitions(element));
                } else if (element != "") {
                    transitions.Add(new PDATransition(1, 1, element, element, ""));
                }
            }

            var alphabet = (from type in terminals
                           from word in type.Value
                            select word).ToList();

            alphabet.AddRange(inputAlphabet);

            var states = new HashSet<int> { 0, 1, 2 };

            transitions.Add(new PDATransition(0, 1, "", PDA.initialStackSymbol, axiom, PDA.initialStackSymbol));
            transitions.Add(new PDATransition(1, 2, "", PDA.initialStackSymbol, ""));

            return new PDA(new HashSet<string>(alphabet,null), new HashSet<string>(stackAlphabet,null), states, 0, transitions);
        }


        public void AddProductionRule(string leftSideNonTerminal, params string[] rightSideElements)
        {
            stackAlphabet.Add(leftSideNonTerminal);
            transitions.Add(new PDATransition(1, 1, "", leftSideNonTerminal, rightSideElements));
        }

        public void AddWords(string terminalSymbol, IEnumerable<string> words)
        {
            if (this.terminals.ContainsKey(terminalSymbol))
            {
                var currentWords = this.terminals[terminalSymbol].ToList();
                currentWords.AddRange(words);
                this.terminals[terminalSymbol] = currentWords.AsEnumerable();
            }
            else
            {
                this.terminals[terminalSymbol] = words;
            }
        }

        private List<PDATransition> GetTransitions(string terminal)
        {
            var transitions = new List<PDATransition>();

            var terminalWords = terminals[terminal];

            foreach (string word in terminalWords)
            {
                transitions.Add(new PDATransition(1, 1, "", terminal, word));
            }

            foreach (string word in terminalWords)
            {
                transitions.Add(new PDATransition(1, 1, word, word, ""));
            }

            return transitions;
        }
    }
}
