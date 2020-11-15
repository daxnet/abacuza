using Abacuza.Common.DataAccess;
using Abacuza.Common.Models;
using Abacuza.Jobs.ApiService.Models;
using Abacuza.Jobs.ApiService.Services;
using DnsClient.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Abacuza.Jobs.ApiService.Controllers
{
    [Route("api/job-runners")]
    [ApiController]
    public class JobRunnersController : ControllerBase
    {
        #region Private Fields

        private readonly CommonApiService _commonService;
        private readonly IDataAccessObject _dao;
        private readonly ILogger<JobRunnersController> _logger;

        #endregion Private Fields

        #region Public Constructors

        public JobRunnersController(IDataAccessObject dao, 
            CommonApiService commonService, 
            ILogger<JobRunnersController> logger)
        {
            _dao = dao;
            _commonService = commonService;
            _logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPost("{id}/files")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddFilesToJobRunnerAsync(Guid id, [FromBody] IEnumerable<S3File> files)
        {
            var updatingEntity = await _dao.GetByIdAsync<JobRunnerEntity>(id);
            if (updatingEntity == null)
            {
                return NotFound($"Job runner {id} doesn't exist.");
            }

            if (updatingEntity.BinaryFiles == null)
            {
                updatingEntity.BinaryFiles = new List<S3File>(files);
            }
            else
            {
                foreach (var file in files)
                {
                    if (!updatingEntity.BinaryFiles.Any(bf => bf.Equals(file)))
                    {
                        updatingEntity.BinaryFiles.Add(file);
                    }
                }
            }

            await _dao.UpdateByIdAsync(id, updatingEntity);
            return Ok(updatingEntity);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateJobRunnerAsync([FromBody] JobRunnerEntity model)
        {
            var existingModel = await _dao.FindBySpecificationAsync<JobRunnerEntity>(x => x.Name == model.Name);
            if (existingModel.FirstOrDefault() != null)
            {
                return Conflict($"The name {model.Name} already exists.");
            }

            await _dao.AddAsync(model);

            return CreatedAtAction(nameof(GetJobRunnerByIdAsync), new { id = model.Id }, model.Id);
        }

        [HttpDelete("{id}/files/{bucket}/{key}/{file}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteBinaryFileAsync(Guid id, string bucket, string key, string file)
        {
            var updatingEntity = await _dao.GetByIdAsync<JobRunnerEntity>(id);
            if (updatingEntity == null)
            {
                return NotFound($"Job runner {id} doesn't exist.");
            }

            var denormalizedBucket = HttpUtility.UrlDecode(bucket);
            var denormalizedKey = HttpUtility.UrlDecode(key);
            var denormalizedFile = HttpUtility.UrlDecode(file);

            var s3File = new S3File(denormalizedBucket, denormalizedKey, denormalizedFile);
            var existingFile = updatingEntity.BinaryFiles?.FirstOrDefault(f => f.Equals(s3File));
            if (existingFile == null)
            {
                return BadRequest($"The specified file doesn't exist in the binary files list of the given job runner.");
            }

            var (ok, statusCode, message) = await _commonService.DeleteS3FileAsync(s3File);

            if (ok)
            {
                updatingEntity.BinaryFiles.Remove(existingFile);
                await _dao.UpdateByIdAsync(id, updatingEntity);

                return Ok(updatingEntity);
            }
            else
            {
                return BadRequest($"Failed to delete the file from S3 storage. Server responded status code: {statusCode}. Error message: {message}.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteJobRunnerByIdAsync(Guid id)
        {
            var entity = await _dao.GetByIdAsync<JobRunnerEntity>(id);
            if (entity == null)
            {
                return NotFound($"The job runner {id} doesn't exist.");
            }

            if (entity.BinaryFiles?.Count > 0)
            {
                foreach (var file in entity.BinaryFiles)
                {
                    var (ok, statusCode, message) = await _commonService.DeleteS3FileAsync(file);
                    if (!ok)
                    {
                        _logger.LogWarning($"Delete file {file} failed with status code {statusCode}. Error message: {message}");
                    }
                }
            }

            await _dao.DeleteByIdAsync<JobRunnerEntity>(id);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<JobRunnerEntity>))]
        public async Task<IActionResult> GetAllJobRunnersAsync()
            => Ok(await _dao.GetAllAsync<JobRunnerEntity>());

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobRunnerEntity))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetJobRunnerByIdAsync(Guid id)
        {
            var entity = await _dao.GetByIdAsync<JobRunnerEntity>(id);
            if (entity == null)
            {
                return NotFound($"The job runner {id} doesn't exist.");
            }

            return Ok(entity);
        }
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchJobRunnerAsync(Guid id, [FromBody] JsonPatchDocument<JobRunnerEntity> patchDoc)
        {
            var updatingEntity = await _dao.GetByIdAsync<JobRunnerEntity>(id);
            if (updatingEntity == null)
            {
                return NotFound($"Job runner {id} doesn't exist.");
            }

            if (patchDoc != null)
            {
                patchDoc.ApplyTo(updatingEntity, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _dao.UpdateByIdAsync(id, updatingEntity);

                return Ok(updatingEntity);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        #endregion Public Methods
    }
}
