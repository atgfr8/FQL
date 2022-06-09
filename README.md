# FQL

Filter Query Language (FQL) is an all purpose query language that acts as an ODATA replacement or alternative.

### Expressions and Statement Syntax in FQL
FQL defines expressions as `(field operator 'fieldValue')`. That is all expressions are surrounded by `(` and `)` respectively. Where operator is some operator that FQL knows. All values that you are comparing against are surounded by `'` to indicate this. FQL treats all continous bit of text before the operator as the field. The field is usually some data field that you are comparing against.

FQL defines statements as one or more expressions tied together with a known logical operator like `and` or `or`. 

### Use paranthesis as you see fit

FQL handles gratutious paranthesis around statements and expressions with style. So throw as many as you would like at it.

## License
This is MIT Licensed.

## Nuget Packages
This will come soon