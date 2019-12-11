using System;
using System.Collections.Generic;
using CIM.Model;
using Common;
using Common.GDA;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    /// <summary>
    /// ProjekatMaster2019Importer
    /// </summary>
    public class ProjekatMaster2019Importer
    {
        ///// <summary> Singleton </summary>
        private static ProjekatMaster2019Importer ptImporter = null;
        private static object singletoneLock = new object();

        private ConcreteModel concreteModel;
        private Delta delta;
        private ImportHelper importHelper;
        private TransformAndLoadReport report;


        #region Properties
        public static ProjekatMaster2019Importer Instance
        {
            get
            {
                if (ptImporter == null)
                {
                    lock (singletoneLock)
                    {
                        if (ptImporter == null)
                        {
                            ptImporter = new ProjekatMaster2019Importer();
                            ptImporter.Reset();
                        }
                    }
                }
                return ptImporter;
            }
        }

        public Delta NMSDelta
        {
            get
            {
                return delta;
            }
        }
        #endregion Properties


        public void Reset()
        {
            concreteModel = null;
            delta = new Delta();
            importHelper = new ImportHelper();
            report = null;
        }

        public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
        {
            LogManager.Log("Importing ProjekatMaster2019 Elements...", LogLevel.Info);
            report = new TransformAndLoadReport();
            concreteModel = cimConcreteModel;
            delta.ClearDeltaOperations();

            if ((concreteModel != null) && (concreteModel.ModelMap != null))
            {
                try
                {
                    // convert into DMS elements
                    ConvertModelAndPopulateDelta();
                }
                catch (Exception ex)
                {
                    string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
                    LogManager.Log(message);
                    report.Report.AppendLine(ex.Message);
                    report.Success = false;
                }
            }
            LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
            return report;
        }

        /// <summary>
        /// Method performs conversion of network elements from CIM based concrete model into DMS model.
        /// </summary>
        private void ConvertModelAndPopulateDelta()
        {
            LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

            // import all concrete model types (DMSType enum)

            ImportSubstations();
            ImportFeederObjects();
            ImportACLineSegments();
            ImportDisconnectors();
            ImportBreakers();
            ImportAsynchronousMachines();
            ImportTerminals();
            ImportConnectivityNodes();
            ImportAnalogs();
            ImportDiscretes();
            ImportPowerTransformers();
            ImportTransformerWindings();
            ImportRatioTapChangers();

            LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
        }

        #region Import
        
        private void ImportTerminals()
        {
            SortedDictionary<string, object> cimTerminals = concreteModel.GetAllObjectsOfType("FTN.Terminal");
            if (cimTerminals != null)
            {
                foreach (KeyValuePair<string, object> cimTerminalPair in cimTerminals)
                {
                    FTN.Terminal cimTerminal = cimTerminalPair.Value as FTN.Terminal;

                    ResourceDescription rd = CreateTerminalResourceDescription(cimTerminal);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Terminal ID = ").Append(cimTerminal.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Terminal ID = ").Append(cimTerminal.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateTerminalResourceDescription(FTN.Terminal cimTerminal)
        {
            ResourceDescription rd = null;
            if (cimTerminal != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TERMINAL, importHelper.CheckOutIndexForDMSType(DMSType.TERMINAL));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimTerminal.ID, gid);

                ////populate ResourceDescription
                ProjekatMaster2019Converter.PopulateTerminalProperties(cimTerminal, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportFeederObjects()
        {
            SortedDictionary<string, object> cimFeederObjects = concreteModel.GetAllObjectsOfType("FTN.FeederObject");
            if (cimFeederObjects != null)
            {
                foreach (KeyValuePair<string, object> cimFeederObjectPair in cimFeederObjects)
                {
                    FTN.FeederObject cimFeederObject = cimFeederObjectPair.Value as FTN.FeederObject;

                    ResourceDescription rd = CreateFeederObjectResourceDescription(cimFeederObject);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("FeederObject ID = ").Append(cimFeederObject.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("FeederObject ID = ").Append(cimFeederObject.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateFeederObjectResourceDescription(FTN.FeederObject feederObject)
        {
            ResourceDescription rd = null;
            if (feederObject != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.FEEDEROBJECT, importHelper.CheckOutIndexForDMSType(DMSType.FEEDEROBJECT));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(feederObject.ID, gid);

                ////populate ResourceDescription
                ProjekatMaster2019Converter.PopulateFeederObjectProperties(feederObject, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportPowerTransformers()
        {
            SortedDictionary<string, object> cimPowerTransformers = concreteModel.GetAllObjectsOfType("FTN.PowerTransformer");
            if (cimPowerTransformers != null)
            {
                foreach (KeyValuePair<string, object> cimPowerTransformerPair in cimPowerTransformers)
                {
                    FTN.PowerTransformer cimPowerTransformer = cimPowerTransformerPair.Value as FTN.PowerTransformer;

                    ResourceDescription rd = CreatePowerTransformerResourceDescription(cimPowerTransformer);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("PowerTransformer ID = ").Append(cimPowerTransformer.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("PowerTransformer ID = ").Append(cimPowerTransformer.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreatePowerTransformerResourceDescription(FTN.PowerTransformer powerTransformer)
        {
            ResourceDescription rd = null;
            if (powerTransformer != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.POWERTRANSFORMER, importHelper.CheckOutIndexForDMSType(DMSType.POWERTRANSFORMER));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(powerTransformer.ID, gid);

                ////populate ResourceDescription
                ProjekatMaster2019Converter.PopulatePowerTransformerProperties(powerTransformer, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportRatioTapChangers()
        {
            SortedDictionary<string, object> cimRatioTapChangers = concreteModel.GetAllObjectsOfType("FTN.RatioTapChanger");
            if (cimRatioTapChangers != null)
            {
                foreach (KeyValuePair<string, object> cimRatioTapChangerPair in cimRatioTapChangers)
                {
                    FTN.RatioTapChanger cimRatioTapChanger = cimRatioTapChangerPair.Value as FTN.RatioTapChanger;

                    ResourceDescription rd = CreateRatioTapChangerResourceDescription(cimRatioTapChanger);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("RatioTapChanger ID = ").Append(cimRatioTapChanger.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("RatioTapChanger ID = ").Append(cimRatioTapChanger.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateRatioTapChangerResourceDescription(FTN.RatioTapChanger ratioTapChanger)
        {
            ResourceDescription rd = null;
            if (ratioTapChanger != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.RATIOTAPCHANGER, importHelper.CheckOutIndexForDMSType(DMSType.RATIOTAPCHANGER));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(ratioTapChanger.ID, gid);

                ////populate ResourceDescription
                ProjekatMaster2019Converter.PopulateRatioTapChangerProperties(ratioTapChanger, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportSubstations()
        {
            SortedDictionary<string, object> cimSubstations = concreteModel.GetAllObjectsOfType("FTN.Substation");
            if (cimSubstations != null)
            {
                foreach (KeyValuePair<string, object> cimSubstationPair in cimSubstations)
                {
                    FTN.Substation cimSubstation = cimSubstationPair.Value as FTN.Substation;

                    ResourceDescription rd = CreateSubstationResourceDescription(cimSubstation);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Substation ID = ").Append(cimSubstation.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Substation ID = ").Append(cimSubstation.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateSubstationResourceDescription(FTN.Substation substation)
        {
            ResourceDescription rd = null;
            if (substation != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SUBSTATION, importHelper.CheckOutIndexForDMSType(DMSType.SUBSTATION));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(substation.ID, gid);

                ////populate ResourceDescription
                ProjekatMaster2019Converter.PopulateSubstationProperties(substation, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportTransformerWindings()
        {
            SortedDictionary<string, object> cimTransformerWindings = concreteModel.GetAllObjectsOfType("FTN.TransformerWinding");
            if (cimTransformerWindings != null)
            {
                foreach (KeyValuePair<string, object> cimTransformerWindingPair in cimTransformerWindings)
                {
                    FTN.TransformerWinding cimTransformerWinding = cimTransformerWindingPair.Value as FTN.TransformerWinding;

                    ResourceDescription rd = CreateTransformerWindingResourceDescription(cimTransformerWinding);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("TransformerWinding ID = ").Append(cimTransformerWinding.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("TransformerWinding ID = ").Append(cimTransformerWinding.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateTransformerWindingResourceDescription(FTN.TransformerWinding transformerWinding)
        {
            ResourceDescription rd = null;
            if (transformerWinding != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TRANSFORMERWINDING, importHelper.CheckOutIndexForDMSType(DMSType.TRANSFORMERWINDING));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(transformerWinding.ID, gid);

                ////populate ResourceDescription
                ProjekatMaster2019Converter.PopulateTransformerWindingProperties(transformerWinding, rd, importHelper, report);
            }
            return rd;
        }

        private void ImportDiscretes()
        {
            SortedDictionary<string, object> cimDiscretes = concreteModel.GetAllObjectsOfType("FTN.Discrete");
            if (cimDiscretes != null)
            {
                foreach (KeyValuePair<string, object> cimDiscretePair in cimDiscretes)
                {
                    FTN.Discrete cimDiscrete = cimDiscretePair.Value as FTN.Discrete;

                    ResourceDescription rd = CreateDiscreteResourceDescription(cimDiscrete);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Discrete ID = ").Append(cimDiscrete.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Discrete ID = ").Append(cimDiscrete.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateDiscreteResourceDescription(FTN.Discrete discrete)
        {
            ResourceDescription rd = null;
            if (discrete != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DISCRETE, importHelper.CheckOutIndexForDMSType(DMSType.DISCRETE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(discrete.ID, gid);

                ////populate ResourceDescription
                ProjekatMaster2019Converter.PopulateDiscreteProperties(discrete, rd, importHelper, report);
            }
            return rd;
        }

        #endregion Import
    }
}

