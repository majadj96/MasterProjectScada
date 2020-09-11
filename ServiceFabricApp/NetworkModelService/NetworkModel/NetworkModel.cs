using Common;
using Common.GDA;
using DataModel;
using DataModel.Core;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using TransactionManagerContracts;

namespace NetworkModelService
{
    public class NetworkModel : ITransactionSteps, IModelUpdateContract, IGetResourceDescriptions
    {
        private IReliableStateManager StateManager;

        /// <summary>
        /// Dictionary which contains all data: Key - DMSType, Value - Container
        /// </summary>
        private Dictionary<DMSType, Container> networkDataModel;

        /// <summary>
        /// Dictionary which contains all data: Key - DMSType, Value - Container. Model copy
        /// </summary>
        private Dictionary<DMSType, Container> networkDataModelCopy;

        /// <summary>
        /// Dictionary which contains all data: Key - DMSType, Value - Container. Old model
        /// </summary>
        private Dictionary<DMSType, Container> networkDataModelOld;

        /// <summary>
        /// ModelResourceDesc class contains metadata of the model
        /// </summary>
        private ModelResourcesDesc resourcesDescs;
        public bool commitFinished = false;

        /// <summary>
        /// Initializes a new instance of the Model class.
        /// </summary>
        public NetworkModel(IReliableStateManager stateManager)
        {
            this.StateManager = stateManager;

            networkDataModel = new Dictionary<DMSType, Container>();
            networkDataModelCopy = new Dictionary<DMSType, Container>();
            resourcesDescs = new ModelResourcesDesc();
            //Initialize();
        }

        private async Task<Container> GetContainerFromModel(DMSType type, string dictName)
        {
            var networkModelCopy = await this.StateManager.GetOrAddAsync<IReliableDictionary<short, Container>>(dictName);

            using (var tx = this.StateManager.CreateTransaction())
            {
                var result = await networkModelCopy.TryGetValueAsync(tx, (short)type);

                // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                // discarded, and nothing is saved to the secondary replicas.
                await tx.CommitAsync();

                return result.Value;
            }
        }

        #region Find

        public async Task<bool> EntityExists(long globalId)
        {
            DMSType type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            if (await ContainerExists(type))
            {
                Container container = await GetContainer(type);

                if (container.EntityExists(globalId))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> EntityExistsCopy(long globalId)
        {
            DMSType type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

            if (await ContainerExistsCopy(type))
            {
                Container container = await GetContainerCopy(type);

                if (container.EntityExists(globalId))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> ContainerExistsCopy(DMSType type)
        {
            if(await GetContainerFromModel(type, "networkDataModelCopy") != null)
            {
                return true;
            }
            else
            {
                return false;
            }

            if (networkDataModelCopy.ContainsKey(type))
            {
                return true;
            }

            return false;
        }

        public async Task<IdentifiedObject> GetEntity(long globalId)
        {
            if (await EntityExists(globalId))
            {
                DMSType type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
                IdentifiedObject io = (await GetContainer(type)).GetEntity(globalId);

                return io;
            }
            else
            {
                string message = string.Format("Entity  (GID = 0x{0:x16}) does not exist.", globalId);
                throw new Exception(message);
            }
        }

        public async Task<IdentifiedObject> GetEntityCopy(long globalId)
        {
            if (await EntityExistsCopy(globalId))
            {
                DMSType type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
                IdentifiedObject io = (await GetContainerCopy(type)).GetEntity(globalId);

                return io;
            }
            else
            {
                string message = string.Format("Entity  (GID = 0x{0:x16}) does not exist.", globalId);
                throw new Exception(message);
            }
        }


        /// <summary>
        /// Checks if container exists in model.
        /// </summary>
        /// <param name="type">Type of container.</param>
        /// <returns>True if container exists, otherwise FALSE.</returns>
        private async Task<bool> ContainerExists(DMSType type)
        {
            if((await GetContainerFromModel(type, "networkDataModel")) != null)
            {
                return true;
            }

            return false;

            //if (networkDataModel.ContainsKey(type))
            //{
            //    return true;
            //}

            //return false;
        }

        /// <summary>
        /// Gets container of specified type.
        /// </summary>
        /// <param name="type">Type of container.</param>
        /// <returns>Container for specified local id</returns>
        private async Task<Container> GetContainer(DMSType type)
        {
            Container cont = await GetContainerFromModel(type, "networkDataModel");

            if(cont != null)
            {
                return cont;
            }
            else
            {
                string message = string.Format("Container does not exist for type {0}.", type);
                throw new Exception(message);
            }

            //if (ContainerExists(type))
            //{
            //    return networkDataModel[type];
            //}
            //else
            //{
            //    string message = string.Format("Container does not exist for type {0}.", type);
            //    throw new Exception(message);
            //}
        }

        private async Task<Container> GetContainerCopy(DMSType type)
        {
            Container cont = await GetContainerFromModel(type, "networkDataModelCopy");

            if(cont != null)
            {
                return cont;
            }
            else
            {
                string message = string.Format("Container does not exist for type {0}.", type);
                throw new Exception(message);
            }

            //if (ContainerExistsCopy(type))
            //{
            //    return networkDataModelCopy[type];
            //}
            //else
            //{
            //    string message = string.Format("Container does not exist for type {0}.", type);
            //    throw new Exception(message);
            //}
        }

        #endregion Find

        public List<ResourceDescription> GetResourceDescriptions()
        {
            List<ResourceDescription> retVal = new List<ResourceDescription>();
            List<long> gids = new List<long>();

            Task.Run(async () => gids = await RetrieveAllGIDs()).Wait();

            try
            {
                foreach (long gid in gids)
                {
                    IdentifiedObject io = null;
                    Task.Run(async () => io = await GetEntity(gid)).Wait();

                    ResourceDescription rd = new ResourceDescription(gid);

                    Property property = null;

                    // insert all properties
                    foreach (ModelCode propId in RetrieveAllProps(gid))
                    {
                        property = new Property(propId);
                        io.GetProperty(property);
                        rd.AddProperty(property);
                    }

                    retVal.Add(rd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return retVal;
        }

        public async Task<UpdateResult> ApplyDelta(Delta delta)
        {
            bool applyingStarted = false;
            UpdateResult updateResult = new UpdateResult();

            try
            {
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Applying  delta to network model.");

                Dictionary<short, int> typesCounters = await GetCounters();
                Dictionary<long, long> globalIdPairs = new Dictionary<long, long>();
                delta.FixNegativeToPositiveIds(ref typesCounters, ref globalIdPairs);
                updateResult.GlobalIdPairs = globalIdPairs;
                delta.SortOperations();

                applyingStarted = true;

                foreach (ResourceDescription rd in delta.InsertOperations)
                {
                    await InsertEntity(rd);
                }

                foreach (ResourceDescription rd in delta.UpdateOperations)
                {
                    UpdateEntity(rd);
                }

                foreach (ResourceDescription rd in delta.DeleteOperations)
                {
                    DeleteEntity(rd);
                }

            }
            catch (Exception ex)
            {
                string message = string.Format("Applying delta to network model failed. {0}.", ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);

                updateResult.Result = ResultType.Failed;
                updateResult.Message = message;
            }
            finally
            {
                if (applyingStarted)
                {
                    //SaveDelta(delta);
                }

                if (updateResult.Result == ResultType.Succeeded)
                {
                    string mesage = "Applying delta to network model successfully finished.";
                    CommonTrace.WriteTrace(CommonTrace.TraceInfo, mesage);
                    updateResult.Message = mesage;
                }
            }

            return updateResult;
        }

        /// <summary>
        /// Inserts entity into the network model.
        /// </summary>
        /// <param name="rd">Description of the resource that should be inserted</param>        
		private async Task InsertEntity(ResourceDescription rd)
        {
            if (rd == null)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Insert entity is not done because update operation is empty.");
                return;
            }

            long globalId = rd.Id;

            CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Inserting entity with GID ({0:x16}).", globalId);

            // check if mapping for specified global id already exists			
            if (await this.EntityExistsCopy(globalId))
            {
                string message = String.Format("Failed to insert entity because entity with specified GID ({0:x16}) already exists in network model.", globalId);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                throw new Exception(message);
            }

            try
            {
                // find type
                DMSType type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);

                Container container = null;

                // get container or create container 
                if (await ContainerExistsCopy(type))
                {
                    container = await GetContainerCopy(type);
                }
                else
                {
                    container = new Container();
                    networkDataModelCopy.Add(type, container);
                    AddContainerToModel(type, container, "networkDataModelCopy");
                }

                // create entity and add it to container
                IdentifiedObject io = container.CreateEntity(globalId);

                // apply properties on created entity
                if (rd.Properties != null)
                {
                    foreach (Property property in rd.Properties)
                    {
                        // globalId must not be set as property
                        if (property.Id == ModelCode.IDOBJ_GID)
                        {
                            continue;
                        }

                        if (property.Type == PropertyType.Reference)
                        {
                            // if property is a reference to another entity 
                            long targetGlobalId = property.AsReference();

                            if (targetGlobalId != 0)
                            {

                                if (!(await EntityExistsCopy(targetGlobalId)))
                                {
                                    string message = string.Format("Failed to get target entity with GID: 0x{0:X16}. {0}", targetGlobalId);
                                    throw new Exception(message);
                                }

                                // get referenced entity for update
                                IdentifiedObject targetEntity = await GetEntityCopy(targetGlobalId);
                                
                                targetEntity.AddReference(property.Id, io.GID);                                
                            }

                            io.SetProperty(property);
                        }
                        else
                        {
                            io.SetProperty(property);
                        }
                    }
                }

                CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Inserting entity with GID ({0:x16}) successfully finished.", globalId);
            }
            catch (Exception ex)
            {
                string message = String.Format("Failed to insert entity (GID = 0x{0:x16}) into model. {1}", rd.Id, ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Updates entity in block model.
        /// </summary>
        /// <param name="rd">Description of the resource that should be updated</param>		
        private async void UpdateEntity(ResourceDescription rd)
        {
            if (rd == null || rd.Properties == null && rd.Properties.Count == 0)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Update entity is not done because update operation is empty.");
                return;
            }

            try
            {
                long globalId = rd.Id;

                CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Updating entity with GID ({0:x16}).", globalId);

                if (!await this.EntityExistsCopy(globalId))
                {
                    string message = String.Format("Failed to update entity because entity with specified GID ({0:x16}) does not exist in network model.", globalId);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    throw new Exception(message);
                }

                IdentifiedObject io = await GetEntityCopy(globalId);

                // updating properties of entity
                foreach (Property property in rd.Properties)
                {
                    if (property.Type == PropertyType.Reference)
                    {
                        long oldTargetGlobalId = io.GetProperty(property.Id).AsReference();

                        if (oldTargetGlobalId != 0)
                        {
                            IdentifiedObject oldTargetEntity = await GetEntityCopy(oldTargetGlobalId);
                            oldTargetEntity.RemoveReference(property.Id, globalId);
                        }

                        // updating reference of entity
                        long targetGlobalId = property.AsReference();

                        if (targetGlobalId != 0)
                        {
                            if (!await EntityExistsCopy(targetGlobalId))
                            {
                                string message = string.Format("Failed to get target entity with GID: 0x{0:X16}.", targetGlobalId);
                                throw new Exception(message);
                            }

                            IdentifiedObject targetEntity = await GetEntityCopy(targetGlobalId);
                            targetEntity.AddReference(property.Id, globalId);
                        }

                        // update value of the property in specified entity
                        io.SetProperty(property);
                    }
                    else
                    {
                        // update value of the property in specified entity
                        io.SetProperty(property);
                    }
                }

                CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Updating entity with GID ({0:x16}) successfully finished.", globalId);
            }
            catch (Exception ex)
            {
                string message = String.Format("Failed to update entity (GID = 0x{0:x16}) in model. {1} ", rd.Id, ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Deletes resource from the netowrk model.
        /// </summary>
        /// <param name="rd">Description of the resource that should be deleted</param>		
        private async void DeleteEntity(ResourceDescription rd)
        {
            if (rd == null)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceInfo, "Delete entity is not done because update operation is empty.");
                return;
            }

            try
            {
                long globalId = rd.Id;

                CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Deleting entity with GID ({0:x16}).", globalId);

                // check if entity exists
                if (!await this.EntityExistsCopy(globalId))
                {
                    string message = String.Format("Failed to delete entity because entity with specified GID ({0:x16}) does not exist in network model.", globalId);
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    throw new Exception(message);
                }

                // get entity to be deleted
                IdentifiedObject io = await GetEntityCopy(globalId);

                // check if entity could be deleted (if it is not referenced by any other entity)
                if (io.IsReferenced)
                {
                    Dictionary<ModelCode, List<long>> references = new Dictionary<ModelCode, List<long>>();
                    io.GetReferences(references, TypeOfReference.Target);

                    StringBuilder sb = new StringBuilder();

                    foreach (KeyValuePair<ModelCode, List<long>> kvp in references)
                    {
                        foreach (long referenceGlobalId in kvp.Value)
                        {
                            sb.AppendFormat("0x{0:x16}, ", referenceGlobalId);
                        }
                    }

                    string message = String.Format("Failed to delete entity (GID = 0x{0:x16}) because it is referenced by entities with GIDs: {1}.", globalId, sb.ToString());
                    CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                    throw new Exception(message);
                }

                // find property ids
                List<ModelCode> propertyIds = resourcesDescs.GetAllSettablePropertyIdsForEntityId(io.GID);

                // remove references
                Property property = null;

                foreach (ModelCode propertyId in propertyIds)
                {
                    PropertyType propertyType = Property.GetPropertyType(propertyId);

                    if (propertyType == PropertyType.Reference)
                    {
                        property = io.GetProperty(propertyId);

                        if (propertyType == PropertyType.Reference)
                        {
                            // get target entity and remove reference to another entity
                            long targetGlobalId = property.AsReference();

                            if (targetGlobalId != 0)
                            {
                                // get target entity
                                IdentifiedObject targetEntity = await GetEntityCopy(targetGlobalId);

                                // remove reference to another entity
                                targetEntity.RemoveReference(propertyId, globalId);
                            }
                        }
                    }
                }

              // remove entity form netowrk model
              DMSType type = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(globalId);
                Container container = await GetContainerCopy(type);
                container.RemoveEntity(globalId);

                CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Deleting entity with GID ({0:x16}) successfully finished.", globalId);
            }
            catch (Exception ex)
            {
                string message = String.Format("Failed to delete entity (GID = 0x{0:x16}) from model. {1}", rd.Id, ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Returns related gids with source according to the association 
        /// </summary>
        /// <param name="source">source id</param>		
        /// <param name="association">desinition of association</param>
        /// <returns>related gids</returns>
        //private List<long> ApplyAssocioationOnSource(long source, Association association)
        //{
        //    List<long> relatedGids = new List<long>();

        //    if (association == null)
        //    {
        //        association = new Association();
        //    }

        //    IdentifiedObject io = GetEntity(source);

        //    if (!io.HasProperty(association.PropertyId))
        //    {
        //        throw new Exception(string.Format("Entity with GID = 0x{0:x16} does not contain prperty with Id = {1}.", source, association.PropertyId));
        //    }

        //    Property propertyRef = null;

        //    if (Property.GetPropertyType(association.PropertyId) == PropertyType.Reference)
        //    {
        //        propertyRef = io.GetProperty(association.PropertyId);
        //        long relatedGidFromProperty = propertyRef.AsReference();

        //        if (relatedGidFromProperty != 0)
        //        {
        //            if (association.Type == 0 || (short)ModelCodeHelper.GetTypeFromModelCode(association.Type) == ModelCodeHelper.ExtractTypeFromGlobalId(relatedGidFromProperty))
        //            {
        //                relatedGids.Add(relatedGidFromProperty);
        //            }
        //        }
        //    }
        //    else if (Property.GetPropertyType(association.PropertyId) == PropertyType.ReferenceVector)
        //    {
        //        propertyRef = io.GetProperty(association.PropertyId);
        //        List<long> relatedGidsFromProperty = propertyRef.AsReferences();

        //        if (relatedGidsFromProperty != null)
        //        {
        //            foreach (long relatedGidFromProperty in relatedGidsFromProperty)
        //            {
        //                if (association.Type == 0 || (short)ModelCodeHelper.GetTypeFromModelCode(association.Type) == ModelCodeHelper.ExtractTypeFromGlobalId(relatedGidFromProperty))
        //                {
        //                    relatedGids.Add(relatedGidFromProperty);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception(string.Format("Association propertyId = {0} is not reference or reference vector type.", association.PropertyId));
        //    }

        //    return relatedGids;
        //}

        private void Initialize()
        {
            List<Delta> result = ReadAllDeltas();

            foreach (Delta delta in result)
            {
                try
                {
                    foreach (ResourceDescription rd in delta.InsertOperations)
                    {
                        InsertEntity(rd);
                    }

                    foreach (ResourceDescription rd in delta.UpdateOperations)
                    {
                        UpdateEntity(rd);
                    }

                    foreach (ResourceDescription rd in delta.DeleteOperations)
                    {
                        DeleteEntity(rd);
                    }
                }
                catch (Exception ex)
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, "Error while applying delta (id = {0}) during service initialization. {1}", delta.Id, ex.Message);
                }
            }
        }

        private void SaveDelta(Delta delta)
        {
            bool fileExisted = false;

            if (File.Exists(Config.Instance.ConnectionString))
            {
                fileExisted = true;
            }

            FileStream fs = new FileStream(Config.Instance.ConnectionString, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fs.Seek(0, SeekOrigin.Begin);

            BinaryReader br = null;
            int deltaCount = 0;

            if (fileExisted)
            {
                br = new BinaryReader(fs);
                deltaCount = br.ReadInt32();
            }

            BinaryWriter bw = new BinaryWriter(fs);
            fs.Seek(0, SeekOrigin.Begin);

            delta.Id = ++deltaCount;
            byte[] deltaSerialized = delta.Serialize();
            int deltaLength = deltaSerialized.Length;

            bw.Write(deltaCount);
            fs.Seek(0, SeekOrigin.End);
            bw.Write(deltaLength);
            bw.Write(deltaSerialized);

            if (br != null)
            {
                br.Close();
            }

            bw.Close();
            fs.Close();
        }

        private List<Delta> ReadAllDeltas()
        {
            List<Delta> result = new List<Delta>();

            if (!File.Exists(Config.Instance.ConnectionString))
            {
                return result;
            }

            FileStream fs = new FileStream(Config.Instance.ConnectionString, FileMode.OpenOrCreate, FileAccess.Read);
            fs.Seek(0, SeekOrigin.Begin);

            if (fs.Position < fs.Length) // if it is not empty stream
            {
                BinaryReader br = new BinaryReader(fs);

                int deltaCount = br.ReadInt32();
                int deltaLength = 0;
                byte[] deltaSerialized = null;
                Delta delta = null;

                for (int i = 0; i < deltaCount; i++)
                {
                    deltaLength = br.ReadInt32();
                    deltaSerialized = new byte[deltaLength];
                    br.Read(deltaSerialized, 0, deltaLength);
                    delta = Delta.Deserialize(deltaSerialized);
                    result.Add(delta);
                }

                br.Close();
            }

            fs.Close();

            return result;
        }

        private async Task<Dictionary<short, int>> GetCounters()
        {
            Dictionary<short, int> typesCounters = new Dictionary<short, int>();

            foreach (DMSType type in Enum.GetValues(typeof(DMSType)))
            {
                typesCounters[(short)type] = 0;

                if (await ModelContainsKey(type, "networkDataModelCopy"))
                {
                    typesCounters[(short)type] = (await GetContainerCopy(type)).Count;
                }

                //if (networkDataModelCopy.ContainsKey(type))
                //{
                //    typesCounters[(short)type] = GetContainerCopy(type).Count;
                //}
            }

            return typesCounters;
        }

        private async void AddContainerToModel(DMSType type, Container cont, string dictName)
        {
            var networkModel = await this.StateManager.GetOrAddAsync<IReliableDictionary<short, Container>>(dictName);

            using (var tx = this.StateManager.CreateTransaction())
            {
                await networkModel.AddAsync(tx, (short)type, cont);

                // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                // discarded, and nothing is saved to the secondary replicas.
                await tx.CommitAsync();
            }
        }

        private async Task<bool> ModelContainsKey(DMSType type, string dictName)
        {
            var networkModel = await this.StateManager.GetOrAddAsync<IReliableDictionary<short, Container>>(dictName);

            using (var tx = this.StateManager.CreateTransaction())
            {
                return await networkModel.ContainsKeyAsync(tx, (short)type);
            }
        }

        public async Task<List<long>> RetrieveAllGIDs()
        {
            List<long> retVal = new List<long>();

            var networkModel = await this.StateManager.GetOrAddAsync<IReliableDictionary<short, Container>>("networkDataModel");

            using (var tx = this.StateManager.CreateTransaction())
            {
                var enumerable = await networkModel.CreateEnumerableAsync(tx);
                var asyncEnumerator = enumerable.GetAsyncEnumerator();

                while (await asyncEnumerator.MoveNextAsync(CancellationToken.None))
                {
                    retVal.AddRange(asyncEnumerator.Current.Value.Entities.Keys);
                }
            }

            //List<long> retVal = new List<long>();

            //foreach (var item in networkDataModel)
            //{
            //    retVal.AddRange(item.Value.Entities.Keys);
            //}

            return retVal;
        }

        public List<ModelCode> RetrieveAllProps(long gid)
        {
            List<ModelCode> ret = new List<ModelCode>();
            short type = ModelCodeHelper.ExtractTypeFromGlobalId(gid);
            ret = resourcesDescs.GetAllPropertyIds((DMSType)type);

            return ret;
        }

        public bool Prepare()
        {
            Console.WriteLine("NMS Prepare.");
            ServiceEventSource.Current.Message("NMS Prepare.");

            return true;
        }

        public bool Commit()
        {
            Console.WriteLine("NMS Commit.");
            ServiceEventSource.Current.Message("NMS Commit.");

            //IReliableDictionary<short, Container> networkModel = GetDataModel("networkDataModel").Result;
            //IReliableDictionary<short, Container> networkModelCopy = GetDataModel("networkDataModelCopy").Result;
            //IReliableDictionary<short, Container> networkModelOld = GetDataModel("networkDataModelOld").Result;

            //networkModelOld = networkModel;
            //networkModel = networkModelCopy;
            //ClearDict("networkDataModelCopy");

            Task.Run(async () => await CopyDataModel("networkDataModel", "networkDataModelOld")).Wait();
            Task.Run(async () => await CopyDataModel("networkDataModelCopy", "networkDataModel")).Wait();
            Task.Run(async () => await ClearDict("networkDataModelCopy")).Wait();

            networkDataModelOld = new Dictionary<DMSType, Container>(networkDataModel);
            networkDataModel = new Dictionary<DMSType, Container>(networkDataModelCopy);
            networkDataModelCopy.Clear();

            this.commitFinished = true;

            return true;
        }

        private async Task ClearDict(string dictName)
        {
            IReliableDictionary<short, Container> networkModel = await GetDataModel(dictName);

            await networkModel.ClearAsync();
        }

        public void Rollback()
        {
            Console.WriteLine("NMS Rollback.");
            ServiceEventSource.Current.Message("NMS Rollback.");

            //IReliableDictionary<short, Container> networkModel = GetDataModel("networkDataModel").Result;
            //IReliableDictionary<short, Container> networkModelCopy = GetDataModel("networkDataModelCopy").Result;
            //IReliableDictionary<short, Container> networkModelOld = GetDataModel("networkDataModelOld").Result;

            //networkModel = networkModelOld;

            Task.Run(async () => await CopyDataModel("networkDataModelOld", "networkDataModel")).Wait();
            Task.Run(async () => await ClearDict("networkDataModelCopy")).Wait();
            //networkModelCopy.ClearAsync();

            networkDataModel = new Dictionary<DMSType, Container>(networkDataModelOld);
            networkDataModelCopy.Clear();
        }

        private async Task<IReliableDictionary<short, Container>> GetDataModel(string dictName)
        {
            var ret = await this.StateManager.TryGetAsync<IReliableDictionary<short, Container>>(dictName);

            if(ret.HasValue)
            {
                return ret.Value;
            }

            throw new Exception($"Dictionary <{dictName}> not found");
        }

        public UpdateResult UpdateModel(Delta delta)
        {
            Console.WriteLine("Update model invoked");
            ServiceEventSource.Current.Message("Update model invoked.");
            UpdateResult result = null;
            Task.Run(async () => result = await ApplyDelta(delta)).Wait();

            //IReliableDictionary<short, Container> networkModel = GetDataModel("networkDataModel").Result;
            //IReliableDictionary<short, Container> networkModelOld = GetDataModel("networkDataModelOld").Result;

            //networkModelOld = networkModel;
            Task.Run(async () => await CopyDataModel("networkDataModel", "networkDataModelOld")).Wait();

            networkDataModelOld = new Dictionary<DMSType, Container>(networkDataModel);

            try
            {
                TMProxy _proxyTM = new TMProxy(this);
                _proxyTM.Enlist();
                _proxyTM.EndEnlist(true);
                //try
                //{
                //    ModelUpdateProxy _proxyCE = new ModelUpdateProxy("CE");
                //    if (_proxyCE.UpdateModel(delta).Result == ResultType.Failed)
                //    {
                //        _proxyTM.EndEnlist(false);
                //        return new UpdateResult() { Result = ResultType.Failed, Message = "CE failed to update model." };
                //    }

                //    //SCADA NDS
                //    ModelUpdateProxy _proxyNDS = new ModelUpdateProxy("NDS");
                //    if (_proxyNDS.UpdateModel(delta).Result == ResultType.Failed)
                //    {
                //        _proxyTM.EndEnlist(false);
                //        return new UpdateResult() { Result = ResultType.Failed, Message = "NDS failed to update model." };
                //    }

                //    _proxyTM.EndEnlist(true);
                //}
                //catch (Exception e)
                //{
                //    _proxyTM.EndEnlist(false);
                //    return new UpdateResult() { Message = e.Message, Result = ResultType.Failed };
                //}
            }
            catch (Exception ex)
            {
                return new UpdateResult() { Message = "NMS enlist failed: " + ex.Message, Result = ResultType.Failed };
            }            

            return result;
        }

        private async Task CopyDataModel(string sourceDictName, string destDictName)
        {
            var sourceDataModel = await GetDataModel(sourceDictName);
            var destinationDataModel = await GetDataModel(destDictName);

            await destinationDataModel.ClearAsync();

            using (var tx = this.StateManager.CreateTransaction())
            {
                var enumerable = await sourceDataModel.CreateEnumerableAsync(tx);
                var asyncEnumerator = enumerable.GetAsyncEnumerator();

                while (await asyncEnumerator.MoveNextAsync(CancellationToken.None))
                {
                    KeyValuePair<short, Container> item = asyncEnumerator.Current;
                    await destinationDataModel.AddAsync(tx, item.Key, item.Value);
                }

                await tx.CommitAsync();
            }
        }
    }
}
