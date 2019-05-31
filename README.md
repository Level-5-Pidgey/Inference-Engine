[Link to GitHub Repo -- for nicely formatted README](https://github.com/Level-5-Pidgey/Inference-Engine)

# Inference Engine
Inference Engine is an assignment done for my introduction to AI Class as part of my Bachelor of Computer Science here at Swinburne. As per the assignment specifications, the program requires "implement an inference engine for propositional logic in software based on the Truth Table (TT) checking, and Backward Chaining (BC) and Forward Chaining (FC) algorithms. Your inference engine will take as arguments a Horn-form Knowledge Base KB and a query q which is a proposition symbol and determine whether q can be entailed from KB."

Within this readme some information will be provided about the implementation of the algorithms, and some of the tests completed using the algorithms.
## Information/Background

### Truth Table Generation

With a O(2^n) time complexity, the truth table algorithm populates a new table with every possible combination of element states for the knowledgebase given to the algorithm. After populating the truth table with unique states, it will then move through every unique set of states (a "model"), change the states of the elements in the each clause of the knowledgebase and then resolve these clauses to see if they result in a truth. If the Query element is true and the other required elements to reach the Query are true, then the model can be assumed to be optimal.

After evaluating all possible models generated in the truth table, the algorithm will output the count of optimal models present.

### Forward Chaining

Forward chaining is a reasoning method to assess whether the queried element can be reached starting from the facts present within a given knowledge base.

Starting with a list of facts, the forward chaining method will iterate through all clauses and assess whether the currentl element being searched for (found from the list of facts) is present within a given clause. If it is, found non-implied elements are removed from the list of elements to search and then the process repeats until a clause runs out of elements left to search. When this happens, the clause is marked to be ignored and the inferred element (to the right of "=>") is now added to the list of elements to explore the remaining clauses for (as this clause's inferred element can be evaluated given previously explored elements).

This process repeats either until all possible clauses and elements have been exhausted, or the currently selected element is the query element being ASKed for in the given file.

If the search is successful, all elements present on the explored list will be printed to the console.

### Backwards Chaining

Acting as the reverse of Forward chaining, Backwards Chaining is a reasoning method that starts at the query element and attempts to search through all clauses to find facts within the knowledgebase.

Starting with the query element, the reasoning method loops through all clauses in the knowledge base. If the implied element of this clause (the one to the right of "=>") is the same element as the one currently being serached for, then the BC method will add all elements within the clause to the input list (as it is assumed this clause/element can now be reached).

This process will repeat until the search loop can no longer find new elements to add from clauses, or the current element is found to be a fact (meaning we have successfully worked all the way through the KB from Query -> Fact)

## Installation

If you're not the marker for this assessment, this repo can be downloaded by .zip or cloned. If downloading from the github repo (i.e. you're not given the assignment submission version) you will have to use the provided visual studio solution file to compile and build the executable file to use the program.

## Usage

The program operates through a command prompt window in the same directory as the program executable. The program is by default named after the solution, "InferenceEngine.exe" and located in /InferenceEngine/bin/Debug/ by default but for the assignment submission is has been renamed to "iengine" and included in the root directory.

To use the executable in the command prompt, you can type:

```c#
iengine <tt/fc/bc> <filename>
```

The filename is the *relative* name without a filetype extension (.txt is automatically added, as that is the only filetype accepted for knowledge bases)

Example command prompt usage:

```c#
iengine tt test_HornKB
```

*Generates a truth table for the file "test_HornKB.txt."*

```c#
iengine fc test_2
```

*Performs forward chaining on the knowledgebase within "test_2.txt."*

### Valid Knowledge Bases

The program accepts *Horn Clause* style knowledge bases, and will only be able to parse those styles of clauses. Examples of valid clauses are:
```c#
a ^ b => c;
a ^ b;
a;
b;
c => d
```

Invalid clauses are:
```c#
(a ^ b) v c => d;
(a ^ b) ^ (c ^ d) => e;
~a => d;
~b;
a => ~d;
```
## Testing

Here are a list of test tables used to test the software with the TT/FC/Test results given by the program.

### Test 1:
*This was the default knowledgebase provided with the assignment.*
```c#
TELL
p2 => p3; p3 => p1; c => e; b & e => f; f & g => h; p1 => d; p1 & p3 => c; a; b; p2;
ASK
d
```

| Inference Method | Result | Time Taken |
| ------ | ------ | ------ |
| FC | YES: a, b, d, p1, p2, p3 | 8ms |
| BC | YES: d, p1, p2, p3 | 6ms |
| TT | YES: 3 | 74ms |

### Test 2:
*Another generated knowledgebase, without testing a particular area*
```c#
TELL
p3=>p1; j=>p3; p2=>m; f&g=>h; b&m=>f; p1=>z; p1&p3=>p2; a; b; j;
ASK
z
```

| Inference Method | Result | Time Taken |
| ------ | ------ | ------ |
| FC | YES: a, b, j, p1, p3, z | 10ms |
| BC | YES: j, p1, p3, z | 7ms |
| TT | YES: 3 | 88ms |

### Test 3:
*A very small knowledgebase to test the logic of the inference methods created within the program.*
```c#
TELL
a=>d; a;
ASK
d
```

| Inference Method | Result | Time Taken |
| ------ | ------ | ------ |
| FC | YES: a, d; | 6ms |
| BC | YES: a, d; | 7ms |
| TT | YES: 1 | 10ms |

### Test 4:
*A variant on the previous knowledgebase without the included fact of A. These test results showcase that there's an issue with chaining rules in knowledge bases that do not contain any facts (and a single clause) -- this may not be a bug, and might just be an improper knowledgebase.*
```c#
TELL
a=>d;
ASK
d
```

| Inference Method | Result | Time Taken |
| ------ | ------ | ------ |
| FC | NO; | 5ms |
| BC | NO; | 6ms |
| TT | YES: 2 | 12ms |

### Test 5:
*Testing an unreachable ask in a knowledgebase.*
```c#
TELL
b=>g; g=>c; k=>z; b;
ASK
z
```

| Inference Method | Result | Time Taken |
| ------ | ------ | ------ |
| FC | NO; | 6ms |
| BC | NO; | 6ms |
| TT | YES: 2 | 11ms |

### Test 6:
*Testing an empty/invalid knowledgebase.*
```c#
TELL
;
ASK
a
```

| Inference Method | Result | Time Taken |
| ------ | ------ | ------ |
| FC | NO; | 5ms |
| BC | NO; | 6ms |
| TT | YES: 1 | 10ms |
