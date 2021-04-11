module ILazy

// Common interface of lazy calculations
type ILazy<'a> =
    abstract member Get: unit ->'a
