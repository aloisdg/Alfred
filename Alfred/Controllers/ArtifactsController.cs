using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Alfred.Model.Artifacts;
using Alfred.Services;

namespace Alfred.Controllers
{
    [RoutePrefix("artifacts")]
    public class ArtifactsController : ApiController
    {
        private readonly IArtifactService _artifactService;

        public ArtifactsController(IArtifactService artifactService)
        {
            _artifactService = artifactService;
        }

        /// <summary>
        /// Get all artifacts
        /// </summary>
        /// <remarks>
        /// Get all artifacts
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<ArtifactModel>))]
        public IHttpActionResult GetArtifacts()
        {
            return Ok(_artifactService.GetArtifacts());
        }

        /// <summary>
        /// Get an artifacts
        /// </summary>
        /// <remarks>
        /// Get an artifacts
        /// </remarks>
        /// <param name="id">id of an artifact</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int?}")]
        [ResponseType(typeof(ArtifactModel))]
        public IHttpActionResult GetArtifact(int id)
        {
            var artifact = _artifactService.GetArtifact(id);
            if (artifact != null)
                return Ok(artifact);
            return NotFound();
        }
    }
}
