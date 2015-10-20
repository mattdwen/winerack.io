using Microsoft.Framework.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;

namespace winerack.Services
{
  public class AzureService
  {
    #region Constructor

    public AzureService(IConfiguration config)
    {
      var connectionString = config["Data:AzureStorage:ConnectionString"];
      _storageAccount = CloudStorageAccount.Parse(connectionString);
      _blobUrl = config["Azure:Blob:Url"];
    }

    #endregion Constructor

    #region Declarations

    private readonly string _blobUrl;
    readonly CloudStorageAccount _storageAccount;

    #endregion Declarations

    #region Private Methods

    private CloudBlobContainer GetContainer(string directory)
    {
      var blobClient = _storageAccount.CreateCloudBlobClient();
      var container = blobClient.GetContainerReference(directory);

      return container;
    }

    #endregion Private Methods

    #region Public Methods

    public void Delete(string directory, string filename)
    {
      var container = GetContainer(directory);
      var blob = container.GetBlockBlobReference(filename);
      blob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
    }

    public string GetUrl(string container, string filename)
    {
      if (string.IsNullOrWhiteSpace(filename))
      {
        return null;
      }

      return _blobUrl + container + "/" + filename;
    }

    public string UploadImage(Stream image, string directory)
    {
      var filename = Guid.NewGuid().ToString() + ".jpg";

      var container = GetContainer(directory);
      var blockBlob = container.GetBlockBlobReference(filename);
      blockBlob.UploadFromStream(image);

      return filename;
    }

    #endregion Public Methods
  }
}