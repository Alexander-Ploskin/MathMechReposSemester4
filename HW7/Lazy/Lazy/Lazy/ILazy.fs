module ILazy

/// Common interface of lazy calculations
type ILazy<'a> =
    /// Returns result of the calculation
    abstract member Get: unit ->'a
