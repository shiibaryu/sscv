namespace batzen
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Network object as a collection of devices.
    /// </summary>
    [ExcludeFromCodeCoverage]
    class Network : MarshalByRefObject
    {
        public Dictionary<string, Device> Device { get; set; }
       
    }
}
