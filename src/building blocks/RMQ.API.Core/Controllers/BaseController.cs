using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RMQ.API.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {

        #region Variables

        private ICollection<string> _errors = new List<string>();

        #endregion

        #region Properties

        public ICollection<string> Errors { get => _errors; }

        #endregion

        #region Methods

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AdicionarErro(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected ActionResult CustomResponse(object result = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (!OperacaoValida())
            {
                var resultErrors = new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    { "Mensagens", _errors.ToArray()}
                });

                if (statusCode != HttpStatusCode.OK) return StatusCode((int)statusCode, resultErrors);

                return BadRequest(resultErrors);
            }

            return StatusCode((int)statusCode, result);
        }

        private bool OperacaoValida()
        {
            return !_errors.Any();
        }

        protected void AdicionarErro(string erro)
        {
            _errors.Add(erro);
        }

        protected void LimparErros()
        {
            _errors.Clear();
        }

        #endregion

    }
}
