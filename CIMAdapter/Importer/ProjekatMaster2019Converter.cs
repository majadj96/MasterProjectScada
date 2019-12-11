namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	using Common;
    using Common.GDA;

    /// <summary>
    /// PowerTransformerConverter has methods for populating
    /// ResourceDescription objects using ProjekatMaster2019CIMProfile_Labs objects.
    /// </summary>
    public static class ProjekatMaster2019Converter
	{

        #region Populate ResourceDescription

        public static void PopulateConductorProperties(FTN.Conductor cimConductor, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimConductor != null) && (rd != null))
            {
                //ProjekatMaster2019Converter.PopulateConductingEquipmentProperties(cimConductor, rd, importHelper, report);                
            }
        }

        public static void PopulateACLineSegmentProperties(FTN.ACLineSegment cimACLineSegment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimACLineSegment != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateConductorProperties(cimACLineSegment, rd, importHelper, report);
            }
        }

        public static void PopulateDisconnectorProperties(FTN.Disconnector cimDisconnector, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimDisconnector != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateSwitchProperties(cimDisconnector, rd, importHelper, report);                
            }
        }

        public static void PopulatePowerTransformerProperties(FTN.PowerTransformer cimPowerTransformer, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimPowerTransformer != null) && (rd != null))
            {
                //ProjekatMaster2019Converter.PopulateEquipmentProperties(cimPowerTransformer, rd, importHelper, report);
            }
        }

        public static void PopulateProtectedSwitchProperties(FTN.ProtectedSwitch cimProtectedSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimProtectedSwitch != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateSwitchProperties(cimProtectedSwitch, rd, importHelper, report);
            }
        }

        public static void PopulateRatioTapChangerProperties(FTN.RatioTapChanger cimRatioTapChanger, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRatioTapChanger != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateTapChangerProperties(cimRatioTapChanger, rd, importHelper, report);

                if (cimRatioTapChanger.TransformerWindingHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimRatioTapChanger.TransformerWinding.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimRatioTapChanger.GetType().ToString()).Append(" rdfID = \"").Append(cimRatioTapChanger.ID);
                        report.Report.Append("\" - Failed to set reference to TransformerWinding: rdfID \"").Append(cimRatioTapChanger.TransformerWinding.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.RATIOTAPCHANGER_TRWINDING, gid));
                }
            }
        }

        public static void PopulateRegulatingCondEqProperties(FTN.RegulatingCondEq cimRegulatingCondEq, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegulatingCondEq != null) && (rd != null))
            {
                //ProjekatMaster2019Converter.PopulateConductingEquipmentProperties(cimRegulatingCondEq, rd, importHelper, report);
            }
        }

        public static void PopulateSwitchProperties(FTN.Switch cimSwitch, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSwitch != null) && (rd != null))
            {
                //ProjekatMaster2019Converter.PopulateConductingEquipmentProperties(cimSwitch, rd, importHelper, report);
            }
        }

        public static void PopulateTapChangerProperties(FTN.TapChanger cimTapChanger, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimTapChanger != null) && (rd != null))
            {
                //ProjekatMaster2019Converter.PopulatePowerSystemResourceProperties(cimTapChanger, rd, importHelper, report);

                if (cimTapChanger.HighStepHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_HIGHSTEP, cimTapChanger.HighStep));
                }
                if (cimTapChanger.LowStepHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_LOWSTEP, cimTapChanger.LowStep));
                }
                if (cimTapChanger.NormalStepHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.TAPCHANGER_NORMALSTEP, cimTapChanger.NormalStep));
                }
            }
        }

        public static void PopulateTransformerWindingProperties(FTN.TransformerWinding cimTransformerWinding, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimTransformerWinding != null) && (rd != null))
            {
                //ProjekatMaster2019Converter.PopulateConductingEquipmentProperties(cimTransformerWinding, rd, importHelper, report);

                if (cimTransformerWinding.PowerTransformerHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimTransformerWinding.PowerTransformer.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimTransformerWinding.GetType().ToString()).Append(" rdfID = \"").Append(cimTransformerWinding.ID);
                        report.Report.Append("\" - Failed to set reference to PowerTransformer: rdfID \"").Append(cimTransformerWinding.PowerTransformer.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TRANSFORMERWINDING_POWERTR, gid));
                }
                if (cimTransformerWinding.RatioTapChangerHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimTransformerWinding.RatioTapChanger.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimTransformerWinding.GetType().ToString()).Append(" rdfID = \"").Append(cimTransformerWinding.ID);
                        report.Report.Append("\" - Failed to set reference to RatioTapChanger: rdfID \"").Append(cimTransformerWinding.RatioTapChanger.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TRANSFORMERWINDING_RATIOTC, gid));
                }
            }
        }

        public static void PopulateAsynchronousMachineProperties(FTN.AsynchronousMachine cimAsynchronousMachine, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimAsynchronousMachine != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateRegulatingCondEqProperties(cimAsynchronousMachine, rd, importHelper, report);

                if (cimAsynchronousMachine.CosPhiHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASYNCMACHINE_COSPHI, cimAsynchronousMachine.CosPhi));
                }
                if (cimAsynchronousMachine.RatedPHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ASYNCMACHINE_RATEDP, cimAsynchronousMachine.RatedP));
                }
            }
        }


        #endregion Populate ResourceDescription

        #region Enums convert

        public static SignalDirection GetDMSSignalDirection(SignalDirection signalDirection)
        {
            switch (signalDirection)
            {
                case SignalDirection.Read:
                    return SignalDirection.Read;
                case SignalDirection.ReadWrite:
                    return SignalDirection.ReadWrite;
                case SignalDirection.Write:
                    return SignalDirection.Write;
                default:
                    return SignalDirection.ReadWrite;                    
            }
        }

        public static MeasurementType GetDMSMeasurementType(MeasurementType measurementType)
        {
            switch (measurementType)
            {
                case MeasurementType.ActiveEnergy:
                    return MeasurementType.ActiveEnergy;
                case MeasurementType.ActivePower:
                    return MeasurementType.ActivePower;
                case MeasurementType.Admittance:
                    return MeasurementType.Admittance;
                case MeasurementType.AdmittancePerLength:
                    return MeasurementType.AdmittancePerLength;
                case MeasurementType.ApparentPower:
                    return MeasurementType.ApparentPower;
                case MeasurementType.CosPhi:
                    return MeasurementType.CosPhi;
                case MeasurementType.CrossSection:
                    return MeasurementType.CrossSection;
                case MeasurementType.Current:
                    return MeasurementType.Current;
                case MeasurementType.CurrentAngle:
                    return MeasurementType.CurrentAngle;
                case MeasurementType.Discrete:
                    return MeasurementType.Discrete;
                case MeasurementType.FeelsLike:
                    return MeasurementType.FeelsLike;
                case MeasurementType.Frequency:
                    return MeasurementType.Frequency;
                case MeasurementType.Humidity:
                    return MeasurementType.Humidity;
                case MeasurementType.Impedance:
                    return MeasurementType.Impedance;
                case MeasurementType.ImpedancePerLength:
                    return MeasurementType.ImpedancePerLength;
                case MeasurementType.Insolation:
                    return MeasurementType.Insolation;
                case MeasurementType.Length:
                    return MeasurementType.Length;
                case MeasurementType.Money:
                    return MeasurementType.Money;
                case MeasurementType.Percent:
                    return MeasurementType.Percent;
                case MeasurementType.ReactiveEnergy:
                    return MeasurementType.ReactiveEnergy;
                case MeasurementType.ReactivePower:
                    return MeasurementType.ReactivePower;
                case MeasurementType.RelativeVoltage:
                    return MeasurementType.RelativeVoltage;
                case MeasurementType.RotationSpeed:
                    return MeasurementType.RotationSpeed;
                case MeasurementType.SkyCover:
                    return MeasurementType.SkyCover;
                case MeasurementType.Status:
                    return MeasurementType.Status;
                case MeasurementType.SunshineMinutes:
                    return MeasurementType.SunshineMinutes;
                case MeasurementType.SwitchStatus:
                    return MeasurementType.SwitchStatus;
                case MeasurementType.Temperature:
                    return MeasurementType.Temperature;
                case MeasurementType.Time:
                    return MeasurementType.Time;
                case MeasurementType.Unitless:
                    return MeasurementType.Unitless;
                case MeasurementType.Voltage:
                    return MeasurementType.Voltage;
                case MeasurementType.VoltageAngle:
                    return MeasurementType.VoltageAngle;
                case MeasurementType.WindSpeed:
                    return MeasurementType.WindSpeed;
                default:
                    return MeasurementType.WindSpeed;                    
            }
        }

        #endregion Enums convert
    }
}
