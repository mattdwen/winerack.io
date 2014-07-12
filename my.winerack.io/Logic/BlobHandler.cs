using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace winerack.Logic {
	public class BlobHandler {

		#region Constructor

		public BlobHandler(string imageDirectory) {
			this.imageDirectory = imageDirectory;

			var connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
			storageAccount = CloudStorageAccount.Parse(connectionString);

			CloudBlobClient client = storageAccount.CreateCloudBlobClient();
			CloudBlobContainer container = client.GetContainerReference(imageDirectory);
			container.CreateIfNotExists();
			container.SetPermissions(new BlobContainerPermissions {
				PublicAccess = BlobContainerPublicAccessType.Blob
			});
		}

		#endregion Constructor

		#region Declarations

		string imageDirectory;

		CloudStorageAccount storageAccount;

		#endregion Declarations

		#region Public Methods

		public Guid? Upload(HttpPostedFileBase file) {
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
			CloudBlobContainer container = blobClient.GetContainerReference(imageDirectory);

			if (file != null) {
				var name = Guid.NewGuid();
				CloudBlockBlob blockBlob = container.GetBlockBlobReference(name.ToString());
				blockBlob.UploadFromStream(file.InputStream);
				return name;
			}

			return null;
		}

		#endregion Public Methods

	}
}