using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum AlarmEventType : short
    {
        SCADA = 0x01,
        CE = 0x02,
        UI = 0x03
    }

    public enum SignalDirection : short
    {

        /// The signal is read from the field
        Read,

        /// The signal is both read and write
        ReadWrite,

        /// The signal is a command or set point
        Write,
    }

    public enum MeasurementType : short
    {

        /// Active energy [kWh]
        ActiveEnergy,

        /// Active power [kW]
        ActivePower,

        /// Admittance, conductance or susceptance [S]
        Admittance,

        /// Admittance, conductance or susceptance per unit length [S/km]
        AdmittancePerLength,

        /// Apparent power [kVA]
        ApparentPower,

        /// Power angle (cos(phi))
        CosPhi,

        /// CrossSection [mm<sup>2</sup>]
        CrossSection,

        /// Current [A]
        Current,

        /// Current angle provided from fault recorder or IEDs. Default unit is degree.
        CurrentAngle,

        /// A discrete signal with a range of integer values
        Discrete,

        /// Equivalent temperature [°C]
        FeelsLike,

        /// Frequency
        Frequency,

        /// Relative humidity [%]
        Humidity,

        /// Impedance, resistance or reactance [&Omega;]
        Impedance,

        /// Impedance, resistance or reactance per unit length [&Omega;/km]
        ImpedancePerLength,

        /// Insolation [W/m<sup>2</sup>]
        Insolation,

        /// Physical length [m]
        Length,

        /// Money unit. The unit symbol is taken from system locale information.
        Money,

        /// A percentage value [%]
        Percent,

        /// Reactive energy [kVArh]
        ReactiveEnergy,

        /// Reactive power [kVAr]
        ReactivePower,

        /// Voltage expressed as relative value of base voltage. Default unit is %, other applicable units are p.u and base 120
        RelativeVoltage,

        /// Speed of a rotational machine
        RotationSpeed,

        /// Sky cover [%]
        SkyCover,

        /// A discrete signal with on/off (true/false) value.
        Status,

        /// Sunshine minutes per hour [min]
        SunshineMinutes,

        /// A discrete signal describing switch status -- open/close/error/transit.
        SwitchStatus,

        /// Temperature [°C]
        Temperature,

        /// Time [s]
        Time,

        /// Quantity which is unitless, or not otherwise in the list of supported types
        Unitless,

        /// Voltage [kV]
        Voltage,

        /// Voltage angle. Typically, this is not a physical measurement but a pseudo-measurement imported from e.g. EMS estimator.
        VoltageAngle,

        /// Wind speed [km/h]
        WindSpeed,
    }
}
