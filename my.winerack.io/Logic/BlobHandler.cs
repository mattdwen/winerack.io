using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace winerack.Logic {
	public class BlobHandler {

		#region Constructor

		public BlobHandler(string imageDirectory) {
			this.imageDirectory = imageDirectory;

			var connectionString = ConfigurationManager.ConnectionStrings["AzureJobsStorage"].ConnectionString;
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

		public Guid? UploadImage(HttpPostedFileBase file, IList<Logic.ImageSize> sizes) {
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
			CloudBlobContainer container = blobClient.GetContainerReference(imageDirectory);

			if (file != null) {
				var name = Guid.NewGuid();
				string filename;
				CloudBlockBlob blockBlob;				

				// Save the original
				filename = name.ToString() + "_o.jpg";
				blockBlob = container.GetBlockBlobReference(filename);
				blockBlob.UploadFromStream(file.InputStream);

				// Handle each size
				foreach (var size in sizes) {
					MemoryStream thumb;
					if (size.Crop) {
						thumb = Images.ResizeAndCrop(file.InputStream, size.MaxWidth, size.MaxHeight, true);
					} else {
						thumb = Images.Resize(file.InputStream, size.MaxWidth, size.MaxHeight, true, false);
					}

					filename = name.ToString() + "_" + size.Suffix + ".jpg";

					blockBlob = container.GetBlockBlobReference(filename);
					blockBlob.UploadFromStream(thumb);
				}	

				return name;
			}

			return null;
		}

		#endregion Public Methods

	}
}