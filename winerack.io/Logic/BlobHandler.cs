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
			_imageDirectory = imageDirectory;

			var connectionString = ConfigurationManager.ConnectionStrings["AzureJobsStorage"].ConnectionString;
			_storageAccount = CloudStorageAccount.Parse(connectionString);

			var client = _storageAccount.CreateCloudBlobClient();
			var container = client.GetContainerReference(imageDirectory);
			container.CreateIfNotExists();
			container.SetPermissions(new BlobContainerPermissions {
				PublicAccess = BlobContainerPublicAccessType.Blob
			});
		}

		#endregion Constructor

		#region Declarations

		private readonly string _imageDirectory;

		private readonly CloudStorageAccount _storageAccount;

		#endregion Declarations

		#region Public Methods

		public Guid? UploadImage(HttpPostedFileBase file, IEnumerable<ImageSize> sizes) {
			var blobClient = _storageAccount.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference(_imageDirectory);

		  if (file == null)
		  {
		    return null;
		  }

		  var name = Guid.NewGuid();

		  // Save the original
		  string filename = $"{name}_o.jpg";
		  var blockBlob = container.GetBlockBlobReference(filename);
		  blockBlob.UploadFromStream(file.InputStream);

		  // Handle each size
		  foreach (var size in sizes) {
		    var thumb = size.Crop
		      ? Images.ResizeAndCrop(file.InputStream, size.MaxWidth, size.MaxHeight, true)
		      : Images.Resize(file.InputStream, size.MaxWidth, size.MaxHeight, true, false);

		    filename = $"{name}_{size.Suffix}.jpg";

		    blockBlob = container.GetBlockBlobReference(filename);
		    blockBlob.UploadFromStream(thumb);
		  }	

		  return name;
		}

		#endregion Public Methods

	}
}