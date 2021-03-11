// ==============================================================
//           _
//     /\   | |
//    /  \  | |__ __ _ ___ _ _ ______ _
//   / /\ \ | '_ \ / _` |/ __| | | |_  / _` |
//  / ____ \| |_) | (_| | (__| |_| |/ / (_| |
// /_/    \_\_.__/ \__,_|\___|\__,_/___\__,_|
//
// Data Processing Platform
// Copyright 2020-2021 by daxnet. All rights reserved.
// Apache License Version 2.0
// ==============================================================

using Abacuza.Common.Models;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Abacuza.Common.ApiService.Controllers
{
    /// <summary>
    /// Represents the API controller that provides the access
    /// to files.
    /// </summary>
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        #region Private Fields

        private readonly ILogger<FilesController> _logger;
        private readonly IAmazonS3 _s3;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <c>FilesController</c> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="s3"></param>
        public FilesController(ILogger<FilesController> logger,
            IAmazonS3 s3) => (_s3, _logger) = (s3, logger);

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Deletes a file from the S3 storage.
        /// </summary>
        /// <param name="bucket">The bucket name.</param>
        /// <param name="key">The file key.</param>
        /// <param name="file">The name of the file.</param>
        /// <returns></returns>
        [HttpDelete("{bucket}/{key}/{file}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteS3FileAsync(string bucket, string key, string file)
        {
            var denormalizedBucket = HttpUtility.UrlDecode(bucket);
            var denormalizedKey = HttpUtility.UrlDecode(key);
            var denormalizedFile = HttpUtility.UrlDecode(file);

            var combinedKey = $"{denormalizedKey}/{denormalizedFile}";
            var response = await _s3.DeleteObjectAsync(denormalizedBucket, combinedKey);
            return StatusCode((int)response.HttpStatusCode, response.ResponseMetadata);
        }

        [HttpDelete("{bucket}/{folderPath}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteS3FolderAsync(string bucket, string folderPath)
        {
            var denormalizedBucket = HttpUtility.UrlDecode(bucket);
            var denormalizedFolderPath = HttpUtility.UrlDecode(folderPath);

            var deleteObjectsRequest = new DeleteObjectsRequest();
            var listObjectsRequest = new ListObjectsRequest
            {
                BucketName = denormalizedBucket,
                Prefix = denormalizedFolderPath
            };
            var listObjectResponse = await _s3.ListObjectsAsync(listObjectsRequest);
            listObjectResponse.S3Objects.ForEach(o => deleteObjectsRequest.AddKey(o.Key));
            deleteObjectsRequest.BucketName = denormalizedBucket;
            var response = await _s3.DeleteObjectsAsync(deleteObjectsRequest);
            return StatusCode((int)response.HttpStatusCode, response.ResponseMetadata);
        }

        [HttpPost("s3")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadToS3Async()
        {
            try
            {
                var bucketName = Request?.Form?.FirstOrDefault(x => x.Key == "bucket")
                    .Value
                    .FirstOrDefault();
                if (string.IsNullOrEmpty(bucketName))
                {
                    return BadRequest("Bucket name was not specified in the form data.");
                }

                var keyName = Request?.Form?.FirstOrDefault(x => x.Key == "key")
                    .Value
                    .FirstOrDefault();
                if (string.IsNullOrEmpty(keyName))
                {
                    return BadRequest("Key name was not specified in the form data.");
                }

                if (Request?.Form?.Files?.Count == 0)
                {
                    return BadRequest("No file can be saved.");
                }

                var fileTransUtility = new TransferUtility(_s3);
                var files = Request?.Form?.Files;
                var responseItems = new List<object>();
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        using var stream = file.OpenReadStream();
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName?.Trim('"');
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            await fileTransUtility.UploadAsync(stream, bucketName, $"{keyName}/{fileName}");
                            responseItems.Add(new S3File(bucketName, keyName, fileName));
                        }
                    }
                }

                return Ok(responseItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred when uploading file to S3.");
                return StatusCode(500, ex.ToString());
            }
        }

        #endregion Public Methods
    }
}
