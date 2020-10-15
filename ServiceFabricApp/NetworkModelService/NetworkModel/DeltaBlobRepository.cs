using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkModelService
{
    public class DeltaBlobRepository
    {
        CloudBlobContainer BlobContainer;
        public DeltaBlobRepository()
        {
            InitBlobs();
        }
        public void InitBlobs()
        {
            try
            {
                // read account configuration settings
                var storageAccount = CloudStorageAccount.Parse(Config.Instance.ConnectionString);
                
                CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
                BlobContainer = blobStorage.GetContainerReference("delta-container");
                BlobContainer.CreateIfNotExists();
                // configure container for public access
                var permissions = BlobContainer.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                BlobContainer.SetPermissions(permissions);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.Message(e.Message);
            }
        }

        public void AddDeltaBlob(string uniqueBlobName, byte[] binaryData)
        {
            CloudBlockBlob blob = BlobContainer.GetBlockBlobReference(uniqueBlobName);
            //blob.Properties.ContentType = "application/octet-stream";

            using (MemoryStream stream = new MemoryStream(binaryData, false))
            {
                blob.UploadFromStream(stream);
            }

            //blob.UploadFromByteArray(binaryData, 0, binaryData.Length);
        }

        public byte[] ReadLastDeltaBlob()
        {
            var blobs = BlobContainer.ListBlobs();
            CloudBlockBlob blob = blobs.OfType<CloudBlockBlob>().LastOrDefault();
            if(blob != null)
            {
                blob.FetchAttributes();
                long blobSize = blob.Properties.Length;
            }
            byte[] returnValue = new byte[] { };
            blob.DownloadToByteArray(returnValue, 0);

            return returnValue;
        }

        public List<byte[]> ReadAllDeltaBlobs()
        {
            List<byte[]> returnValue = new List<byte[]>();

            var blobs = BlobContainer.ListBlobs().ToList();

            if(blobs.Count() > 0)
            {
                List<CloudBlockBlob> listblob = blobs.OfType<CloudBlockBlob>().ToList();
                foreach (CloudBlockBlob item in listblob)
                {
                    item.FetchAttributes();
                    long blobSize = item.Properties.Length;

                    byte[] data = new byte[blobSize];
                    item.DownloadToByteArray(data, 0);

                    returnValue.Add(data);
                }
            }

            

            return returnValue;
        }
    }
}
