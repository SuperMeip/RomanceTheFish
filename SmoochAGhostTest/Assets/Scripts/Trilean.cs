/// <summary>
/// Used for Trileans bool values + a none option
/// </summary>
public enum TriValue { True,False,None}

/// <summary>
/// A Boolean with an Unset/None option
/// </summary>
public class Trilean
{
    /// <summary>
    /// The value, true, false, or none
    /// </summary>
    TriValue Value;
    Trilean() { Value = TriValue.None; }
    /// <summary>
    /// Returns an evaluation of the simple bool
    /// </summary>
    /// <returns>True if true, false if none or false</returns>
    bool True() { return Value == TriValue.True ? true : false; }
    /// <summary>
    /// Return s weather or not it's set or unset
    /// </summary>
    /// <returns>True if true or false, false if none</returns>
    bool IsSet() { return Value == TriValue.None ? false : true; }
    /// <summary>
    /// Set the Trilean using a bool
    /// </summary>
    /// <param name="setTo">Value to set value to</param>
    void Set(bool setTo) { Value = setTo ? TriValue.True : TriValue.False; }
}