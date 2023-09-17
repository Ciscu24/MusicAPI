using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.Generic
{
    public class GenericResponseDto
    {
        private string errorMessage { get; set; }

        /// <summary>
        ///     Variable para saber si han habido errores
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        ///     Variable para que contiene el mensaje de error
        /// </summary>
        public string ErrorMessage {
            get { return errorMessage; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    errorMessage = value;
                    Success = false;
                }
            }
        }

        /// <summary>
        ///     Variable que cotiene el dato que va a devolver el método
        /// </summary>
        public object Data { get; set; }

        public GenericResponseDto() {
            Success = true;
            ErrorMessage = string.Empty;
            Data = null;
        }
    }
}
