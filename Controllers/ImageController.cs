using LimFx.Business.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.Extensions.ML;
using PornJudger.Dto;
using PornJudger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PornJudger.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ImageController : ControllerBase
    {
        PredictionEnginePool<ImageInput, ModelOutput> enginePool;
        public ImageController(PredictionEnginePool<ImageInput, ModelOutput> enginePool)
        {
            this.enginePool = enginePool;
        }

        [AllowAnonymous]
        [HttpPost("checkporn")]
        public async ValueTask<ModelOutput> CheckImageAsync([FromForm] IFormFileCollection imgs)
        {
            if (imgs.Count < 1)
            {
                throw new _400Exception("A file input is expected");
            }
            using var stream = imgs[0].OpenReadStream();
            var imgname = imgs[0].FileName;
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes);
            var output = enginePool.Predict(new ImageInput
            {
                Image = bytes,
                Label = ""
            });
            return output;
        }




    }
}
