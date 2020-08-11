using System;

namespace ScadaCommon
{
    /// <summary>
    /// Digital states.
    /// </summary>
	public enum DState : short
	{
		OFF = 0,
		ON = 1,
	}

    /// <summary>
    /// Modbus Function codes for reading and writing modbus points.
    /// </summary>
	public enum ModbusFunctionCode : short
	{
		READ_COILS = 0x01,
		READ_DISCRETE_INPUTS = 0x02,
		READ_HOLDING_REGISTERS = 0x03,
		READ_INPUT_REGISTERS = 0x04,
		WRITE_SINGLE_COIL = 0x05,
		WRITE_SINGLE_REGISTER = 0x06,
	}

    /// <summary>
    /// Possible modbus point types.
    /// </summary>
	public enum PointType : short
	{
        DIGITAL_OUTPUT = 0x01,
        DIGITAL_INPUT = 0x02,
        ANALOG_INPUT = 0x03,
        ANALOG_OUTPUT = 0x04,
        HR_LONG = 0x05,

        NONE = 0x0,

        //Ovo je za DNP3 protokol, gore su za MODBUS (njih brisemo)
        BINARY_OUTPUT = 0x01,
        BINARY_INPUT = 0x02,
        ANALOG_INPUT_16 = 0x03,
        ANALOG_OUTPUT_16 = 0x04,
        COUNTER_INPUT_16 = 0x05,
    }

    public enum CommandResult : short
    {
        Success = 0,
        Failure = 1
    }

    public enum TypeField : short
    {
        BINARY_INPUT_PACKED_FORMAT = 0x0101,
        BINARY_INPUT_WITH_STATUS = 0x0102,
        BINARY_OUTPUT_PACKED_FORMAT = 0x0a01,
        BINATY_OUTPUT_WITH_STATUS = 0x0a02,
        BINARY_COMMAND = 0x0c01,
        ANALOG_INPUT_16BIT = 0x1e04,
        COUNTER_16BIT = 0x1406,
        FROZEN_COUNTER_16BIT = 0x150a,
        ANALOG_OUTPUT_16BIT = 0x2902,
        ANALOG_OUTPUT_STATUS_16BIT = 0x2802,
        BINARY_OUTPUT_WITHOUT_TIME = 0x0b01,
        BINARY_OUTPUT_EVENT = 0x0b02,
        ANALOG_INPUT_EVENT_16BIT = 0x2002,
        BINARY_INPUT_EVENT_WITHOUT_TIME = 0x0201,
        ANALOG_OUTPUT_EVENT_16BIT_WITHOUT_TIME = 0x2a02,
        FLOATING_POINT_OUTPUT_EVENT_32BIT = 0x2a07,
        COUNTER_CHANGE_EVENT_16BIT = 0x1602,
        CLASS_0_DATA = 0x3c01,
        TIME_MESSAGE = 0x3201,
        INTERNAL_INDICATIONS = 0x5001
    };

    public enum Qualifier : short
    {
        //object prefix
        INDEX,
        OBJECT_SIZE,

        //range
        START_STOP_INDEX,
        VIRTUAL_ADDRESS,
        NONE,
        OBJECT_COUNT
    }

    public enum DNP3FunctionCode : short
    {
        CONFIRM = 0,
        READ,
        WRITE,
        SELECT,
        OPERATE,
        DIRECT_OPERATE,
        DIRECT_OPERATE_NR,
        IMMED_FREEZE,
        IMMED_FREEZE_NR,
        FREEZE_CLEAR,
        FREEZE_CLEAR_NR,
        FREEZE_AT_TIME,
        FREEZE_AT_TIME_NR,
        COLD_RESTART,
        WARM_RESTART,
        INITIALIZE_DATA,
        INITIALIZE_APPL,
        START_APPL,
        STOP_APPL,
        SAVE_CONFIG,
        ENABLE_UNSOLICITED,
        DISABLE_UNSOLICITED,
        ASSIGN_CLASS,
        DELAY_MEASUREMENT,
        RECORD_CURRENT_TIME,
        OPEN_FILE,
        CLOSE_FILE,
        DELETE_FILE,
        GET_FILE_INFO,
        AUTHENTICATE_FILE,
        ABORT_FILE,
        ACTIVATE_CONFIG,
        AUTHENTICATION_REQUEST,
        AUTHENTICATE_ERR,
        RESPONSE = 129,
        UNSOLICITED_RESPONSE,
        AUTHENTICATE_RESP,
    }

    [Flags]
    public enum InternalIndications : ushort
    {
        ALL_STATIONS = 0x0001,
        CLASS_1_EVENTS = 0x0002,
        CLASS_2_EVENTS = 0x0004,
        CLASS_3_EVENTS = 0x0008,
        NEED_TIME = 0x0010,
        LOCAL_CONTROL = 0x0020,
        DEVICE_TROUBLE = 0x0040,
        DEVICE_RESTART = 0x0080,
        NO_FUNC_CODE_SUPPORT = 0x0100,
        OBJECT_UNKNOWN = 0x0200,
        PARAMETER_ERROR = 0x0400,
        EVENT_BUFFER_OVERFLOW = 0x0800,
        ALREADY_EXECUTING = 0x1000,
        CONFIG_CORRUPT = 0x2000,
        RESERVED_2 = 0x4000,
        RESERVED_1 = 0x8000,
    }

    /// <summary>
    /// Connection states.
    /// </summary>
	public enum ConnectionState : short
	{
		CONNECTED = 0,
		DISCONNECTED = 1,
	}

    /// <summary>
    /// Alarm types.
    /// </summary>
	public enum AlarmType : short
	{
		NO_ALARM = 0x01,
		REASONABILITY_FAILURE = 0x02,
        LOW_ALARM = 0x03,
        HIGH_ALARM = 0x04,
        ABNORMAL_VALUE = 0x05,
	}
}