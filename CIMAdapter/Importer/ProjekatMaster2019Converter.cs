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

        public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
        {
            if ((cimIdentifiedObject != null) && (rd != null))
            {
                if (cimIdentifiedObject.MRIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
                }
                if (cimIdentifiedObject.NameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
                }
                if (cimIdentifiedObject.DescriptionHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_DESC, cimIdentifiedObject.Description));
                }
            }
        }

        public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimPowerSystemResource != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);
            }
        }

        public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimEquipment != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulatePowerSystemResourceProperties(cimEquipment, rd, importHelper, report);

                if (cimEquipment.EquipmentContainerHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimEquipment.EquipmentContainer.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimEquipment.GetType().ToString()).Append(" rdfID = \"").Append(cimEquipment.ID);
                        report.Report.Append("\" - Failed to set reference to EquipmentContainer: rdfID \"").Append(cimEquipment.EquipmentContainer.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_EQUIPCONTAINER, gid));
                }
            }
        }

        public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment cimConductingEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimConductingEquipment != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateEquipmentProperties(cimConductingEquipment, rd, importHelper, report);
            }
        }

        public static void PopulateTerminalProperties(FTN.Terminal cimTerminal, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimTerminal != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateIdentifiedObjectProperties(cimTerminal, rd);

                if (cimTerminal.ConnectivityNodeHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimTerminal.ConnectivityNode.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimTerminal.GetType().ToString()).Append(" rdfID = \"").Append(cimTerminal.ID);
                        report.Report.Append("\" - Failed to set reference to ConnectivityNode: rdfID \"").Append(cimTerminal.ConnectivityNode.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TERMINAL_CONNNODE, gid));
                }
                //if (cimTerminal.ConnectivityNodeHasValue)
                //{
                //    long gid = importHelper.GetMappedGID(cimTerminal.ConnectivityNode.ID);
                //    if (gid < 0)
                //    {
                //        report.Report.Append("WARNING: Convert ").Append(cimTerminal.GetType().ToString()).Append(" rdfID = \"").Append(cimTerminal.ID);
                //        report.Report.Append("\" - Failed to set reference to ConnectivityNode: rdfID \"").Append(cimTerminal.ConnectivityNode.ID).AppendLine(" \" is not mapped to GID!");
                //    }
                //    rd.AddProperty(new Property(ModelCode.TERMINAL_CONNNODE, gid));
                //} TODO for condEquip
            }
        }

        public static void PopulateConnectivityNodeProperties(FTN.ConnectivityNode cimConnectivityNode, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimConnectivityNode != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateIdentifiedObjectProperties(cimConnectivityNode, rd);

                if (cimConnectivityNode.ConnectivityNodeContainerHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimConnectivityNode.ConnectivityNodeContainer.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimConnectivityNode.GetType().ToString()).Append(" rdfID = \"").Append(cimConnectivityNode.ID);
                        report.Report.Append("\" - Failed to set reference to ConnectivityNodeContainer: rdfID \"").Append(cimConnectivityNode.ConnectivityNodeContainer.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.CONNECTIVITYNODE_CNODECONT, gid));
                }
            }
        }

        public static void PopulateConnectivityNodeContainerProperties(FTN.ConnectivityNodeContainer cimConnectivityNodeCont, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimConnectivityNodeCont != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulatePowerSystemResourceProperties(cimConnectivityNodeCont, rd, importHelper, report);
            }
        }

        public static void PopulateEquipmentContainerProperties(FTN.EquipmentContainer cimEquipmentCont, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimEquipmentCont != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateConnectivityNodeContainerProperties(cimEquipmentCont, rd, importHelper, report);
            }
        }

        public static void PopulateSubstationProperties(FTN.Substation cimSubstation, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSubstation != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateEquipmentContainerProperties(cimSubstation, rd, importHelper, report);
            }
        }

        public static void PopulateFeederObjectProperties(FTN.FeederObject cimFeederObject, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimFeederObject != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateEquipmentContainerProperties(cimFeederObject, rd, importHelper, report);
            }
        }

        public static void PopulateMeasurementProperties(FTN.Measurement cimMeasurement, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimMeasurement != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateIdentifiedObjectProperties(cimMeasurement, rd);

                if (cimMeasurement.PowerSystemResourceHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimMeasurement.PowerSystemResource.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimMeasurement.GetType().ToString()).Append(" rdfID = \"").Append(cimMeasurement.ID);
                        report.Report.Append("\" - Failed to set reference to PowerSystemResource: rdfID \"").Append(cimMeasurement.PowerSystemResource.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.MEASUREMENT_PSR, gid));
                }
            }
        }

        public static void PopulateAnalogProperties(FTN.Analog cimAnalog, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimAnalog != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateMeasurementProperties(cimAnalog, rd, importHelper, report);

                if (cimAnalog.MaxValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ANALOG_MAXVALUE, cimAnalog.MaxValue));
                }
                if (cimAnalog.MinValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ANALOG_MINVALUE, cimAnalog.MinValue));
                }
                if (cimAnalog.NormalValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ANALOG_NORMALVALUE, cimAnalog.NormalValue));
                }
            }
        }

        public static void PopulateDiscreteProperties(FTN.Discrete cimDiscrete, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimDiscrete != null) && (rd != null))
            {
                ProjekatMaster2019Converter.PopulateMeasurementProperties(cimDiscrete, rd, importHelper, report);

                if (cimDiscrete.MaxValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DISCRETE_MAXVALUE, cimDiscrete.MaxValue));
                }
                if (cimDiscrete.MinValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DISCRETE_MINVALUE, cimDiscrete.MinValue));
                }
                if (cimDiscrete.NormalValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.DISCRETE_NORMALVALUE, cimDiscrete.NormalValue));
                }
            }
        }

        #endregion Populate ResourceDescription

        #region Enums convert

        //TODO

        #endregion Enums convert
    }
}
