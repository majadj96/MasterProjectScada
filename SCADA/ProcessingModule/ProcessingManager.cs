﻿using ScadaCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using DNP3.DNP3Functions;
using ScadaCommon.Interfaces;
using DNP3;

namespace ProcessingModule
{
    /// <summary>
    /// Class containing logic for processing points and executing commands.
    /// </summary>
    public class ProcessingManager : IProcessingManager
    {
        private IFunctionExecutor functionExecutor;
        private IStorage storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessingManager"/> class.
        /// </summary>
        /// <param name="storage">The point storage.</param>
        /// <param name="functionExecutor">The function executor.</param>
        public ProcessingManager(IStorage storage, IFunctionExecutor functionExecutor)
        {
            this.storage = storage;
            this.functionExecutor = functionExecutor;
            this.functionExecutor.UpdatePointEvent += CommandExecutor_UpdatePointEvent;
        }

        /// <inheritdoc />
        public void ExecuteReadCommand(IConfigItem configItem, ushort transactionId, byte remoteUnitAddress, ushort startAddress, ushort numberOfPoints)
        {
            //ModbusReadCommandParameters p = new ModbusReadCommandParameters(6, (byte)GetReadFunctionCode(configItem.RegistryType), startAddress, numberOfPoints, transactionId, remoteUnitAddress);
            //IModbusFunction fn = FunctionFactory.CreateModbusFunction(p);
            //this.functionExecutor.EnqueueCommand(fn);

            if (configItem.RegistryType == PointType.ANALOG_INPUT)
            {
                ExecuteAnalogInputRead(configItem, transactionId, remoteUnitAddress, startAddress, numberOfPoints);
            }
            else if(configItem.RegistryType == PointType.ANALOG_OUTPUT)
            {
                ExecuteAnalogOutputRead(configItem, transactionId, remoteUnitAddress, startAddress, numberOfPoints);
            }
            else if (configItem.RegistryType == PointType.DIGITAL_INPUT)
            {
                ExecuteDigitalInputRead(configItem, transactionId, remoteUnitAddress, startAddress, numberOfPoints);
            }
            else if (configItem.RegistryType == PointType.DIGITAL_OUTPUT)
            {
                ExecuteDigitalOutputRead(configItem, transactionId, remoteUnitAddress, startAddress, numberOfPoints);
            }
            else
            {
                ExecuteClass0DataRead(configItem, transactionId, remoteUnitAddress, startAddress, numberOfPoints);
            }
        }

        private void ExecuteDigitalOutputRead(IConfigItem configItem, ushort transactionId, byte remoteUnitAddress, ushort startAddress, ushort numberOfPoints)
        {
            DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters(0xc1, (byte)DNP3FunctionCode.READ, (ushort)TypeField.BINARY_OUTPUT_PACKED_FORMAT, 0x00, startAddress, 0, 0, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc1);
            IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Function(p);
            this.functionExecutor.EnqueueCommand(fn);
        }

        private void ExecuteDigitalInputRead(IConfigItem configItem, ushort transactionId, byte remoteUnitAddress, ushort startAddress, ushort numberOfPoints)
        {
            DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters(0xc1, (byte)DNP3FunctionCode.READ, (ushort)TypeField.BINARY_INPUT_PACKED_FORMAT, 0x00, startAddress, 0, 0, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc1);
            IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Function(p);
            this.functionExecutor.EnqueueCommand(fn);
        }

        private void ExecuteAnalogInputRead(IConfigItem configItem, ushort transactionId, byte remoteUnitAddress, ushort startAddress, ushort numberOfPoints)
        {
            DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters(0xc1, (byte)DNP3FunctionCode.READ, (ushort)TypeField.ANALOG_INPUT_16BIT, 0x00, startAddress, 0, 0, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc1);
            IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Function(p);
            this.functionExecutor.EnqueueCommand(fn);
        }

        private void ExecuteAnalogOutputRead(IConfigItem configItem, ushort transactionId, byte remoteUnitAddress, ushort startAddress, ushort numberOfPoints)
        {
            DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters(0xc1, (byte)DNP3FunctionCode.READ, (ushort)TypeField.ANALOG_OUTPUT_STATUS_16BIT, 0x00, startAddress, 0, 0, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc1);
            IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Function(p);
            this.functionExecutor.EnqueueCommand(fn);
        }

        private void ExecuteClass0DataRead(IConfigItem configItem, ushort transactionId, byte remoteUnitAddress, ushort startAddress, ushort numberOfPoints)
        {
            DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters(0xc1, (byte)DNP3FunctionCode.READ, (ushort)TypeField.CLASS_0_DATA, 0x06, 0, 0, 0, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc1);
            IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Function(p);
            this.functionExecutor.EnqueueCommand(fn);
        }

        /// <inheritdoc />
        public void ExecuteWriteCommand(IConfigItem configItem, ushort transactionId, byte remoteUnitAddress, ushort pointAddress, int value)
        {
            if (configItem.RegistryType == PointType.ANALOG_OUTPUT)
            {
                ExecuteAnalogCommand(configItem, transactionId, remoteUnitAddress, pointAddress, value);
            }
            else
            {
                ExecuteDigitalCommand(configItem, transactionId, remoteUnitAddress, pointAddress, value);
            }
        }

        /// <summary>
        /// Executes a digital write command.
        /// </summary>
        /// <param name="configItem">The configuration item.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="remoteUnitAddress">The remote unit address.</param>
        /// <param name="pointAddress">The point address.</param>
        /// <param name="value">The value.</param>
        private void ExecuteDigitalCommand(IConfigItem configItem, ushort transactionId, byte remoteUnitAddress, ushort pointAddress, int value)
        {
            //ModbusWriteCommandParameters p = new ModbusWriteCommandParameters(6, (byte)ModbusFunctionCode.WRITE_SINGLE_COIL, pointAddress, (ushort)value, transactionId, remoteUnitAddress);
            //IModbusFunction fn = FunctionFactory.CreateModbusFunction(p);
            //this.functionExecutor.EnqueueCommand(fn);
                                                                                //apl cont, function code,                      type field,                        qualf|range |   obj pref   |obj value   |start|lenght|control|dest  |sourc|transport header
            DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters(0xc1, (byte)DNP3FunctionCode.DIRECT_OPERATE, (ushort)TypeField.BINARY_COMMAND, 0x28, 0x0001, pointAddress, (uint)value, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc1);
            IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Function(p);
            this.functionExecutor.EnqueueCommand(fn);
        }

        /// <summary>
        /// Executes an analog write command.
        /// </summary>
        /// <param name="configItem">The configuration item.</param>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <param name="remoteUnitAddress">The remote unit address.</param>
        /// <param name="pointAddress">The point address.</param>
        /// <param name="value">The value.</param>
        private void ExecuteAnalogCommand(IConfigItem configItem, ushort transactionId, byte remoteUnitAddress, ushort pointAddress, int value)
        {
            //ModbusWriteCommandParameters p = new ModbusWriteCommandParameters(6, (byte)ModbusFunctionCode.WRITE_SINGLE_REGISTER, pointAddress, (ushort)value, transactionId, remoteUnitAddress);
            //IModbusFunction fn = FunctionFactory.CreateModbusFunction(p);
            //this.functionExecutor.EnqueueCommand(fn);
                                                                                //apl cont, function code,                      type field,                             qualf|range |   obj pref   |obj value   |start|lenght|control|dest  |sourc|transport header
            DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters(0xc1, (byte)DNP3FunctionCode.DIRECT_OPERATE, (ushort)TypeField.ANALOG_OUTPUT_16BIT, 0x28, 0x0001, pointAddress, (uint)value, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc1);
            IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Function(p);
            this.functionExecutor.EnqueueCommand(fn);
        }

        /// <summary>
        /// Gets the modbus function code for the point type.
        /// </summary>
        /// <param name="registryType">The register type.</param>
        /// <returns>The modbus function code.</returns>
        private ModbusFunctionCode? GetReadFunctionCode(PointType registryType)
        {
            switch (registryType)
            {
                case PointType.DIGITAL_OUTPUT: return ModbusFunctionCode.READ_COILS;
                case PointType.DIGITAL_INPUT: return ModbusFunctionCode.READ_DISCRETE_INPUTS;
                case PointType.ANALOG_INPUT: return ModbusFunctionCode.READ_INPUT_REGISTERS;
                case PointType.ANALOG_OUTPUT: return ModbusFunctionCode.READ_HOLDING_REGISTERS;
                case PointType.HR_LONG: return ModbusFunctionCode.READ_HOLDING_REGISTERS;
                default: return null;
            }
        }

        /// <summary>
        /// Method for handling received points.
        /// </summary>
        /// <param name="type">The point type.</param>
        /// <param name="pointAddress">The point address.</param>
        /// <param name="newValue">The new value.</param>
        private void CommandExecutor_UpdatePointEvent(PointType type, ushort pointAddress, ushort newValue)
        {
            List<IPoint> points = storage.GetPoints(new List<PointIdentifier>(1) { new PointIdentifier(type, pointAddress) });

            if (type == PointType.ANALOG_INPUT || type == PointType.ANALOG_OUTPUT)
            {
                ProcessAnalogPoint(points.First() as IAnalogPoint, newValue);
            }
            else
            {
                ProcessDigitalPoint(points.First() as IDigitalPoint, newValue);
            }
        }

        /// <summary>
        /// Processes a digital point.
        /// </summary>
        /// <param name="point">The digital point</param>
        /// <param name="newValue">The new value.</param>
        private void ProcessDigitalPoint(IDigitalPoint point, ushort newValue)
        {
            point.RawValue = newValue;
            point.Timestamp = DateTime.Now;
            point.State = (DState)newValue;
        }

        /// <summary>
        /// Processes an analog point
        /// </summary>
        /// <param name="point">The analog point.</param>
        /// <param name="newValue">The new value.</param>
        private void ProcessAnalogPoint(IAnalogPoint point, ushort newValue)
        {
            point.RawValue = newValue;
            point.EguValue = newValue;
            point.Timestamp = DateTime.Now;
        }

        /// <inheritdoc />
        public void InitializePoint(PointType type, ushort pointAddress, ushort defaultValue)
        {
            List<IPoint> points = storage.GetPoints(new List<PointIdentifier>(1) { new PointIdentifier(type, pointAddress) });

            if (type == PointType.ANALOG_INPUT || type == PointType.ANALOG_OUTPUT)
            {
                ProcessAnalogPoint(points.First() as IAnalogPoint, defaultValue);
            }
            else
            {
                ProcessDigitalPoint(points.First() as IDigitalPoint, defaultValue);
            }
        }

        public void SendRawBytesMessage(DNP3FunctionCode functionCode, byte[] message)
        {
            if (functionCode == DNP3FunctionCode.CONFIRM)
            {
                byte transportHeaderSeq = (byte)(message[10] & 0x3f);
                byte applicationControl = (byte)(message[11] & 0xf);

                DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters((byte)(0xd0 | applicationControl), (byte)DNP3FunctionCode.CONFIRM, 0, 0, 0x0001, 0, 0, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, (byte)((0xc0 | transportHeaderSeq) + 1));
                IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Message(p);
                this.functionExecutor.SendMessage(fn);
            }
            else if (functionCode == DNP3FunctionCode.WRITE)
            {
                byte app = message[11];
                                                                                //0xc0 - apl cont, function code,                      type field,        qualf|range|obj pref|obj value|start|lenght|control|dest  |sourc|transport header
                DNP3ApplicationObjectParameters p = new DNP3ApplicationObjectParameters(app, (byte)DNP3FunctionCode.WRITE, (ushort)TypeField.TIME_MESSAGE, 0x07, 0x0001, 0, 0, 0x6405, 0x05, 0xc4, 0x0001, 0x0002, 0xc0);
                IDNP3Functions fn = DNP3FunctionFactory.CreateDNP3Message(p);
                this.functionExecutor.SendMessage(fn);
            }
            else
            {

            }
        }
    }
}
