using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum DMSType : short
    {
        MASK_TYPE = unchecked((short)0xFFFF),

        ASYNCHRONOUSMACHINE =   0x0001,
        ANALOG =                0x0002,
        ACLINESEGMENT =         0x0003,
        BREAKER =               0x0004,
        CONNECTIVITYNODE =      0x0005,
        DISCONNECTOR =          0x0006,
        DISCRETE =              0x0007,
        FEEDEROBJECT =          0x0008,
        POWERTRANSFORMER =      0x0009,
        RATIOTAPCHANGER =       0x000a,
        SUBSTATION =            0x000b,
        TERMINAL =              0x000c,
        TRANSFORMERWINDING =    0x000d,      
    }

    [Flags]
    public enum ModelCode : long
    {
        IDOBJ                       = 0x1000000000000000,
        IDOBJ_GID                   = 0x1000000000000104,
        IDOBJ_DESC                  = 0x1000000000000207,
        IDOBJ_MRID                  = 0x1000000000000307,
        IDOBJ_NAME                  = 0x1000000000000407,

        PSR                         = 0x1100000000000000,
        PSR_MEASUREMENTS            = 0x1100000000000119,

        EQUIPMENT                   = 0x1110000000000000,
        EQUIPMENT_EQUIPCONTAINER    = 0x1110000000000109,

        CONDEQ                      = 0x1111000000000000,
        CONDEQ_TERMINALS            = 0x1111000000000119,

        TERMINAL                    = 0x12000000000c0000,
        TERMINAL_CONNNODE           = 0x12000000000c0109,
        TERMINAL_MEASUREMENTS       = 0x12000000000c0219,
        TERMINAL_CONDEQUIPMENT      = 0x12000000000c0309,

        CONDUCTOR                   = 0x1111100000000000,

        ACLINESEGMENT               = 0x1111110000030000,

        REGULATINGCONDEQ            = 0x1111200000000000,

        ASYNCMACHINE                = 0x1111210000010000,
        ASYNCMACHINE_COSPHI         = 0x1111210000010105,
        ASYNCMACHINE_RATEDP         = 0x1111210000010205,

        SWITCH                      = 0x1111300000000000,

        DISCONNECTOR                = 0x1111310000060000,

        PROTECTEDSWITCH             = 0x1111320000000000,

        BREAKER                     = 0x1111321000040000,

        TRANSFORMERWINDING          = 0x11114000000d0000,
        TRANSFORMERWINDING_POWERTR  = 0x11114000000d0109,
        TRANSFORMERWINDING_RATIOTC  = 0x11114000000d0209,

        POWERTRANSFORMER            = 0x1112000000090000,
        POWERTRANSFORMER_TRWINDINGS = 0x1112000000090119,

        CONNNODECONTAINER           = 0x1120000000000000,
        CONNNODECONTAINER_CONNNODES = 0x1120000000000119,

        EQUIPMENTCONTAINER          = 0x1121000000000000,
        EQUIPMENTCONTAINER_EQUIPS   = 0x1121000000000119,

        SUBSTATION                  = 0x11211000000b0000,

        FEEDEROBJECT                = 0x1121200000080000,

        TAPCHANGER                  = 0x1130000000000000,
        TAPCHANGER_HIGHSTEP         = 0x1130000000000103,
        TAPCHANGER_LOWSTEP          = 0x1130000000000203,
        TAPCHANGER_NORMALSTEP       = 0x1130000000000303,

        RATIOTAPCHANGER             = 0x11310000000a0000,
        RATIOTAPCHANGER_TRWINDING   = 0x11310000000a0109,

        CONNECTIVITYNODE            = 0x1300000000050000,
        CONNECTIVITYNODE_CNODECONT  = 0x1300000000050109,
        CONNECTIVITYNODE_TERMINALS  = 0x1300000000050219,

        MEASUREMENT                 = 0x1400000000000000,
        MEASUREMENT_DIRECTION       = 0x140000000000010a,
        MEASUREMENT_MEASTYPE        = 0x140000000000020a,
        MEASUREMENT_PSR             = 0x1400000000000309,
        MEASUREMENT_TERMINALS       = 0x1400000000000419,

        ANALOG                      = 0x1410000000020000,
        ANALOG_MAXVALUE             = 0x1410000000020105,
        ANALOG_MINVALUE             = 0x1410000000020205,
        ANALOG_NORMALVALUE          = 0x1410000000020305,

        DISCRETE                    = 0x1420000000070000,
        DISCRETE_MAXVALUE           = 0x1420000000070103,
        DISCRETE_MINVALUE           = 0x1420000000070203,
        DISCRETE_NORMALVALUE        = 0x1420000000070303,
    }

    [Flags]
    public enum ModelCodeMask : long
    {
        MASK_TYPE = 0x00000000ffff0000,
        MASK_ATTRIBUTE_INDEX = 0x000000000000ff00,
        MASK_ATTRIBUTE_TYPE = 0x00000000000000ff,

        MASK_INHERITANCE_ONLY = unchecked((long)0xffffffff00000000),
        MASK_FIRSTNBL = unchecked((long)0xf000000000000000),
        MASK_DELFROMNBL8 = unchecked((long)0xfffffff000000000),
    }
}
