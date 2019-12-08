using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum SignalDirection
    {

        /// The signal is read from the field
        Read,

        /// The signal is both read and write
        ReadWrite,

        /// The signal is a command or set point
        Write,
    }

    public enum UnitMultiplier
    {

        /// Centi 10**-2
        c,

        /// Deci 10**-1
        d,

        /// Giga 10**9
        G,

        /// Kilo 10**3
        k,

        /// Milli 10**-3
        m,

        /// Mega 10**6
        M,

        /// Micro 10**-6
        micro,

        /// Nano 10**-9
        n,

        none,

        /// Pico 10**-12
        p,

        /// Tera 10**12
        T,
    }

    public enum UnitSymbol
    {

        /// Current in ampere
        A,

        /// Plane angle in degrees
        deg,

        /// Capacitance in farad
        F,

        /// Mass in gram
        g,

        /// Time in hours
        h,

        /// Inductance in henry
        H,

        /// Frequency in hertz
        Hz,

        /// per Hertz
        HzMINUS1,

        /// Energy in joule
        J,

        /// Joule per second
        J_s,

        /// Mass per energy
        kg_J,

        /// Length in meter
        m,

        /// Area in square meters
        m2,

        /// Volume in cubic meters
        m3,

        /// Time in minutes
        min,

        /// Force in newton
        N,

        /// Dimension less quantity, e.g. count, per unit, etc.
        none,

        /// Resistance in ohm
        ohm,

        /// Pressure in pascal (n/m2)
        Pa,

        /// Plane angle in radians
        rad,

        /// Time in seconds
        s,

        /// Conductance in siemens
        S,

        /// per second
        sMINUS1,

        /// Relative temperature in degrees Celsius
        DegC,

        /// Voltage in volt
        V,

        /// Volt per volt ampere reactive
        V_VAr,

        /// Apparent energy in volt ampere hours
        VAh,

        /// Reactive power in volt ampere reactive
        VAr,

        /// Reactive energy in volt ampere reactive hours
        VArh,

        /// Active power in watt
        W,

        /// Watt per hertz
        W_Hz,

        /// Watt per second
        W_s,

        /// Real energy in what hours
        Wh,
    }
}
